namespace Lab01;

public class Task06
{
    public static void Run()
    {
        int card = int.Parse(Console.ReadLine()!);
        int last = card % 10;
        
        string department = last switch
        {
            0 or 1 => "загальна терапія",
            2 or 3 => "хірургія", 
            4 or 5 => "кардіологія",
            6 or 7 => "неврологія",
            8 or 9 => "офтальмологія",
            _ => "невідоме відділення" 
        };
        
        string beneficiary = (card % 2 == 0) ? "так" : "ні";
        string checkup = (card % 3 == 0) ? "так" : "ні";
        
        Console.WriteLine($"Відділення: {department}");
        Console.WriteLine($"Пільгова: {beneficiary}");
        Console.WriteLine($"Огляд: {checkup}");
    }
}