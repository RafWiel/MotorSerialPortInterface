using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communication.Serial
{
    public class SerialConnection
    {
        #region Initialization

        public delegate void DataReceivedDelegate(string data);
        public event DataReceivedDelegate DataReceived;
        
        private SerialPort _serialPort;       

        #endregion

        #region Properties

        public bool IsOpen
        { 
            get => _serialPort != null ? _serialPort.IsOpen : false;            
        }               

        #endregion

        #region Methods

        public void OpenPort(string port, int baudRate)
        {
            try
            {                
                if (_serialPort != null)
                    ClosePort();
                
                _serialPort = new SerialPort();

                _serialPort.PortName = port;
                _serialPort.BaudRate = baudRate;
                _serialPort.DataBits = 8;
                _serialPort.Parity = Parity.None;
                _serialPort.StopBits = StopBits.One;
                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;

                _serialPort.Open();
                
                if (_serialPort.IsOpen == false)
                    return;

                //cleanup in case of trash in buffers
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();

                byte[] buffer = new byte[1024];
                Action readAction = null;
                
                //init receiving loop
                readAction = () =>
                {
                    try
                    {
                        if (_serialPort == null || _serialPort.IsOpen == false)
                            return;

                        _serialPort.BaseStream.BeginRead(buffer, 0, buffer.Length, delegate(IAsyncResult ar)
                        {
                            if (_serialPort == null || _serialPort.IsOpen == false)
                                return;

                            int length = _serialPort.BaseStream.EndRead(ar);
                            byte[] data = new byte[length];
                            Buffer.BlockCopy(buffer, 0, data, 0, length);
                            
                            if (DataReceived != null)
                                DataReceived(System.Text.Encoding.UTF8.GetString(data, 0, length));
                            
                            readAction();
                        }, null);
                    }
                    catch { }
                };

                readAction();
            }
            catch (Exception ex)
            {                
                Console.WriteLine(ex.ToString());
            }            
        }

        public void ClosePort()
        {
            try
            {
                if (_serialPort == null || _serialPort.IsOpen == false)
                    return;

                _serialPort.Close();

                //small delay, so port can actually close
                Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (_serialPort != null)
                    _serialPort.Dispose();

                _serialPort = null;
            }
        }                

        public void Send(string data)
        {
            try
            {
                _serialPort.Write(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion
    }
}
