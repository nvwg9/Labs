namespace ClinicApp.Models;

public class UrgentAppointment : Appointment
{
    private const decimal UrgencyMultiplier = 1.5m;

    public string UrgencyNote { get; }

    public UrgentAppointment(int patientId, int doctorId, DateTime scheduledAt, string urgencyNote = "", int durationMinutes = 30)
        : base(patientId, doctorId, scheduledAt, durationMinutes)
    {
        UrgencyNote = urgencyNote;
    }

    public override decimal GetCost() => base.GetCost() * UrgencyMultiplier;

    public sealed override string GetDescription()
    {
        if (string.IsNullOrWhiteSpace(UrgencyNote))
        {
            return "Терміновий";
        }

        return $"Терміновий ({UrgencyNote})";
    }

    public new int GetPriority() => 1;
}
