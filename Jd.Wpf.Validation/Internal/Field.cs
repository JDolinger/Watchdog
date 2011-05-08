namespace Jd.Wpf.Validation.Internal
{
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Linq;
    using ValidationError = Jd.Wpf.Validation.ValidationError;

    internal class Field : IField
    {
        private readonly List<ValidationError> currentErrors;
        private readonly IList<ValidationHandler> elementHandlers;

        public Field()
        {
            this.elementHandlers = new List<ValidationHandler>();
            this.currentErrors = new List<ValidationError>();
        }

        public IEnumerable<IError> Errors
        {
            get { return this.currentErrors; }
        }

        public void AddControl(Control control)
        {
            this.elementHandlers.Add(new ValidationHandler(control));
        }

        public void AttachError(ValidationError e)
        {
            this.currentErrors.Add(e);

            var message = string.Join(", ", this.currentErrors.Select(err => err.Message));

            foreach (var eh in this.elementHandlers)
            {
                eh.Show(message);
            }
        }

        public void ClearError(ValidationError e)
        {
            this.currentErrors.Remove(e);

            if (this.currentErrors.Count > 0)
            {
                var message = string.Join(", ", this.currentErrors.Select(err => err.Message));
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