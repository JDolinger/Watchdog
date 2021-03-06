﻿//  
// ErrorBase.cs
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

    /// <summary>
    /// Base class for the different categories of Errors that
    /// a field can contain.
    /// </summary>
    public abstract class ErrorBase : IError
    {
        /// <summary>
        /// The fieldKey for the field that the error is attached.
        /// </summary>
        private readonly string fieldKey;

        /// <summary>
        /// The associated error message.
        /// </summary>
        private readonly string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBase"/> class.
        /// </summary>
        /// <param name="fieldKey">The target binding.</param>
        /// <param name="message">The message.</param>
        protected ErrorBase(string fieldKey, string message)
        {
            if (string.IsNullOrEmpty(fieldKey))
            {
                throw new ArgumentNullException("fieldKey");
            }

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }

            this.fieldKey = fieldKey;
            this.message = message;
        }

        /// <summary>
        /// A key identifier for the field that this
        /// error is associated with.  This is typically
        /// the ViewModel property that is being flagged
        /// as an error.  Remember that there may be multiple
        /// physical controls bound to a field.
        /// </summary>
        public string FieldKey
        {
            get { return this.fieldKey; }
        }

        /// <summary>
        /// The associated error message.
        /// </summary>
        public string Message
        {
            get { return this.message; }
        }

        /// <summary>
        /// Equals implementation for the fields at this level of the hierarchy.  An additional
        /// type parameter is supplied for the caller to provide a type for the comparison object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        protected bool CompareCore<T>(object obj) where T : ErrorBase
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof (T))
            {
                return false;
            }

            var other = (T) obj;

            return Equals(this.FieldKey, other.FieldKey) &&
                   Equals(this.message, other.Message);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.fieldKey.GetHashCode() ^ this.message.GetHashCode();
        }
    }
}