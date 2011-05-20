//  
// ElementValidationHandler.cs
//
// Copyright (C) 2011 by Jason Dolinger
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//
namespace Watchdog.Validation.Core.Internal
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// A manager for a single <see cref="FrameworkElement"/>.   This is the lowest-level in Watchdog, where
    /// the controls are forced into error with an artifical dummy binding between two do-nothing <see cref="DependencyProperty"/>
    /// which are attached to the <see cref="FrameworkElement"/> and marked as invalid.
    /// </summary>
    internal class ElementValidationHandler
    {
        /// <summary>
        /// Registers the ValidationSource attached DependencyProperty.  This property does nothing but serve as one end of
        /// a binding that we use to mark the <see cref="FrameworkElement"/> as invalid.
        /// </summary>
        public static readonly DependencyProperty ValidationSourceProperty =
            DependencyProperty.RegisterAttached("ValidationSource", typeof (string), typeof (ElementValidationHandler));

        /// <summary>
        /// Registers the ValidationTarget attached DependencyProperty.  This property does nothing but serve as one end of
        /// a binding that we use to mark the <see cref="FrameworkElement"/> as invalid.
        /// </summary>
        public static readonly DependencyProperty ValidationTargetProperty =
            DependencyProperty.RegisterAttached("ValidationTarget", typeof (string), typeof (ElementValidationHandler));

        /// <summary>
        /// A reference to the <see cref="FrameworkElement"/> being marked and unmarked.  We use a <see cref="WeakReference"/> to
        /// ensure that we don't prevent the element from being GC'ed if no one else has a reference.
        /// </summary>
        private readonly WeakReference controlRef;

        /// <summary>
        /// A WPF framework ValidationRule used to force the dummy binding here into error.
        /// </summary>
        private ArtificalValidationRule rule;

        /// <summary>
        /// A <see cref="BindingExpressionBase"/> between the attached <see cref="ValidationSourceProperty"/> and
        /// <see cref="ValidationTargetProperty"/>.  This expression serves as the invalidation point for creating validation
        /// errors in the control.
        /// </summary>
        private BindingExpressionBase errorHostingBindingExpr;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementValidationHandler"/> class.  Sets of a <see cref="WeakReference"/>
        /// to the given element.  The rest of the invalid mechanisms are not initialized until the first time we need to
        /// invalid the <see cref="element"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        public ElementValidationHandler(FrameworkElement element)
        {
            this.controlRef = new WeakReference(element);
        }

        /// <summary>
        /// Gets the validation target property value.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string GetValidationTarget(DependencyObject obj)
        {
            return (string) obj.GetValue(ValidationTargetProperty);
        }

        /// <summary>
        /// Sets the validation target property value.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetValidationTarget(DependencyObject obj, string value)
        {
            obj.SetValue(ValidationTargetProperty, value);
        }

        /// <summary>
        /// Gets the validation source property value.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string GetValidationSource(DependencyObject obj)
        {
            return (string) obj.GetValue(ValidationSourceProperty);
        }

        /// <summary>
        /// Sets the validation source property value.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="value">The value.</param>
        public static void SetValidationSource(DependencyObject obj, string value)
        {
            obj.SetValue(ValidationSourceProperty, value);
        }

        /// <summary>
        /// Shows the specified message.  This is accomplished by marking the errorHostingBindingExpr
        /// as invalid with a ValidationError containing the given message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Show(string message)
        {
            this.EnsureBinding();

            var newError = new ValidationError(this.rule, this.errorHostingBindingExpr)
            {
                ErrorContent = message
            };

            Validation.MarkInvalid(this.errorHostingBindingExpr, newError);
        }

        /// <summary>
        /// Creates the val error.
        /// </summary>
        /// <param name="e">The e.</param>
        public void CreateValError(ConversionError e)
        {
            Debug.WriteLine("Reattaching conversion error");
            
            var t = this.controlRef.Target as FrameworkElement;

            if (t != null)
            {
                var prop = ValidationProperties.GetBoundProperty(t);
                var be = BindingOperations.GetBindingExpression(t, prop);

                if (prop != null && be != null)
                {
                    t.SetValue(prop, e.InvalidData);
                    be.UpdateSource();
                }
            }
        }

        /// <summary>
        /// Clears the errorHostingBindingExpr of any validation errors.
        /// </summary>
        public void Clear()
        {
            this.EnsureBinding();
            Validation.ClearInvalid(this.errorHostingBindingExpr);
        }

        /// <summary>
        /// Checks for existence (and creates if necessary) the rules and bindings
        /// need to flag the control as invalid.
        /// </summary>
        private void EnsureBinding()
        {
            if (this.errorHostingBindingExpr == null || this.rule == null)
            {
                this.rule = new ArtificalValidationRule();

                var element = this.controlRef.Target as FrameworkElement;

                if (element != null)
                {
                    // Creates a binding between the ValidationSource and the ValidationTarget property
                    // on the element.  One this binding exists, it can be used with Validation.MarkInvalid
                    // in the Show() method, to make the WPF mechanisms work (ie. Validation.HasError will become
                    // true and Validation.Errors will have a ValidationError, so any Validation.ErrorTemplate on
                    // a control will be shown.
                    //
                    // Something interesting here is that we never actually attach the ValidationSource or 
                    // ValidationTarget properties to the element!  I guess creating the Binding is enough!
                    var b = new Binding
                    {
                        Source = element,
                        Path = new PropertyPath(ValidationSourceProperty),
                    };

                    this.errorHostingBindingExpr = BindingOperations.SetBinding(element, ValidationTargetProperty, b);
                }
            }
        }

        #region Nested type: ArtificalValidationRule

        /// <summary>
        /// A WPF ValidationRule which does nothing.  Only exists so we have something to use with Validation.MarkInvalid().
        /// </summary>
        private sealed class ArtificalValidationRule : ValidationRule
        {
            /// <summary>
            /// When overridden in a derived class, performs validation checks on a value.
            /// </summary>
            /// <param name="value">The value from the binding target to check.</param>
            /// <param name="cultureInfo">The culture to use in this rule.</param>
            /// <returns>
            /// A <see cref="T:System.Windows.Controls.ValidationResult"/> object.
            /// </returns>
            public override ValidationResult Validate(object value, CultureInfo cultureInfo)
            {
                return new ValidationResult(false, string.Empty);
            }
        }

        #endregion
    }

    public class SafeTypeToStringConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? value.ToString() : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;

            if (stringValue != null)
            {
                object o;
                if (TypeData.GetParser(targetType).Parse(stringValue, out o))
                {
                    return o;
                }
            }
            
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}