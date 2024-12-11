using CleanCllinicSystem.models;

namespace CleanCllinicSystem.RepoSitory
{
    public interface IPatientRepo
    {
        void Add(Patient addpient);
        List<Patient> GetIDPatient(string name);
        List<Patient> ViewPatient();
    }
}