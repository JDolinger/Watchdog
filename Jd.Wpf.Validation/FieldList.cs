namespace Jd.Wpf.Validation
{
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Jd.Wpf.Validation.Internal;

    public class FieldList
    {
        private readonly IDictionary<string, Field> backing;

        public FieldList()
        {
            this.backing = new Dictionary<string, Field>();
        }

        public bool TryAdd(Control control)
        {
            string fieldName;
            Binding b;
            if (!TryResolveValidationKey(control, out fieldName, out b))
            {
                return false;
            }

            Field field;
            if (!this.backing.TryGetValue(fieldName, out field))
            {
                field = new Field();
                this.backing.Add(fieldName, field);
            }

            field.AddControl(control);
            return true;
        }

        internal Field FindField(string validationKey)
        {
            Field f;
            return this.backing.TryGetValue(validationKey, out f) ? f : null;
        }

        private static bool TryResolveValidationKey(Control control, out string fieldName, out Binding binding)
        {
            fieldName = ValidationProperties.GetValidationKey(control);
            binding = null;

            var boundProperty = ValidationProperties.GetBoundProperty(control);
            if (boundProperty != null)
            {
                binding = BindingOperations.GetBinding(control, boundProperty);

                if (string.IsNullOrEmpty(fieldName))
                {
                    if (binding != null)
                    {
                        fieldName = binding.Path.Path;
                    }   
                }
               
            }

            return (binding != null && fieldName != null);
        }
    }
}