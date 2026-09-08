namespace Lab01;

public class Task02
{
    public static void Run () 
    {
        double price = double.Parse(Console.ReadLine()!);
        int visits =  int.Parse(Console.ReadLine()!);
        int discount = int.Parse(Console.ReadLine()!);

        double totalPrice = price * visits * (1 - discount / 100.0);
        
        Console.WriteLine($"Сума: {totalPrice:F2} грн");

    }
}