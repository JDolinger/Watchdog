namespace Jd.Wpf.Validation.Internal
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