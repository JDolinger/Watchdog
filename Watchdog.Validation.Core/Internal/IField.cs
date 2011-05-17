//  
// IField.cs
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
namespace Watchdog.Validation.Core.Internal
{
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Represents a logical "Field" in the application.  A field is not
    /// a physical control, it is closer in idea to a ViewModel property.
    /// Multiple <see cref="FrameworkElement"/> in the View layer can be
    /// represented by a single field (by all binding to the same
    /// ViewModel property.
    /// </summary>
    internal interface IField
    {
        /// <summary>
        /// Returns all <see cref="IError"/> currently attached to this field.
        /// </summary>
        IEnumerable<IError> Errors { get; }

        /// <summary>
        /// Adds a <see cref="FrameworkElement"/> as part of this logical field.
        /// </summary>
        /// <param name="element">The element.</param>
        void AddElement(FrameworkElement element);

        /// <summary>
        /// Attaches a <see cref="ValidationError"/> to a field.
        /// </summary>
        /// <param name="e">The error.</param>
        void AttachError(ValidationError e);

        /// <summary>
        /// Attaches the error.
        /// </summary>
        /// <param name="e">The e.</param>
        void AttachError(ConversionError e);

        /// <summary>
        /// Remove the given <see cref="ValidationError"/> from the field.
        /// </summary>
        /// <param name="e">The error.</param>
        void ClearError(ValidationError e);

        /// <summary>
        /// Clears all errors from a field.
        /// </summary>
        void ClearAll();
    }
}