using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Appointment : IPayable, ICancellable
{
    private static int _nextId = 1;
    private const decimal CostPerMinute = 10m;
    public const decimal VipDiscountMultiplier = 0.8m;

    private int _durationMinutes;
    private bool _isPaid;
    private decimal _discountMultiplier = 1m;

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

    public bool IsCancelled => Status == AppointmentStatus.Cancelled;

    public string CancellationReason => IsCancelled ? Notes : "";
    
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
    
    public decimal DiscountMultiplier
    {
        get => _discountMultiplier;
        set
        {
            if (value <= 0m || value > 1m)
                throw new ArgumentOutOfRangeException(nameof(DiscountMultiplier), "Множник знижки має бути в межах (0; 1].");
            _discountMultiplier = value;
        }
    }

    public virtual decimal GetCost() => DurationMinutes * CostPerMinute * DiscountMultiplier;

    public virtual string GetDescription() => "Прийом";

    public int GetPriority() => 3;

    public void MarkPaid()
    {
        if (!IsCancelled)
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
        string result = $"[{Id}] {GetDescription()} | Пацієнт #{PatientId} → Лікар #{DoctorId} | {timeFormat} | {Status}";

        if (Notes.Length > 0)
        {
            result += $" | {Notes}";
        }

        result += $" | {GetCost():F2} грн";
        return result;
    }
}