//  
//  FieldMap.cs
//
//  Copyright (C) 2011 Jason Dolinger
//
//  This program is free software; you can redistribute it and/or modify it under the terms 
//	of the GNU General Public License as published by the Free Software Foundation; either
//	version 2 of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
//	without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
//	See the GNU General Public License for more details. You should have received a copy of 
//	the GNU General Public License along with this program; if not, write to the Free Software 
//	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
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