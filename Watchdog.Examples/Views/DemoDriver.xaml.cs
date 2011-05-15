namespace Watchdog.Examples.Views
{
    using System;
    using System.Windows;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DemoDriver : Window
    {
        public DemoDriver()
        {
            this.InitializeComponent();
            // this.DataContext = new OrderTicketViewModel();
        }

        ~DemoDriver()
        {
            Console.WriteLine("GCing Demo");    
        }
    }
}