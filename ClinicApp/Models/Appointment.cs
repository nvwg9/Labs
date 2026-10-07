using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Appointment : IPayable
{
    private static int _nextId = 1;
    private const decimal CostPerMinute = 10m;

    private int _durationMinutes;
    private bool _isPaid;

    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }

    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes
    {
        get => _durationMinutes;
        set
        {
            ClinicValidator.ValidatePositive(value, nameof(DurationMinutes));
            _durationMinutes = value;
        }
    }
    
    public AppointmentStatus Status { get; private set; } 
    public string Notes { get; private set; }
    
    public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);
    
    public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled;

    public bool IsPaid => _isPaid;
    
    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        Status = AppointmentStatus.Scheduled;
        Notes = "";
        Id = _nextId++;
    }
    
    public bool Cancel(string reason = "")
    {
        if (Status == AppointmentStatus.Scheduled) 
        {
            Status = AppointmentStatus.Cancelled; 
            if (!string.IsNullOrEmpty(reason))
            {
                Notes = reason;
            }
            return true;
        }

        return false;
    }
    
    public decimal GetCost() => DurationMinutes * CostPerMinute;

    public void MarkPaid()
    {
        if (Status != AppointmentStatus.Cancelled)
        {
            _isPaid = true;
        }
    }
    
    public bool Complete()
    {
        if (Status == AppointmentStatus.Scheduled) 
        {
            Status = AppointmentStatus.Completed; 
            return true;
        }

        return false;
    }
    
    public override string ToString()
    {
        string timeFormat = $"{ScheduledAt:dd.MM.yyyy HH:mm}–{EndsAt:HH:mm}";
        string result = $"[{Id}] Пацієнт #{PatientId} -> Лікар #{DoctorId} | {timeFormat} | {Status}";

        if (Notes.Length > 0)
        {
            result += $" | {Notes}";
        }

        return result;
    }
}