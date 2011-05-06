namespace Jd.Wpf.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class Field : IField
    {
        private readonly List<ErrorBase> currentErrors;
        private readonly IList<WeakReference> controls;
        private readonly ForceErrorValidationRule forceValidationRule;

        public Field()
        {
           this.controls = new List<WeakReference>();
           this.currentErrors = new List<ErrorBase>();
           this.forceValidationRule = new ForceErrorValidationRule();
        }

        public void AddControl(Control control)
        {
            this.controls.Add(new WeakReference(control));
        }

        public void AttachError(Error e)
        {
            this.currentErrors.Add(e);

            foreach (var controlWeak in this.controls)
            {
                var control = controlWeak.Target as Control;
                if (control != null)
                {
                    var binding = BindingOperations.GetBindingExpression(control, ValidationProperties.GetBoundProperty(control));
                    if (binding != null)
                    {
                        Validation.MarkInvalid(binding, new ValidationError(forceValidationRule, binding));
                    }
                }
            }
        }

        public void ClearError(Error e)
        {
            this.currentErrors.Remove(e);
            this.ClearAll();
        }

        public void ClearAll()
        {
           foreach (var controlWeak in this.controls)
           {
                var control = controlWeak.Target as Control;
                if (control != null)
                {
                    var binding = BindingOperations.GetBindingExpression(control, ValidationProperties.GetBoundProperty(control));
                    if (binding != null)
                    {
                        Validation.ClearInvalid(binding);
                    }
                }
            }
        }

        public IEnumerable<IError> Errors
        {
            get { return this.currentErrors; }
        }
    }

     public class ForceErrorValidationRule : ValidationRule
        {
            public string ErrorContent { private get; set; }

            public override ValidationResult Validate(object value, CultureInfo cultureInfo)
            {
                return new ValidationResult(false, this.ErrorContent);
            }
        }

    //internal class BoundControlWrapper
    //{
    //    private readonly WeakReference controlReference;
    //    private readonly BindingExpression binding;

    //    private readonly ForceErrorValidationRule forcedRule;
        
    //    public BoundControlWrapper(Control boundControl)
    //    {
    //        if (boundControl == null)
    //        {
    //            throw new ArgumentNullException("boundControl");
    //        }

    //        this.controlReference = new WeakReference(boundControl);
    //        this.forcedRule = new ForceErrorValidationRule();
    //        this.binding = BindingOperations.GetBindingExpression(boundControl, ValidationProperties.GetBoundProperty(boundControl));
    //    }

    //    public void AttachError(string message)
    //    {
    //        var c = this.controlReference.Target as Control;

    //        if (c != null)
    //        {
    //            this.forcedRule.ErrorContent = message;
    //            Validation.MarkInvalid(binding, new ValidationError(forcedRule, binding));
    //        }
    //    }

    //    public void Clear()
    //    {
    //        Validation.ClearInvalid(binding);
    //        this.forcedRule.ErrorContent = string.Empty;
    //    }

    //    private class ForceErrorValidationRule : ValidationRule
    //    {
    //        public string ErrorContent { private get; set; }

    //        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    //        {
    //            return new ValidationResult(false, this.ErrorContent);
    //        }
    //    }
                
    //}
}