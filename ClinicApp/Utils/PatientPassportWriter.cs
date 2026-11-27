using System.Text;
using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class PatientPassportWriter
{
    private readonly Clinic _clinic;
    private readonly string _passportDir;

    public PatientPassportWriter(Clinic clinic, string passportDir = "patients")
    {
        _clinic = clinic;
        _passportDir = passportDir;
        Directory.CreateDirectory(_passportDir);
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e)
    {
        WritePassport(e.PatientId);
    }

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
    {
        WritePassport(e.PatientId);
    }

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
    {
        WritePassport(e.PatientId);
    }

    private void WritePassport(int patientId)
    {
        Patient? patient = _clinic.Patients.FindById(patientId);
        if (patient == null)
        {
            return;
        }

        string path = Path.Combine(_passportDir, $"passport_{patientId}.txt");
        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);

        writer.WriteLine($"=== Паспорт пацієнта #{patient.Id} ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine();
        writer.WriteLine($"Ім'я: {patient.FullName}");
        writer.WriteLine($"Дата народження: {patient.DateOfBirth:dd.MM.yyyy} ({ClinicFormatter.FormatAge(patient.Age)})");
        writer.WriteLine($"Група крові: {ClinicFormatter.FormatBloodType(patient.BloodType)}   Телефон: {ClinicFormatter.FormatPhone(patient.Phone)}");
        writer.WriteLine();

        MedicalRecord[] records = _clinic.MedicalRecords.GetByPatient(patientId);

        writer.WriteLine("-- Діагнози --");
        foreach (MedicalRecord record in records)
        {
            if (record is Diagnosis diagnosis)
            {
                writer.WriteLine(diagnosis.GetSummary());
            }
        }

        writer.WriteLine("-- Аналізи --");
        foreach (MedicalRecord record in records)
        {
            if (record is LabResult labResult)
            {
                writer.WriteLine(labResult.GetSummary());
            }
        }

        writer.WriteLine("-- Рецепти --");
        foreach (MedicalRecord record in records)
        {
            if (record is Prescription prescription)
            {
                writer.WriteLine(prescription.GetSummary());
            }
        }

        writer.WriteLine();
        writer.WriteLine("-- Записи --");
        foreach (Appointment appointment in _clinic.Appointments.GetByPatient(patientId))
        {
            writer.WriteLine($"[{appointment.Id}] {appointment.GetDescription()} | {appointment.ScheduledAt:dd.MM.yyyy HH:mm} | {appointment.Status} | {appointment.GetCost():F2} грн");
        }

        writer.WriteLine();
        writer.WriteLine("-- Плани лікування --");
        foreach (TreatmentPlan plan in _clinic.TreatmentPlans.GetByPatient(patientId))
        {
            writer.WriteLine($"[{plan.Id}] {plan.Diagnosis} | {plan.DurationDays} днів | {plan.Status}");
        }

        writer.WriteLine();
        writer.WriteLine($"Заборгованість: {_clinic.Billing.GetPatientDebt(patientId):F2} грн");
    }
}
