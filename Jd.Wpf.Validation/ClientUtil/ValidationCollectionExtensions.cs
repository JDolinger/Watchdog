namespace Jd.Wpf.Validation.ClientUtil
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ValidationCollectionExtensions
    {
        public static void Add(this ICollection<IError> collection, string targetField, string message)
        {
            collection.Add(new Error(targetField, message));
        }

        public static void ClearField(this ICollection<IError> collection, string target)
        {
            foreach (var remove in collection.Where(v => v.TargetBinding == target).ToList())
            {
                collection.Remove(remove);
            }
        }      
    }
}