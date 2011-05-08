namespace Jd.Wpf.Validation.Internal
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class ValidationHandler
    {
        public static readonly DependencyProperty ValidationTargetProperty =
            DependencyProperty.RegisterAttached("ValidationTarget", typeof (string), typeof (ValidationHandler));

        public static readonly DependencyProperty ValidationSourceProperty =
            DependencyProperty.RegisterAttached("ValidationSource", typeof (string), typeof (ValidationHandler));

        private readonly WeakReference controlRef;
        private readonly AlwaysInvalidRule air;
        private BindingExpressionBase errorHostingBindingExpr;

        public ValidationHandler(FrameworkElement control)
        {
            this.controlRef = new WeakReference(control);
            this.air = new AlwaysInvalidRule();
        }

        public static string GetValidationTarget(DependencyObject obj)
        {
            return (string) obj.GetValue(ValidationTargetProperty);
        }

        public static void SetValidationTarget(DependencyObject obj, string value)
        {
            obj.SetValue(ValidationTargetProperty, value);
        }

        public static string GetValidationSource(DependencyObject obj)
        {
            return (string) obj.GetValue(ValidationSourceProperty);
        }

        public static void SetValidationSource(DependencyObject obj, string value)
        {
            obj.SetValue(ValidationSourceProperty, value);
        }

        private void EnsureBinding()
        {
            if (this.errorHostingBindingExpr == null)
            {
                var element = this.controlRef.Target as FrameworkElement;

                if (element != null)
                {
                    var b = new Binding
                    {
                        NotifyOnValidationError = true,
                        Source = element,
                        Path = new PropertyPath(ValidationSourceProperty),
                    };

                    this.errorHostingBindingExpr = BindingOperations.SetBinding(element, ValidationTargetProperty, b);
                }
            }
        }

        public void Show(string message)
        {
            this.EnsureBinding();

            var newError = new ValidationError(this.air, this.errorHostingBindingExpr)
            {
                ErrorContent = message
            };

            Validation.MarkInvalid(this.errorHostingBindingExpr, newError);
        }

        public void Clear()
        {
            this.EnsureBinding();
            Validation.ClearInvalid(this.errorHostingBindingExpr);
        }

        #region Nested type: AlwaysInvalidRule

        private sealed class AlwaysInvalidRule : ValidationRule
        {
            public override ValidationResult Validate(object value, CultureInfo cultureInfo)
            {
                return new ValidationResult(false, string.Empty);
            }
        }

        #endregion
    }
}