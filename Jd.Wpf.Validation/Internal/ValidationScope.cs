namespace Jd.Wpf.Validation.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Jd.Wpf.Validation.ClientUtil;
    using Jd.Wpf.Validation.Util;
    using ValidationError = Jd.Wpf.Validation.ValidationError;

    /// <summary>
    ///     The implementation of validation management for a portion of a
    ///     WPF LogicalTree.  When <see cref = "FrameworkElement" /> register
    ///     themseives with the scope, it figures which field they are associated
    ///     with, typically through the the property name that the element is bound to. 
    /// 
    ///     The scope also is given a <see cref = "ICollection{T}" />.  When the 
    ///     collection is modified, the scope makes the association between the 
    ///     errors and the logical field that the error should be attached to.
    /// 
    ///     Additionally, the scope watches for any WPF Validation errors
    ///     created by any controls under the scope.  If found, they are inserted
    ///     into the collection.  This way the ViewModel layer of an application (which
    ///     supplied the collection), is now aware of these types of error.
    /// </summary>
    internal class ValidationScope
    {
        /// <summary>
        /// The list of all Fields in the scope.  
        /// </summary>
        private readonly FieldCollection fieldList;

        /// <summary>
        /// Responsible for observing the collection of <see cref="IError"/>
        /// and notifying the scope when items are added or removed.
        /// </summary>
        private readonly CollectionWatcher<IError> errorWatcher;

        /// <summary>
        /// Guards against re-entrancy from the errorSource collection
        /// when adding WPF conversion errors into it.  Because this can
        /// 
        /// 
        /// </summary>
        private readonly ReentrancyGuard addingGuard;

        /// <summary>
        /// 
        /// </summary>
        private ICollection<IError> errorSource;

        public ValidationScope(FrameworkElement root)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }

            this.fieldList = new FieldCollection();
            this.errorWatcher = new CollectionWatcher<IError>(this.OnAdded, this.OnRemoved, this.OnCleared);
            this.addingGuard = new ReentrancyGuard();

            Validation.AddErrorHandler(root, this.HandleDataConversionError);
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

        internal void Register(FrameworkElement element)
        {
            string fieldName;

            if (!element.TryResolveValidationKey(out fieldName))
            {
                return;
            }

            var field = this.fieldList.FindOrCreate(fieldName);
            field.AddElement(element);
        }

        private void HandleDataConversionError(object sender, ValidationErrorEventArgs args)
        {
            //if (this.addingGuard.IsSet)
            //{
            //    return;
            //}

            var bindingTargetElement = args.OriginalSource as FrameworkElement;
            var erroredExpression = args.Error.BindingInError as BindingExpression;

            if (erroredExpression == null || bindingTargetElement == null)
            {
                return;
            }

            var bindingPath = erroredExpression.ParentBinding.Path.Path;

            //using (this.addingGuard.Set())
            //{
                if (args.Action.Equals(ValidationErrorEventAction.Added))
                {
                    var bindingProperty = ValidationProperties.GetBoundProperty(bindingTargetElement);
                    var badData = bindingTargetElement.GetValue(bindingProperty);
                    var conversionError = new ConversionError(bindingPath, "Invalid format", badData);
                    this.errorSource.Add(conversionError);
                }
                else if (args.Action.Equals(ValidationErrorEventAction.Removed))
                {
                    foreach (var err in this.errorSource.OfType<ConversionError>().MatchingField(bindingPath).ToList())
                    {
                        this.errorSource.Remove(err);
                    }
                }
            //}
        }

        private void OnAdded(IError added)
        {
            if (added is ConversionError)
            {
                return;
            }

            //if (this.addingGuard.IsSet)
            //{
            //    return;
            //}

            //using (this.addingGuard.Set())
            //{
                var f = this.fieldList.Find(added.FieldKey);

                if (f == null)
                {
                    Trace.WriteLine("No registered control for validation message {0}", added.Message);
                    return;
                }

                f.AttachError(added as ValidationError);
            //}
        }

        private void OnRemoved(IError removed)
        {
            //if (this.addingGuard.IsSet)
            //{
            //    return;
            //}

            //using (this.addingGuard.Set())
            //{
                var f = this.fieldList.Find(removed.FieldKey);

                if (f == null)
                {
                    return;
                }

                f.ClearError(removed as ValidationError);
            //}
        }

        private void OnCleared()
        {
        }
    }
}