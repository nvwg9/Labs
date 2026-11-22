using System.Globalization;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class CsvImporter
{
    private const int FieldCount = 5;

    public ImportResult ImportPatients(Clinic clinic, string filePath)
    {
        ImportResult result = new ImportResult();

        if (!File.Exists(filePath))
        {
            result.AddError(0, $"Файл не знайдено: {filePath}");
            return result;
        }

        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                string[] parts = line.Split(',');
                if (parts.Length != FieldCount)
                {
                    throw new FormatException($"Рядок має {parts.Length} {(parts.Length == 1 ? "поле" : "полів")} замість {FieldCount}.");
                }

                ClinicValidator.ValidateName(parts[0].Trim(), "FirstName");
                ClinicValidator.ValidateName(parts[1].Trim(), "LastName");

                DateTime dateOfBirth = DateTime.ParseExact(parts[2].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), parts[3].Trim());
                if (!Enum.IsDefined(bloodType))
                {
                    throw new ArgumentException($"Невідома група крові: {parts[3].Trim()}.");
                }

                Patient patient = new Patient(parts[0].Trim(), parts[1].Trim(), dateOfBirth, bloodType, parts[4].Trim());
                clinic.Patients.Add(patient);
                clinic.Logger.LogInfo($"Імпортовано пацієнта #{patient.Id}: {patient.FullName}");
                result.AddSuccess();
            }
            catch (FormatException e)
            {
                result.AddError(i + 1, CleanMessage(e.Message));
            }
            catch (ArgumentException e)
            {
                result.AddError(i + 1, CleanMessage(e.Message));
            }
        }

        return result;
    }

    private static string CleanMessage(string message)
    {
        int index = message.IndexOf(" (Parameter", StringComparison.Ordinal);
        return index >= 0 ? message.Substring(0, index) : message;
    }
}
