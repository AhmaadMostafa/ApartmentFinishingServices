using System.ComponentModel.DataAnnotations;

namespace ApartmentFinishingServices.APIs.Helpers
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private string PropertyName { get; }
        private object DesiredValue { get; }

        public RequiredIfAttribute(string propertyName, object desiredValue)
        {
            PropertyName = propertyName;
            DesiredValue = desiredValue;
            ErrorMessage = $"The {0} field is required when {PropertyName} is {DesiredValue}";
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();
            var propertyValue = type.GetProperty(PropertyName)?.GetValue(instance);

            if (propertyValue?.ToString() == DesiredValue.ToString() && value == null)
            {
                return new ValidationResult(string.Format(ErrorMessage, context.MemberName));
            }
            return ValidationResult.Success;
        }
    }

    public class RangeIfAttribute : RangeAttribute
    {
        private string PropertyName { get; }
        private object DesiredValue { get; }

        public RangeIfAttribute(string propertyName, object desiredValue,
            double minimum, double maximum) : base(minimum, maximum)
        {
            PropertyName = propertyName;
            DesiredValue = desiredValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();
            var propertyValue = type.GetProperty(PropertyName)?.GetValue(instance);

            if (propertyValue?.ToString() == DesiredValue.ToString())
            {
                return base.IsValid(value, context);
            }
            return ValidationResult.Success;
        }
    }
}
