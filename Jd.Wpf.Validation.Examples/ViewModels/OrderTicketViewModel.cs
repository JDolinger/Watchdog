namespace Jd.Wpf.Validation.Examples
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Input;
    using Jd.Wpf.Validation.Examples.Util;
    using Jd.Wpf.Validation.Examples.ViewModels;
    using Watchdog.Validation.Core;
    using Watchdog.Validation.Core.ClientUtil;

    public class OrderTicketViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<IError> validationErrors;
        private readonly ICommand bookTicketCommand;
        private string side;
        private int quantity;
        private string symbol;
        private decimal price;
        private decimal total;

        private IParameters tradingParams;

        public OrderTicketViewModel(IParameters tradingParams)
        {
            this.tradingParams = tradingParams;
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
                this.ValidateSymbol();
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

        private void CalculateTotal()
        {
            this.Total = this.Price * this.Quantity;
            
            if (total > this.tradingParams.TradingLimit)
            {
                this.validationErrors.Add("Price", "Over trading limit");    
                this.validationErrors.Add("Quantity", "Over trading limit");    
            } 
            else
            {
                this.validationErrors.ClearValidationError("Price");
                this.validationErrors.ClearValidationError("Quantity");
            }
        }

        private void ValidateSymbol()
        {
            if (this.tradingParams.RestrictedSymbols.Contains(this.symbol))
            {
                this.validationErrors.Add("Symbol", string.Format("{0} is restricted", this.symbol));
            } 
            else
            {
                this.validationErrors.ClearValidationError("Symbol");    
            }
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