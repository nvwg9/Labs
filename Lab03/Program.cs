namespace Lab03;

class Program
{
    static void Main(string[] args)
    {
        DoctorManager manager = new DoctorManager();
        
        var doc1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        doc1.WorkEndHour = 16;
        manager.Add(doc1);

        var doc2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
        doc2.WorkStartHour = 9;
        doc2.WorkEndHour = 18;
        manager.Add(doc2);

        manager.Add(new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789"));

        Console.WriteLine();
        RunDoctorMenu(manager);
    }
    
    static void RunDoctorMenu(DoctorManager manager)
    {
        while (true)
        {
            Console.WriteLine("\n--- Підменю «Лікарі» ---");
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Додати лікаря");
            Console.WriteLine("3. Знайти за спеціальністю");
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
                    Console.Write("Спеціальність: ");
                    string spec = Console.ReadLine() ?? "";

                    var newDoctor = new Doctor(fn, ln, spec);

                    Console.Write("Година початку роботи (0-23, за замовчуванням 8): ");
                    if (int.TryParse(Console.ReadLine(), out int startHour) && startHour >= 0 && startHour <= 23)
                    {
                        newDoctor.WorkStartHour = startHour;
                    }

                    Console.Write("Година завершення роботи (0-23, за замовчуванням 17): ");
                    if (int.TryParse(Console.ReadLine(), out int endHour) && endHour >= 0 && endHour <= 23)
                    {
                        newDoctor.WorkEndHour = endHour;
                    }

                    manager.Add(newDoctor);
                    break;

                case "3":
                    Console.Write("Введіть спеціальність для пошуку: ");
                    string q = Console.ReadLine() ?? "";
                    var found = manager.FindBySpeciality(q);
                    if (found.Length == 0)
                    {
                        Console.WriteLine("Лікарів такої спеціальності не знайдено.");
                    }
                    else
                    {
                        Console.WriteLine($"Знайдено ({found.Length}):");
                        foreach (var d in found)
                        {
                            Console.WriteLine(d);
                        }
                    }
                    break;

                case "4":
                    Console.Write("Введіть ID лікаря для видалення: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (manager.Remove(id))
                            Console.WriteLine($"Лікаря з ID {id} успішно видалено.");
                        else
                            Console.WriteLine($"Лікаря з ID {id} не знайдено.");
                    }
                    else
                    {
                        Console.WriteLine("Некоректне введення ID.");
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