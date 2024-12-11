using CleanCllinicSystem.models;
using Microsoft.EntityFrameworkCore;

namespace CleanCllinicSystem.RepoSitory
{

    public class BookRepo : IBookRepo
    {
        private readonly AppDbcontext _context;

        public BookRepo(AppDbcontext context)
        {
            _context = context; // Injecting the DbContext to interact with the database.
        }

        // Method to add a booking (appointment) to the database
        public void Add(booking appointment)
        {
            try
            {
                // Add the booking to the database
                _context.bookings.Add(appointment);
                _context.SaveChanges(); // Save changes to the database.
            }
            catch (Exception ex)
            {
                // Catch any exception and log or handle it.
                // Here, an internal error message can be returned or logged.
                throw new InvalidOperationException("An error occurred while adding the appointment.", ex);
            }
        }

        // Method to get appointments by clinic specialization
        public List<booking> AppointmentByClinic(string nameClinic)
        {
            try
            {
                // Fetch appointments where the clinic's specialization matches the provided name
                return _context.bookings
                               .Include(c => c.clinics) // Include clinic information
                               .Where(c => c.clinics.spe == nameClinic) // Filter by specialization
                               .ToList(); // Return the filtered list of bookings
            }
            catch (Exception ex)
            {
                // Handle errors that occur during the retrieval of appointments
                throw new InvalidOperationException("An error occurred while retrieving appointments by clinic specialization.", ex);
            }
        }

        // Method to get appointments by patient name
        public List<booking> AppointmentByPaient(string namepaient)
        {
            try
            {
                // Fetch appointments where the patient's name matches the given name
                return _context.bookings
                               .Include(c => c.patients) // Include patient information
                               .Where(c => c.patients.pname == namepaient) // Filter by patient name
                               .ToList(); // Return the filtered list of bookings
            }
            catch (Exception ex)
            {
                // Handle errors that occur during the retrieval of appointments by patient name
                throw new InvalidOperationException("An error occurred while retrieving appointments by patient name.", ex);
            }
        }

        // Method to get a booking by its ID
        public booking GetById(int id)
        {
            try
            {
                // Fetch a booking by its ID, including the associated clinic and patient data
                return _context.bookings
                               .Include(u => u.clinics) // Include clinic information
                               .Include(b => b.patients) // Include patient information
                               .FirstOrDefault(br => br.bookid == id); // Find the booking by ID
            }
            catch (Exception ex)
            {
                // Handle any errors that occur when trying to fetch a booking by ID
                throw new InvalidOperationException("An error occurred while retrieving the booking by ID.", ex);
            }
        }

        // Method to get bookings for a specific clinic on a specific date
        public List<booking> GetBookingsByClinicAndDate(string spec, DateTime date)
        {
            try
            {
                // Fetch bookings that match the clinic ID and the provided date
                return _context.bookings
                               .Where(b => b.spec == spec && b.date.Date == date.Date) // Filter by clinic ID and date
                               .ToList(); // Return the filtered list of bookings
            }
            catch (Exception ex)
            {
                // Handle any errors that occur while fetching bookings by clinic and date
                throw new InvalidOperationException("An error occurred while retrieving bookings by clinic and date.", ex);
            }
        }
    }

}
