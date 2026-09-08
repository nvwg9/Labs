namespace Lab01;

public class Task07
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);
        decimal[] costs = new decimal[n];

        for (int i = 0; i < n; i++)
        {
            costs[i] = decimal.Parse(Console.ReadLine()!);
        }
        
        decimal sum = 0;
        decimal min = costs[0];
        decimal max = costs[0];
        
        foreach (decimal cost in costs)
        {
            sum += cost;
            if (cost < min) min = cost;
            if (cost > max) max = cost;
        }
        
        decimal average = sum / n;
        
        int aboveAverage = 0;
        for (int i = 0; i < n; i++)
        {
            if (costs[i] > average)
            {
                aboveAverage++;
            }
        }
        
        int firstElement = -1;
        int j = 0;
        while (j < n)
        {
            if (costs[j] > 1000m)
            {
                firstElement = j;
                break;
            }
            j++; 
        }
        
        
        Console.WriteLine("=== Звіт по прийомах ===");
        Console.WriteLine($"Кількість: {n}");
        Console.WriteLine($"Загальна сума: {sum:F2} грн");
        Console.WriteLine($"Середня: {average:F2} грн");
        Console.WriteLine($"Мін / Макс: {min:F2} / {max:F2} грн");
        Console.WriteLine($"Вище середнього: {aboveAverage} з {n}");
        
        if (firstElement != -1)
        {
            Console.WriteLine($"Перший > 1000:    #{firstElement + 1} — {costs[firstElement]:F2} грн");
        }
        else
        {
            Console.WriteLine("Перший > 1000: немає");
        }
        Console.WriteLine("========================");

    }
}