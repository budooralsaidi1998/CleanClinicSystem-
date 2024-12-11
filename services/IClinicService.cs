using CleanCllinicSystem.models;

namespace CleanCllinicSystem.services
{
    public interface IClinicService
    {
        string AddClinic(Clinic clinic);
        List<Clinic> GetAllClinic();
        List<Clinic> GetClinicsBySpecialization(string specialization);
        void UpdateAvailableSlots(string specializationn);
        List<Clinic> ViewClinicDetatils();
    }
}