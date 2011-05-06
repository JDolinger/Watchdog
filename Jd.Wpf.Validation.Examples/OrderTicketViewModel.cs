namespace Jd.Wpf.Validation.Examples
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Jd.Wpf.Validation.ClientUtil;

    public class OrderTicketViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<IError> validationErrors;
        private string side;
        private int quantity;
        private string symbol;
        private decimal price;

        private int currentPosition;

        public OrderTicketViewModel()
        {
            this.validationErrors = new ObservableCollection<IError>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Side
        {
            get { return this.side; }
            set
            {
                this.side = value;
                this.RaisePropertyChanged("Side");
            }
        }

        public int Quantity
        {
            get { return this.quantity; }
            set
            {
                this.quantity = value;
                this.RaisePropertyChanged("Quantity");
            }
        }

        public string Symbol
        {
            get { return this.symbol; }
            set
            {
                this.symbol = value;
                this.RaisePropertyChanged("Symbol");
            }
        }

        public decimal Price
        {
            get { return this.price; }
            set
            {
                this.price = value;
                this.RaisePropertyChanged("Price");
                this.ValidatePrice();
            }
        }

        public int CurrentPosition
        {
            get { return this.currentPosition; }
            set
            {
                this.currentPosition = value;
                this.RaisePropertyChanged("CurrentPosition");
            }
        }

        public ObservableCollection<IError> ValidationErrors
        {
            get { return this.validationErrors; }
        }

        protected virtual void RaisePropertyChanged(string name)
        {
            var h = this.PropertyChanged;
            if (h != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void ValidatePrice()
        {
            if (this.Price > 100)
            {
                this.validationErrors.Add("Price", "Price should be less than 100");
            } 
            else
            {
                this.validationErrors.ClearField("Price");
            }
        }
    }
}