namespace Lab02;

public class Task04
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine()!);
        int m = int.Parse(Console.ReadLine()!);
        
        int[,] matrix = new int[n, m];
        
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            string[] parts = Console.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = int.Parse(parts[j]);
            }
        }

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            int rowSum = 0;
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                rowSum += matrix[i, j];
            }
            Console.WriteLine($"Лікар {i + 1}: {rowSum} прийомів");
        }
        
        int[] daySums = new int[matrix.GetLength(1)];
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            int colSum = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                colSum += matrix[i, j];
            }
            daySums[j] = colSum;
        }
        Console.WriteLine($"По днях: {string.Join(", ", daySums)}");
        
        int maxVal = matrix[0, 0];
        int maxRow = 0;
        int maxCol = 0;

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] > maxVal)
                {
                    maxVal = matrix[i, j];
                    maxRow = i;
                    maxCol = j;
                }
            }
        }

        Console.WriteLine($"Максимум: {maxVal} (Лікар {maxRow + 1}, День {maxCol + 1})");
    }
}