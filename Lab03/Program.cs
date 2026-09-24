namespace Lab03;

class Program
{
    static void Main(string[] args)
    {
        Clinic clinic = new Clinic("Медична Клініка");
        
        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 14), "A+", "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 2, 20), "B-", "0672345678"));
        clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 8, 10), "0+", "0933456789"));
        clinic.Patients.Add(new Patient("Марія", "Ткач"));
        
        var doc1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567") { WorkEndHour = 16 };
        var doc2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678") { WorkStartHour = 9, WorkEndHour = 18 };
        var doc3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");

        clinic.Doctors.Add(doc1);
        clinic.Doctors.Add(doc2);
        clinic.Doctors.Add(doc3);
        
        clinic.Appointments.Book(1, 1, new DateTime(2026, 5, 9, 10, 0, 0), 30);
        clinic.Appointments.Book(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        clinic.Appointments.Book(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        Console.WriteLine("\n--- Перевірка роботи за зразком виводу ---\n");
        clinic.DisplaySchedule(new DateTime(2026, 5, 9));
        Console.WriteLine();
        clinic.GenerateReport();
        
        RunMainMenu(clinic);
    }

    static void RunMainMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n=== ГОЛОВНЕ МЕНЮ КЛІНІКИ ===");
            Console.WriteLine("1. Пацієнти");
            Console.WriteLine("2. Лікарі");
            Console.WriteLine("3. Записи");
            Console.WriteLine("4. Розклад на день");
            Console.WriteLine("5. Підсумковий звіт");
            Console.WriteLine("0. Вихід");
            Console.Write("Оберіть пункт: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    RunPatientMenu(clinic);
                    break;
                case "2":
                    RunDoctorMenu(clinic);
                    break;
                case "3":
                    RunAppointmentMenu(clinic);
                    break;
                case "4":
                    Console.Write("Введіть дату (рррр-мм-дд): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime selectedDate))
                    {
                        clinic.DisplaySchedule(selectedDate);
                    }
                    else
                    {
                        Console.WriteLine("Некоректний формат дати.");
                    }
                    break;
                case "5":
                    clinic.GenerateReport();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }
    }

    static void RunPatientMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n--- Меню «Пацієнти» ---");
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Додати пацієнта");
            Console.WriteLine("3. Знайти за ім'ям");
            Console.WriteLine("4. Видалити за ID");
            Console.WriteLine("5. Статистика");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть дію: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    clinic.Patients.DisplayAll();
                    break;
                case "2":
                    Console.Write("Ім'я: ");
                    string fn = Console.ReadLine() ?? "";
                    Console.Write("Прізвище: ");
                    string ln = Console.ReadLine() ?? "";
                    clinic.Patients.Add(new Patient(fn, ln));
                    break;
                case "3":
                    Console.Write("Запит для пошуку: ");
                    var found = clinic.Patients.FindByName(Console.ReadLine() ?? "");
                    if (found.Length == 0) Console.WriteLine("Нікого не знайдено.");
                    else foreach (var p in found) Console.WriteLine(p);
                    break;
                case "4":
                    Console.Write("ID для видалення: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (clinic.Patients.Remove(id)) Console.WriteLine("Пацієнта видалено.");
                        else Console.WriteLine("Пацієнта не знайдено.");
                    }
                    break;
                case "5":
                    clinic.Patients.DisplayStats();
                    break;
                case "0":
                    return;
            }
        }
    }

    static void RunDoctorMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n--- Меню «Лікарі» ---");
            Console.WriteLine("1. Показати всіх");
            Console.WriteLine("2. Додати лікаря");
            Console.WriteLine("3. Знайти за спеціальністю");
            Console.WriteLine("4. Видалити за ID");
            Console.WriteLine("5. Статистика");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть дію: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    clinic.Doctors.DisplayAll();
                    break;
                case "2":
                    Console.Write("Ім'я: ");
                    string fn = Console.ReadLine() ?? "";
                    Console.Write("Прізвище: ");
                    string ln = Console.ReadLine() ?? "";
                    Console.Write("Спеціальність: ");
                    string sp = Console.ReadLine() ?? "";
                    clinic.Doctors.Add(new Doctor(fn, ln, sp));
                    break;
                case "3":
                    Console.Write("Спеціальність для пошуку: ");
                    var found = clinic.Doctors.FindBySpeciality(Console.ReadLine() ?? "");
                    if (found.Length == 0) Console.WriteLine("Лікарів не знайдено.");
                    else foreach (var d in found) Console.WriteLine(d);
                    break;
                case "4":
                    Console.Write("ID для видалення: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (clinic.Doctors.Remove(id)) Console.WriteLine("Лікаря видалено.");
                        else Console.WriteLine("Лікаря не знайдено.");
                    }
                    break;
                case "5":
                    clinic.Doctors.DisplayStats();
                    break;
                case "0":
                    return;
            }
        }
    }

    static void RunAppointmentMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n--- Меню «Записи» ---");
            Console.WriteLine("1. Створити запис");
            Console.WriteLine("2. Показати майбутні записи");
            Console.WriteLine("3. Скасувати запис");
            Console.WriteLine("4. Завершити прийом");
            Console.WriteLine("5. Знайти за ID пацієнта");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть дію: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Доступні пацієнти:");
                    clinic.Patients.DisplayAll();
                    Console.WriteLine("Доступні лікарі:");
                    clinic.Doctors.DisplayAll();

                    Console.Write("ID пацієнта: ");
                    int.TryParse(Console.ReadLine(), out int pId);
                    Console.Write("ID лікаря: ");
                    int.TryParse(Console.ReadLine(), out int dId);

                    Console.Write("Дата та час (рррр-мм-дд гг:хх): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime dt))
                    {
                        Console.Write("Тривалість у хвилинах (дефолт 30): ");
                        string? durStr = Console.ReadLine();
                        int dur = string.IsNullOrEmpty(durStr) ? 30 : int.Parse(durStr);
                        clinic.Appointments.Book(pId, dId, dt, dur);
                    }
                    break;
                case "2":
                    clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
                    break;
                case "3":
                    Console.Write("ID запису: ");
                    if (int.TryParse(Console.ReadLine(), out int cId))
                    {
                        Console.Write("Причина скасування: ");
                        string reason = Console.ReadLine() ?? "";
                        clinic.Appointments.Cancel(cId, reason);
                    }
                    break;
                case "4":
                    Console.Write("ID запису: ");
                    if (int.TryParse(Console.ReadLine(), out int cmpId))
                    {
                        clinic.Appointments.Complete(cmpId);
                    }
                    break;
                case "5":
                    Console.Write("ID пацієнта: ");
                    if (int.TryParse(Console.ReadLine(), out int patientId))
                    {
                        clinic.Appointments.DisplayList(clinic.Appointments.GetByPatient(patientId));
                    }
                    break;
                case "0":
                    return;
            }
        }
    }
}