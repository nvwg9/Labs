namespace Lab01;

public class Task08
{
     public static void Run()
    {
       
        double weight = double.Parse(Console.ReadLine()!);
        double height = double.Parse(Console.ReadLine()!);
        
        double price = double.Parse(Console.ReadLine()!);
        int visits = int.Parse(Console.ReadLine()!);
        int discount = int.Parse(Console.ReadLine()!);
        
        int birthYear = int.Parse(Console.ReadLine()!);
        
        int systolic = int.Parse(Console.ReadLine()!);
        int diastolic = int.Parse(Console.ReadLine()!);
        
        double bmi = CalculateBMI(weight, height);
        string bmiCategory = GetBMICategory(bmi);
        
        double cost = CalculateCost(price, visits, discount);
        
        string ageCategory = GetAgeCategory(2026 - birthYear);
        
        string pressureStatus = GetPressureStatus(systolic, diastolic);

        Console.WriteLine($"IMT: {bmi:F2} -> {bmiCategory}");
        Console.WriteLine($"Сума: {cost:F2} грн");
        Console.WriteLine($"Вік: {2026 - birthYear} р., категорія: {ageCategory}");
        Console.WriteLine($"Тиск: {systolic}/{diastolic} — {pressureStatus}");
}
    
    static double CalculateBMI(double weight, double height)
    {
        return weight / (height * height);
    }

    static string GetBMICategory(double bmi)
    {
        if (bmi < 18.5) return "недостатня вага";
        if (bmi < 25.0) return "норма";
        if (bmi < 30.0) return "надмірна вага";
        return "ожиріння";
    }

    static double CalculateCost(double price, int visits, int discount)
    {
        return price * visits * (1 - discount / 100.0);
    }

    static string GetAgeCategory(int age)
    {
        if (age <= 17) return "дитина";
        if (age <= 59) return "дорослий";
        return "пенсіонер";
    }

    static string GetPressureStatus(int systolic, int diastolic)
    {
        if (systolic < 120 && diastolic < 80) return "норма";
        if (systolic < 130 && diastolic < 80) return "підвищений";
        if (systolic < 140 || diastolic < 90) return "гіпертонія 1 ступеня";
        return "гіпертонія 2 ступеня";
    }
}