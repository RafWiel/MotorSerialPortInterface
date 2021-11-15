using Communication.Serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortTestConsole
{
    public class MotorInterface
    {
        #region Initialization

        private SerialConnection _serial = new SerialConnection();

        public MotorInterface()
        {
            _serial.DataReceived += _serial_DataReceived;
        }

        #endregion

        #region Events

        private void _serial_DataReceived(string data)
        {
            Console.WriteLine(data);
        }

        #endregion

        #region Methods

        public void SendData()
        {
            try
            {
                _serial.OpenPort("COM6", 9600);

                _serial.Send("123");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

    }
}
