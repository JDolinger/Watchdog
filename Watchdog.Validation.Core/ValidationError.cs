// $FILENAME$
// 
// Copyright (C) 2011 Jason Dolinger
// 
// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
namespace Watchdog.Validation.Core
{
    using System;

    /// <summary>
    /// A Validation Error that can be used to flag a field as invalid along with a message.  This is
    /// for building property validation logic at the ViewModel layer, and being able to associate
    /// those errors back to the View.
    /// </summary>
    public class ValidationError : ErrorBase
    {
        private readonly string errorKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="fieldKey">The target binding.</param>
        /// <param name="errorKey">The error key.</param>
        /// <param name="message">The message.</param>
        public ValidationError(string fieldKey, string errorKey, string message) : base(fieldKey, message)
        {
            if (string.IsNullOrEmpty(errorKey))
            {
                throw new ArgumentException("errorKey");
            }

            this.errorKey = errorKey;
        }

        public string ErrorKey
        {
            get { return this.errorKey; }
        }

#pragma warning disable 659
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            if (!this.CompareCore<ValidationError>(obj))
            {
                return false;
            }

            var ve = (ValidationError) obj;

            return Equals(this.errorKey, ve.ErrorKey);
        }
    }
}