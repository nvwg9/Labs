namespace ClinicApp.Utils;

public static class ClinicValidator
{
    private const int MaxNameLength = 50;
    private const int PhoneLength = 10;
    private const int MinYear = 1900;

    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"Поле {fieldName} не може бути порожнім.", fieldName);
        if (value.Length > MaxNameLength)
            throw new ArgumentException($"Поле {fieldName} не може бути довшим за {MaxNameLength} символів.", fieldName);
    }

    public static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Телефон не може бути порожнім.", nameof(phone));
        if (phone.Length != PhoneLength)
            throw new ArgumentException($"Телефон має містити рівно {PhoneLength} цифр.", nameof(phone));
        for (int i = 0; i < phone.Length; i++)
        {
            if (phone[i] < '0' || phone[i] > '9')
                throw new ArgumentException("Телефон має містити лише цифри.", nameof(phone));
        }
    }

    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value > DateTime.Today)
            throw new ArgumentOutOfRangeException(fieldName, $"Поле {fieldName} не може бути в майбутньому.");
        if (value.Year < MinYear)
            throw new ArgumentOutOfRangeException(fieldName, $"Поле {fieldName} не може бути раніше {MinYear} року.");
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(fieldName, $"Поле {fieldName} має бути більшим за нуль.");
    }
}
