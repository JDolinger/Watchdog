//  
// ValidationCollectionExtensions.cs
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
namespace Watchdog.Validation.Core.ClientUtil
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension method for an <see cref="ICollection{T}"/>.  These can be
    /// used by users of Watchdog to assist with managing their collections of
    /// Errors on their viewmodel.
    /// </summary>
    public static class ValidationCollectionExtensions
    {
        /// <summary>
        /// Creates a <see cref="ValidationError"/> and adds it to the collection.
        /// </summary>
        /// <param name="collection">The target collection to add to.</param>
        /// <param name="message">The associated message.</param>
        /// <param name="targetFieldKeys">The list of fields to flag with the message.</param>
        public static void Add(this ICollection<IError> collection, string message, params string[] targetFieldKeys)
        {
            foreach (var fieldKey in targetFieldKeys)
            {
                var err = new ValidationError(fieldKey, message);

                if (!collection.Contains(err))
                {
                    collection.Add(new ValidationError(fieldKey, message));
                }
            }
        }

        /// <summary>
        /// Clears all <see cref="ValidationError"/> for the named target field.
        /// This will not clear any <see cref="ConversionError"/> present.  These should
        /// only be removed by WPF when it detects that a binding with a failed conversion
        /// has now been correct.
        /// </summary>
        /// <param name="collection">The source collection.</param>
        /// <param name="targetFieldKey">The field for which errors are being cleared.</param>
        public static void ClearValidationError(this ICollection<IError> collection, string targetFieldKey)
        {
            // ToList() is called to "physicalize" the IEnumerable so we aren't iterating over
            // the collection while removing items.
            foreach (var remove in collection.GetValidationErrors().MatchingField(targetFieldKey).ToList())
            {
                collection.Remove(remove);
            }
        }

        /// <summary>
        /// Gets the <see cref="ValidationError"/> in the given <see cref="ICollection{IError}"/>
        /// </summary>
        /// <param name="collection">The source collection.</param>
        /// <returns>
        /// An <see cref="IEnumerable{ValidationError}"/> containing only the <see cref="ValidationError"/> in the collection.
        /// </returns>
        public static IEnumerable<ValidationError> GetValidationErrors(this ICollection<IError> collection)
        {
            return collection.OfType<ValidationError>();
        }

        /// <summary>
        /// Retrieves the <see cref="IError"/> matching the given field key.
        /// </summary>
        /// <param name="collection">The source collection.</param>
        /// <param name="targetFieldKey">The field for which errors are being search for.</param>
        /// <returns></returns>
        public static IEnumerable<IError> MatchingField(this IEnumerable<IError> collection, string targetFieldKey)
        {
            return collection.Where(e => e.FieldKey == targetFieldKey);
        }
    }
}