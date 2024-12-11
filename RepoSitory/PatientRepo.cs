using CleanCllinicSystem.models;

namespace CleanCllinicSystem.RepoSitory
{
    // The PatientRepo class implements the IPatientRepo interface.
    // It handles the data access logic for performing CRUD operations on the Patient entity.
    public class PatientRepo : IPatientRepo
    {
        // Private read-only field for the database context (AppDbContext) to interact with the database.
        private readonly AppDbcontext _context;

        // Constructor to initialize the repository with the AppDbContext via dependency injection.
        public PatientRepo(AppDbcontext context)
        {
            _context = context;  // Assign the injected context to the class field.
        }

        // Method to retrieve all patients from the database.
        public List<Patient> ViewPatient()
        {
            try
            {
                // Return all patients from the patients table as a list.
                return _context.patients.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception message to the console (this can be replaced with a logging framework).
                Console.WriteLine($"An error occurred: {ex.Message}");

                // Return an empty list if an error occurs while fetching patients to avoid crashing the application.
                return new List<Patient>();
            }
        }

        // Method to add a new patient to the database.
        public void Add(Patient addpient)
        {
            try
            {
                // Add the new patient to the patients table in the database.
                _context.patients.Add(addpient);

                // Save the changes to the database (persist the new patient).
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during the add operation.
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Method to retrieve patients from the database by their name.
        public List<Patient> GetIDPatient(string name)
        {
            try
            {
                // Return all patients from the patients table whose pname matches the given name.
                return _context.patients.Where(u => u.pname == name).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception message to the console.
                Console.WriteLine($"An error occurred: {ex.Message}");

                // Return an empty list if an error occurs while fetching patients to maintain consistent return type.
                return new List<Patient>();
            }
        }
    }
}

