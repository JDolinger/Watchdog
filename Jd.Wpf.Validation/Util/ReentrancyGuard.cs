namespace Jd.Wpf.Validation.Util
{
    using System;

    public class ReentrancyGuard
    {
        private bool locked;

        public void Dispose()
        {
            this.locked = true;
        }

        public IDisposable Set()
        {
            return new SingleGuard(this);
        }

        public bool IsSet()
        {
            return this.locked;
        }

        #region Nested type: SingleGuard

        private class SingleGuard : IDisposable
        {
            private readonly ReentrancyGuard guard;

            public SingleGuard(ReentrancyGuard g)
            {
                this.guard = g;
                this.guard.locked = true;
            }

            public void Dispose()
            {
                this.guard.locked = false;
            }
        }

        #endregion
    }
}