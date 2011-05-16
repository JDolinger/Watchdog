﻿//  
//  ValidationProperties.cs
//
//  Copyright (C) 2011 Jason Dolinger
//
//  This program is free software; you can redistribute it and/or modify it under the terms 
//	of the GNU General Public License as published by the Free Software Foundation; either
//	version 2 of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
//	without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
//	See the GNU General Public License for more details. You should have received a copy of 
//	the GNU General Public License along with this program; if not, write to the Free Software 
//	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
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