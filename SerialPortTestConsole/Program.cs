using Communication.Serial;
using SerialPortTestShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SerialPortTestConsole
{
    public class Program
    {
        static int Main(string[] args)
        {
            int result = ErrorCodes.Success;

            if (ValidateArgs(args) == false)
            {
                Console.WriteLine("Error: Incorrect parameters");
                return ErrorCodes.IncorrectParameters;
            }

            using (var motorInterface = new Motor())
            {
                result = motorInterface.Run(args);                
            }

            return result;
        }

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
            if (Regex.IsMatch(port, @"^COM\d+$") == false)
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
            if (Regex.IsMatch(command, @"^LS$|^HS$") == false)
                return false;

            return true;
        }
    }
}
