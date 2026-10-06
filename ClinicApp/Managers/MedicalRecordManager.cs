using ClinicApp.Models;

namespace ClinicApp.Managers;

public class MedicalRecordManager
{
    private const int MaxRecords = 1000;
    private readonly MedicalRecord[] _records = new MedicalRecord[MaxRecords];
    private int _count = 0;

    public int Count => _count;

    public MedicalRecord? this[int index]
    {
        get
        {
            if (index >= 0 && index < _count)
            {
                return _records[index];
            }
            return null;
        }
    }

    public void Add(MedicalRecord record)
    {
        if (_count >= MaxRecords)
        {
            Console.WriteLine($"Помилка: досягнуто ліміту медичних записів ({MaxRecords}).");
            return;
        }

        _records[_count++] = record;
        Console.WriteLine($"Запис [{record.Id}] {record.GetRecordType()} додано.");
    }

    public MedicalRecord? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].Id == id)
            {
                return _records[i];
            }
        }

        return null;
    }

    public MedicalRecord[] GetByPatient(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
                matches++;
        }

        MedicalRecord[] result = new MedicalRecord[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
                result[index++] = _records[i];
        }

        return result;
    }

    public MedicalRecord[] GetByDoctor(int doctorId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
                matches++;
        }

        MedicalRecord[] result = new MedicalRecord[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
                result[index++] = _records[i];
        }

        return result;
    }

    public Diagnosis[] GetDiagnoses(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis)
                matches++;
        }

        Diagnosis[] result = new Diagnosis[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis diagnosis)
                result[index++] = diagnosis;
        }

        return result;
    }

    public LabResult[] GetLabResults(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult)
                matches++;
        }

        LabResult[] result = new LabResult[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult labResult)
                result[index++] = labResult;
        }

        return result;
    }

    public Prescription[] GetPrescriptions(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription)
                matches++;
        }

        Prescription[] result = new Prescription[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription prescription)
                result[index++] = prescription;
        }

        return result;
    }

    public Diagnosis[] GetChronicDiagnoses(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis d && d.IsChronic)
                matches++;
        }

        Diagnosis[] result = new Diagnosis[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis d && d.IsChronic)
                result[index++] = d;
        }

        return result;
    }

    public Prescription[] GetActivePrescriptions(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription p && p.IsActive())
                matches++;
        }

        Prescription[] result = new Prescription[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription p && p.IsActive())
                result[index++] = p;
        }

        return result;
    }

    public void DisplayPatientSummary(int patientId)
    {
        MedicalRecord[] records = GetByPatient(patientId);

        Console.WriteLine($"=== Медична картка пацієнта #{patientId} ===");
        if (records.Length == 0)
        {
            Console.WriteLine("Медичних записів немає.");
            return;
        }

        int diagnoses = 0;
        int labResults = 0;
        int prescriptions = 0;
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is Diagnosis)
                diagnoses++;
            else if (records[i] is LabResult)
                labResults++;
            else if (records[i] is Prescription)
                prescriptions++;
        }

        Console.WriteLine($"Всього записів: {records.Length} (діагнозів: {diagnoses}, аналізів: {labResults}, рецептів: {prescriptions})");

        Diagnosis[] chronic = GetChronicDiagnoses(patientId);
        if (chronic.Length > 0)
        {
            Console.WriteLine($"Хронічні діагнози ({chronic.Length}):");
            foreach (var d in chronic)
            {
                Console.WriteLine($"  {d}");
            }
        }

        Prescription[] active = GetActivePrescriptions(patientId);
        if (active.Length > 0)
        {
            Console.WriteLine($"Активні рецепти ({active.Length}):");
            foreach (var p in active)
            {
                Console.WriteLine($"  {p} | до {p.ExpiresAt:dd.MM.yyyy}");
            }
        }
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Медичних записів немає.");
            return;
        }

        Console.WriteLine($"=== Медичні записи ({_count} / {MaxRecords}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_records[i]);
        }
    }

    public void DisplayList(MedicalRecord[] list)
    {
        if (list.Length == 0)
        {
            Console.WriteLine("Записів не знайдено.");
            return;
        }

        foreach (var record in list)
        {
            Console.WriteLine(record);
        }
    }
}
