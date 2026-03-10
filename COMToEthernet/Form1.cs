using System.Configuration;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace COMToEthernet
{
    public partial class Form1 : Form
    {
        Configuration Config;
        SerialPort _serialPort = new SerialPort();
        TcpListener _tcpListener;
        List<TcpClient> _connectedClients = new List<TcpClient>();
        // Ensures all threads synchronize on this single object
        private readonly object _clientsLock = new object();
        // Buffer to hold incoming data from the serial port
        List<byte> _buffer = new List<byte>();
        private readonly object _bufferLock = new object();
        // Ensures only one TCP client writes to the serial port at a time (Modbus RTU is half-duplex)
        private readonly object _serialPortWriteLock = new object();
        System.Timers.Timer _timer = new System.Timers.Timer(50); // Adjust interval as needed
        bool _listening;
        bool hideApp;
        bool runWhenStart;
        private Task _reconnectTask = Task.CompletedTask;

        public Form1()
        {
            InitializeComponent();
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = false;

            //Get available port list from PC
            RefreshPortList(_serialPort, cbxCOM);

            //Load configuration from App.config
            try
            {
                // Get the path for the custom configuration file in the %APPDATA% folder
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string configPath = Path.Combine(appDataPath, "COMToEthernet", "App.config");

                // Load the configuration settings from the custom configuration file
                if (File.Exists(configPath))
                {
                    ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configPath };
                    Config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                }
                else
                {
                    lblStatusContent.Text = "Configuration file not found.";
                    //Use App.config in Application folder
                    Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }

            }
            catch (Exception ex)
            {
                lblStatusContent.Text = ex.Message;
                Logger.Log($"[{DateTime.Now.ToString()}] Error loading configuration: {ex.Message}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Config == null)
            {
                lblStatusContent.Text = "Configuration could not be loaded. Using defaults.";
                Logger.Log($"[{DateTime.Now}] Config is null in Form1_Load — skipping settings load.");
                return;
            }

            try
            {
                string comPort   = Config.AppSettings.Settings["COMPort"]?.Value   ?? string.Empty;
                string baudRate  = Config.AppSettings.Settings["BaudRate"]?.Value  ?? "9600";
                string dataBit   = Config.AppSettings.Settings["DataBit"]?.Value   ?? "8";
                string parity    = Config.AppSettings.Settings["Parity"]?.Value    ?? "None";
                string stopBits  = Config.AppSettings.Settings["StopBits"]?.Value  ?? "1";
                string tcpPort   = Config.AppSettings.Settings["TCPPort"]?.Value   ?? "8000";

                if (!bool.TryParse(Config.AppSettings.Settings["HideApp"]?.Value, out hideApp))
                    hideApp = false;
                if (!bool.TryParse(Config.AppSettings.Settings["RunWhenStart"]?.Value, out runWhenStart))
                    runWhenStart = false;

                cbxCOM.Text      = comPort;
                cbxBaudrate.Text = baudRate;
                cbxDataBit.Text  = dataBit;
                cbxParity.Text   = parity;
                cbxStopBit.Text  = stopBits;
                tbxPort.Text     = tcpPort;

                if (runWhenStart) btConnect_Click(sender, e);
                if (hideApp)
                {
                    this.ShowInTaskbar = false;
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                lblStatusContent.Text = $"Error loading settings: {ex.Message}";
                Logger.Log($"[{DateTime.Now}] Error in Form1_Load: {ex.Message}");
            }
        }
        private void btConnect_Click(object sender, EventArgs e)
        {
            if (!_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.PortName = cbxCOM.Text;
                    _serialPort.BaudRate = int.Parse(cbxBaudrate.Text);
                    _serialPort.DataBits = int.Parse(cbxDataBit.Text);
                    _serialPort.Parity = MyHelper.GetParity(cbxParity.Text);
                    _serialPort.StopBits = MyHelper.GetStopBits(cbxStopBit.Text);
                    _serialPort.WriteTimeout = 1000; // 1 second — prevents infinite hang on blocked serial port
                    // Unsubscribe first to prevent duplicate handlers on reconnect
                    _serialPort.DataReceived -= DataReceivedHandler;
                    _serialPort.DataReceived += DataReceivedHandler;

                    _serialPort.Open();
                    Logger.Log($"[{DateTime.Now}] Serial port {cbxCOM.Text} opened.");
                }
                catch (Exception ex)
                {
                    SafeUI(() =>
                    {
                        lblStatusContent.Text = $"[{DateTime.Now.ToString()}] Error: {ex.Message}";
                    });
                    Logger.Log($"[{DateTime.Now.ToString()}] Error loading configuration: {ex.Message}");
                    return;
                }
            }

            if (!int.TryParse(tbxPort.Text, out int tcpPort))
            {
                SafeUI(() =>
                {
                    lblStatusContent.Text = "[" + DateTime.Now.ToString() + "]" + "Invalid port number.";
                });
                Logger.Log($"[{DateTime.Now.ToString()}] Error: Invalid port number.");
                return;
            }

            if (_tcpListener == null || !_listening)
            {
                _tcpListener = new TcpListener(IPAddress.Any, tcpPort);
                _tcpListener.Start();
                _listening = true;
                Logger.Log($"[{DateTime.Now.ToString()}] TCP server started on port {tcpPort}.");

                Task.Run(ListenForClients);
            }

            if (_serialPort.IsOpen && _listening)
            {
                try
                {
                    lblCOMStatus.Text = "Connected";
                    lblClients.Text = "Clients: " + _connectedClients.Count.ToString();
                    panel1.Enabled = false;
                    panel2.Enabled = false;
                    btConnect.Enabled = false;
                    btDisconnect.Enabled = true;

                    Config.AppSettings.Settings["COMPort"].Value = cbxCOM.Text;
                    Config.AppSettings.Settings["BaudRate"].Value = cbxBaudrate.Text;
                    Config.AppSettings.Settings["DataBit"].Value = cbxDataBit.Text;
                    Config.AppSettings.Settings["Parity"].Value = cbxParity.Text;
                    Config.AppSettings.Settings["StopBits"].Value = cbxStopBit.Text;
                    Config.AppSettings.Settings["TCPPort"].Value = tbxPort.Text;
                    Config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");

                    SafeUI(() =>
                    {
                        lblSaveStatus.Text = "Save successful.";
                    });
                    Logger.Log($"[{DateTime.Now.ToString()}] Configuration saved.");
                }
                catch (Exception ex)
                {
                    SafeUI(() =>
                    {
                        lblSaveStatus.Text = $"[{DateTime.Now.ToString()}] Error: {ex.Message}";
                    });
                    Logger.Log($"[{DateTime.Now.ToString()}] Error: {ex.Message}");
                }

            }
        }
        private void btDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                _listening = false;
                _timer.Stop(); // Stop the timer to prevent it firing after disconnect
                _serialPort.Close();

                // Null-guard: _tcpListener may be null if Connect never fully succeeded
                _tcpListener?.Stop();

                // Take a snapshot under lock, then close each client outside the lock
                List<TcpClient> clientsToClose;
                lock (_clientsLock)
                {
                    clientsToClose = _connectedClients.ToList();
                    _connectedClients.Clear();
                }

                foreach (var client in clientsToClose)
                {
                    client.Close();
                }

                lblClients.Text = "Clients: 0";

                Logger.Log($"[{DateTime.Now.ToString()}] Serial port {cbxCOM.Text} closed.");
                Logger.Log($"[{DateTime.Now.ToString()}] TCP server stopped.");

                if (_serialPort.IsOpen)
                {
                    lblCOMStatus.Text = "Connected";
                    panel1.Enabled = false;
                    panel2.Enabled = false;
                    btConnect.Enabled = false;
                    btDisconnect.Enabled = true;
                }
                else
                {
                    lblCOMStatus.Text = "Disconnected";
                    panel1.Enabled = true;
                    panel2.Enabled = true;
                    btConnect.Enabled = true;
                    btDisconnect.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                SafeUI(() =>
                {
                    lblSaveStatus.Text = $"[{DateTime.Now.ToString()}] Error: {ex.Message}";
                });
                Logger.Log($"[{DateTime.Now.ToString()}] Error: {ex.Message}");
            }
        }
        private void btRefresh_Click(object sender, EventArgs e)
        {
            //Get available port list from PC
            RefreshPortList(_serialPort, cbxCOM);
        }
        public void RefreshPortList(SerialPort serialPort, ComboBox comboBox)
        {
            string[] ports = SerialPort.GetPortNames();

            // Clear the existing items in the ComboBox
            comboBox.Items.Clear();
            // Populate the ComboBox with the available COM ports
            comboBox.Items.AddRange(ports);
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.Hide();
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // Cancel the default close behavior
                this.WindowState = FormWindowState.Minimized; // Minimize the form
            }
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }
        public async Task ListenForClients()
        {
            try
            {
                while (_listening)
                {
                    TcpClient client = await _tcpListener.AcceptTcpClientAsync(); //Wait for a client to connect
                    lock (_clientsLock)
                    {
                        _connectedClients.Add(client);
                    }

                    SafeUI(() =>
                    {
                        lblClients.Text = "Clients: " + _connectedClients.Count.ToString();
                    });

                    _ = Task.Run(() => HandleClient(client));
                }
            }
            catch (SocketException ex)
            {
                // If we stopped intentionally, this exception is expected — not a real error
                if (!_listening) return;
                SafeUI(() =>
                {
                    lblStatusContent.Text = $"[{DateTime.Now.ToString()}] Error SocketException: {ex.Message}";
                    Logger.Log($"[{DateTime.Now.ToString()}] Error SocketException: {ex.Message}");
                });
            }
            catch (Exception ex)
            {
                // If we stopped intentionally, this exception is expected — not a real error
                if (!_listening) return;
                SafeUI(() =>
                {
                    lblStatusContent.Text = $"[{DateTime.Now.ToString()}] Error ListenForClients failed: {ex.Message}";
                    Logger.Log($"[{DateTime.Now.ToString()}] Error ListenForClients failed: {ex.Message}");
                });
            }
        }
        public async Task HandleClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                try
                {
                    SafeUI(() =>
                    {
                        if (_serialPort.IsOpen) lblCOMStatus.Text = "Connected";
                        else lblCOMStatus.Text = "Disconnected";
                    });

                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        string data = BitConverter.ToString(buffer, 0, bytesRead).Replace("-", " ");

                        if (data == "73 74 6F 70") //"stop" ASCII: 0x73 0x74 0x6F 0x70
                        {
                            SafeUI(() =>
                            {
                                btDisconnect_Click(null, null);
                            });
                            break;
                        }

                        SafeUI(() =>
                        {
                            txtTCPReceived.AppendText($"({data}) ");
                        });

                        // Log received TCP data
                        //Logger.Log($"TCP Received: {data}");

                        if (_serialPort != null && _serialPort.IsOpen)
                        {
                            try
                            {
                                // Lock so multiple TCP clients cannot interleave bytes on the RS-485 bus
                                lock (_serialPortWriteLock)
                                {
                                    _serialPort.Write(buffer, 0, bytesRead);
                                }
                            }
                            catch (Exception writeEx)
                            {
                                SafeUI(() =>
                                {
                                    lblStatusContent.Text = $"[{DateTime.Now}] COM write error: {writeEx.Message}";
                                    Logger.Log($"[{DateTime.Now}] COM write error: {writeEx.Message}");
                                });
                            }
                        }
                        else
                        {
                            if (_reconnectTask.IsCompleted)
                            {
                                _reconnectTask = TryReconnectSerialPort();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // If we stopped intentionally, the ReadAsync abort is expected — not a real error
                    if (!_listening) return;
                    SafeUI(() =>
                    {
                        lblStatusContent.Text = $"[{DateTime.Now.ToString()}] Error: {ex.Message}";
                        Logger.Log($"[{DateTime.Now.ToString()}] Error: {ex.Message}");
                    });
                }
            }

            // After finishing with a client:
            lock (_clientsLock)
            {
                _connectedClients.Remove(client);
            }
            client.Close();
            SafeUI(() => lblClients.Text = "Clients: " + _connectedClients.Count);
        }
        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {

                int bytesToRead = _serialPort.BytesToRead;

                // Guard: some drivers fire DataReceived with 0 bytes — skip to avoid empty timer resets
                if (bytesToRead <= 0) return;

                byte[] buffer = new byte[bytesToRead];
                // Read() may return fewer bytes than bytesToRead — use the actual count
                // to avoid adding zero-padded garbage that corrupts Modbus RTU frames
                int actualRead = _serialPort.Read(buffer, 0, bytesToRead);
                if (actualRead <= 0) return;

                // Add only the bytes actually read (lock to sync with OnTimedEvent)
                lock (_bufferLock)
                {
                    _buffer.AddRange(buffer.Take(actualRead));
                }

                // Reset and start the timer
                _timer.Stop();
                _timer.Start();
            }
            catch (Exception ex)
            {
                SafeUI(() =>
                {
                    lblStatusContent.Text = $"[{DateTime.Now.ToString()}] Error DataReceivedHandler: {ex.Message}";
                    Logger.Log($"[{DateTime.Now.ToString()}] Error DataReceivedHandler: {ex.Message}");
                });
            }
        }
        public void OnTimedEvent(System.Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (_buffer == null)
                {
                    return;
                }
                // Assume the message is complete if no new data received within the interval
                byte[] completeMessage = null;
                lock (_bufferLock)
                {
                    if (_buffer.Count > 0)
                    {
                        completeMessage = _buffer.ToArray();
                        _buffer.Clear();
                    }
                }

                if (completeMessage != null)
                {
                    try
                    {

                        string data = BitConverter.ToString(completeMessage).Replace("-", " ");
                        SafeUI(() =>
                        {
                            txtCOMReceived.AppendText($"({data}) ");
                        });

                        // Create a snapshot to avoid collection modification
                        List<TcpClient> clientsCopy;
                        lock (_clientsLock)
                        {
                            // Create a copy so you can iterate outside the lock
                            clientsCopy = _connectedClients.ToList();
                        }
                        foreach (var client in clientsCopy)
                        {
                            if (client != null)
                            {
                                try
                                {
                                    if (client.Connected)
                                    {
                                        // Send the complete message to the connected client
                                        NetworkStream stream = client.GetStream();
                                        stream.WriteTimeout = 1000; // 1 second timeout
                                        stream.Write(completeMessage, 0, completeMessage.Length);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    lock (_clientsLock)
                                    {
                                        _connectedClients.Remove(client); // Remove from original list under lock
                                    }
                                    client.Close();
                                    SafeUI(() =>
                                    {
                                        lblStatusContent.Text = $"[{DateTime.Now.ToString()}] Error: {ex.Message}";
                                        Logger.Log($"[{DateTime.Now.ToString()}] Error: {ex.Message}");
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SafeUI(() =>
                        {
                            lblStatusContent.Text = $"[{DateTime.Now.ToString()}] Error processing data: {ex.Message}";
                            Logger.Log($"[{DateTime.Now.ToString()}] Error processing data: {ex.Message}");
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"[{DateTime.Now.ToString()}] OnTimedEvent Error: {ex.Message}");
                SafeUI(() =>
                    lblStatusContent.Text = $"[{DateTime.Now.ToString()}] OnTimedEvent Error: {ex.Message}"
                );
            }
        }
        private async Task TryReconnectSerialPort()
        {
            int retryAttempts = 2; // Number of attempts
            while (retryAttempts > 0)
            {
                try
                {
                    await Task.Delay(5000); // Wait for 5 seconds
                                            // Check if object was disposed
                    if (_serialPort == null)
                    {
                        Logger.Log("SerialPort was disposed, cannot reconnect");
                        return;
                    }

                    _serialPort.Open();
                    if (_serialPort.IsOpen)
                    {
                        SafeUI(() =>
                        {
                            lblCOMStatus.Text = "Reconnected";
                            lblStatusContent.Text = $"[{DateTime.Now}] Successfully reconnected to COM port";
                            Logger.Log($"[{DateTime.Now.ToString()}] Successfully reconnected to COM port");
                        });
                        return;
                    }
                }
                catch (ObjectDisposedException)
                {
                    Logger.Log("SerialPort disposed during reconnection attempt");
                    return; // Exit gracefully
                }
                catch (Exception ex)
                {
                    SafeUI(() =>
                    {
                        lblStatusContent.Text = $"[{DateTime.Now}] Reconnection attempt failed: {ex.Message}";
                        Logger.Log($"[{DateTime.Now.ToString()}] Error: Reconnection attempt failed: {ex.Message}");
                    });
                }
                retryAttempts--;
            }

            SafeUI(() =>
            {
                lblCOMStatus.Text = "Disconnected";
                lblStatusContent.Text = $"[{DateTime.Now}] Could not reconnect to COM port after several attempts";
                Logger.Log($"[{DateTime.Now.ToString()}] Error: Could not reconnect to COM port after several attempts");
            });
        }

        private void SafeUI(Action action)
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(action);
                }
                catch
                {
                    // Swallow
                }
            }
            else
            {
                action();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Clear the text boxes to release memory
            txtCOMReceived.Text = "";
            txtTCPReceived.Text = "";

            //Logger.Log($"[{DateTime.Now.ToString()}] Clearing text boxes to release memory");
        }

    }
}
