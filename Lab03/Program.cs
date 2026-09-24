namespace Lab03;

class Program
{
    static void Main(string[] args)
    {
        Appointment a1 = new Appointment(1, 1, new DateTime(2026, 5, 9, 10, 0, 0), 30);
        Appointment a2 = new Appointment(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        Appointment a3 = new Appointment(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        Console.WriteLine(a1);
        Console.WriteLine(a2);
        Console.WriteLine(a3);

        Console.WriteLine("\n// Після Cancel та Complete:");
        a1.Cancel("Пацієнт не зміг прийти");
        a2.Complete();

        Console.WriteLine(a1);
        Console.WriteLine(a2);
    }
}