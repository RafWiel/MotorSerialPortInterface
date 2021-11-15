using gTools.Log;
using gTools.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        #endregion

        #region Methods

        public void GetLsMicroFirmware()
        {
            Task.Run(() =>
            {
                var result = RunCommand("port:com6 baud:9600 command:ls");
                ParseOutput(result);
            });            
        }

        public void GetHsMicroFirmware()
        {
            Task.Run(() =>
            {
                var result = RunCommand("port:com6 baud:9600 command:hs");
                ParseOutput(result);
            });
        }

        private string RunCommand(string args)
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
                        Arguments = args,
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
