using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SerialPortTestGuiApp.Commands
{
    public static class RoutedUICommands
    {        
        public static readonly RoutedUICommand GetLsFirmware = new RoutedUICommand("GetLsFirmware", "GetLsFirmware", typeof(RoutedUICommands), new InputGestureCollection() 
        { 
            new KeyGesture(Key.None, ModifierKeys.None) 
        });

        public static readonly RoutedUICommand GetHsFirmware = new RoutedUICommand("GetHsFirmware", "GetHsFirmware", typeof(RoutedUICommands), new InputGestureCollection()
        {
            new KeyGesture(Key.None, ModifierKeys.None)
        });
    }
}

            