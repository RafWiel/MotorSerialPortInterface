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
            if (MotorInterface.ValidateArgs(args) == false)
            {
                Console.WriteLine("Error: Incorrect parameters");
                return;
            }

            using (var motorInterface = new MotorInterface())
            {                
                motorInterface.Run(args);                
            }

            //Console.ReadKey();
        }
    }
}
