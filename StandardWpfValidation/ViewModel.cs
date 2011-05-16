namespace StandardWpfValidation
{
    using System;
    using System.ComponentModel;

    public class ViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private string text;
        private string userId;
        private string password1;
        private string password2;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                this.RaisePropertyChanged("Text");
            }
        }

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
                this.RaisePropertyChanged("Password2");
            }
        }

        public string Password2
        {
            get { return this.password2; }
            set
            {
                this.password2 = value;
                this.RaisePropertyChanged("Password1");
                this.RaisePropertyChanged("Password2");
            }
        }

        protected virtual void RaisePropertyChanged(string name)
        {
            var h = this.PropertyChanged;
            if (h != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Password1")
                {
                    if (!string.Equals(this.password1, this.password2))
                    {
                        return "Passwords must match";
                    }
                }

                if (columnName == "Password2")
                {
                    if (!string.Equals(this.password1, this.password2))
                    {
                        return "Passwords must match";
                    }
                }

                return string.Empty;
            }
        }

        #endregion
    }
}