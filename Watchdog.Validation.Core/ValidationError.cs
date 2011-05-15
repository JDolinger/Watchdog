namespace Jd.Wpf.Validation
{
    /// <summary>
    ///     A Validation Error that can be used to flag a field as invalid along with a message.  This is
    ///     for building property validation logic at the ViewModel layer, and being able to associate
    ///     those errors back to the View.
    /// </summary>
    public class ValidationError : ErrorBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref = "ValidationError" /> class.
        /// </summary>
        /// <param name = "fieldKey">The target binding.</param>
        /// <param name = "message">The message.</param>
        public ValidationError(string fieldKey, string message) : base(fieldKey, message)
        {
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
            return this.CompareCore<ValidationError>(obj);
        }
    }
}