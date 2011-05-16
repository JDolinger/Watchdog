namespace StandardWpfValidation
{
    using System.Globalization;
    using System.Windows.Controls;

    public class NumberRangeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int intVal;
            if (!int.TryParse(value.ToString(), out intVal))
            {
                return new ValidationResult(false, "not a valid integer");
            }

            if (intVal < 25 || intVal > 75)
            {
                return new ValidationResult(false, "Out of allowed interval");
            }

            return new ValidationResult(true, null);
        }
    }
}