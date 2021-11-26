using gTools.Log;
using SerialPortTestShared;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SerialPortTestGuiApp.Models
{
    public class InputModel
    {
        #region Initialization

        public delegate void ShowMessageDelegate(string message);
        public event ShowMessageDelegate ShowMessage;

        public delegate void FirmwareReceivedDelegate(MotorCommands command, string firmware);
        public event FirmwareReceivedDelegate FirmwareReceived;

        public string Port
        {
            get => App.Config.Port;
            set
            {
                if (App.Config.Port == value)
                    return;

                App.Config.Port = value;
                App.Config.Save();
            }
        }

        public int BaudRate
        {
            get => App.Config.BaudRate;
            set
            {
                if (App.Config.BaudRate == value)
                    return;

                App.Config.BaudRate = value;
                App.Config.Save();
            }
        }

        #endregion

        #region Methods

        public void GetFirmware(MotorCommands command)
        {            
            RunCommandTask(command);
        }

        private void RunCommandTask(MotorCommands command)
        {
            if (string.IsNullOrEmpty(Port))
            {
                ShowMessage?.Invoke("COM port not set");                
                return;
            }

            if (BaudRate == 0)
            {
                ShowMessage?.Invoke("Baud Rate not set");                
                return;
            }

            Task.Run(() =>
            {
                RunCommand(String.Format("port:{0} baud:{1} command:{2}", Port, BaudRate, command));
            });
        }

        private void RunCommand(string command)
        {
            string result = string.Empty;            

            try
            {
                //run console application
                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "SerialPortTestConsole.exe",
                        Arguments = command,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    };

                    process.Start();
                    process.WaitForExit();

                    //read output
                    result = process.StandardOutput.ReadToEnd();

                    HandleResponse(process.ExitCode, result);
                }
            }
            catch (Exception ex)
            {
                gLog.Write(ex.ToString());
            }            
        }

        private void HandleResponse(int result, string json)
        {
            if (result == ErrorCodes.Success)
            {
                var response = ResponseManager.GetFirmware(json);
                if (response == null)
                {
                    ShowMessage?.Invoke("Incorrect command response");
                    return;
                }
               
                //update GUI
                FirmwareReceived?.Invoke((MotorCommands)response.Command, response.Firmware);                
            }
            else
            {
                //show error message
                var errors = ResponseManager.GetErrors(json);
                if (errors.Count == 0)
                {
                    ShowMessage?.Invoke("Incorrect command response");
                    return;
                }

                string message = string.Format("{0} occured:\r\n{1}",
                    errors.Count == 1 ? "Error" : "Errors",
                    string.Join("\r\n", errors.Select(u => u.ErrorMessage)));

                ShowMessage?.Invoke(message);
            }
        }

        #endregion
    }
}
