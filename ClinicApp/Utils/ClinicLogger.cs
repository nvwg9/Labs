using System.Text;

namespace ClinicApp.Utils;

public class ClinicLogger
{
    private readonly string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    public void LogInfo(string message)
    {
        Write("INFO", message);
    }

    public void LogWarning(string message)
    {
        Write("WARN", message);
    }

    public void LogError(string message)
    {
        Write("ERROR", message);
    }

    public string[] GetLastLines(int n)
    {
        if (!Exists() || n <= 0)
        {
            return new string[0];
        }

        string[] lines = File.ReadAllLines(_logPath, Encoding.UTF8);
        int skip = Math.Max(0, lines.Length - n);
        string[] result = new string[lines.Length - skip];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = lines[skip + i];
        }

        return result;
    }

    public void Clear()
    {
        if (Exists())
        {
            File.Delete(_logPath);
        }
    }

    public bool Exists()
    {
        return File.Exists(_logPath);
    }

    private void Write(string level, string message)
    {
        string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level.PadRight(5)}] {message}{Environment.NewLine}";
        File.AppendAllText(_logPath, line, Encoding.UTF8);
    }
}
