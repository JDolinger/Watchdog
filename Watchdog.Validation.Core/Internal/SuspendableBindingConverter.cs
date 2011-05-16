//  
//  SuspendableBindingConverter.cs
//
//  Copyright (C) 2011 Jason Dolinger
//
//  This program is free software; you can redistribute it and/or modify it under the terms 
//	of the GNU General Public License as published by the Free Software Foundation; either
//	version 2 of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
//	without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
//	See the GNU General Public License for more details. You should have received a copy of 
//	the GNU General Public License along with this program; if not, write to the Free Software 
//	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
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