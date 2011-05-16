//  
//  TypeData.cs
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
    using System;
    using System.Collections.Generic;

    public static class TypeData
    {
        private static readonly IDictionary<Type, object> DefaultsVals;

        static TypeData()
        {
            DefaultsVals = new Dictionary<Type, object>();
            DefaultsVals.Add(typeof (bool), false);
            DefaultsVals.Add(typeof (int), 0);
            DefaultsVals.Add(typeof (double), 0.0d);
            DefaultsVals.Add(typeof (float), 0.0f);
            DefaultsVals.Add(typeof (decimal), 0.0m);
            DefaultsVals.Add(typeof (DateTime), DateTime.MinValue);
        }

        public static object DefaultValue(Type t)
        {
            return DefaultsVals[t];
        }
    }
}