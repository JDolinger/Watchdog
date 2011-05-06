namespace Jd.Wpf.Validation
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    public static class ValidationProperties
    {
        public static readonly DependencyProperty ErrorSourceProperty =
            DependencyProperty.RegisterAttached("ErrorSource", typeof (ICollection<IError>), typeof (ValidationProperties), new FrameworkPropertyMetadata(null, OnErrorSourceChanged));

        public static readonly DependencyProperty ScopeProperty =
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

        public static ValidationScope GetScope(DependencyObject obj)
        {
            return (ValidationScope) obj.GetValue(ScopeProperty);
        }

        public static void SetScope(DependencyObject obj, ValidationScope value)
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