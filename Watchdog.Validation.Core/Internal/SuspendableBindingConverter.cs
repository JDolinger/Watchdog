namespace Watchdog.Validation.Core.Internal
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    internal class SuspendableBindingConverter : IValueConverter
    {
        private readonly IValueConverter internalConverter;
        private readonly WatchdogBinding hostBinding;

        public SuspendableBindingConverter(IValueConverter internalConverter, WatchdogBinding binding)
        {
            this.internalConverter = internalConverter;
            this.hostBinding = binding;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.hostBinding.SuspendTransfer ? 
                Binding.DoNothing : 
                this.internalConverter.Convert(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.hostBinding.SuspendTransfer ? 
                Binding.DoNothing : 
                this.internalConverter.ConvertBack(value, targetType, parameter, culture);
        }
    }
}