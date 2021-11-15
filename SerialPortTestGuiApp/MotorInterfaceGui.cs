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

        #endregion

        #region Methods

        public void RunCommand()
        {
            Task.Run(() =>
            {
                string output = string.Empty;

                SetMouseCursor(Cursors.Wait);

                try
                {                    
                    //run console application
                    using (var process = new Process())
                    {
                        process.StartInfo = new ProcessStartInfo
                        {
                            FileName = "SerialPortTestConsole.exe",
                            Arguments = "p:COM6 f:ls",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        };

                        process.Start();
                        process.WaitForExit();

                        //read output
                        output = process.StandardOutput.ReadToEnd();

                        ParseOutput(output);
                        Console.Write(output);
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
            });
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

            if (output.StartsWith("Result: "))
                LsFirmware = output.Replace("Result: ", string.Empty).Trim();            
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
