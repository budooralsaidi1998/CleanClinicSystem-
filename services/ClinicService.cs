using CleanCllinicSystem.models;
using CleanCllinicSystem.RepoSitory;
using Microsoft.EntityFrameworkCore;

namespace CleanCllinicSystem.services
{
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepo _clinicRepo;

        public ClinicService(IClinicRepo clinicRepo)
        {
            // Constructor: Initialize the clinic repository to interact with data storage
            _clinicRepo = clinicRepo;
        }

        public List<Clinic> ViewClinicDetatils()
        {
            try
            {
                // Fetch and return all clinic details
                return _clinicRepo.ViewClinic().ToList();
            }
            catch (Exception ex)
            {
                // Log the exception if needed (e.g., logging service can be added here)
                throw new InvalidOperationException("An error occurred while fetching clinic details.", ex);
            }
        }

        public string AddClinic(Clinic clinic)
        {
            try
            {
                // Validate specialization field
                if (string.IsNullOrWhiteSpace(clinic.spe))
                {
                    throw new ArgumentException("Specification is required.");
                }

                // Validate number of slots, ensuring it doesn't exceed the maximum allowed (20)
                if (clinic.num_of_slots >= 20)
                {
                    throw new ArgumentException("Maximum number of slots allowed is 20.");
                }

                // Validate that the number of slots is not negative
                if (clinic.num_of_slots < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(clinic.num_of_slots), "Number of slots cannot be negative.");
                }

                // Add the clinic to the database
                _clinicRepo.Add(clinic);

                // Return success message
                return "Added Successfully.";
            }
            catch (ArgumentException ex)
            {
                // Handle specific validation errors
                throw new ArgumentException("Invalid input data.", ex);
            }
            catch (Exception ex)
            {
                // Handle the specific case where the number of slots is negative
                throw new ArgumentOutOfRangeException("The number of slots cannot be negative.", ex);
            }
            //catch (Exception ex)
            //{
            //    // Handle any general errors
            //    throw new ArgumentOutOfRangeException("An error occurred while adding the clinic.", ex);
            //}
        }

        public List<Clinic> GetAllClinic()
        {
            try
            {
                // Fetch all clinics and sort by number of available slots in descending order
                var clinicView = _clinicRepo.ViewClinic().OrderByDescending(b => b.num_of_slots).ToList();

                // If no clinics are found, throw an exception
                if (clinicView == null || clinicView.Count == 0)
                {
                    throw new InvalidOperationException("No clinics found.");
                }

                // Return the list of clinics
                return clinicView;
            }
            catch (InvalidOperationException ex)
            {
                // Handle the case where no clinics are found
                throw new InvalidOperationException("Error retrieving clinic list.", ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                throw new InvalidOperationException("An error occurred while fetching all clinics.", ex);
            }
        }

        public List<Clinic> GetClinicsBySpecialization(string specialization)
        {
            try
            {
                // Fetch clinics by specialization
                var clinics = _clinicRepo.ViewClinic().Where(c => c.spe == specialization).ToList();

                // If no clinics are found for the given specialization, throw an exception
                if (clinics.Count == 0)
                {
                    throw new InvalidOperationException("No clinics found with the given specialization.");
                }

                // Return the list of clinics
                return clinics;
            }
            catch (InvalidOperationException ex)
            {
                // Handle the case where no clinics match the given specialization
                throw new InvalidOperationException("Error retrieving clinics for the given specialization.", ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                throw new InvalidOperationException("An error occurred while fetching clinics by specialization.", ex);
            }
        }

        public void UpdateAvailableSlots(string specializationn)
        {
            try
            {
                // Fetch clinic by specialization
                var clinic = _clinicRepo.GetClinicsBySpecializationOneSPE(specializationn);

                // If the clinic is not found, throw an exception
                if (clinic == null)
                {
                    throw new InvalidOperationException("Clinic not found.");
                }

                // Check if there are available slots to decrement
                if (clinic.num_of_slots > 0)
                {
                    // Decrement the available slot by 1
                    clinic.num_of_slots -= 1;

                    // Update the clinic with the new slot count in the database
                    _clinicRepo.updateClinicSlots(clinic);
                }
                else
                {
                    // If no slots are available, throw an exception
                    throw new InvalidOperationException("No available slots left.");
                }
            }
            catch (InvalidOperationException ex)
            {
                // Handle the specific case where clinic is not found or no slots left
                throw new InvalidOperationException("Error updating available slots.", ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                throw new InvalidOperationException("An error occurred while updating available slots.", ex);
            }
        }
    }



}


