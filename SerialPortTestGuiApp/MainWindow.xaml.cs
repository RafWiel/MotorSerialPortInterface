using System.Windows;

namespace SerialPortTestGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {
            InitializeComponent();

            InputView.MotorViewModel = MotorView.ViewModel;
        }
    }
}
