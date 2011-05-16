//  
//  Field.cs
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    /// <summary>
    ///     Implementation of <see cref = "IField" />.  It keeps track of handlers
    ///     for each <see cref = "FrameworkElement" /> that is attached to a field.
    /// </summary>
    internal class Field : IField
    {
        /// <summary>
        ///     A list of wrappers for each attached <see cref = "FrameworkElement" />. Each wrapper
        ///     also provides the API for attaching and removing errors from each individual element.
        /// </summary>
        private readonly IList<ElementValidationHandler> elementHandlers;

        /// <summary>
        ///     All errors currnently attached to this field.
        /// </summary>
        private readonly List<ValidationError> currentErrors;

        /// <summary>
        ///     Initializes a new instance of the <see cref = "Field" /> class.
        /// </summary>
        public Field()
        {
            this.elementHandlers = new List<ElementValidationHandler>();
            this.currentErrors = new List<ValidationError>();
        }

        /// <summary>
        ///     Returns all <see cref = "IError" /> currently attached to this field.
        /// </summary>
        public IEnumerable<IError> Errors
        {
            get { return this.currentErrors; }
        }

        /// <summary>
        ///     Associates a <see cref = "FrameworkElement" /> with this field.
        /// </summary>
        /// <param name = "element">The control.</param>
        public void AddElement(FrameworkElement element)
        {
            this.elementHandlers.Add(new ElementValidationHandler(element));
        }

        /// <summary>
        ///     Attaches a <see cref = "ValidationError" /> to this field.  For
        ///     each associated <see cref = "FrameworkElement" />, the list of concatened
        ///     error messages is shown on the control.
        /// </summary>
        /// <param name = "e">The error.</param>
        public void AttachError(ValidationError e)
        {
            this.currentErrors.Add(e);
            var message = string.Join(", ", this.currentErrors.Select(err => err.Message));
            foreach (var eh in this.elementHandlers)
            {
                eh.Show(message);
            }
        }

        /// <summary>
        ///     Removes the given <see cref = "ValidationError" /> from the field.
        ///     If there are still some errors present, then the message
        ///     is merely updated.  If no more errors will be present, then the each
        ///     <see cref = "FrameworkElement" /> is cleared of errors.
        /// </summary>
        /// <param name = "e">The error.</param>
        public void ClearError(ValidationError e)
        {
            this.currentErrors.Remove(e);

            if (this.currentErrors.Count > 0)
            {
                var message = string.Join(", ", this.currentErrors.Select(err => err.Message));
                foreach (var eh in this.elementHandlers)
                {
                    eh.Show(message);
                }
            }
            else
            {
                foreach (var eh in this.elementHandlers)
                {
                    eh.Clear();
                }
            }
        }

        /// <summary>
        ///     Clears all errors from a field.
        /// </summary>
        public void ClearAll()
        {
            this.currentErrors.Clear();

            foreach (var eh in this.elementHandlers)
            {
                eh.Clear();
            }
        }

        public void AttachError(ConversionError e)
        {
            foreach (var er in this.elementHandlers)
            {
                er.CreateValError(e);
            }
        }
    }
}