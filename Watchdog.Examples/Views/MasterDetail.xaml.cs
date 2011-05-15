namespace Watchdog.Examples.Views
{
    using System.Windows.Controls;
    using Watchdog.Examples.ViewModels;

    /// <summary>
    ///     Interaction logic for MasterDetail.xaml
    /// </summary>
    public partial class MasterDetail : UserControl
    {
        public MasterDetail()
        {
            this.InitializeComponent();
            this.DataContext = new MasterDetailViewModel();
        }
    }
}