namespace ClinicApp.Models;

public sealed class SpecialistAppointment : Appointment
{
    private const decimal SpecialistMultiplier = 1.3m;

    public string ConsultationTopic { get; }

    public SpecialistAppointment(int patientId, int doctorId, DateTime scheduledAt, string topic = "", int durationMinutes = 45)
        : base(patientId, doctorId, scheduledAt, durationMinutes)
    {
        ConsultationTopic = topic;
    }

    public override decimal GetCost() => base.GetCost() * SpecialistMultiplier;

    public override string GetDescription()
    {
        if (string.IsNullOrWhiteSpace(ConsultationTopic))
        {
            return "Консультація спеціаліста";
        }

        return $"Консультація спеціаліста: {ConsultationTopic}";
    }
}
