namespace Watchdog.Validation.Core.Internal
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    ///     Builds and manages a list of <see cref = "IField" />.  Provides
    ///     key based lookup based on a fieldKey.
    /// </summary>
    internal class FieldMap : IEnumerable<IField>
    {
        /// <summary>
        /// </summary>
        private readonly IDictionary<string, IField> backing;

        public FieldMap()
        {
            this.backing = new Dictionary<string, IField>();
        }

        internal IField Find(string fieldKey)
        {
            IField f;
            this.backing.TryGetValue(fieldKey, out f);
            return f;
        }

        internal IField FindOrCreate(string fieldKey)
        {
            var f = this.Find(fieldKey);

            if (f == null)
            {
                f = new Field();
                this.backing.Add(fieldKey, f);
            }

            return f;
        }

        #region IEnumerable<IField> Members

        public IEnumerator<IField> GetEnumerator()
        {
            return this.backing.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}