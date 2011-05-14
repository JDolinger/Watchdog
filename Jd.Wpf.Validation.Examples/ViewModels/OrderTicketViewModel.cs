namespace Jd.Wpf.Validation.Examples
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Input;
    using Jd.Wpf.Validation.ClientUtil;
    using Jd.Wpf.Validation.Examples.Util;

    public class OrderTicketViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<IError> validationErrors;
        private readonly ICommand bookTicketCommand;
        private string side;
        private int quantity;
        private string symbol;
        private decimal price;
        private decimal total;

        private int currentPosition;

        public OrderTicketViewModel()
        {
            this.bookTicketCommand = new DelegateCommand<object>(
                x => this.CanBook(),
                x =>
                {
                    /* nothing here for demo */
                });

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
                this.CalculateTotal();
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
                this.CalculateTotal();
                this.ValidatePrice();
            }
        }

        public decimal Total
        {
            get { return this.total; }
            set
            {
                this.total = value;
                this.RaisePropertyChanged("Total");
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

        public ICommand BookTicketCommand
        {
            get { return this.bookTicketCommand; }
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
                this.validationErrors.ClearValidationError("Price");
            }
        }

        private void CalculateTotal()
        {
            this.Total = this.Price * this.Quantity;
        }

        private bool CanBook()
        {
            return this.ValidationErrors.Count == 0;
        }
    }

    public class Tet : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

}