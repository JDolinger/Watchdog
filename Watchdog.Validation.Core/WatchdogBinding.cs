//  
//  WatchdogBinding.cs
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