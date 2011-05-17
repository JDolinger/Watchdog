//  
// ValidationProperties.cs
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
namespace Watchdog.Validation.Core
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using Watchdog.Validation.Core.Internal;

    public static class ValidationProperties
    {
        public static readonly DependencyProperty ErrorSourceProperty =
            DependencyProperty.RegisterAttached("ErrorSource", typeof (ICollection<IError>), typeof (ValidationProperties), new FrameworkPropertyMetadata(null, OnErrorSourceChanged));

        internal static readonly DependencyProperty ScopeProperty =
            DependencyProperty.RegisterAttached("Scope", typeof (ValidationScope), typeof (ValidationProperties), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnScopeChanged));

        public static readonly DependencyProperty BoundPropertyProperty =
            DependencyProperty.RegisterAttached("BoundProperty", typeof (DependencyProperty), typeof (ValidationProperties));

        public static readonly DependencyProperty ValidationKeyProperty =
            DependencyProperty.RegisterAttached("ValidationKey", typeof (string), typeof (ValidationProperties), new UIPropertyMetadata(null));

        static ValidationProperties()
        {
            BoundPropertyProperty.OverrideMetadata(typeof (TextBox), new PropertyMetadata(TextBox.TextProperty));
            BoundPropertyProperty.OverrideMetadata(typeof (ComboBox), new PropertyMetadata(Selector.SelectedValueProperty));
            BoundPropertyProperty.OverrideMetadata(typeof (ToggleButton), new PropertyMetadata(ToggleButton.IsCheckedProperty));
        }

        public static string GetValidationKey(DependencyObject obj)
        {
            return (string) obj.GetValue(ValidationKeyProperty);
        }

        public static void SetValidationKey(DependencyObject obj, string value)
        {
            obj.SetValue(ValidationKeyProperty, value);
        }

        public static DependencyProperty GetBoundProperty(DependencyObject obj)
        {
            return (DependencyProperty) obj.GetValue(BoundPropertyProperty);
        }

        public static void SetBoundProperty(DependencyObject obj, DependencyProperty value)
        {
            obj.SetValue(BoundPropertyProperty, value);
        }

        internal static ValidationScope GetScope(DependencyObject obj)
        {
            return (ValidationScope) obj.GetValue(ScopeProperty);
        }

        internal static void SetScope(DependencyObject obj, ValidationScope value)
        {
            obj.SetValue(ScopeProperty, value);
        }

        public static ICollection<IError> GetErrorSource(DependencyObject obj)
        {
            return (ICollection<IError>) obj.GetValue(ErrorSourceProperty);
        }

        public static void SetErrorSource(DependencyObject obj, ICollection<IError> value)
        {
            obj.SetValue(ErrorSourceProperty, value);
        }

        private static void OnErrorSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var fe = obj as FrameworkElement;

            if (fe != null)
            {
                // check if there is a scope object created yet on this control.  If not create one
                // and attach it to the point in the Logical Tree that defining the scope.
                var scopeObject = GetScope(fe);

                if (scopeObject == null)
                {
                    scopeObject = new ValidationScope(fe);
                    SetScope(obj, scopeObject);
                }

                scopeObject.ErrorSource = args.NewValue as ICollection<IError>;
            }
        }

        private static void OnScopeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as Control;

            if (control != null)
            {
                var scopeObject = args.NewValue as ValidationScope;
                if (scopeObject != null)
                {
                    scopeObject.Register(control);
                }
            }
        }
    }
}