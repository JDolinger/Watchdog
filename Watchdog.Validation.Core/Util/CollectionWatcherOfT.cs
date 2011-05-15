namespace Jd.Wpf.Validation.Util
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    ///     A utility which watches a <see cref = "ObserablveCollection{T}" />.  When the
    ///     collection raises <see cref = "System.Collections.Specialized.INotifyCollectionChanged" /> event, it
    ///     calls back the provided Actions corresponding to each type of event.  This class simple helps you
    ///     move the typical boilerplate out of the classes which must be respond to these events.
    /// </summary>
    /// <typeparam name = "T">The type contained in the collection being watched.</typeparam>
    public class CollectionWatcher<T, TFilter>
    {
        /// <summary>
        ///     Callback invoked for each <see cref = "T" /> that gets added to the collection.
        /// </summary>
        private readonly Action<TFilter> addHandler;

        /// <summary>
        ///     Callback invoked for each <see cref = "T" /> that gets removed from the collection. 
        /// </summary>
        private readonly Action<TFilter> removeHandler;

        /// <summary>
        ///     Callback invoked when the collection has dramatically changed.
        /// </summary>
        private readonly Action resetHandler;

        /// <summary>
        ///     The collection under watch.
        /// </summary>
        private ObservableCollection<T> collection;

        /// <summary>
        ///     Initializes a new instance of the <see cref = "CollectionWatcher&lt;T&gt;" /> class.
        /// </summary>
        /// <param name = "addHandler">The add handler.</param>
        /// <param name = "removeHandler">The remove handler.</param>
        /// <param name = "resetHandler">The reset handler.</param>
        public CollectionWatcher(Action<TFilter> addHandler, Action<TFilter> removeHandler, Action resetHandler)
        {
            if (addHandler == null)
            {
                throw new ArgumentNullException("addHandler");
            }

            if (removeHandler == null)
            {
                throw new ArgumentNullException("removeHandler");
            }

            if (resetHandler == null)
            {
                throw new ArgumentNullException("resetHandler");
            }

            this.addHandler = addHandler;
            this.removeHandler = removeHandler;
            this.resetHandler = resetHandler;
        }

        /// <summary>
        ///     Sets the <see cref = "System.Collections.ObjectModel.ObservableCollection{T}"> that will be observed, 
        ///                  and hooks up to the <see cref = "CollectionChanged" /> event.
        /// </summary>
        /// <param name = "newCollection">The new collection.</param>
        public void Watch(ObservableCollection<T> newCollection)
        {
            this.Detach();
            this.Attach(newCollection);
        }

        public void Attach(ObservableCollection<T> newCollection)
        {
            if (newCollection == null)
            {
                throw new ArgumentNullException("newCollection");
            }

            this.collection = newCollection;
            this.ReportAdds(this.collection);
            this.collection.CollectionChanged += this.HandleCollectionChanged;
        }

        public void Detach()
        {
            if (this.collection != null)
            {
                this.collection.CollectionChanged -= this.HandleCollectionChanged;
                this.ReportRemoves(this.collection);
            }
        }

        /// <summary>
        ///     Listens for the different CollectionChanged events, and appropriately calls back
        ///     each registered <see cref = "Action{T}" /> for each added or removed <see cref = "T" />
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">
        ///     The <see cref = "System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing the event data.
        /// </param>
        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.ReportAdds(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    this.ReportRemoves(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.resetHandler();
                    break;
            }
        }

        /// <summary>
        ///     Iterates the list of any added <see cref = "T" /> 
        ///     and invokes the <see cref = "addHandler" /> callback.
        /// </summary>
        /// <param name = "newItems">The list of new item.</param>
        private void ReportAdds(IList newItems)
        {
            foreach (var i in newItems)
            {
                Console.WriteLine("Item added:" + i.GetType());
            }

            foreach (var i in newItems.OfType<TFilter>().ToList())
            {
                this.addHandler(i);
            }
        }

        /// <summary>
        ///     Iterates the list of any removed <see cref = "T" /> 
        ///     and invokes the <see cref = "removeHandler" /> callback.
        /// </summary>
        /// <param name = "removedItems">The list of new item.</param>
        private void ReportRemoves(IList removedItems)
        {
            foreach (var i in removedItems.OfType<TFilter>())
            {
                this.removeHandler(i);
            }
        }
    }
}