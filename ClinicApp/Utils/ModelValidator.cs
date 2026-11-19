using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class ModelValidator
{
    public static ValidationResult Validate(object obj)
    {
        ValidationResult result = new ValidationResult();

        foreach (PropertyInfo property in obj.GetType().GetProperties())
        {
            object? value = property.GetValue(obj);

            RequiredAttribute? required = property.GetCustomAttribute<RequiredAttribute>();
            if (required != null && (value == null || (value is string text && string.IsNullOrWhiteSpace(text))))
            {
                result.AddError($"{property.Name}: {required.ErrorMessage}");
            }

            MaxLengthAttribute? maxLength = property.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLength != null && value is string str && str.Length > maxLength.Length)
            {
                string message = maxLength.ErrorMessage != "" ? maxLength.ErrorMessage : $"Length must not exceed {maxLength.Length} characters.";
                result.AddError($"{property.Name}: {message}");
            }

            MinValueAttribute? minValue = property.GetCustomAttribute<MinValueAttribute>();
            if (minValue != null && value != null && IsNumeric(value) && Convert.ToDouble(value) < minValue.Min)
            {
                string message = minValue.ErrorMessage != "" ? minValue.ErrorMessage : $"Value must be at least {minValue.Min}.";
                result.AddError($"{property.Name}: {message}");
            }
        }

        return result;
    }

    public static void PrintInfo(Type type)
    {
        Console.WriteLine($"Тип: {type.Name}");

        foreach (PropertyInfo property in type.GetProperties())
        {
            Console.WriteLine($"  {property.PropertyType.Name} {property.Name}");

            foreach (object attribute in property.GetCustomAttributes(false))
            {
                Console.WriteLine($"    [{DescribeAttribute(attribute)}]");
            }
        }
    }

    private static string DescribeAttribute(object attribute)
    {
        if (attribute is RequiredAttribute required)
        {
            return $"Required: \"{required.ErrorMessage}\"";
        }

        if (attribute is MaxLengthAttribute maxLength)
        {
            return $"MaxLength({maxLength.Length}): \"{maxLength.ErrorMessage}\"";
        }

        if (attribute is MinValueAttribute minValue)
        {
            return $"MinValue({minValue.Min}): \"{minValue.ErrorMessage}\"";
        }

        return attribute.GetType().Name;
    }

    private static bool IsNumeric(object value)
    {
        return value is sbyte || value is byte || value is short || value is ushort ||
               value is int || value is uint || value is long || value is ulong ||
               value is float || value is double || value is decimal;
    }
}
