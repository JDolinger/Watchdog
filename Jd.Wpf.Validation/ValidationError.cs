namespace Jd.Wpf.Validation
{
    using System;

    public abstract class ErrorBase : IError
    {
        private readonly string message;
        private readonly string fieldKey;

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

            this.fieldKey = targetBinding;
            this.message = message;
        }

        public string FieldKey
        {
            get { return this.fieldKey; }
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

            if (obj.GetType() != typeof (T))
            {
                return false;
            }

            var other = (T) obj;

            return Equals(this.FieldKey, other.FieldKey) &&
                   Equals(this.message, other.Message);
        }

        public override int GetHashCode()
        {
            return this.fieldKey.GetHashCode() ^ this.message.GetHashCode();
        }
    }

    public class ValidationError : ErrorBase
    {
        public ValidationError(string targetBinding, string message) : base(targetBinding, message)
        {
        }

#pragma warning disable 659
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            return this.CompareCore<ValidationError>(obj);
        }
    }
}