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

                var response = motorInterface.GetLsMicroFirmware();
                Console.WriteLine("LS: {0}", response);

                response = motorInterface.GetHsMicroFirmware();
                Console.WriteLine("HS: {0}", response);
            }

            Console.ReadKey();
        }
    }
}
