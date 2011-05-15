﻿namespace Watchdog.Validation.Core.Internal
{
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    ///     Represents a logical "Field" in the application.  A field is not
    ///     a physical control, it is closer in idea to a ViewModel property.
    ///     Multiple <see cref = "FrameworkElement" /> in the View layer can be
    ///     represented by a single field (by all binding to the same
    ///     ViewModel property.
    /// </summary>
    internal interface IField
    {
        /// <summary>
        ///     Returns all <see cref = "IError" /> currently attached to this field.
        /// </summary>
        IEnumerable<IError> Errors { get; }

        /// <summary>
        /// Adds a <see cref="FrameworkElement"/> as part of this logical field.
        /// </summary>
        /// <param name="element">The element.</param>
        void AddElement(FrameworkElement element);

        /// <summary>
        ///     Attaches a <see cref = "ValidationError" /> to a field.
        /// </summary>
        /// <param name = "e">The error.</param>
        void AttachError(ValidationError e);

        void AttachError(ConversionError e);

        /// <summary>
        ///     Remove the given <see cref = "ValidationError" /> from the field.
        /// </summary>
        /// <param name = "e">The error.</param>
        void ClearError(ValidationError e);

        /// <summary>
        ///     Clears all errors from a field. 
        /// </summary>
        void ClearAll();
    }
}