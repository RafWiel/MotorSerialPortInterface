using Communication.Serial;
using SerialPortTestShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SerialPortTestConsole
{
    public class Motor : IDisposable
    {
        #region Initialization

        private SerialConnection _serial = new SerialConnection();        
        private string _response = string.Empty;
        private Commands _command;        

        public Motor()
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

        public int Run(string[] args)
        {
            try
            {
                var port = args[0].Replace("port:", string.Empty);
                var baud = Convert.ToInt32(args[1].Replace("baud:", string.Empty));
                var command = args[2].Replace("command:", string.Empty);

                //open port
                int result = Initialize(port, baud);
                if (result != ErrorCodes.Success)
                    return result;

                //run command
                if (command.Equals(Commands.LS.ToString()))
                    return GetLsMicroFirmware();

                if (command.Equals(Commands.HS.ToString()))
                    return GetHsMicroFirmware();

                return ErrorCodes.UnsupportedCommand;
            }
            catch (Exception ex)
            {
                ResponseManager.SendError(ErrorCodes.Exception, ex.Message);                
                return ErrorCodes.Exception;
            }
        }

        private int Initialize(string port, int baudRate)
        {
            //validate input
            if (String.IsNullOrWhiteSpace(port))
            {
                ResponseManager.SendError(ErrorCodes.UnknownPort, "COM port not set");
                return ErrorCodes.UnknownPort;
            }

            if (baudRate == 0)
            {
                ResponseManager.SendError(ErrorCodes.UnknownBaudRate, "BaudRate not set");
                return ErrorCodes.UnknownBaudRate;
            }
                            
            //open COM port
            _serial.OpenPort(port, baudRate);

            if (_serial.IsOpen == false)
            {
                ResponseManager.SendError(ErrorCodes.PortClosed, "COM port is closed");
                return ErrorCodes.PortClosed;
            }

            return ErrorCodes.Success;
        }

        private int GetLsMicroFirmware()
        {
            _command = Commands.LS;
            return RunCommand("v\n");            
        }

        private int GetHsMicroFirmware()
        {
            _command = Commands.HS;
            return RunCommand("V\n");
        }

        private int RunCommand(string command)
        {
            int result = SendCommand(command);
            if (result != ErrorCodes.Success)
                return result;

            //asynchronous communication, wait for reponse
            if (WaitForResponse() == false)
            {
                ResponseManager.SendError(ErrorCodes.Timeout, "Command timed out");
                return ErrorCodes.Timeout;
            }

            ResponseManager.SendFirmware((int)_command, _response);
            return ErrorCodes.Success;
        }

        private int SendCommand(string command)
        {
            if (_serial.IsOpen == false)
                return ErrorCodes.PortClosed;

            //reset response before send
            _response = string.Empty;

            _serial.Send(command);
            return ErrorCodes.Success;
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
    }
}
