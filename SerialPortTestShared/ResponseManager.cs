using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SerialPortTestShared
{
    public class ResponseError
    {
        public int Result { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ResponseFirmware
    {
        public int Command { get; set; }
        public string Firmware { get; set; }        
    }

    public class ResponseManager
    {
        public static void SendError(int errorCode, string format, params object[] args)
        {
            var response = new ResponseError
            {
                Result = errorCode,
                ErrorMessage = String.Format(format, args)
            };
            
            //output json
            Console.WriteLine(new JavaScriptSerializer().Serialize(response));            
        }

        public static void SendFirmware(int command, string firmware)
        {
            var response = new ResponseFirmware
            {
                Command = command,
                Firmware = firmware
            };

            //output json
            Console.WriteLine(new JavaScriptSerializer().Serialize(response));
        }

        public static List<ResponseError> GetErrors(string json)
        {            
            var result = new List<ResponseError>();            

            try
            {
                //there may be more than 1 errror, split output by newline
                foreach (var item in json.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var x = new JavaScriptSerializer().Deserialize<ResponseError>(item);
                    if (x != null && string.IsNullOrWhiteSpace(x.ErrorMessage) == false)
                        result.Add(x);
                }
            }
            catch
            {
                return result;
            }

            return result;
        }

        public static ResponseFirmware GetFirmware(string json)
        {
            try
            {
                return new JavaScriptSerializer().Deserialize<ResponseFirmware>(json);
            }
            catch
            {
                return null;
            }            
        }
    }
}
