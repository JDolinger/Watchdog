namespace Jd.Wpf.Validation.Internal
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Jd.Wpf.Validation.Util;

    internal class ValidationScope
    {
        private ICollection<IError> errorSource;

        private readonly CollectionWatcher<IError> errorWatcher;
        private readonly FieldList fieldList;

        private readonly ReentrancyGuard addingGuard;

        public ValidationScope(FrameworkElement root)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");    
            }

            this.fieldList = new FieldList();
            this.errorWatcher = new CollectionWatcher<IError>(OnAdded, OnRemoved, OnCleared);
            this.addingGuard = new ReentrancyGuard();

            Validation.AddErrorHandler(root, HandleDataConversionError);
        }

        public ICollection<IError> ErrorSource
        {
            set
            {
                this.errorSource = value;
                var o = this.errorSource as ObservableCollection<IError>;
                if (o != null)
                {
                    this.errorWatcher.Watch(o);
                }
            }
        }

        internal void Register(Control control)
        {
            this.fieldList.TryAdd(control);
        }

        private void HandleDataConversionError(object sender, ValidationErrorEventArgs args)
        {
            if (this.addingGuard.IsSet())
            {
                return;
            }

            var bindingTargetElement = args.OriginalSource as FrameworkElement;
            var erroredExpression = args.Error.BindingInError as BindingExpression;

            if (erroredExpression == null || bindingTargetElement == null)
            {
                return;
            }

            var bindingPath = erroredExpression.ParentBinding.Path.Path;

            using (addingGuard.Set())
            {
                if (args.Action.Equals(ValidationErrorEventAction.Added))
                {
                    var bindingProperty = ValidationProperties.GetBoundProperty(bindingTargetElement);
                    var badData = bindingTargetElement.GetValue(bindingProperty);
                    var conversionError = new ConversionError(bindingPath, "Invalid format", badData);
                    this.errorSource.Add(conversionError);
                } 
                else if (args.Action.Equals(ValidationErrorEventAction.Removed))
                {
                    foreach (var err in this.errorSource
                                            .OfType<ConversionError>()
                                            .Where(c => c.TargetBinding == bindingPath).ToList())
                    {
                        this.errorSource.Remove(err);
                    }
                }
            }
        }

        private void OnAdded(IError added)
        {
            if (this.addingGuard.IsSet())
            {
                return;
            }

            using (addingGuard.Set())
            {
                var f = this.fieldList.FindField(added.TargetBinding);

                if (f == null)
                {
                    Trace.WriteLine("No registered control for validation message {0}", added.Message);
                    return;
                }

                f.AttachError(added as Error);
            }
        }

        private void OnRemoved(IError removed)
        {
            if (this.addingGuard.IsSet())
            {
                return;
            }

            using (addingGuard.Set())
            {
                var f = this.fieldList.FindField(removed.TargetBinding);

                if (f == null)
                {
                    return;
                }

                f.ClearError(removed as Error);
            }
            
        }

        private void OnCleared()
        {
            
        }
    }
}