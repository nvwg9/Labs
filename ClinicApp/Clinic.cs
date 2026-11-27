using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp;

public class Clinic
{
    public string Name { get; set; }
    
    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }
    public MedicalRecordManager MedicalRecords { get; }
    public BillingManager Billing { get; }
    public WaitingQueue<Patient> WaitingRoom { get; }
    public AnalyticsManager Analytics { get; }
    public TreatmentPlanManager TreatmentPlans { get; }
    public ClinicLogger Logger { get; }
    public ClinicExporter Exporter { get; }
    public SessionManager Session { get; }
    public PatientPassportWriter Passport { get; }
    public SessionEventTracker Tracker { get; }

    public Clinic(string name)
    {
        Name = name;
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
        MedicalRecords = new MedicalRecordManager();
        Billing = new BillingManager(Appointments);
        WaitingRoom = new WaitingQueue<Patient>();
        Analytics = new AnalyticsManager(Appointments, Doctors, Patients);
        TreatmentPlans = new TreatmentPlanManager();
        Logger = new ClinicLogger();
        Exporter = new ClinicExporter(this);
        Session = new SessionManager();
        Passport = new PatientPassportWriter(this);
        Tracker = new SessionEventTracker(this);
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        Patients.PatientAdded += Logger.OnPatientAdded;
        Appointments.AppointmentBooked += Logger.OnAppointmentBooked;
        Appointments.UrgentAppointmentBooked += Logger.OnUrgentAppointmentBooked;
        Appointments.AppointmentCancelled += Logger.OnAppointmentCancelled;
        Appointments.AppointmentCompleted += Logger.OnAppointmentCompleted;
        Billing.PaymentReceived += Logger.OnPaymentReceived;
        TreatmentPlans.PlanCompleted += Logger.OnPlanCompleted;

        Patients.PatientAdded += Passport.OnPatientAdded;
        Appointments.AppointmentCompleted += Passport.OnAppointmentCompleted;
        TreatmentPlans.PlanCompleted += Passport.OnPlanCompleted;

        Patients.PatientAdded += Tracker.OnPatientAdded;
        Appointments.AppointmentBooked += Tracker.OnAppointmentBooked;
        Appointments.UrgentAppointmentBooked += Tracker.OnUrgentAppointmentBooked;
        Appointments.AppointmentCancelled += Tracker.OnAppointmentCancelled;
        Appointments.AppointmentCompleted += Tracker.OnAppointmentCompleted;
        Billing.PaymentReceived += Tracker.OnPaymentReceived;
        TreatmentPlans.PlanCompleted += Tracker.OnPlanCompleted;
    }
    
    public void DisplaySchedule(DateTime date)
    {
        Console.WriteLine($"=== Розклад на {date:dd.MM.yyyy} ===");
        var schedule = Appointments.GetByDate(date);
        Appointments.DisplayList(schedule);
    }

 
    public void GenerateReport()
    {
        var upcoming = Appointments.GetUpcoming();
        var doctors = Doctors.GetAll();

        const int innerWidth = 58;

        Console.WriteLine("╔" + new string('═', innerWidth + 2) + "╗");
        PrintBoxLine($"Звіт — {Name}", innerWidth);
        Console.WriteLine("╠" + new string('═', innerWidth + 2) + "╣");
        PrintBoxLine($"Пацієнтів:        {Patients.Count}", innerWidth);
        PrintBoxLine($"Лікарів:          {Doctors.Count}", innerWidth);
        PrintBoxLine($"Майбутніх записів: {upcoming.Length}", innerWidth);
        Console.WriteLine("╠" + new string('═', innerWidth + 2) + "╣");
        PrintBoxLine("Навантаження лікарів (майбутні записи):", innerWidth);
        
        for (int i = 0; i < doctors.Length; i++)
        {
            int docId = doctors[i].Id;
            int count = 0;

            for (int j = 0; j < upcoming.Length; j++)
            {
                if (upcoming[j].DoctorId == docId)
                {
                    count++;
                }
            }

            PrintBoxLine($"  {doctors[i].FullName} ({doctors[i].Speciality}): {count} записів", innerWidth);
        }

        Console.WriteLine("╚" + new string('═', innerWidth + 2) + "╝");
    }
    
    private void PrintBoxLine(string text, int width)
    {
        Console.WriteLine($"║ {text.PadRight(width)} ║");
    }
}