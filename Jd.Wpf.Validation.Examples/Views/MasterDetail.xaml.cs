﻿namespace Jd.Wpf.Validation.Examples.Views
{
    using System.Windows.Controls;
    using Jd.Wpf.Validation.Examples.ViewModels;

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