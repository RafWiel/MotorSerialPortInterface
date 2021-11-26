using SerialPortTestGuiApp.ViewModels;
using System.Windows.Controls;

namespace SerialPortTestGuiApp.Views
{
    /// <summary>
    /// Interaction logic for MotorView.xaml
    /// </summary>
    public partial class MotorView : UserControl
    {
        public MotorViewModel ViewModel { get; private set; } = new MotorViewModel();

        public MotorView()
        {
            InitializeComponent();

            this.DataContext = ViewModel;            
        }
    }
}
