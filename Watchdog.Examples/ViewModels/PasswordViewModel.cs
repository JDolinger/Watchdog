namespace Watchdog.Examples.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Watchdog.Validation.Core;
    using Watchdog.Validation.Core.ClientUtil;

    public class PasswordViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<IError> validationErrors;
        private string userId;
        private string password1;
        private string password2;

        public PasswordViewModel()
        {
            this.validationErrors = new ObservableCollection<IError>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string UserId
        {
            get { return this.userId; }
            set
            {
                this.userId = value;
                this.RaisePropertyChanged("UserId");
            }
        }

        public string Password1
        {
            get { return this.password1; }
            set
            {
                this.password1 = value;
                this.RaisePropertyChanged("Password1");
                this.ValidatePasswords();
            }
        }

        public string Password2
        {
            get { return this.password2; }
            set
            {
                this.password2 = value;
                this.RaisePropertyChanged("Password2");
                this.ValidatePasswords();
            }
        }

        public ObservableCollection<IError> ValidationErrors
        {
            get { return this.validationErrors; }
        }

        protected virtual void RaisePropertyChanged(string name)
        {
            var h = this.PropertyChanged;
            if (h != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void ValidatePasswords()
        {
            if (!string.IsNullOrEmpty(this.password1) && !string.IsNullOrEmpty(this.password2))
            {
                if (this.password1 != this.password2)
                {
                    this.validationErrors.Add("Password1", "Passwords must match");
                    this.validationErrors.Add("Password2", "Passwords must match");
                }
                else
                {
                    this.validationErrors.ClearValidationError("Password1");
                    this.validationErrors.ClearValidationError("Password2");
                }
            }
        }
    }
}