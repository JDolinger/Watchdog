//  
//  IError.cs
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
namespace Watchdog.Validation.Core
{
    /// <summary>
    ///     Simple interface defining a UI error.
    /// </summary>
    public interface IError
    {
        /// <summary>
        ///     A key identifier for the field that this
        ///     error is associated with.  This is typically
        ///     the ViewModel property that is being flagged
        ///     as an error.  Remember that there may be multiple
        ///     physical controls bound to a field.
        /// </summary>
        string FieldKey { get; }

        /// <summary>
        ///     The associated error message.
        /// </summary>
        string Message { get; }
    }
}