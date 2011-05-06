namespace Jd.Wpf.Validation
{
    using System.Collections.Generic;

    internal interface IField
    {
        void AttachError(Error e);
        void ClearError(Error e);
        void ClearAll();
        IEnumerable<IError> Errors { get; }
    }
}