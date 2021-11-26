using SerialPortTestGuiApp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SerialPortTestGuiApp.Views
{
    /// <summary>
    /// Interaction logic for MotorView.xaml
    /// </summary>
    public partial class InputView : UserControl
    {
        private InputViewModel _viewModel = new InputViewModel();

        public MotorViewModel MotorViewModel
        {
            get => _viewModel.MotorViewModel;
            set => _viewModel.MotorViewModel = value;
        }

        public InputView()
        {
            InitializeComponent();

            this.DataContext = _viewModel;
            _viewModel.Window = Application.Current.MainWindow;
        }

        private void GetLsFirmware_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModel.GetLsFirmware();
        }

        private void GetHsFirmware_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModel.GetHsFirmware();
        }
    }
}
