using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class BillingManager
{
    private readonly AppointmentManager _appointments;

    public BillingManager(AppointmentManager appointments)
    {
        _appointments = appointments;
    }

    public IPayable[] GetAllUnpaid()
    {
        return FilterUnpaid(_appointments.GetAll());
    }

    public IPayable[] GetUnpaidByPatient(int patientId)
    {
        return FilterUnpaid(_appointments.GetByPatient(patientId));
    }

    public decimal GetTotalDebt()
    {
        return SumCost(GetAllUnpaid());
    }

    public decimal GetPatientDebt(int patientId)
    {
        return SumCost(GetUnpaidByPatient(patientId));
    }

    public bool PayAppointment(int appointmentId)
    {
        Appointment[] all = _appointments.GetAll();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].Id == appointmentId)
            {
                if (all[i].IsPaid || all[i].IsCancelled)
                {
                    return false;
                }

                all[i].MarkPaid();
                return true;
            }
        }

        return false;
    }

    public void DisplayUnpaid(IPayable[] items)
    {
        if (items.Length == 0)
        {
            Console.WriteLine("Немає неоплачених записів.");
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            string cost = items[i].GetCost().ToString("F2");
            if (items[i] is Appointment appointment)
            {
                Console.WriteLine($"{appointment} | Сума: {cost} грн");
            }
            else
            {
                Console.WriteLine($"#{i + 1} | Сума: {cost} грн");
            }
        }
    }

    private static IPayable[] FilterUnpaid(Appointment[] source)
    {
        int matches = 0;
        for (int i = 0; i < source.Length; i++)
        {
            if (!source[i].IsPaid && !source[i].IsCancelled)
                matches++;
        }

        IPayable[] result = new IPayable[matches];
        int index = 0;
        for (int i = 0; i < source.Length; i++)
        {
            if (!source[i].IsPaid && !source[i].IsCancelled)
                result[index++] = source[i];
        }

        return result;
    }

    private static decimal SumCost(IPayable[] items)
    {
        decimal total = 0m;
        for (int i = 0; i < items.Length; i++)
        {
            total += items[i].GetCost();
        }
        return total;
    }
}
