using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Watchdog.Examples.Views
{
    /// <summary>
    /// Interaction logic for Launcher.xaml
    /// </summary>
    public partial class Launcher : Window
    {
        public Launcher()
        {
            InitializeComponent();
        }

        private void LaunchClick(object sender, RoutedEventArgs e)
        {
            new DemoDriver().Show();
        }

        private void GcClick(object sender, RoutedEventArgs e)
        {
            System.GC.Collect();
        }
    }
}
