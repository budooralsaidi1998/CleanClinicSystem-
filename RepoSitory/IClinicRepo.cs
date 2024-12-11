using CleanCllinicSystem.models;

namespace CleanCllinicSystem.RepoSitory
{
    public interface IClinicRepo
    {
        void Add(Clinic addclinic);
        List<Clinic> GetClinicsBySpecialization(string specialization);
        Clinic GetClinicsBySpecializationOneSPE(string specializationn);
        void updateClinicSlots(Clinic currentClinic);
        List<Clinic> ViewClinic();
    }
}