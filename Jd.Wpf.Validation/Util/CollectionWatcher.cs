namespace Jd.Wpf.Validation.Util
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    public class CollectionWatcher<T>
    {
        private readonly Action<T> addHandler;
        private readonly Action<T> removeHandler;
        private readonly Action resetHandler;
        private ObservableCollection<T> collection;

        public CollectionWatcher(Action<T> addHandler, Action<T> removeHandler, Action resetHandler)
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

        public void Watch(ObservableCollection<T> newCollection)
        {
            if (newCollection == null)
            {
                throw new ArgumentNullException("newCollection");
            }

            if (this.collection != null)
            {
                this.collection.CollectionChanged -= this.HandleCollectionChanged;
            }

            this.collection = newCollection;
            this.collection.CollectionChanged += this.HandleCollectionChanged;
        }

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
            }
        }

        private void ReportAdds(IList newItems)
        {
            foreach (var i in newItems.Cast<T>())
            {
                this.addHandler(i);
            }
        }

        private void ReportRemoves(IList removedItems)
        {
            foreach (var i in removedItems.Cast<T>())
            {
                this.removeHandler(i);
            }
        }
    }
}