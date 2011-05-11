namespace Jd.Wpf.Validation.Examples.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Jd.Wpf.Validation.Examples.Util;

    public class MasterDetailViewModel
    {
        private readonly ObservableCollection<OrderTicketViewModel> allOrders;
        private readonly ICommand newOrderCommand;

        public MasterDetailViewModel()
        {
            this.allOrders = new ObservableCollection<OrderTicketViewModel>();
            this.newOrderCommand = new DelegateCommand<object>(x => true, x => this.CreateNewOrder());
            this.CreateNewOrder();
        }

        public ICommand NewOrderCommand
        {
            get { return this.newOrderCommand; }
        }

        public ObservableCollection<OrderTicketViewModel> AllOrders
        {
            get { return this.allOrders; }
        }

        private void CreateNewOrder()
        {
            this.AllOrders.Add(new OrderTicketViewModel());
        }
    }
}