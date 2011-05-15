namespace Watchdog.Validation.Core
{
    using System;
    using System.Windows;
    using Watchdog.Validation.Core.Internal;

    /// <summary>
    ///     Represents a Validation Exception thrown by WPF when it is
    ///     unable to transfer a value from a View element (<see cref = "FrameworkElement" />
    ///     in WPF Logical Tree) to its Binding source.  It is caught by the <see cref = "ValidationScope" />
    ///     and turned added into the scope, <see cref = "ValidationScope.ErrorSource" /> collection so the
    ///     ViewModel layer of the application is aware of it.
    /// 
    ///     This error is similar to a <see cref = "ValidationError" /> regarding its members, except
    ///     it can also track the invalid data that was entered.
    /// </summary>
    public class ConversionError : ErrorBase
    {
        /// <summary>
        ///     The data that caused the Binding transfer to fail.
        /// </summary>
        private readonly object invalidData;

        /// <summary>
        ///     Initializes a new instance of the <see cref = "ConversionError" /> class.
        /// </summary>
        /// <param name = "fieldKey">The target binding.</param>
        /// <param name = "message">The message.</param>
        /// <param name = "invalidData">The invalid data.</param>
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
        ///     Gets the invalid data.
        /// </summary>
        public object InvalidData
        {
            get { return this.invalidData; }
        }

#pragma warning disable 659
        /// <summary>
        ///     Determines whether the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     true if the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name = "obj">The <see cref = "T:System.Object" /> to compare with the current <see cref = "T:System.Object" />. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            // Delegate to the ErrorBase.Equals() implementation, only supplying
            // a type to ensure that the class types match.
            return this.CompareCore<ConversionError>(obj);
        }
    }
}