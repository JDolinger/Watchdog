namespace Jd.Wpf.Validation.Examples
{
    using System.Windows;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DemoDriver : Window
    {
        public DemoDriver()
        {
            this.InitializeComponent();
            this.DataContext = new OrderTicketViewModel();
        }
    }
}