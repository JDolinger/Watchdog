﻿namespace Jd.Wpf.Validation
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
    }

    public class Error : ErrorBase
    {
        public Error(string targetBinding, string message) : base(targetBinding, message)
        {
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

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(ConversionError))
            {
                return false;
            }

            var ce = (ConversionError) obj;

            return object.Equals(
                    this.TargetBinding, 
                    ce.TargetBinding);
        }

        public override int GetHashCode()
        {
            return this.TargetBinding.GetHashCode();
        }
    }
}