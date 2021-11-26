using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortTestShared
{
    public static class ErrorCodes
    {
        public const int Success = 0;        
        public const int IncorrectParameters = 1;
        public const int UnknownPort = 2;
        public const int UnknownBaudRate = 3;
        public const int PortClosed = 4;
        public const int Timeout = 5;
        public const int UnsupportedCommand = 6;
        public const int Exception = 7;
    }
}
