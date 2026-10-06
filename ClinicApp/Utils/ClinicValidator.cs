using System.Text.RegularExpressions;

namespace ClinicApp.Utils;

public static class ClinicValidator
{
    private const int MaxNameLength = 50;
    private const int MinYear = 1900;

    private static readonly Regex PhoneRegex = new Regex(@"^[0-9]{10}\z");
    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+\z");

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
        if (!PhoneRegex.IsMatch(phone))
            throw new ArgumentException("Телефон має містити рівно 10 цифр (0–9).", nameof(phone));
    }

    public static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !EmailRegex.IsMatch(email))
            throw new ArgumentException("Некоректний формат email.", nameof(email));
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
