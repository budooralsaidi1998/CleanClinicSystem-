using CleanCllinicSystem.models;

namespace CleanCllinicSystem.services
{
    public interface IBookService
    {
        string BookAppointment(string namep, string spe, DateTime date, int slotNumber);
        bool IsSlotAvailable(string spec, DateTime date, int slotNumber);
        List<booking> ViewAppointmentsByPatient(string name);
        List<booking> ViewAppointmentsBySpeClinic(string specialization);
    }
}