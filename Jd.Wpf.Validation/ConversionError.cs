namespace Jd.Wpf.Validation
{
    using System;

    public class ConversionError : ErrorBase
    {
        private readonly object invalidData;

        public ConversionError(string targetBinding, string message, object invalidData) 
            : base(targetBinding, message)
        {
            if (invalidData == null)
            {
                throw new ArgumentNullException("invalidData");
            }

            this.invalidData = invalidData;
        }

        public object InvalidData
        {
            get { return this.invalidData; }
        }

#pragma warning disable 659
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            return this.CompareCore<ConversionError>(obj);
        }
    }
}