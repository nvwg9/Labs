using System.Text;
using ClinicApp.Events;

namespace ClinicApp.Utils;

public class SessionEventTracker
{
    private readonly Clinic _clinic;

    public int PatientsAdded { get; private set; }
    public int AppointmentsBooked { get; private set; }
    public int UrgentBooked { get; private set; }
    public int AppointmentsCancelled { get; private set; }
    public int AppointmentsCompleted { get; private set; }
    public int PaymentsReceived { get; private set; }
    public int PlansCompleted { get; private set; }

    public SessionEventTracker(Clinic clinic)
    {
        _clinic = clinic;
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e)
    {
        PatientsAdded++;
    }

    public void OnAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        AppointmentsBooked++;
    }

    public void OnUrgentAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        UrgentBooked++;
    }

    public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e)
    {
        AppointmentsCancelled++;

        if (!_clinic.WaitingRoom.IsEmpty)
        {
            Console.WriteLine($"[ЧЕРГА] Слот звільнився. Наступний: {_clinic.WaitingRoom.Peek().FullName}");
        }
    }

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
    {
        AppointmentsCompleted++;
    }

    public void OnPaymentReceived(object? sender, PaymentEventArgs e)
    {
        PaymentsReceived++;
    }

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
    {
        PlansCompleted++;
    }

    public void PrintSummary()
    {
        foreach (string line in BuildSummary())
        {
            Console.WriteLine(line);
        }
    }

    public void SaveSummary(string path = "session_summary.txt")
    {
        string[] lines = BuildSummary();
        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
        foreach (string line in lines)
        {
            writer.WriteLine(line);
        }
    }

    private string[] BuildSummary()
    {
        return new string[]
        {
            "=== Підсумок сесії ===",
            $"Пацієнтів додано:    {PatientsAdded}",
            $"Записів створено:    {AppointmentsBooked} (термінових: {UrgentBooked})",
            $"Скасовано:           {AppointmentsCancelled}",
            $"Завершено:           {AppointmentsCompleted}",
            $"Оплат:               {PaymentsReceived}",
            $"Планів завершено:    {PlansCompleted}"
        };
    }
}
