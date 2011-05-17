namespace Watchdog.Examples.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Watchdog.Examples.Util;
    using Watchdog.Validation.Core;
    using Watchdog.Validation.Core.ClientUtil;

    public class OrderTicketViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<IError> validationErrors;
        private readonly ICommand bookTicketCommand;
        private readonly IParameters tradingParams;
        private string side;
        private int? quantity;
        private string symbol;
        private decimal price;
        private decimal total;

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
                this.ValidatePosition();
            }
        }

        public int? Quantity
        {
            get { return this.quantity; }
            set
            {
                this.quantity = value;
                this.RaisePropertyChanged("Quantity");
                this.CalculateTotal();
                this.ValidatePosition();
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
                this.ValidatePosition();
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
            this.Total = this.Price * (this.Quantity.HasValue ? this.Quantity.Value : 0);

            if (this.total > this.tradingParams.TradingLimit)
            {
                this.validationErrors.Add("Over trading limit", "Price", "Quantity");
            }
            else
            {
                this.validationErrors.ClearValidationError("Price");
                this.validationErrors.ClearValidationError("Quantity");
            }
        }

        private void ValidatePosition()
        {
            this.validationErrors.ClearValidationError("Side");
            this.validationErrors.ClearValidationError("Quantity");
            this.validationErrors.ClearValidationError("Symbol");

            if (this.Side == "Short")
            {
                if (this.quantity > this.tradingParams.GetPosition(this.symbol))
                {
                    this.validationErrors.Add("Can not short more than current position.", "Side", "Quantity", "Symbol");
                }
            }
        }

        private void ValidateSymbol()
        {
            this.validationErrors.ClearValidationError("Symbol");
            if (this.tradingParams.RestrictedSymbols.Contains(this.symbol))
            {
                this.validationErrors.Add(string.Format("{0} is restricted", this.symbol), "Symbol");
            }
        }

        private bool CanBook()
        {
            return this.ValidationErrors.Count == 0;
        }
    }
}