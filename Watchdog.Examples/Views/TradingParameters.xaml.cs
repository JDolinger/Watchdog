namespace Watchdog.Examples.Views
{
    using System.Windows.Controls;
    using Watchdog.Examples.ViewModels;

    /// <summary>
    ///     Interaction logic for TradingParameters.xaml
    /// </summary>
    public partial class TradingParameters : UserControl
    {
        public TradingParameters()
        {
            this.InitializeComponent();
            this.DataContext = Parameters.SharedInstance;
        }
    }
}