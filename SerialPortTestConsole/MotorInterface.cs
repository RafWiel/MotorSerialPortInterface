using Communication.Serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SerialPortTestConsole
{
    public class MotorInterface : IDisposable
    {
        #region Initialization

        private SerialConnection _serial = new SerialConnection();        
        private string _response = string.Empty;
        private CommandType _commandType;

        private enum CommandType
        {
            LS,
            HS
        }

        public MotorInterface()
        {              
            _serial.DataReceived += (data) => 
            {
                //simple interface, we assume that it can only respond with valid firmware number                
                _response = data;
            };
        }

        public void Dispose()
        {
            _serial.ClosePort();
        }

        #endregion               

        #region Methods        

        public static bool ValidateArgs(string[] args)
        {
            if (args.Length != 3)
                return false;

            if (args.Count(u => u.StartsWith("port:")) == 0)
                return false;

            if (args.Count(u => u.StartsWith("baud:")) == 0)
                return false;

            if (args.Count(u => u.StartsWith("command:")) == 0)
                return false;

            //expect comX
            var port = args[0].Replace("port:", string.Empty);
            if (Regex.IsMatch(port, @"^com\d+$") == false)
                return false;

            //expect valid baud rate
            var baudStr = args[1].Replace("baud:", string.Empty);
            int baud = 0;

            if (int.TryParse(baudStr, out baud) == false)
                return false;

            if (SerialConnection.BaudRates.Contains(baud) == false)
                return false;

            //expect ls or hs
            var command = args[2].Replace("command:", string.Empty);
            if (Regex.IsMatch(command, @"^ls$|^hs$") == false)
                return false;

            return true;
        }

        public void Run(string[] args)
        {
            try
            {
                var port = args[0].Replace("port:", string.Empty);
                var baud = Convert.ToInt32(args[1].Replace("baud:", string.Empty));
                var command = args[2].Replace("command:", string.Empty);

                //open port
                Initialize(port, baud);

                //run command
                switch (command)
                {
                    case "ls":
                        GetLsMicroFirmware();
                        break;
                    case "hs":
                        GetHsMicroFirmware();
                        break;
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }

        private void Initialize(string port, int baudRate)
        {
            //validate input
            if (String.IsNullOrWhiteSpace(port))
            {
                Console.WriteLine("Error: COM port not set");
                return;
            }

            if (baudRate == 0)
            {
                Console.WriteLine("Error: BaudRate not set");
                return;
            }
                            
            //open COM port
            _serial.OpenPort(port, baudRate);     
                
            if (_serial.IsOpen == false)                
                Console.WriteLine("Error: Port open failed");
        }

        private void GetLsMicroFirmware()
        {
            _commandType = CommandType.LS;
            RunCommand("v\n");            
        }

        private void GetHsMicroFirmware()
        {
            _commandType = CommandType.HS;
            RunCommand("V\n");
        }

        private void RunCommand(string command)
        {
            SendCommand(command);

            //asynchronous communication, wait for reponse
            if (WaitForResponse() == false)
                Console.WriteLine("Error: Command timed out");
            else
                Console.WriteLine("{0}: {1}", _commandType, _response);
        }

        private void SendCommand(string command)
        {
            if (_serial.IsOpen == false)
                return;

            //reset response before send
            _response = string.Empty;

            _serial.Send(command);                      
        }

        private bool WaitForResponse()
        {
            int counter = 0;

            //wait for response for 3 seconds
            while (String.IsNullOrEmpty(_response))
            {
                Thread.Sleep(100);

                if (++counter >= 30)
                    return false;
            }

            return true;
        }

        #endregion

        #region Tests

        #if DEBUG

        public bool DebugTest_WaitForResponse()
        {
            return WaitForResponse();
        }

        public void DebugTest_SetResponse()
        {
            _response = "OK";
        }

        #endif

        #endregion
    }
}
