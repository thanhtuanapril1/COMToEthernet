namespace COMToEthernet
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            panel1 = new Panel();
            cbxStopBit = new ComboBox();
            label5 = new Label();
            cbxParity = new ComboBox();
            label4 = new Label();
            cbxDataBit = new ComboBox();
            label3 = new Label();
            cbxBaudrate = new ComboBox();
            label2 = new Label();
            btRefresh = new Button();
            cbxCOM = new ComboBox();
            label1 = new Label();
            lblStatusContent = new Label();
            btConnect = new Button();
            btDisconnect = new Button();
            notifyIcon1 = new NotifyIcon(components);
            lblSaveStatus = new Label();
            tbxPort = new TextBox();
            label6 = new Label();
            panel2 = new Panel();
            label7 = new Label();
            panel3 = new Panel();
            lblCOMStatus = new Label();
            lblClients = new Label();
            label9 = new Label();
            txtTCPReceived = new TextBox();
            label8 = new Label();
            txtCOMReceived = new TextBox();
            pictureBox1 = new PictureBox();
            timer1 = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(cbxStopBit);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(cbxParity);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(cbxDataBit);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(cbxBaudrate);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(btRefresh);
            panel1.Controls.Add(cbxCOM);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(14, 14);
            panel1.Name = "panel1";
            panel1.Size = new Size(178, 264);
            panel1.TabIndex = 0;
            // 
            // cbxStopBit
            // 
            cbxStopBit.FormattingEnabled = true;
            cbxStopBit.Items.AddRange(new object[] { "0", "1", "1.5", "2" });
            cbxStopBit.Location = new Point(15, 225);
            cbxStopBit.Name = "cbxStopBit";
            cbxStopBit.Size = new Size(121, 23);
            cbxStopBit.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 207);
            label5.Name = "label5";
            label5.Size = new Size(48, 15);
            label5.TabIndex = 9;
            label5.Text = "Stop Bit";
            // 
            // cbxParity
            // 
            cbxParity.FormattingEnabled = true;
            cbxParity.Items.AddRange(new object[] { "None", "Odd", "Even", "Mark", "Space" });
            cbxParity.Location = new Point(15, 175);
            cbxParity.Name = "cbxParity";
            cbxParity.Size = new Size(121, 23);
            cbxParity.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 157);
            label4.Name = "label4";
            label4.Size = new Size(37, 15);
            label4.TabIndex = 7;
            label4.Text = "Parity";
            // 
            // cbxDataBit
            // 
            cbxDataBit.FormattingEnabled = true;
            cbxDataBit.Items.AddRange(new object[] { "7", "8" });
            cbxDataBit.Location = new Point(15, 125);
            cbxDataBit.Name = "cbxDataBit";
            cbxDataBit.Size = new Size(121, 23);
            cbxDataBit.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 107);
            label3.Name = "label3";
            label3.Size = new Size(48, 15);
            label3.TabIndex = 5;
            label3.Text = "Data Bit";
            // 
            // cbxBaudrate
            // 
            cbxBaudrate.FormattingEnabled = true;
            cbxBaudrate.Items.AddRange(new object[] { "110", "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "38400", "57600", "115200", "128000", "256000 " });
            cbxBaudrate.Location = new Point(15, 74);
            cbxBaudrate.Name = "cbxBaudrate";
            cbxBaudrate.Size = new Size(121, 23);
            cbxBaudrate.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 56);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 3;
            label2.Text = "Baudrate";
            // 
            // btRefresh
            // 
            btRefresh.Location = new Point(142, 27);
            btRefresh.Name = "btRefresh";
            btRefresh.Size = new Size(26, 23);
            btRefresh.TabIndex = 2;
            btRefresh.Text = "↻";
            btRefresh.UseVisualStyleBackColor = true;
            btRefresh.Click += btRefresh_Click;
            // 
            // cbxCOM
            // 
            cbxCOM.FormattingEnabled = true;
            cbxCOM.Location = new Point(15, 27);
            cbxCOM.Name = "cbxCOM";
            cbxCOM.Size = new Size(121, 23);
            cbxCOM.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 9);
            label1.Name = "label1";
            label1.Size = new Size(35, 15);
            label1.TabIndex = 0;
            label1.Text = "COM";
            // 
            // lblStatusContent
            // 
            lblStatusContent.AutoSize = true;
            lblStatusContent.Dock = DockStyle.Bottom;
            lblStatusContent.Location = new Point(0, 297);
            lblStatusContent.Name = "lblStatusContent";
            lblStatusContent.Size = new Size(66, 15);
            lblStatusContent.TabIndex = 14;
            lblStatusContent.Text = "Disconnect";
            // 
            // btConnect
            // 
            btConnect.Location = new Point(206, 209);
            btConnect.Name = "btConnect";
            btConnect.Size = new Size(108, 24);
            btConnect.TabIndex = 11;
            btConnect.Text = "Connect";
            btConnect.UseVisualStyleBackColor = true;
            btConnect.Click += btConnect_Click;
            // 
            // btDisconnect
            // 
            btDisconnect.Enabled = false;
            btDisconnect.Location = new Point(206, 239);
            btDisconnect.Name = "btDisconnect";
            btDisconnect.Size = new Size(108, 24);
            btDisconnect.TabIndex = 12;
            btDisconnect.Text = "Disconnect";
            btDisconnect.UseVisualStyleBackColor = true;
            btDisconnect.Click += btDisconnect_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "COMToEthernet";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // lblSaveStatus
            // 
            lblSaveStatus.AutoSize = true;
            lblSaveStatus.Dock = DockStyle.Top;
            lblSaveStatus.Location = new Point(0, 0);
            lblSaveStatus.Name = "lblSaveStatus";
            lblSaveStatus.Size = new Size(0, 15);
            lblSaveStatus.TabIndex = 15;
            // 
            // tbxPort
            // 
            tbxPort.Location = new Point(13, 27);
            tbxPort.Name = "tbxPort";
            tbxPort.Size = new Size(108, 23);
            tbxPort.TabIndex = 16;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(13, 9);
            label6.Name = "label6";
            label6.Size = new Size(52, 15);
            label6.TabIndex = 11;
            label6.Text = "TCP Port";
            // 
            // panel2
            // 
            panel2.Controls.Add(label6);
            panel2.Controls.Add(tbxPort);
            panel2.Location = new Point(193, 14);
            panel2.Name = "panel2";
            panel2.Size = new Size(131, 61);
            panel2.TabIndex = 16;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Bottom;
            label7.Location = new Point(0, 282);
            label7.Name = "label7";
            label7.Size = new Size(42, 15);
            label7.TabIndex = 17;
            label7.Text = "Status:";
            // 
            // panel3
            // 
            panel3.Controls.Add(lblCOMStatus);
            panel3.Controls.Add(lblClients);
            panel3.Controls.Add(label9);
            panel3.Controls.Add(txtTCPReceived);
            panel3.Controls.Add(label8);
            panel3.Controls.Add(txtCOMReceived);
            panel3.Location = new Point(330, 14);
            panel3.Name = "panel3";
            panel3.Size = new Size(370, 264);
            panel3.TabIndex = 18;
            // 
            // lblCOMStatus
            // 
            lblCOMStatus.AutoSize = true;
            lblCOMStatus.Location = new Point(288, 139);
            lblCOMStatus.Name = "lblCOMStatus";
            lblCOMStatus.Size = new Size(66, 15);
            lblCOMStatus.TabIndex = 4;
            lblCOMStatus.Text = "Disconnect";
            // 
            // lblClients
            // 
            lblClients.AutoSize = true;
            lblClients.Location = new Point(299, 10);
            lblClients.Name = "lblClients";
            lblClients.Size = new Size(55, 15);
            lblClients.TabIndex = 0;
            lblClients.Text = "Clients: 0";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(6, 10);
            label9.Name = "label9";
            label9.Size = new Size(109, 15);
            label9.TabIndex = 3;
            label9.Text = "Received from TCP ";
            // 
            // txtTCPReceived
            // 
            txtTCPReceived.Location = new Point(6, 28);
            txtTCPReceived.Multiline = true;
            txtTCPReceived.Name = "txtTCPReceived";
            txtTCPReceived.Size = new Size(357, 94);
            txtTCPReceived.TabIndex = 2;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 139);
            label8.Name = "label8";
            label8.Size = new Size(114, 15);
            label8.TabIndex = 1;
            label8.Text = "Received from COM";
            // 
            // txtCOMReceived
            // 
            txtCOMReceived.Location = new Point(6, 157);
            txtCOMReceived.Multiline = true;
            txtCOMReceived.Name = "txtCOMReceived";
            txtCOMReceived.Size = new Size(357, 92);
            txtCOMReceived.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.PigIconRemoveBg;
            pictureBox1.Location = new Point(223, 95);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(71, 73);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 19;
            pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            timer1.Interval = 900000;
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(705, 312);
            Controls.Add(pictureBox1);
            Controls.Add(panel3);
            Controls.Add(label7);
            Controls.Add(panel2);
            Controls.Add(lblStatusContent);
            Controls.Add(lblSaveStatus);
            Controls.Add(panel1);
            Controls.Add(btConnect);
            Controls.Add(btDisconnect);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "COMToEthernet";
            Load += Form1_Load;
            Resize += Form1_Resize;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private ComboBox cbxStopBit;
        private Label label5;
        private ComboBox cbxParity;
        private Label label4;
        private ComboBox cbxDataBit;
        private Label label3;
        private ComboBox cbxBaudrate;
        private Label label2;
        private Button btRefresh;
        private ComboBox cbxCOM;
        private Label label1;
        private Label lblStatusContent;
        private Button btDisconnect;
        private Button btConnect;
        private NotifyIcon notifyIcon1;
        private Label lblSaveStatus;
        private TextBox tbxPort;
        private Label label6;
        private Panel panel2;
        private Label label7;
        private Panel panel3;
        private Label label9;
        private TextBox txtTCPReceived;
        private Label label8;
        private TextBox txtCOMReceived;
        private Label lblClients;
        private Label lblCOMStatus;
        private PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
    }
}
