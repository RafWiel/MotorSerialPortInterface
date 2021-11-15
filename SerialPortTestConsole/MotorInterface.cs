﻿using Communication.Serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SerialPortTestConsole
{
    public class MotorInterface : IDisposable
    {
        #region Initialization

        private SerialConnection _serial = new SerialConnection();
        private string _response = string.Empty;

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

        public void Initialize(string port, int baudRate)
        {
            //validate input
            if (String.IsNullOrWhiteSpace(port))
            {
                Console.WriteLine("COM port not set");
                return;
            }

            if (baudRate == 0)
            {
                Console.WriteLine("BaudRate not set");
                return;
            }
            
            Console.WriteLine("Opening port {0} at {1}", port, baudRate);
                
            //open COM port
            _serial.OpenPort(port, baudRate);     
                
            if (_serial.IsOpen)
                Console.WriteLine("Port opened sucessfully");
            else
                Console.WriteLine("Port open failed");
        }

        public string GetLsMicroFirmware()
        {
            return RunCommand("v\n");            
        }

        public string GetHsMicroFirmware()
        {
            return RunCommand("V\n");
        }

        private string RunCommand(string command)
        {
            SendCommand(command);

            //asynchronous communication, wait for reponse
            if (WaitForResponse() == false)
                Console.WriteLine("Command timed out");
            
            return _response;
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
