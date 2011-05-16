//  
//  ReentrancyGuard.cs
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