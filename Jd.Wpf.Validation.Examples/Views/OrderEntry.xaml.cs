namespace Jd.Wpf.Validation.Examples.Views
{
    using System.Windows.Controls;
    using Jd.Wpf.Validation.Examples.ViewModels;

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