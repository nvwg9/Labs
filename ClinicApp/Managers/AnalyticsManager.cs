using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AnalyticsManager
{
    private readonly AppointmentManager _appointments;
    private readonly DoctorManager _doctors;
    private readonly PatientManager _patients;

    public AnalyticsManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    public IEnumerable<DoctorStats> ComputeDoctorStats()
    {
        foreach (Doctor doctor in _doctors.GetAll())
        {
            Appointment[] appointments = _appointments.GetByDoctor(doctor.Id);
            decimal revenue = 0m;
            DateTime last = DateTime.MinValue;

            foreach (Appointment appointment in appointments)
            {
                revenue += appointment.GetCost();
                if (appointment.ScheduledAt > last)
                {
                    last = appointment.ScheduledAt;
                }
            }

            yield return new DoctorStats(doctor.Id, doctor.FullName, appointments.Length, revenue, last);
        }
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        foreach (Patient patient in _patients.GetAll())
        {
            Appointment[] appointments = _appointments.GetByPatient(patient.Id);
            decimal spent = 0m;
            DateTime last = DateTime.MinValue;

            foreach (Appointment appointment in appointments)
            {
                spent += appointment.GetCost();
                if (appointment.ScheduledAt > last)
                {
                    last = appointment.ScheduledAt;
                }
            }

            yield return new PatientStats(patient.Id, patient.FullName, appointments.Length, spent, last);
        }
    }
}
