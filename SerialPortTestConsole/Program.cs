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
            var motor = new MotorInterface();

            motor.SendData();
            Console.ReadLine();
        }
    }
}
