namespace Lab01;

public class Task03
{
    public static void Run() 
    {
        int birthYear = int.Parse(Console.ReadLine()!);
        
        int age = 2026 - birthYear;

        Console.WriteLine($"Вік: {age} р.");

        if (age <= 17 && age >= 0) 
        {
            Console.WriteLine("Категорія: дитина");
        }
        else if (age >= 18 && age <= 59)
        {
            Console.WriteLine("Категорія: дорослий");
        }
        
        else if (age >= 60) 
        {
            Console.WriteLine("Категорія: пенсіонер");
        }
        else
        {
            Console.WriteLine("Категорія: інше");
        }
        
    }
}