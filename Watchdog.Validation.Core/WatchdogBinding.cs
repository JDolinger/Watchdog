namespace Watchdog.Validation.Core
{
    using System.Windows.Data;
    using Watchdog.Validation.Core.Internal;

    public class WatchdogBinding : Binding
    {
        private static readonly SafeTypeToStringConverter SharedConverterInstance;

        private IValueConverter internalConverter;
        private bool resetOnConvertFailure;
        private bool useGenericTextConverter;

        static WatchdogBinding()
        {
            SharedConverterInstance = new SafeTypeToStringConverter();
        }

        public WatchdogBinding(string path) : base(path)
        {
            this.NotifyOnValidationError = true;
            this.ResetOnConvertFailure = true;
        }

        public bool SuspendTransfer { get; set; }

        public bool UseGenericTextConverter
        {
            get { return this.useGenericTextConverter; }
            set
            {
                this.useGenericTextConverter = value;

                if (this.useGenericTextConverter)
                {
                    this.Converter = SharedConverterInstance;
                }
            }
        }

        public new IValueConverter Converter
        {
            get { return this.internalConverter; }
            set
            {
                this.internalConverter = value;
                ((Binding) this).Converter = new SuspendableBindingConverter(this.internalConverter, this);
            }
        }

        public bool ResetOnConvertFailure
        {
            get { return this.resetOnConvertFailure; }
            set { this.resetOnConvertFailure = value; }
        }
    }
}