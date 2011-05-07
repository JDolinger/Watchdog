namespace Jd.Wpf.Validation
{
    using System;

    public interface IError
    {
        string TargetBinding { get; }
        string Message { get; }
    }

    public abstract class ErrorBase : IError
    {
        private readonly string message;
        private readonly string targetBinding;

        protected ErrorBase(string targetBinding, string message)
        {
            if (string.IsNullOrEmpty(targetBinding))
            {
                throw new ArgumentNullException("targetBinding");
            }

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }

            this.targetBinding = targetBinding;
            this.message = message;
        }

        public string TargetBinding
        {
            get { return this.targetBinding; }
        }

        public string Message
        {
            get { return this.message; }
        }

        protected bool CompareCore<T>(object obj) where T : ErrorBase
        {
            if (obj == null)
            {
                return false;
            }

            if (this.GetType() != typeof(T))
            {
                return false;
            }

            var other = (T) obj;
            
            return object.Equals(this.TargetBinding, other.TargetBinding) &&
                   object.Equals(this.message, other.Message);
        }

        public override int GetHashCode()
        {
            return this.targetBinding.GetHashCode() ^ this.message.GetHashCode();
        }
       
    }

    public class Error : ErrorBase
    {
        public Error(string targetBinding, string message) : base(targetBinding, message)
        {
        }

#pragma warning disable 659
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            return this.CompareCore<Error>(obj);
        }
    }

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