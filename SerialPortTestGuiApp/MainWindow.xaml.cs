﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SerialPortTestGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Initialization

        private Motor _motorInterface;

        public MainWindow()
        {
            InitializeComponent();

            _motorInterface = new Motor { Window = this };
            this.DataContext = _motorInterface;
        }

        #endregion

        #region Events

        private void ReadLsButton_Click(object sender, RoutedEventArgs e)
        {
            _motorInterface.GetLsMicroFirmware();
        }

        private void ReadHsButton_Click(object sender, RoutedEventArgs e)
        {
            _motorInterface.GetHsMicroFirmware();
        }

        #endregion
    }
}
