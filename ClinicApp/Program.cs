using System.Text;
using ClinicApp;

namespace Lab03;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Clinic clinic = new Clinic("Медична Клініка");
        
        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 14), BloodType.APositive, "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 2, 20), BloodType.BNegative, "0672345678"));
        clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 8, 10), BloodType.OPositive, "0933456789"));
        clinic.Patients.Add(new Patient("Марія", "Ткач"));
        
        var doc1 = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567") 
        { 
            Schedule = new WorkSchedule(8, 16) 
        };
        var doc2 = new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678") 
        { 
            Schedule = new WorkSchedule(9, 18) 
        };
        var doc3 = new Doctor("Андрій", "Власенко", Speciality.Pediatrics, "LIC-003", "0443456789");

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
        
        Console.WriteLine("\n=== Демонстрація Завдання 2 (Value Type) ===");
        WorkSchedule morning = new WorkSchedule(8, 16);
        WorkSchedule evening = new WorkSchedule(14, 22);
        Console.WriteLine($"morning: {morning}");
        Console.WriteLine($"morning.IsNow: {morning.IsNow}");

       
        WorkSchedule copy = morning;
        copy = new WorkSchedule(10, 18); 
        Console.WriteLine($"Після зміни copy: morning = {morning} | copy = {copy}");
        Console.WriteLine("============================================\n");
        
        Console.WriteLine("=== Демонстрація Завдання 4 ===");
        
        Doctor[] cardiologists = clinic.Doctors.FindBySpeciality(Speciality.Cardiology);
        Doctor[] found = clinic.Doctors.FindBySpeciality("кардіо");
        Console.WriteLine($"Перевантаження FindBySpeciality: знайдено {cardiologists.Length} (enum) та {found.Length} (string)");
        
        Appointment[] today = clinic.Appointments.GetByDate(2026, 5, 10);
        Console.WriteLine($"Перевантаження GetByDate(2026, 5, 10): знайдено {today.Length} запис(ів)");
        
        if (clinic.Patients.TryFindById(3, out Patient? patient))
        {
            Console.WriteLine("TryFindById: знайдено -> " + patient!.FullName);
        }
        else
        {
            Console.WriteLine("TryFindById: Пацієнта не знайдено.");
        }
        
        var aPosPatients = clinic.Patients.FindByBloodType(BloodType.APositive);
        Console.WriteLine($"FindByBloodType(APositive): знайдено {aPosPatients.Length}");
        
        string name = clinic.Patients.FindById(99)?.FullName ?? "не знайдено";
        Console.WriteLine($"Демонстрація ?. та ??: Пацієнт #99 -> {name}");
        Console.WriteLine("==============================\n");

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
            Console.WriteLine("6. Тест GrowablePatientManager");
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
                case "6":
                    TestGrowablePatientManager();
                    break;
                case "0":
                    Console.WriteLine("Роботу програми завершено.");
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
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
            Console.WriteLine("0. Назад до головного меню");
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
                    if (found.Length == 0)
                    {
                        Console.WriteLine("Нікого не знайдено.");
                    }
                    else
                    {
                        foreach (var p in found)
                        {
                            Console.WriteLine(p);
                        }
                    }
                    break;
                case "4":
                    Console.Write("ID для видалення: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (clinic.Patients.Remove(id))
                            Console.WriteLine("Пацієнта успішно видалено.");
                        else
                            Console.WriteLine("Пацієнта з таким ID не знайдено.");
                    }
                    else
                    {
                        Console.WriteLine("Некоректний ID.");
                    }
                    break;
                case "5":
                    clinic.Patients.DisplayStats();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
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
            Console.WriteLine("0. Назад до головного меню");
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

                    Console.WriteLine("Оберіть спеціальність:");
                    Console.WriteLine("0 - General");
                    Console.WriteLine("1 - Cardiology");
                    Console.WriteLine("2 - Neurology");
                    Console.WriteLine("3 - Pediatrics");
                    Console.WriteLine("4 - Surgery");
                    Console.WriteLine("5 - Orthopedics");
                    Console.WriteLine("6 - Dermatology");
                    Console.WriteLine("7 - Emergency");
                    Console.Write("Введіть номер спеціальності (0-7): ");
    
                    int.TryParse(Console.ReadLine(), out int specNum);
                    Speciality sp = (Speciality)specNum;

                    var newDoctor = new Doctor(fn, ln, sp);

                    int startH = 8;
                    Console.Write("Година початку роботи (0-23, за замовчуванням 8): ");
                    if (int.TryParse(Console.ReadLine(), out int parsedStart) && parsedStart >= 0 && parsedStart <= 23)
                    {
                        startH = parsedStart;
                    }

                    int endH = 17;
                    Console.Write("Година завершення роботи (0-23, за замовчуванням 17): ");
                    if (int.TryParse(Console.ReadLine(), out int parsedEnd) && parsedEnd >= 0 && parsedEnd <= 23)
                    {
                        endH = parsedEnd;
                    }

                    newDoctor.Schedule = new WorkSchedule(startH, endH);

                    clinic.Doctors.Add(newDoctor);
                    break;
                case "3":
                    Console.Write("Спеціальність для пошуку: ");
                    var found = clinic.Doctors.FindBySpeciality(Console.ReadLine() ?? "");
                    if (found.Length == 0)
                    {
                        Console.WriteLine("Лікарів такої спеціальності не знайдено.");
                    }
                    else
                    {
                        foreach (var d in found)
                        {
                            Console.WriteLine(d);
                        }
                    }
                    break;
                case "4":
                    Console.Write("ID для видалення: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        if (clinic.Doctors.Remove(id))
                            Console.WriteLine("Лікаря успішно видалено.");
                        else
                            Console.WriteLine("Лікаря не знайдено.");
                    }
                    else
                    {
                        Console.WriteLine("Некоректний ID.");
                    }
                    break;
                case "5":
                    clinic.Doctors.DisplayStats();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
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
            Console.WriteLine("0. Назад до головного меню");
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

                    Console.Write("Введіть ID пацієнта: ");
                    int.TryParse(Console.ReadLine(), out int pId);
                    Console.Write("Введіть ID лікаря: ");
                    int.TryParse(Console.ReadLine(), out int dId);

                    Console.Write("Дата та час (рррр-мм-дд гг:хх): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime dt))
                    {
                        Console.Write("Тривалість у хвилинах (за замовчуванням 30): ");
                        string? durStr = Console.ReadLine();
                        int dur = int.TryParse(durStr, out int parsedDur) ? parsedDur : 30;
                        clinic.Appointments.Book(pId, dId, dt, dur);
                    }
                    else
                    {
                        Console.WriteLine("Некоректний формат дати й часу.");
                    }
                    break;
                case "2":
                    clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
                    break;
                case "3":
                    Console.Write("ID запису: ");
                    if (int.TryParse(Console.ReadLine(), out int cId))
                    {
                        Console.Write("Причина скасування (необов'язково): ");
                        string reason = Console.ReadLine() ?? "";
                        if (clinic.Appointments.Cancel(cId, reason))
                            Console.WriteLine($"Запис [{cId}] скасовано.");
                        else
                            Console.WriteLine("Не вдалося скасувати запис.");
                    }
                    break;
                case "4":
                    Console.Write("ID запису: ");
                    if (int.TryParse(Console.ReadLine(), out int cmpId))
                    {
                        if (clinic.Appointments.Complete(cmpId))
                            Console.WriteLine($"Запис [{cmpId}] завершено.");
                        else
                            Console.WriteLine("Не вдалося завершити запис.");
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
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }
    }
    
    static void TestGrowablePatientManager()
    {
        Console.WriteLine("=== Тест GrowablePatientManager ===");
        Console.WriteLine("Додаємо пацієнтів одного за одним...");

        GrowablePatientManager growableManager = new GrowablePatientManager();

        for (int i = 1; i <= 20; i++)
        {
            growableManager.Add(new Patient("Тест", $"Пацієнт{i}"));
        }

        Console.WriteLine("\nТест пошуку:");
        var p10 = growableManager.FindById(10);
        Console.WriteLine($"FindById(10) → {(p10 != null ? p10.FullName : "не знайдено")}");

        var p99 = growableManager.FindById(99);
        Console.WriteLine($"FindById(99) → {(p99 != null ? p99.FullName : "не знайдено")}");

        Console.WriteLine("\nПорівняння:");
        Console.WriteLine("PatientManager:         100 місць (фіксовано)");
        Console.WriteLine($"GrowablePatientManager:  {growableManager.Capacity} місця (зросте при потребі)");
        Console.WriteLine("===================================");
    }
}