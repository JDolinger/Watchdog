namespace Jd.Wpf.Validation.Examples.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IParameters
    {
        decimal TradingLimit { get; }
        ICollection<string> RestrictedSymbols { get; }
        int GetPosition(string symbol);
    }

    public class Parameters : IParameters
    {
        private static readonly Parameters sharedInstance;
        private readonly IDictionary<string, int> positionsStore;

        private decimal tradingLimit;
        private string restrictedList;
        private string positions;

        private List<string> restrictedSymbols;

        static Parameters()
        {
            sharedInstance = new Parameters();
        }

        public Parameters()
        {
            this.tradingLimit = 5000000;
            this.restrictedList = "UBS,GOOG";
            this.positions = "IBM:2000000,GOOG:500,AAA:4250000,AAPL:50000,MSFT:3750000,GE:20000,VODLN:1000000";
            this.positionsStore = new Dictionary<string, int>();
            this.ParseRestrictedList();
            this.ParsePositions();
        }

        public decimal TradingLimit
        {
            get { return this.tradingLimit; }
            set { this.tradingLimit = value; }
        }

        public string RestrictedListString
        {
            get { return this.restrictedList; }
            set
            {
                this.restrictedList = value;
                this.ParseRestrictedList();
            }
        }

        public string PositionsString
        {
            get { return this.positions; }
            set
            {
                this.positions = value;
                this.ParsePositions();
            }
        }

        public ICollection<string> RestrictedSymbols
        {
            get { return this.restrictedSymbols; }
        }

        public static Parameters SharedInstance
        {
            get { return sharedInstance; }
        }

        private void ParseRestrictedList()
        {
            this.restrictedSymbols = this.restrictedList.Split(',').ToList();
        }

        private void ParsePositions()
        {
            this.positions
                .Split(',')
                .ToList()
                .ForEach(pair =>
                {
                    var parts = pair.Split(':');
                    this.positionsStore.Add(parts[0], Int32.Parse(parts[1]));
                });
        }

        public int GetPosition(string symbol)
        {
            int pos;
            if (this.positionsStore.TryGetValue(symbol, out pos))
            {
                return pos;
            }

            return 0;
        }
    }
}