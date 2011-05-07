namespace Jd.Wpf.Validation.Examples.Util
{
    using System;
    using System.Windows.Input;

    public class DelegateCommand<T> : ICommand
    {
        private readonly Func<T, bool> canExecute;
        private readonly Action<T> execute;

        public DelegateCommand(Func<T, bool> canExecute, Action<T> execute)
        {
            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.canExecute = canExecute;
            this.execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute((T) parameter);
        }

        public void Execute(object parameter)
        {
            this.execute((T) parameter);
        }
    }
}