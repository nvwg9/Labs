namespace Lab03;

class Program
{
    static void Main(string[] args)
    {
        PatientManager manager = new PatientManager();
        
        manager.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 14), "A+", "0501234567"));
        manager.Add(new Patient("Олена", "Коваль", new DateTime(1993, 2, 20), "B-", "0672345678"));
        manager.Add(new Patient("Максим", "Бойко", new DateTime(2010, 8, 10), "0+", "0933456789"));
        manager.Add(new Patient("Марія", "Ткач"));

        Console.WriteLine();
        RunPatientMenu(manager);
    }
    
    static void RunPatientMenu(PatientManager manager)
    {
        while (true)
        {
            Console.WriteLine("\n--- Підменю «Пацієнти» ---");
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Додати пацієнта");
            Console.WriteLine("3. Знайти за ім'ям");
            Console.WriteLine("4. Видалити за ID");
            Console.WriteLine("5. Статистика");
            Console.WriteLine("0. Вихід");
            Console.Write("Оберіть дію: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    manager.DisplayAll();
                    break;

                case "2":
                    Console.Write("Ім'я: ");
                    string fn = Console.ReadLine() ?? "";
                    Console.Write("Прізвище: ");
                    string ln = Console.ReadLine() ?? "";
                    manager.Add(new Patient(fn, ln));
                    break;

                case "3":
                    Console.Write("Введіть пошуковий запит: ");
                    string q = Console.ReadLine() ?? "";
                    var found = manager.FindByName(q);
                    if (found.Length == 0)
                    {
                        Console.WriteLine("Нікого не знайдено.");
                    }
                    else
                    {
                        Console.WriteLine($"Знайдено ({found.Length}):");
                        foreach (var p in found)
                        {
                            Console.WriteLine(p);
                        }
                    }
                    break;

                case "4":
                    Console.Write("Введіть ID для видалення: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (manager.Remove(id))
                            Console.WriteLine($"Пацієнта з ID {id} успішно видалено.");
                        else
                            Console.WriteLine($"Пацієнта з ID {id} не знайдено.");
                    }
                    break;

                case "5":
                    manager.DisplayStats();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }
}