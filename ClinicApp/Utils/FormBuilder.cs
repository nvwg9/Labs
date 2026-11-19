using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class FormBuilder
{
    public static T Build<T>() where T : new()
    {
        T instance = new T();

        foreach (PropertyInfo property in typeof(T).GetProperties())
        {
            if (property.GetSetMethod() == null || property.Name == "Id" || property.Name == "Status")
            {
                continue;
            }

            string hint = BuildHint(property);
            Type targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            while (true)
            {
                Console.Write($"{property.Name} ({property.PropertyType.Name}){hint}: ");
                string input = Console.ReadLine() ?? "";

                try
                {
                    object value = targetType.IsEnum
                        ? Enum.Parse(targetType, input, true)
                        : Convert.ChangeType(input, targetType);
                    property.SetValue(instance, value);
                    break;
                }
                catch (Exception e) when (e is FormatException || e is InvalidCastException || e is OverflowException || e is ArgumentException)
                {
                    Console.WriteLine($"Некоректне значення для {property.Name}. Спробуйте ще раз.");
                }
            }
        }

        return instance;
    }

    private static string BuildHint(PropertyInfo property)
    {
        string hint = "";

        if (property.GetCustomAttribute<RequiredAttribute>() != null)
        {
            hint += " [обов'язкове]";
        }

        MaxLengthAttribute? maxLength = property.GetCustomAttribute<MaxLengthAttribute>();
        if (maxLength != null)
        {
            hint += $" [макс. довжина: {maxLength.Length}]";
        }

        MinValueAttribute? minValue = property.GetCustomAttribute<MinValueAttribute>();
        if (minValue != null)
        {
            hint += $" [мін.: {minValue.Min}]";
        }

        return hint;
    }
}
