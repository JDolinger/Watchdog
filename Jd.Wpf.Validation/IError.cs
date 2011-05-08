namespace Jd.Wpf.Validation
{
    /// <summary>
    /// Simple interface defining a UI error.
    /// </summary>
    public interface IError
    {
        /// <summary>
        /// A key identifier for the field that this
        /// error is associated with.  This is typically
        /// the ViewModel property that is being flagged
        /// as an error.  Remember that there may be multiple
        /// physical controls bound to a field.
        /// </summary>
        string FieldKey { get; }

        /// <summary>
        /// The associated error message.
        /// </summary>
        string Message { get; }
    }
}