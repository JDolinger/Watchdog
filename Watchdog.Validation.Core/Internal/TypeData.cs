//  
// TypeData.cs
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
    using System;
    using System.Collections.Generic;

    public static class TypeData
    {
        public interface IStringParser
        {
            bool Parse(string s, out object parsed);
        }

        public delegate bool ConvertFunc<T>(string s, out T parsed);

        public class StringParser<T> : IStringParser
        {
            private readonly ConvertFunc<T> func;

            public StringParser(ConvertFunc<T> func)
            {
                this.func = func;
            }

            bool IStringParser.Parse(string s, out object parsed)
            {
                T outParam;
                var success = this.Parse(s, out outParam);
                parsed = outParam;
                return success;
            }

            public bool Parse(string s, out T parsed)
            {
                return this.func(s, out parsed);
            }
        }


        private static readonly IDictionary<Type, object> DefaultsVals;
        private static readonly IDictionary<Type, IStringParser> Parsers;

        static TypeData()
        {
            DefaultsVals = new Dictionary<Type, object>();
            Parsers = new Dictionary<Type, IStringParser>();

            RegisterData<bool>(Boolean.TryParse);
            RegisterData<int>(Int32.TryParse);
            RegisterData<double>(Double.TryParse);
            RegisterData<float>(float.TryParse);
            RegisterData<decimal>(decimal.TryParse);
            RegisterData<DateTime>(DateTime.TryParse);
        }

        public static object DefaultValue(Type t)
        {
            return DefaultsVals[t];
        }

        public static IStringParser GetParser(Type t)
        {
            return Parsers[t];
        }

        public static void RegisterData<T>(ConvertFunc<T> func) where T : struct
        {
            DefaultsVals.Add(typeof (T), default(T));
            DefaultsVals.Add(typeof (T?), default(T?));

            Parsers.Add(typeof(T), new StringParser<T>(func));
            Parsers.Add(typeof(T?), new StringParser<T>(func));
        }
    }
}