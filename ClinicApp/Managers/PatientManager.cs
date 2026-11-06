using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class PatientManager
{
    private readonly List<Patient> _patients = new List<Patient>();

    public int Count => _patients.Count;

    public Patient? this[int index]
    {
        get
        {
            if (index >= 0 && index < _patients.Count)
            {
                return _patients[index];
            }
            return null;
        }
    }
    
    public void Add(Patient patient)
    {
        _patients.Add(patient);
        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }

        return null;
    }
    
    public bool TryFindById(int id, out Patient? patient)
    {
        patient = FindById(id);
        return patient != null;
    }

    public Patient[] FindByName(string query)
    {
        string lowerQuery = query.ToLower();
        List<Patient> matches = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(lowerQuery) ||
                _patients[i].LastName.ToLower().Contains(lowerQuery))
            {
                matches.Add(_patients[i]);
            }
        }

        return matches.ToArray();
    }
    
    public Patient[] FindByBloodType(BloodType bloodType)
    {
        List<Patient> matches = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].BloodType == bloodType)
            {
                matches.Add(_patients[i]);
            }
        }

        return matches.ToArray();
    }

    public Patient[] GetAll()
    {
        return _patients.ToArray();
    }
    
    public bool Remove(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                _patients.RemoveAt(i);
                return true;
            }
        }

        return false;
    }
    
    public void DisplayAll()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"=== Пацієнти ({_patients.Count}) ===");
        for (int i = 0; i < _patients.Count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
        Console.WriteLine(new string('-', 50));
    }

    public void DisplayStats()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Статистика недоступна: список порожній.");
            return;
        }

        int sumAges = 0;
        int youngestIndex = 0;
        int oldestIndex = 0;
        int adultCount = 0;

        for (int i = 0; i < _patients.Count; i++)
        {
            int age = _patients[i].Age;
            sumAges += age;

            if (age < _patients[youngestIndex].Age)
            {
                youngestIndex = i;
            }

            if (age > _patients[oldestIndex].Age)
            {
                oldestIndex = i;
            }

            if (_patients[i].IsAdult)
            {
                adultCount++;
            }
        }

        double averageAge = (double)sumAges / _patients.Count;

        Console.WriteLine("=== Статистика пацієнтів ===");
        Console.WriteLine($"Всього:        {_patients.Count}");
        Console.WriteLine($"Середній вік:  {averageAge:F1} р.");
        Console.WriteLine($"Наймолодший:   {_patients[youngestIndex].FullName} ({_patients[youngestIndex].Age} р.)");
        Console.WriteLine($"Найстарший:    {_patients[oldestIndex].FullName} ({_patients[oldestIndex].Age} р.)");
        Console.WriteLine($"Дорослих:      {adultCount} з {_patients.Count}");
        Console.WriteLine("============================");
    }
}