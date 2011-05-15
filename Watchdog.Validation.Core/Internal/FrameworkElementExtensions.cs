namespace Watchdog.Validation.Core.Internal
{
    using System.Windows;
    using System.Windows.Data;

    public static class FrameworkElementExtensions
    {
        internal static bool TryResolveValidationKey(this FrameworkElement element, out string fieldName)
        {
            fieldName = ValidationProperties.GetValidationKey(element);

            var boundProperty = ValidationProperties.GetBoundProperty(element);
            
            if (boundProperty != null)
            {
                var binding = BindingOperations.GetBinding(element, boundProperty);

                if (string.IsNullOrEmpty(fieldName))
                {
                    if (binding != null)
                    {
                        fieldName = binding.Path.Path;
                    }
                }
            }

            return fieldName != null;
        }
    }
}