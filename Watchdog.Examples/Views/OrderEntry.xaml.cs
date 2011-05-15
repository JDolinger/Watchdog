namespace Watchdog.Examples.Views
{
    using System.Windows.Controls;
    using Watchdog.Examples.ViewModels;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class OrderEntry : UserControl
    {
        public OrderEntry()
        {
            this.InitializeComponent();
            this.DataContext = new OrderTicketViewModel(Parameters.SharedInstance);
        }
    }
}