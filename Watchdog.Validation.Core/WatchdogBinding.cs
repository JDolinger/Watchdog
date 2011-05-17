//  
// WatchdogBinding.cs
//
// Copyright (C) 2011 by Jason Dolinger
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
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