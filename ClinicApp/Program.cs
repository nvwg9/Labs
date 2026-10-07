using System.Globalization;
using System.Text;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

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

DateTime todayDate = DateTime.Today;
clinic.MedicalRecords.Add(new Diagnosis(1, 1, todayDate.AddDays(-30), "I10", "Гіпертонічна хвороба", isChronic: true));
clinic.MedicalRecords.Add(new Diagnosis(1, 1, todayDate.AddDays(-5), "J06.9", "Гострий ринофарингіт"));
clinic.MedicalRecords.Add(new LabResult(1, 1, todayDate.AddDays(-3), "Гемоглобін", 145, "г/л", "120–160", isNormal: true));
clinic.MedicalRecords.Add(new LabResult(1, 1, todayDate.AddDays(-3), "Холестерин", 6.2, "ммоль/л", "< 5.2", isNormal: false));
clinic.MedicalRecords.Add(new Prescription(1, 1, todayDate.AddDays(-5), "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));
clinic.MedicalRecords.Add(new Prescription(2, 2, todayDate.AddDays(-40), "Амоксицилін", "500 мг", 10, "3 рази на добу"));
clinic.MedicalRecords.Add(new Diagnosis(2, 2, todayDate.AddDays(-20), "G43", "Мігрень"));
clinic.MedicalRecords.Add(new LabResult(3, 3, todayDate.AddDays(-2), "Глюкоза", 4.8, "ммоль/л", "3.3–5.5", isNormal: true));

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

Console.WriteLine("=== Демонстрація Завдання 3 ===");
Console.WriteLine(ClinicFormatter.FormatBloodType(BloodType.APositive));
Console.WriteLine(ClinicFormatter.FormatAge(1));
Console.WriteLine(ClinicFormatter.FormatAge(3));
Console.WriteLine(ClinicFormatter.FormatAge(11));

Patient? first = clinic.Patients[0];
Doctor? second = clinic.Doctors[1];
Console.WriteLine($"Індексатор пацієнта [0]: {first?.FullName}");
Console.WriteLine($"Індексатор лікаря [1]: {second?.FullName}");
Console.WriteLine("==============================\n");

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

Console.WriteLine("=== Демонстрація Лаби 05 (валідація) ===");
try
{
    new Patient("", "Петренко", new DateTime(1990, 1, 1), BloodType.APositive, "0501234567");
}
catch (ArgumentException e)
{
    Console.WriteLine($"Порожнє ім'я -> {e.GetType().Name}: {e.Message}");
}

try
{
    new Patient("Іван", "Петренко", DateTime.Today.AddDays(1), BloodType.APositive, "0501234567");
}
catch (ArgumentOutOfRangeException e)
{
    Console.WriteLine($"Дата народження «завтра» -> {e.GetType().Name}: {e.Message}");
}

try
{
    new WorkSchedule(20, 6);
}
catch (ArgumentOutOfRangeException e)
{
    Console.WriteLine($"WorkSchedule(20, 6) -> {e.GetType().Name}: {e.Message}");
}
catch (ArgumentException e)
{
    Console.WriteLine($"WorkSchedule(20, 6) -> {e.GetType().Name}: {e.Message}");
}

for (int i = 0; i < 2; i++)
{
    try
    {
        clinic.Patients.Add(new Patient("", "Помилковий"));
    }
    catch (ArgumentException e)
    {
        Console.WriteLine($"Невдала спроба #{i + 1}: {e.Message}");
    }
}
clinic.Patients.Add(new Patient("Софія", "Мельник", new DateTime(2001, 3, 15), BloodType.ABPositive, "0975556677"));
Console.WriteLine("=========================================\n");

Console.WriteLine("=== Демонстрація Лаби 06 (поліморфізм) ===");
MedicalRecord[] patientRecords = clinic.MedicalRecords.GetByPatient(1);
for (int i = 0; i < patientRecords.Length; i++)
{
    Console.WriteLine($"{patientRecords[i].GetRecordType()}: {patientRecords[i].GetSummary()}");
}
Console.WriteLine("==========================================\n");

RunMainMenu(clinic);

static void RunMainMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("╔══════════════════════════════════════════════╗");
        Console.WriteLine("║           МЕДИЧНА КЛІНІКА                    ║");
        Console.WriteLine("╠══════════════════════════════════════════════╣");
        Console.WriteLine("║  1. Пацієнти       — реєстрація, пошук       ║");
        Console.WriteLine("║  2. Лікарі         — персонал, розклад       ║");
        Console.WriteLine("║  3. Записи         — прийоми, скасування     ║");
        Console.WriteLine("║  4. Медична картка — діагнози, рецепти       ║");
        Console.WriteLine("║  5. Рахунки        — оплата, борги           ║");
        Console.WriteLine("║  6. Звіт           — загальна статистика     ║");
        Console.WriteLine("║  7. Розклад        — прийоми на день         ║");
        Console.WriteLine("║  8. Тест масиву    — GrowablePatientManager  ║");
        Console.WriteLine("║  0. Вийти                                    ║");
        Console.WriteLine("╚══════════════════════════════════════════════╝");
        Console.Write("Оберіть розділ: ");

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
                RunMedicalRecordMenu(clinic);
                break;
            case "5":
                RunBillingMenu(clinic);
                break;
            case "6":
                clinic.GenerateReport();
                break;
            case "7":
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
            case "8":
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
                try
                {
                    Console.Write("Ім'я: ");
                    string fn = Console.ReadLine() ?? "";
                    Console.Write("Прізвище: ");
                    string ln = Console.ReadLine() ?? "";

                    Console.Write("Дата народження (рррр-мм-дд): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime dob))
                    {
                        Console.WriteLine("Некоректний формат дати.");
                        break;
                    }

                    Console.WriteLine("Група крові: 0 - Невідомо, 1 - A+, 2 - A-, 3 - B+, 4 - B-, 5 - AB+, 6 - AB-, 7 - 0+, 8 - 0-");
                    Console.Write("Введіть номер (0-8): ");
                    int.TryParse(Console.ReadLine(), out int btNum);
                    if (!Enum.IsDefined((BloodType)btNum))
                        throw new ArgumentOutOfRangeException(nameof(BloodType), "Невідомий номер групи крові.");

                    Console.Write("Телефон (10 цифр): ");
                    string phone = Console.ReadLine() ?? "";

                    clinic.Patients.Add(new Patient(fn, ln, dob, (BloodType)btNum, phone));
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
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
        Console.WriteLine("5. Вільні години лікаря");
        Console.WriteLine("6. Статистика");
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
                try
                {
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
                    if (!Enum.IsDefined((Speciality)specNum))
                        throw new ArgumentOutOfRangeException(nameof(Speciality), "Невідомий номер спеціальності.");
                    Speciality sp = (Speciality)specNum;

                    Console.Write("Номер ліцензії: ");
                    string license = Console.ReadLine() ?? "";
                    Console.Write("Телефон (10 цифр): ");
                    string phone = Console.ReadLine() ?? "";

                    Console.Write("Година початку роботи (0-23, за замовчуванням 8): ");
                    int startH = int.TryParse(Console.ReadLine(), out int parsedStart) ? parsedStart : 8;
                    Console.Write("Година завершення роботи (1-24, за замовчуванням 17): ");
                    int endH = int.TryParse(Console.ReadLine(), out int parsedEnd) ? parsedEnd : 17;

                    var schedule = new WorkSchedule(startH, endH);
                    var newDoctor = new Doctor(fn, ln, sp, license, phone) { Schedule = schedule };

                    clinic.Doctors.Add(newDoctor);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
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
            {
                Console.Write("ID лікаря: ");
                if (!int.TryParse(Console.ReadLine(), out int doctorId))
                {
                    Console.WriteLine("Некоректне число.");
                    break;
                }
                Doctor? doctor = clinic.Doctors.FindById(doctorId);
                if (doctor == null)
                {
                    Console.WriteLine("Лікаря не знайдено.");
                    break;
                }
                ISchedulable schedulable = doctor;

                Console.Write("Дата (dd.MM.yyyy): ");
                if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    Console.WriteLine("Некоректний формат дати.");
                    break;
                }
                Console.Write("Скільки слотів: ");
                if (!int.TryParse(Console.ReadLine(), out int slotCount))
                {
                    Console.WriteLine("Некоректне число.");
                    break;
                }

                try
                {
                    DateTime[] slots = schedulable.GetAvailableSlots(date, slotCount);
                    for (int i = 0; i < slots.Length; i++)
                    {
                        Console.WriteLine(slots[i].ToString("HH:mm"));
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                break;
            }
            case "6":
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
        Console.WriteLine("6. Скасувати всі записи пацієнта");
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
                    try
                    {
                        clinic.Appointments.Book(pId, dId, dt, dur);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine("Помилка: " + e.Message);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine("Помилка: " + e.Message);
                    }
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
            case "6":
            {
                Console.Write("ID пацієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int cancelPatientId))
                {
                    Console.WriteLine("Некоректне число.");
                    break;
                }
                Console.Write("Причина (Enter — без причини): ");
                string cancelReason = Console.ReadLine() ?? "";

                int cancelled = AppointmentManager.CancelAll(clinic.Appointments.GetByPatient(cancelPatientId), cancelReason);
                Console.WriteLine($"Скасовано записів: {cancelled}");
                break;
            }
            case "0":
                return;
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}

static void RunMedicalRecordMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine("\n--- Меню «Медична картка» ---");
        Console.WriteLine("1. Картка пацієнта");
        Console.WriteLine("2. Усі записи пацієнта");
        Console.WriteLine("3. Додати діагноз");
        Console.WriteLine("4. Додати аналіз");
        Console.WriteLine("5. Додати рецепт");
        Console.WriteLine("6. Записи лікаря");
        Console.WriteLine("0. Назад до головного меню");
        Console.Write("Оберіть дію: ");

        string? choice = Console.ReadLine();
        Console.WriteLine();

        switch (choice)
        {
            case "1":
            {
                if (TryReadPatientId(clinic, out int patientId))
                {
                    clinic.MedicalRecords.DisplayPatientSummary(patientId);
                }
                break;
            }
            case "2":
            {
                if (TryReadPatientId(clinic, out int patientId))
                {
                    clinic.MedicalRecords.DisplayList(clinic.MedicalRecords.GetByPatient(patientId));
                }
                break;
            }
            case "3":
            {
                clinic.Patients.DisplayAll();
                clinic.Doctors.DisplayAll();
                if (!TryReadPatientId(clinic, out int patientId) || !TryReadDoctorId(clinic, out int doctorId))
                    break;

                Console.Write("Код діагнозу (напр. J06.9): ");
                string code = Console.ReadLine() ?? "";
                Console.Write("Опис: ");
                string description = Console.ReadLine() ?? "";
                if (!TryReadYesNo("Хронічне? (1=так, 0=ні): ", out bool isChronic))
                    break;

                try
                {
                    clinic.MedicalRecords.Add(new Diagnosis(patientId, doctorId, DateTime.Today, code, description, isChronic));
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                break;
            }
            case "4":
            {
                clinic.Patients.DisplayAll();
                clinic.Doctors.DisplayAll();
                if (!TryReadPatientId(clinic, out int patientId) || !TryReadDoctorId(clinic, out int doctorId))
                    break;

                Console.Write("Назва аналізу: ");
                string testName = Console.ReadLine() ?? "";
                Console.Write("Значення (число): ");
                if (!double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    Console.WriteLine("Некоректне число (дробову частину відокремлюйте крапкою).");
                    break;
                }
                Console.Write("Одиниці виміру: ");
                string unit = Console.ReadLine() ?? "";
                Console.Write("Норма (напр. 4.0–9.0): ");
                string range = Console.ReadLine() ?? "";
                if (!TryReadYesNo("В нормі? (1=так, 0=ні): ", out bool isNormal))
                    break;

                try
                {
                    clinic.MedicalRecords.Add(new LabResult(patientId, doctorId, DateTime.Today, testName, value, unit, range, isNormal));
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                break;
            }
            case "5":
            {
                clinic.Patients.DisplayAll();
                clinic.Doctors.DisplayAll();
                if (!TryReadPatientId(clinic, out int patientId) || !TryReadDoctorId(clinic, out int doctorId))
                    break;

                Console.Write("Препарат: ");
                string medication = Console.ReadLine() ?? "";
                Console.Write("Дозування (напр. 10 мг): ");
                string dosage = Console.ReadLine() ?? "";
                Console.Write("Кількість днів: ");
                if (!int.TryParse(Console.ReadLine(), out int days))
                {
                    Console.WriteLine("Некоректне число.");
                    break;
                }
                Console.Write("Інструкція (Enter — пропустити): ");
                string instructions = Console.ReadLine() ?? "";

                try
                {
                    clinic.MedicalRecords.Add(new Prescription(patientId, doctorId, DateTime.Today, medication, dosage, days, instructions));
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
                break;
            }
            case "6":
            {
                if (TryReadDoctorId(clinic, out int doctorId))
                {
                    clinic.MedicalRecords.DisplayList(clinic.MedicalRecords.GetByDoctor(doctorId));
                }
                break;
            }
            case "0":
                return;
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}

static bool TryReadPatientId(Clinic clinic, out int patientId)
{
    Console.Write("ID пацієнта: ");
    if (!int.TryParse(Console.ReadLine(), out patientId))
    {
        Console.WriteLine("Некоректне число.");
        return false;
    }
    if (!clinic.Patients.TryFindById(patientId, out _))
    {
        Console.WriteLine($"Пацієнта з ID {patientId} не знайдено.");
        return false;
    }
    return true;
}

static bool TryReadDoctorId(Clinic clinic, out int doctorId)
{
    Console.Write("ID лікаря: ");
    if (!int.TryParse(Console.ReadLine(), out doctorId))
    {
        Console.WriteLine("Некоректне число.");
        return false;
    }
    if (!clinic.Doctors.TryFindById(doctorId, out _))
    {
        Console.WriteLine($"Лікаря з ID {doctorId} не знайдено.");
        return false;
    }
    return true;
}

static bool TryReadYesNo(string prompt, out bool answer)
{
    Console.Write(prompt);
    string? input = Console.ReadLine();
    answer = input == "1";
    if (input != "1" && input != "0")
    {
        Console.WriteLine("Некоректний вибір: введіть 1 або 0.");
        return false;
    }
    return true;
}

static void RunBillingMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine("\n── Рахунки ───────────────────");
        Console.WriteLine("  1. Борги пацієнта");
        Console.WriteLine("  2. Всі неоплачені записи");
        Console.WriteLine("  3. Оплатити запис");
        Console.WriteLine("  4. Загальний борг клініки");
        Console.WriteLine("  0. Назад");
        Console.Write("Оберіть: ");

        string? choice = Console.ReadLine();
        Console.WriteLine();

        switch (choice)
        {
            case "1":
            {
                Console.Write("ID пацієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int patientId))
                {
                    Console.WriteLine("Некоректне число.");
                    break;
                }
                IPayable[] unpaid = clinic.Billing.GetUnpaidByPatient(patientId);
                Console.WriteLine($"Неоплачені записи пацієнта #{patientId}:");
                clinic.Billing.DisplayUnpaid(unpaid);
                Console.WriteLine($"Борг: {clinic.Billing.GetPatientDebt(patientId):F2} грн");
                break;
            }
            case "2":
            {
                IPayable[] unpaid = clinic.Billing.GetAllUnpaid();
                clinic.Billing.DisplayUnpaid(unpaid);
                break;
            }
            case "3":
            {
                Console.Write("ID запису для оплати: ");
                if (!int.TryParse(Console.ReadLine(), out int appointmentId))
                {
                    Console.WriteLine("Некоректне число.");
                    break;
                }
                if (clinic.Billing.PayAppointment(appointmentId))
                    Console.WriteLine($"Запис [{appointmentId}] оплачено.");
                else
                    Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                break;
            }
            case "4":
                Console.WriteLine($"Загальний борг по клініці: {clinic.Billing.GetTotalDebt():F2} грн");
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
