using CleanCllinicSystem.models;

namespace CleanCllinicSystem.services
{
    public interface IPateientService
    {
        string AddPatient(Patient patint);
        List<Patient> GetAllPatient();
        List<Patient> GetPatientByName(string name);
    }
}