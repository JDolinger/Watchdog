//  
// ReentrancyGuard.cs
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
namespace Watchdog.Validation.Core.Util
{
    using System;

    /// <summary>
    ///     A utility class which helps guard against reentrancy.  This is safer and
    ///     cleaner alternative to the typical pattern of adding a bool flag to a class,
    ///     and setting it to true when you wish to guard, then resetting it back to
    ///     false.  
    /// 
    ///     The <see cref = "ReentrancyGuard" /> returns an <see cref = "IDisposable" /> token
    ///     when call its <see cref = "Set" /> method.  From this point on, its <see cref = "IsSet" />
    ///     property will be true.  When you wish to exit out of the safe block, call <see cref = "Dispose" />
    ///     on the token.  The best usage is within a using block:
    ///     <code>
    ///         // class member:
    ///         private readonly ReentrancyGuard guard;
    /// 
    ///         // to protect a block from reentrancy:
    /// 
    ///         using (this.guard.Set())
    ///         {
    ///         ... safe code
    ///         }
    /// 
    ///         And wherever you need to guard:
    /// 
    ///         if (this.guard.IsSet)
    ///         {
    ///         return;
    ///         }
    ///     </code>
    /// 
    ///     This class is meant to be used only a single thread.
    /// </summary>
    public class ReentrancyGuard
    {
        /// <summary>
        ///     Whether the guard is currently in effect.
        /// </summary>
        //private bool isSet;
        private int count;

        /// <summary>
        ///     Gets a value the value of the <see cref = "isSet" /> field.
        /// </summary>
        public bool IsSet
        {
            get { return this.count > 0; }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IDisposable Set()
        {
            return new ExitToken(this);
        }

        #region Nested type: ExitToken

        /// <summary>
        ///     A single token created to delineate a guarded 
        ///     scope.  When created, it sets the locked flag 
        ///     to of the given guard to true.  When disposed it
        ///     sets the flag back to false.
        /// </summary>
        private class ExitToken : IDisposable
        {
            /// <summary>
            ///     The guard class that created this token, and for which
            ///     the token is managing a set / dipose scope.
            /// </summary>
            private readonly ReentrancyGuard guard;

            /// <summary>
            ///     Initializes a new instance of the <see cref = "ExitToken" /> class, which
            ///     has the effect of locking the <see cref = "ReentrancyGuard" />.
            /// </summary>
            /// <param name = "g">
            ///     The <see cref = "ReentrancyGuard" /> containing the 
            ///     locked state.
            /// </param>
            public ExitToken(ReentrancyGuard g)
            {
                this.guard = g;
                this.guard.count++;
            }

            /// <summary>
            ///     Resets the guards locked state.
            /// </summary>
            public void Dispose()
            {
                this.guard.count--;
            }
        }

        #endregion
    }
}