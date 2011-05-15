namespace Jd.Wpf.Validation.Util
{
    using System;
    using System.Windows.Threading;

    public class RunAfterDispatchCommand
    {
        private readonly Action code;
        private bool activeWaiting;

        public RunAfterDispatchCommand(Action code)
        {
            this.code = code;
        }

        public bool ActiveWaiting
        {
            get { return this.activeWaiting; }
        }

        public void Execute()
        {
            this.activeWaiting = true;
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                this.code();
                this.activeWaiting = false;
            }));
        }
    }
}