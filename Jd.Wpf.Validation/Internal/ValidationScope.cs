namespace Jd.Wpf.Validation.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
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
        ///     The list of all Fields in the scope.
        /// </summary>
        private readonly FieldMap fieldList;

        /// <summary>
        ///     Responsible for observing the collection of <see cref = "IError" />
        ///     and notifying the scope when items are added or removed.
        /// </summary>
        private readonly CollectionWatcher<IError, ValidationError> errorWatcher;

       
        private readonly RunAfterDispatchCommand reattachErrorCommand;

        /// <summary>
        ///     The collection of <see cref = "IError" /> representing any <see cref = "Wpf.Validation.ValidationError" /> 
        ///     which have been added to the collection, as well <see cref = "ConversionError" /> that
        ///     the WPF framework has raised.
        /// </summary>
        private ICollection<IError> errorSource;

        /// <summary>
        ///     Initializes a new instance of the <see cref = "ValidationScope" /> class.
        /// </summary>
        /// <param name = "root">
        ///     The root element of the scope where the initial <see cref = "ICollection{IError}" /> has been attached.
        /// </param>
        public ValidationScope(FrameworkElement root)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }

            this.fieldList = new FieldMap();
            this.errorWatcher = new CollectionWatcher<IError, ValidationError>(this.OnAdded, this.OnRemoved, this.OnCleared);
            this.reattachErrorCommand = new RunAfterDispatchCommand(this.ReattachConversionErrors);

            Validation.AddErrorHandler(root, this.HandleDataConversionError);
        }

        public ICollection<IError> ErrorSource
        {
            set
            {
                var o = value as ObservableCollection<IError>;
                this.errorSource = o;
                this.errorWatcher.Watch(o); // if null, this will at least unsubscribe any old collection.
                this.reattachErrorCommand.Execute();
            }
        }

        private void ReattachConversionErrors()
        {
            if (this.errorSource != null)
            {
                foreach (var item in this.errorSource.OfType<ConversionError>().ToList())
                {
                    var f = this.fieldList.Find(item.FieldKey);
                    if (f != null)
                    {
                        f.AttachError(item);
                    }
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
            if (this.reattachErrorCommand.ActiveWaiting)
            {
                return;
            }

            Console.WriteLine("Conversion error {0}" + args.Action);
            var bindingTargetElement = args.OriginalSource as FrameworkElement;
            var erroredExpression = args.Error.BindingInError as BindingExpression;

            if (erroredExpression == null || bindingTargetElement == null)
            {
                return;
            }

            var bindingPath = erroredExpression.ParentBinding.Path.Path;

            if (args.Action.Equals(ValidationErrorEventAction.Added))
            {
                var bindingProperty = ValidationProperties.GetBoundProperty(bindingTargetElement);
                var badData = bindingTargetElement.GetValue(bindingProperty);
                var conversionError = new ConversionError(bindingPath, string.Format("Invalid format {0}", bindingProperty), badData);
                this.errorSource.Add(conversionError);
            }
            else if (args.Action.Equals(ValidationErrorEventAction.Removed))
            {
                foreach (var err in this.errorSource.OfType<ConversionError>().MatchingField(bindingPath).ToList())
                {
                    this.errorSource.Remove(err);
                }
            }
        }

        /// <summary>
        ///     Called when an <see cref = "IError" /> has been added to the collection.  It handles
        ///     finding the <see cref = "IField" /> with the given fieldKey and adding the error to the field.
        /// </summary>
        /// <param name = "added">The added.</param>
        private void OnAdded(ValidationError added)
        {
            var f = this.fieldList.Find(added.FieldKey);

            if (f == null)
            {
                Trace.WriteLine("No registered control for validation message {0}", added.Message);
                return;
            }

            f.AttachError(added);
        }

        /// <summary>
        ///     Called when an <see cref = "IError" /> has been removed from the collection.  It handles
        ///     finding the <see cref = "IField" /> with the given fieldKey and removing the error to the field.
        /// </summary>
        /// <param name = "removed">The removed.</param>
        private void OnRemoved(ValidationError removed)
        {
            var f = this.fieldList.Find(removed.FieldKey);

            if (f == null)
            {
                return;
            }

            f.ClearError(removed);
        }

        private void OnCleared()
        {
            //foreach (var f in this.fieldList)
            //{
            //    f.ClearAll();
            //}

            //foreach (var )
        }
    }
}