using gTools.Log;
using gTools.WPF;
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
    public class MotorInterfaceGui : gBindableBase
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
            RunCommandTask("LS");
        }

        public void GetHsMicroFirmware()
        {
            RunCommandTask("HS");
        }

        private void RunCommandTask(string command)
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
                var result = RunCommand(String.Format("port:{0} baud:{1} command:{2}", Port, BaudRate, command));
                ParseOutput(result);
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
                        
                    Console.Write(result);
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

        private void ParseOutput(string output)
        {
            if (output.StartsWith("Error: "))
            {
                //inform user
                Window.Dispatcher.Invoke(() => 
                {
                    MessageBox.Show(Window, output.Trim(), Window.Title);
                });
                
                return;
            }

            //update Gui
            if (output.StartsWith("LS: "))
                LsFirmware = output.Replace("LS: ", string.Empty).Trim();

            if (output.StartsWith("HS: "))
                HsFirmware = output.Replace("HS: ", string.Empty).Trim();
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
