namespace Watchdog.Examples.Views
{
    using System.Windows.Controls;
    using Watchdog.Examples.ViewModels;

    /// <summary>
    ///     Interaction logic for PasswordCreation.xaml
    /// </summary>
    public partial class PasswordCreation : UserControl
    {
        public PasswordCreation()
        {
            this.InitializeComponent();
            this.DataContext = new PasswordViewModel();
        }
    }
}