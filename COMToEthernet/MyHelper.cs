using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMToEthernet
{
    public static class MyHelper
    {
        public static Parity GetParity(string parity)
        {
            return parity switch
            {
                "None" => Parity.None,
                "Odd" => Parity.Odd,
                "Even" => Parity.Even,
                "Mark" => Parity.Mark,
                _ => Parity.Space
            };
        }
        public static StopBits GetStopBits(string stopBits)
        {
            return stopBits == "1.5" ? StopBits.OnePointFive : (StopBits)Enum.Parse(typeof(StopBits), stopBits);
        }

    }
}
