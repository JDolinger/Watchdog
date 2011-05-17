//  
// ConversionError.cs
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
    using System;
    using System.Windows;
    using Watchdog.Validation.Core.Internal;

    /// <summary>
    /// Represents a Validation Exception thrown by WPF when it is
    /// unable to transfer a value from a View element (<see cref="FrameworkElement"/>
    /// in WPF Logical Tree) to its Binding source.  It is caught by the <see cref="ValidationScope"/>
    /// and turned added into the scope, <see cref="ValidationScope.ErrorSource"/> collection so the
    /// ViewModel layer of the application is aware of it.
    /// This error is similar to a <see cref="ValidationError"/> regarding its members, except
    /// it can also track the invalid data that was entered.
    /// </summary>
    public class ConversionError : ErrorBase
    {
        /// <summary>
        /// The data that caused the Binding transfer to fail.
        /// </summary>
        private readonly object invalidData;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversionError"/> class.
        /// </summary>
        /// <param name="fieldKey">The target binding.</param>
        /// <param name="message">The message.</param>
        /// <param name="invalidData">The invalid data.</param>
        public ConversionError(string fieldKey, string message, object invalidData)
            : base(fieldKey, message)
        {
            if (invalidData == null)
            {
                throw new ArgumentNullException("invalidData");
            }

            this.invalidData = invalidData;
        }

        /// <summary>
        /// Gets the invalid data.
        /// </summary>
        public object InvalidData
        {
            get { return this.invalidData; }
        }

#pragma warning disable 659
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            // Delegate to the ErrorBase.Equals() implementation, only supplying
            // a type to ensure that the class types match.
            return this.CompareCore<ConversionError>(obj);
        }
    }
}