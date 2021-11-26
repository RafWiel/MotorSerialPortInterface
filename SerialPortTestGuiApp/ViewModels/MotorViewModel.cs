using gTools.WPF;
using SerialPortTestGuiApp.Models;

namespace SerialPortTestGuiApp.ViewModels
{
    public class MotorViewModel : gBindableBase
    {
        private readonly MotorModel _model = new MotorModel();
  
        public string LsFirmware
        {
            get => _model.LsFirmware; 
            set
            {
                if (_model.LsFirmware == value)
                    return;

                _model.LsFirmware = value;
                NotifyPropertyChanged();
            }
        }

        public string HsFirmware
        {
            get => _model.HsFirmware;
            set
            {
                if (_model.HsFirmware == value)
                    return;

                _model.HsFirmware = value;
                NotifyPropertyChanged();
            }
        }
    }
}
