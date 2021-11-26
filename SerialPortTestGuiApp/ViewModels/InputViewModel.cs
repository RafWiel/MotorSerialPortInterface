using gTools.WPF;
using SerialPortTestGuiApp.Models;
using SerialPortTestShared;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;
using System.Windows.Input;

namespace SerialPortTestGuiApp.ViewModels
{
    public class InputViewModel : gBindableBase
    {
        #region Initialization

        public Window Window { get; set; }        

        private readonly InputModel _model = new InputModel();        

        public string Port
        {
            get => _model.Port;
            set
            {
                if (_model.Port == value)
                    return;

                _model.Port = value;
                NotifyPropertyChanged();
            }
        }

        public int BaudRate
        {
            get => _model.BaudRate;
            set
            {
                if (_model.BaudRate == value)
                    return;

                _model.BaudRate = value;
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

        public MotorViewModel MotorViewModel { get; set; }
        
        public InputViewModel()
        {
            _model.ShowMessage += (message) =>
            {
                Window.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show(Window, message, Window.Title);
                });
            };

            _model.FirmwareReceived += (command, firmware) =>
            {
                Window.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;                    
                });

                //update GUI
                if (command == MotorCommands.LS)
                    MotorViewModel.LsFirmware = firmware;

                if (command == MotorCommands.HS)
                    MotorViewModel.HsFirmware = firmware;
            };
        }

        #endregion

        #region Methods

        public void GetLsFirmware()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            MotorViewModel.LsFirmware = string.Empty;

            _model.GetFirmware(MotorCommands.LS);            
        }

        public void GetHsFirmware()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            MotorViewModel.HsFirmware = string.Empty;

            _model.GetFirmware(MotorCommands.HS);
        }              

        #endregion
    }
}
