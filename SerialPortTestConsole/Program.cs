using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var motorInterface = new MotorInterface())
            {
                motorInterface.Initialize("COM6", 9600);
                motorInterface.GetLsMicroFirmware();                
            }

            //Console.ReadKey();
        }
    }
}
