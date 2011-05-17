//  
// FieldMap.cs
//
// Copyright (C) 2011 by Jason Dolinger
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//
namespace Watchdog.Validation.Core.Internal
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Builds and manages a list of <see cref="IField"/>.  Provides
    /// key based lookup based on a fieldKey.
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