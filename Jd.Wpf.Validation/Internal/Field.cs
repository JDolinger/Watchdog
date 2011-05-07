namespace Jd.Wpf.Validation.Internal
{
    using System.Collections.Generic;
    using System.Windows.Controls;

    internal class Field : IField
    {
        private readonly List<Error> currentErrors;
        private readonly IList<ValidationHandler> elementHandlers;

        public Field()
        {
            this.elementHandlers = new List<ValidationHandler>();
            this.currentErrors = new List<Error>();
        }

        public IEnumerable<IError> Errors
        {
            get { return this.currentErrors; }
        }

        public void AddControl(Control control)
        {
            this.elementHandlers.Add(new ValidationHandler(control));
        }

        public void AttachError(Error e)
        {
            this.currentErrors.Add(e);

            var message = string.Join(", ", this.currentErrors);

            foreach (var eh in this.elementHandlers)
            {
                eh.Show(message);
            }
        }

        public void ClearError(Error e)
        {
            this.currentErrors.Remove(e);

            if (this.currentErrors.Count > 0)
            {
                var message = string.Join(", ", this.currentErrors);
                foreach (var eh in this.elementHandlers)
                {
                    eh.Show(message);
                }
            }
            else
            {
                foreach (var eh in this.elementHandlers)
                {
                    eh.Clear();
                }
            }
        }

        public void ClearAll()
        {
            this.currentErrors.Clear();
            foreach (var eh in this.elementHandlers)
            {
                eh.Clear();
            }
        }
    }
}