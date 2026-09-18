namespace Lab02;

public class Task06
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);
        
        int[][] costs = new int[n][];
        
        for (int i = 0; i < n; i++)
        {
            int k = int.Parse(Console.ReadLine()!);
            costs[i] = new int[k];

            for (int j = 0; j < k; j++)
            {
                costs[i][j] = int.Parse(Console.ReadLine()!);
            }
        }

        int bestDoctorIdx = 0;
        int maxIncome = -1;
        
        for (int i = 0; i < costs.Length; i++)
        {
            int sum = 0;
            for (int j = 0; j < costs[i].Length; j++)
            {
                sum += costs[i][j];
            }
            
            double average = (double)sum / costs[i].Length;

            Console.WriteLine($"Лікар {i + 1}: {costs[i].Length} прийоми, сума={sum} грн, середня={average:F2} грн");
            
            if (sum > maxIncome)
            {
                maxIncome = sum;
                bestDoctorIdx = i;
            }
        }

        Console.WriteLine($"Найбільший дохід: Лікар {bestDoctorIdx + 1} ({maxIncome} грн)");
    }
}