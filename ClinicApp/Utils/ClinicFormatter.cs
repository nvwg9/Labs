using ClinicApp.Enums;

namespace ClinicApp.Utils;

public static class ClinicFormatter
{
    public static string FormatBloodType(BloodType bt) => bt switch
    {
        BloodType.APositive => "A+",
        BloodType.ANegative => "A-",
        BloodType.BPositive => "B+",
        BloodType.BNegative => "B-",
        BloodType.ABPositive => "AB+",
        BloodType.ABNegative => "AB-",
        BloodType.OPositive => "0+",
        BloodType.ONegative => "0-",
        _ => "Невідомо"
    };

    public static string FormatSpeciality(Speciality s) => s switch
    {
        Speciality.General => "Загальна практика",
        Speciality.Cardiology => "Кардіологія",
        Speciality.Neurology => "Неврологія",
        Speciality.Pediatrics => "Педіатрія",
        Speciality.Surgery => "Хірургія",
        Speciality.Orthopedics => "Ортопедія",
        Speciality.Dermatology => "Дерматологія",
        Speciality.Emergency => "Швидка допомога",
        _ => "Невідомо"
    };

    public static string FormatAge(int age)
    {
        int rem100 = age % 100;
        if (rem100 >= 11 && rem100 <= 19)
        {
            return $"{age} років";
        }

        int rem10 = age % 10;
        return rem10 switch
        {
            1 => $"{age} рік",
            2 or 3 or 4 => $"{age} роки",
            _ => $"{age} років"
        };
    }

    public static string FormatPhone(string phone)
    {
        if (string.IsNullOrEmpty(phone) || phone.Length != 10)
        {
            return phone ?? "";
        }

        for (int i = 0; i < phone.Length; i++)
        {
            if (!char.IsDigit(phone[i]))
            {
                return phone;
            }
        }

        return $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6, 4)}";
    }
}