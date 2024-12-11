using CleanCllinicSystem.models;
using CleanCllinicSystem.RepoSitory;

namespace CleanCllinicSystem.services
{
    public class BookService : IBookService
    {
        private readonly IBookRepo _bookingrepo;
        private readonly IClinicService _clinicservice;
        private readonly IPateientService _patientservices;

        // Constructor injection of dependencies (repositories and services)
        public BookService(IBookRepo bookrepo, IClinicService clinciservices, IPateientService patientservices)
        {
            _bookingrepo = bookrepo;
            _clinicservice = clinciservices;
            _patientservices = patientservices;
        }

        // Method to book an appointment for a patient
        public string BookAppointment(string namep, string spe, DateTime date, int slotNumber)
        {
            try
            {
                // Validate input parameters

                // Check if the patient name is valid (not empty or null)
                if (string.IsNullOrWhiteSpace(namep))
                {
                    throw new ArgumentException("Patient name cannot be empty.");
                }

                // Check if the specialization is valid (not empty or null)
                if (string.IsNullOrWhiteSpace(spe))
                {
                    throw new ArgumentException("Specialization cannot be empty.");
                }

                // Check if the slot number is valid (positive number)
                if (slotNumber <= 0)
                {
                    throw new ArgumentException("Slot number must be a positive integer.");
                }

                // Validate if the date is not in the past
                if (date < DateTime.Now)
                {
                    throw new ArgumentException("Appointment date cannot be in the past.");
                }

                // Get patient by name
                var patient = _patientservices.GetPatientByName(namep).FirstOrDefault();

                if (patient == null)
                {
                    throw new KeyNotFoundException("Patient not found.");
                }

                // Get clinic by specialization
                var clinic = _clinicservice.GetAllClinic().FirstOrDefault(c => c.spe == spe);
                if (clinic == null)
                {
                    throw new KeyNotFoundException("Clinic not found.");
                }

                // Check if the patient already has an appointment at the clinic on the same date and time
                var existingAppointment = _bookingrepo.GetBookingsByPatientAndDate(patient.Pid, spe, date);
                if (existingAppointment.Any(b => b.slot_number == slotNumber))
                {
                    throw new InvalidOperationException("Patient already has an appointment at this time and clinic.");
                }

                // Check if the requested slot is available
                if (!IsSlotAvailable(spe, date, slotNumber))
                {
                    throw new InvalidOperationException("Slot is already booked.");
                }

                // Create a new booking
                var booking = new booking
                {
                    pid = patient.Pid,
                    spec = spe,
                    date = date,
                    slot_number = slotNumber
                };

                // Add booking to the repository
                _bookingrepo.Add(booking);

                // Return success message
                return "Appointment booked successfully.";
            }
            catch (ArgumentException ex)
            {
                // Handle input validation errors (e.g., empty name, invalid slot number)
                throw new InvalidOperationException("Input validation error: " + ex.Message, ex);
            }
            catch (KeyNotFoundException ex)
            {
                // Handle specific error: patient or clinic not found
                throw new InvalidOperationException("Error during appointment booking: " + ex.Message, ex);
            }
            catch (InvalidOperationException ex)
            {
                // Handle specific error: slot already booked or duplicate appointment
                throw new InvalidOperationException("Error during appointment booking: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                throw new InvalidOperationException("An unexpected error occurred while booking the appointment.", ex);
            }
        }

        // Method to check if a slot is available for a given clinic and date
        public bool IsSlotAvailable(string spec, DateTime date, int slotNumber)
        {
            try
            {
                // Get clinic by specialization
                var clinic = _clinicservice.GetClinicsBySpecialization(spec);

                if (clinic == null)
                {
                    throw new KeyNotFoundException("Clinic not found.");
                }

                // Get bookings for the specified clinic and date
                var bookings = _bookingrepo.GetBookingsByClinicAndDate(spec, date);

                // Check if the slot is already booked
                var slotAlreadyBooked = bookings.Any(b => b.slot_number == slotNumber);

                return !slotAlreadyBooked; // Return true if the slot is available, false if it's booked
            }
            catch (KeyNotFoundException ex)
            {
                // Handle specific error: clinic not found
                throw new InvalidOperationException("Error while checking slot availability: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                throw new InvalidOperationException("An unexpected error occurred while checking slot availability.", ex);
            }
        }

        // Method to view appointments for a specific clinic by specialization
        public List<booking> ViewAppointmentsBySpeClinic(string specialization)
        {
            try
            {
                // Get clinics by specialization
                var clinics = _clinicservice.GetClinicsBySpecialization(specialization);

                if (clinics == null || clinics.Count == 0)
                {
                    throw new KeyNotFoundException("No clinics found for the specified specialization.");
                }

                var appointments = new List<booking>();

                // Retrieve bookings for each clinic
                foreach (var clinic in clinics)
                {
                    var clinicAppointments = _bookingrepo.AppointmentByClinic(clinic.spe);
                    appointments.AddRange(clinicAppointments);
                }

                return appointments;
            }
            catch (KeyNotFoundException ex)
            {
                // Handle specific error: no clinics found for the given specialization
                throw new InvalidOperationException("Error while viewing appointments: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                throw new InvalidOperationException("An unexpected error occurred while viewing appointments.", ex);
            }
        }

        // Method to view appointments for a specific patient by name
        public List<booking> ViewAppointmentsByPatient(string name)
        {
            try
            {
                // Get patient by name
                var pa = _patientservices.GetPatientByName(name);

                if (pa == null || pa.Count == 0)
                {
                    throw new KeyNotFoundException("No patient found for the specified name.");
                }

                var appointments = new List<booking>();

                // Retrieve bookings for each patient
                foreach (var patient in pa)
                {
                    var patientAppointments = _bookingrepo.AppointmentByPaient(patient.pname);
                    appointments.AddRange(patientAppointments);
                }

                return appointments;
            }
            catch (KeyNotFoundException ex)
            {
                // Handle specific error: no patient found with the given name
                throw new InvalidOperationException("Error while viewing appointments by patient: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                throw new InvalidOperationException("An unexpected error occurred while viewing appointments by patient.", ex);
            }
        }
    }

}

