namespace Lab03;

class Program
{
    static void Main(string[] args)
    {
        PatientManager patientManager = new PatientManager();
        patientManager.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 14), "A+", "0501234567"));
        patientManager.Add(new Patient("Олена", "Коваль", new DateTime(1993, 2, 20), "B-", "0672345678"));
        patientManager.Add(new Patient("Максим", "Бойко", new DateTime(2010, 8, 10), "0+", "0933456789"));

        DoctorManager doctorManager = new DoctorManager();
        var doc1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        doc1.WorkEndHour = 16;
        doctorManager.Add(doc1);

        var doc2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
        doc2.WorkStartHour = 9;
        doc2.WorkEndHour = 18;
        doctorManager.Add(doc2);

        doctorManager.Add(new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789"));
        
        AppointmentManager appManager = new AppointmentManager(patientManager, doctorManager);

        Console.WriteLine("\n--- Перевірка роботи за зразком ---");
        appManager.Book(1, 1, new DateTime(2026, 5, 9, 10, 0, 0), 30);
        appManager.Book(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        appManager.Book(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);
        appManager.Book(99, 1, new DateTime(2026, 5, 9, 12, 0, 0), 30); // Помилка: пацієнта з ID 99 не знайдено

        Console.WriteLine("\nМайбутні записи:");
        appManager.DisplayList(appManager.GetUpcoming());

        Console.WriteLine();
        if (appManager.Cancel(1))
        {
            Console.WriteLine("Запис [1] скасовано.");
        }

        Console.WriteLine("\nЗаписи пацієнта #2:");
        appManager.DisplayList(appManager.GetByPatient(2));
        
        RunAppointmentMenu(appManager, patientManager, doctorManager);
    }
    
    static void RunAppointmentMenu(AppointmentManager appManager, PatientManager patientManager, DoctorManager doctorManager)
    {
        while (true)
        {
            Console.WriteLine("\n--- Підменю «Записи» ---");
            Console.WriteLine("1. Створити запис");
            Console.WriteLine("2. Показати майбутні записи");
            Console.WriteLine("3. Скасувати запис");
            Console.WriteLine("4. Завершити прийом");
            Console.WriteLine("5. Знайти записи за ID пацієнта");
            Console.WriteLine("0. Вихід");
            Console.Write("Оберіть дію: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Доступні пацієнти:");
                    patientManager.DisplayAll();
                    Console.WriteLine("Доступні лікарі:");
                    doctorManager.DisplayAll();

                    Console.Write("Введіть ID пацієнта: ");
                    int.TryParse(Console.ReadLine(), out int pId);
                    Console.Write("Введіть ID лікаря: ");
                    int.TryParse(Console.ReadLine(), out int dId);

                    Console.Write("Введіть рік: ");
                    int.TryParse(Console.ReadLine(), out int y);
                    Console.Write("Введіть місяць: ");
                    int.TryParse(Console.ReadLine(), out int m);
                    Console.Write("Введіть день: ");
                    int.TryParse(Console.ReadLine(), out int d);
                    Console.Write("Введіть годину: ");
                    int.TryParse(Console.ReadLine(), out int h);
                    Console.Write("Введіть хвилини: ");
                    int.TryParse(Console.ReadLine(), out int min);

                    Console.Write("Тривалість у хвилинах (за замовчуванням 30): ");
                    string? durInput = Console.ReadLine();
                    int dur = string.IsNullOrEmpty(durInput) ? 30 : int.Parse(durInput);

                    appManager.Book(pId, dId, new DateTime(y, m, d, h, min, 0), dur);
                    break;

                case "2":
                    Console.WriteLine("Майбутні записи:");
                    appManager.DisplayList(appManager.GetUpcoming());
                    break;

                case "3":
                    Console.Write("Введіть ID запису для скасування: ");
                    if (int.TryParse(Console.ReadLine(), out int cancelId))
                    {
                        Console.Write("Причина скасування (необов'язково): ");
                        string reason = Console.ReadLine() ?? "";
                        if (appManager.Cancel(cancelId, reason))
                            Console.WriteLine($"Запис [{cancelId}] успішно скасовано.");
                        else
                            Console.WriteLine("Не вдалося скасувати (можливо, не знайдено або вже змінено статус).");
                    }
                    break;

                case "4":
                    Console.Write("Введіть ID запису для завершення: ");
                    if (int.TryParse(Console.ReadLine(), out int completeId))
                    {
                        if (appManager.Complete(completeId))
                            Console.WriteLine($"Запис [{completeId}] завершено.");
                        else
                            Console.WriteLine("Не вдалося завершити запис.");
                    }
                    break;

                case "5":
                    Console.Write("Введіть ID пацієнта: ");
                    if (int.TryParse(Console.ReadLine(), out int searchPId))
                    {
                        appManager.DisplayList(appManager.GetByPatient(searchPId));
                    }
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }
    }
}