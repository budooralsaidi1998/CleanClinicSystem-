using CleanCllinicSystem.models;

namespace CleanCllinicSystem.RepoSitory
{
    public class ClinicRepo : IClinicRepo
    {
        private readonly AppDbcontext _context;

        public ClinicRepo(AppDbcontext context)
        {
            _context = context;
        }

        // Method to retrieve all clinics
        public List<Clinic> ViewClinic()
        {
            try
            {
                // Try to retrieve all clinics from the database
                return _context.clinics.ToList();
            }
            catch (Exception ex)
            {
                // Catch any exception that occurs during the database query
                // Log the exception (Logging can be implemented with a logging framework)
                throw new InvalidOperationException("Error retrieving clinics.", ex);
            }
        }

        // Method to add a new clinic
        public void Add(Clinic addclinic)
        {
            try
            {
                // Try to add a new clinic to the database
                _context.clinics.Add(addclinic);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Catch any exception that occurs during the add operation
                // Log the exception
                throw new InvalidOperationException("Error adding clinic.", ex);
            }
        }

        // Method to retrieve clinics by specialization
        public List<Clinic> GetClinicsBySpecialization(string specialization)
        {
            try
            {
                // Try to retrieve clinics by the given specialization from the database
                return _context.clinics
                    .Where(c => c.spe == specialization)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Catch any exception that occurs during the query operation
                // Log the exception
                throw new InvalidOperationException("Error retrieving clinics by specialization.", ex);
            }
        }

        // Method to retrieve one clinic by specialization
        public Clinic GetClinicsBySpecializationOneSPE(string specializationn)
        {
            try
            {
                // Try to retrieve a single clinic based on the specialization
                return _context.clinics
                    .FirstOrDefault(c => c.spe == specializationn);
            }
            catch (Exception ex)
            {
                // Catch any exception that occurs during the query operation
                // Log the exception
                throw new InvalidOperationException("Error retrieving clinic by specialization.", ex);
            }
        }

        // Method to update the number of available slots for a clinic
        public void updateClinicSlots(Clinic currentClinic)
        {
            try
            {
                // Try to update the clinic's slot count in the database
                _context.Update(currentClinic.num_of_slots);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Catch any exception that occurs during the update operation
                // Log the exception
                throw new InvalidOperationException("Error updating clinic slots.", ex);
            }
        }
    }


}

