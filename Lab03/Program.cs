namespace Lab03;

class Program
{
    static void Main(string[] args)
    {
        Doctor doc1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        doc1.WorkEndHour = 16;

        Doctor doc2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
        doc2.WorkStartHour = 9;
        doc2.WorkEndHour = 18; 

        Doctor doc3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");
        
        Doctor doc4 = new Doctor();

        Doctor[] doctors = { doc1, doc2, doc3, doc4 };

        foreach (var doc in doctors)
        {
            Console.WriteLine(doc);
        }
    }
}