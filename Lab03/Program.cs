namespace Lab03;

class Program
{
    static void Main(string[] args)
    {
        Patient p1 = new Patient("Іван", "Петренко", new DateTime(1985, 5, 14), "A+", "0501234567");
        Patient p2 = new Patient("Олена", "Коваль", new DateTime(1993, 2, 20), "B-", "0672345678");
        Patient p3 = new Patient("Максим", "Бойко", new DateTime(2010, 8, 10), "0+", "0933456789");
        
        Patient p4 = new Patient();
        
        Patient p5 = new Patient("Марія", "Ткач");
        
        Patient[] patients = { p1, p2, p3, p4, p5 };

        foreach (var patient in patients)
        {
            Console.WriteLine(patient);
        }
    }
}