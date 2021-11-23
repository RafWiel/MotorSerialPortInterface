using gTools.Log;
using gTools.WPF;
using SerialPortTestShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SerialPortTestGuiApp
{
    public class Motor : gBindableBase
    {
        #region Properties

        public Window Window { get; set; }

        private string _lsFirmware;
        public string LsFirmware
        {
            get => _lsFirmware;
            set => Set(ref _lsFirmware, value);
        }

        private string _hsFirmware;
        public string HsFirmware
        {
            get => _hsFirmware;
            set => Set(ref _hsFirmware, value);
        }

        public string Port
        {
            get => App.Config.Port;
            set
            {
                if (App.Config.Port == value)
                    return;

                App.Config.Port = value;
                App.Config.Save();
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();                
            }
        }

        public static List<string> SerialPorts
        {
            get => new List<string>(SerialPort.GetPortNames());            
        }

        public static List<int> BaudRates
        {
            get => new List<int>(new int[]
            {
                300,
                1200,
                2400,
                4800,
                9600,
                14400,
                19200,
                28800,
                38400,
                57600,
                115200,
                230400
            });
        }

        #endregion

        #region Methods

        public void GetLsMicroFirmware()
        {
            RunCommandTask(Commands.LS);
        }

        public void GetHsMicroFirmware()
        {
            RunCommandTask(Commands.HS);
        }

        private void RunCommandTask(Commands command)
        {
            if (string.IsNullOrEmpty(Port))
            {
                MessageBox.Show(Window, "COM port not set", Window.Title);
                return;
            }

            if (BaudRate == 0)
            {
                MessageBox.Show(Window, "Baud Rate not set", Window.Title);
                return;
            }

            Task.Run(() =>
            {                
                RunCommand(String.Format("port:{0} baud:{1} command:{2}", Port, BaudRate, command));                
            });
        }

        private string RunCommand(string command)
        {
            string result = string.Empty;
            
            SetMouseCursor(Cursors.Wait);

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
            finally
            {
                SetMouseCursor(null);
            }            

            return result;
        }

        private void HandleResponse(int result, string json)
        {
            if (result == ErrorCodes.Success)
            {
                var response = ResponseManager.GetFirmware(json);
                if (response == null)
                {
                    InvokeMessageBox("Incorrect command response");
                    return;
                }

                //update Gui
                if (response.Command == (int)Commands.LS)
                    LsFirmware = response.Firmware;

                if (response.Command == (int)Commands.HS)
                    HsFirmware = response.Firmware;
            }
            else
            {
                //show error message
                var errors = ResponseManager.GetErrors(json);
                if (errors.Count == 0)
                {
                    InvokeMessageBox("Incorrect command response");
                    return;
                }

                string message = string.Format("{0} occured:\r\n{1}",
                    errors.Count == 1 ? "Error" : "Errors",
                    string.Join("\r\n", errors.Select(u => u.ErrorMessage)));

                InvokeMessageBox(message);                
            }            
        }

        private void InvokeMessageBox(string message)
        {
            Window.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(Window, message, Window.Title);
            });
        }

        private void SetMouseCursor(Cursor cursor)
        {
            Window.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = cursor;
            });
        }

        #endregion
    }
}
