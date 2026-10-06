using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Doctor
{
    private static int _nextId = 1;

    private string _firstName = "";
    private string _lastName = "";
    private string _licenseNumber = "";
    private string _phone = "";

    public int Id { get; }

    public string FirstName
    {
        get => _firstName;
        set => _firstName = value;
    }

    public string LastName
    {
        get => _lastName;
        set => _lastName = value;
    }

    public Speciality Speciality { get; set; }

    public string LicenseNumber
    {
        get => _licenseNumber;
        set => _licenseNumber = value;
    }

    public string Phone
    {
        get => _phone;
        set => _phone = value;
    }
    
    public WorkSchedule Schedule { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
    
    public bool IsAvailableNow => Schedule.IsNow;
    public bool CanAcceptAt(int hour) => Schedule.Contains(hour);
    
    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;
        Schedule = new WorkSchedule(8, 17); 
    }
    
    public Doctor(string firstName, string lastName, Speciality speciality)
        : this(firstName, lastName, speciality, "LIC-000", "0000000000")
    {
    }
    
    public Doctor()
        : this("Невідомий", "Лікар", Speciality.General)
    {
    }
    
    public override string ToString()
    {
        string status = IsAvailableNow ? "доступний" : "не в робочий час";
        return $"[{Id}] {FullName} | {ClinicFormatter.FormatSpeciality(Speciality)} | {LicenseNumber} | Тел: {ClinicFormatter.FormatPhone(Phone)} | {Schedule} | {status}";
    }
}