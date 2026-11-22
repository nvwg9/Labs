using System.Globalization;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class SessionManager
{
    private const string PatientsSection = "[PATIENTS]";
    private const string EndSection = "[END]";

    private readonly string _sessionPath;

    public SessionManager(string sessionPath = "session.dat")
    {
        _sessionPath = sessionPath;
    }

    public bool Exists()
    {
        return File.Exists(_sessionPath);
    }

    public void Save(Clinic clinic)
    {
        using StreamWriter writer = new StreamWriter(_sessionPath, false, Encoding.UTF8);
        writer.WriteLine(PatientsSection);

        foreach (Patient p in clinic.Patients.GetAll())
        {
            writer.WriteLine($"{p.FirstName},{p.LastName},{p.DateOfBirth:dd.MM.yyyy},{p.BloodType},{p.Phone}");
        }

        writer.WriteLine(EndSection);
    }

    public int Load(Clinic clinic)
    {
        if (!Exists())
        {
            return 0;
        }

        int added = 0;
        string section = "";

        foreach (string line in File.ReadAllLines(_sessionPath, Encoding.UTF8))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.StartsWith("["))
            {
                section = line.Trim();
                continue;
            }

            if (section != PatientsSection)
            {
                continue;
            }

            try
            {
                string[] parts = line.Split(',');
                if (parts.Length != 5)
                {
                    continue;
                }

                DateTime dateOfBirth = DateTime.ParseExact(parts[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), parts[3]);

                if (PatientExists(clinic, parts[0], parts[1], dateOfBirth))
                {
                    continue;
                }

                clinic.Patients.Add(new Patient(parts[0], parts[1], dateOfBirth, bloodType, parts[4]));
                added++;
            }
            catch (FormatException)
            {
            }
            catch (ArgumentException)
            {
            }
        }

        return added;
    }

    private static bool PatientExists(Clinic clinic, string firstName, string lastName, DateTime dateOfBirth)
    {
        foreach (Patient p in clinic.Patients.GetAll())
        {
            if (p.FirstName == firstName && p.LastName == lastName && p.DateOfBirth == dateOfBirth)
            {
                return true;
            }
        }

        return false;
    }
}
