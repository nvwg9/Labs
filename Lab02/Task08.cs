namespace Lab02;

public class Task08
{
    public static void Run()
    {
        int d = int.Parse(Console.ReadLine()!);
        int w = int.Parse(Console.ReadLine()!);
        
        int[,,] data = new int[d, w, 2];
        
        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                for (int k = 0; k < data.GetLength(2); k++)
                {
                    data[i, j, k] = int.Parse(Console.ReadLine()!);
                }
            }
        }
        
        int[] deptTotals = new int[data.GetLength(0)];
        
        for (int i = 0; i < data.GetLength(0); i++)
        {
            Console.WriteLine($"Відділення {i + 1}:");
            int currentDeptTotal = 0;

            for (int j = 0; j < data.GetLength(1); j++)
            {
                int morning = data[i, j, 0];
                int evening = data[i, j, 1];
                int weekTotal = morning + evening;

                currentDeptTotal += weekTotal;

                Console.WriteLine($"  Тиждень {j + 1}: ранок {morning}, вечір {evening} -> разом {weekTotal}");
            }

            deptTotals[i] = currentDeptTotal;
            Console.WriteLine($"  Разом: {currentDeptTotal} пацієнтів");
        }
        
        int maxDeptIdx = 0;
        for (int i = 1; i < deptTotals.Length; i++)
        {
            if (deptTotals[i] > deptTotals[maxDeptIdx])
            {
                maxDeptIdx = i;
            }
        }

        Console.WriteLine($"Найзавантаженіше: Відділення {maxDeptIdx + 1} ({deptTotals[maxDeptIdx]} пацієнтів)");
    }
}