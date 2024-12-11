using CleanCllinicSystem.models;
using CleanCllinicSystem.RepoSitory;

namespace CleanCllinicSystem.services
{
    // The PateientService class implements the IPateientService interface. 
    // It handles business logic for operations related to patient management.
    public class PateientService : IPateientService
    {
        // Private read-only field for the repository, which provides data access to the patient entities.
        private readonly IPatientRepo _patientRepo;

        // Constructor to initialize the PateientService with an instance of IPatientRepo.
        // The dependency injection pattern allows the repository to be passed into the service.
        public PateientService(IPatientRepo patientrepo)
        {
            _patientRepo = patientrepo;  // Assign the injected repository to the class field.
        }

        // Method to add a new patient to the system.
        // This method validates that the patient's name is not null or empty before adding it.
        public string AddPatient(Patient patint)
        {
            // Validate that the patient's name is not null, empty, or whitespace.
            if (string.IsNullOrWhiteSpace(patint.pname))
            {
                // If the name is invalid, throw an ArgumentException with an appropriate message.
                throw new ArgumentException("Name of patient is required.");
            }

            // Validate that the patient's age is a positive number.
            if (patint.age <= 0)
            {
                // If the age is invalid (zero or negative), throw an ArgumentException with an appropriate message.
                throw new ArgumentException("Age of patient must be a positive number.");
            }

            // Optionally, you can also add a check for age range if needed (for example, an age between 1 and 120):
            if (patint.age < 1 || patint.age > 120)
            {
                // If the age is out of range, throw an ArgumentException with a relevant message.
                throw new ArgumentException("Age must be between 1 and 120.");
            }

            // Call the repository's Add method to persist the patient object.
            _patientRepo.Add(patint);

            // Return a success message after adding the patient.
            return "Added Successfully.";
        }


        // Method to retrieve all patients from the system, sorted by patient name in descending order.
        public List<Patient> GetAllPatient()
        {
            // Retrieve the list of all patients using the ViewPaient method from the repository.
            // The patients are then sorted by name in descending order.
            var patientview = _patientRepo.ViewPatient().OrderByDescending(b => b.pname).ToList();

            // Check if the list is null or empty, which indicates that no patients were found.
            if (patientview == null || patientview.Count == 0)
            {
                // If no patients are found, throw an InvalidOperationException.
                throw new InvalidOperationException("No patient found.");
            }

            // Return the list of patients if they were found and successfully retrieved.
            return patientview;
        }

        // Method to retrieve patients by their name.
        public List<Patient> GetPatientByName(string name)
        {
            // Retrieve a list of patients with the specified name using the GetIDPatient method from the repository.
            var patientview = _patientRepo.GetIDPatient(name);

            // Check if the retrieved list is null, which indicates no patients were found with the given name.
            if (patientview == null)
            {
                // If no patients are found, throw an InvalidOperationException with a relevant message.
                throw new InvalidOperationException("No patient found.");
            }

            // Return the list of patients that match the specified name.
            return patientview;
        }
    }
}
