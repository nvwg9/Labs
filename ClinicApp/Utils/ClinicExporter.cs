using System.Text;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class ClinicExporter
{
    private const string Separator = "----------------------------------------";

    private readonly Clinic _clinic;
    private readonly string _baseDir;

    public ClinicExporter(Clinic clinic, string baseDir = "reports")
    {
        _clinic = clinic;
        _baseDir = baseDir;
    }

    public string ExportPatients()
    {
        Patient[] patients = _clinic.Patients.GetAll();
        string path = PrepareFile("patients.txt");

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        WriteHeader(writer, "Пацієнти");
        for (int i = 0; i < patients.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {patients[i]}");
        }
        WriteFooter(writer, $"Всього: {patients.Length}");

        return path;
    }

    public string ExportAppointments()
    {
        Appointment[] appointments = _clinic.Appointments.GetAll();
        string path = PrepareFile("appointments.txt");

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        WriteHeader(writer, "Записи");
        for (int i = 0; i < appointments.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {appointments[i]}");
        }
        WriteFooter(writer, $"Всього: {appointments.Length}");

        return path;
    }

    public string ExportBilling()
    {
        IPayable[] unpaid = _clinic.Billing.GetAllUnpaid();
        string path = PrepareFile("billing.txt");

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        WriteHeader(writer, "Неоплачені записи");
        for (int i = 0; i < unpaid.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {unpaid[i]}");
        }
        WriteFooter(writer, $"Всього неоплачених: {unpaid.Length}", $"Загальний борг: {_clinic.Billing.GetTotalDebt():F2} грн");

        return path;
    }

    public string ExportTreatmentPlans()
    {
        TreatmentPlan[] plans = _clinic.TreatmentPlans.GetAll();
        string path = PrepareFile("treatment_plans.txt");

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        WriteHeader(writer, "Плани лікування");
        for (int i = 0; i < plans.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {plans[i]}");
        }
        WriteFooter(writer, $"Всього: {plans.Length}");

        return path;
    }

    public string[] ExportAll()
    {
        return new string[]
        {
            ExportPatients(),
            ExportAppointments(),
            ExportBilling(),
            ExportTreatmentPlans()
        };
    }

    private string PrepareFile(string fileName)
    {
        string dir = Path.Combine(_baseDir, DateTime.Now.ToString("yyyy-MM-dd"));
        Directory.CreateDirectory(dir);
        return Path.Combine(dir, fileName);
    }

    private static void WriteHeader(StreamWriter writer, string title)
    {
        writer.WriteLine($"=== {title} — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine(Separator);
    }

    private static void WriteFooter(StreamWriter writer, params string[] summary)
    {
        writer.WriteLine(Separator);
        foreach (string line in summary)
        {
            writer.WriteLine(line);
        }
    }
}
