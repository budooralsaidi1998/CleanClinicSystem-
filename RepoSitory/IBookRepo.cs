using CleanCllinicSystem.models;

namespace CleanCllinicSystem.RepoSitory
{
    public interface IBookRepo
    {
        void Add(booking appointment);
        List<booking> AppointmentByClinic(string nameClinic);
        List<booking> AppointmentByPaient(string namepaient);
        List<booking> GetBookingsByClinicAndDate(string spec, DateTime date);
        booking GetById(int id);
    }
}