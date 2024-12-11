using CleanCllinicSystem.models;
using CleanCllinicSystem.services;
using Microsoft.AspNetCore.Mvc;

namespace CleanCllinicSystem.Controllers
{
    // Enum to categorize the different error types for better error management.
    public enum ErrorType
    {
        InvalidInput = 1,  // Invalid input (e.g., negative slots, missing specialization)
        NotFound = 2,      // Item not found (e.g., clinic not found)
        InternalError = 3, // Unexpected server error
        SlotExceeded = 4   // Max slots exceeded (e.g., more than 20 slots)
    }

    [ApiController] // Indicates that this is an API controller and the controller will automatically respond with JSON or XML.
    [Route("api/[Controller]")] // Specifies the route template. The name of the controller will be part of the URL path (e.g., api/clinic).
    public class ClinicController : ControllerBase
    {
        private readonly IClinicService _ClinicService; // Instance of the clinic service to interact with the business logic.

        // Constructor that takes in the clinic service interface (dependency injection)
        public ClinicController(IClinicService clinicser)
        {
            _ClinicService = clinicser; // Assign the injected service to the private field.
        }

        // POST endpoint to add a new clinic.
        [HttpPost]
        public IActionResult AddClinic(string spec, int num)
        {
            try
            {
                // Check for invalid input: negative number of slots
                if (num < 0)
                {
                    // Return a BadRequest response with an appropriate error message.
                    return BadRequest(CreateErrorResponse(ErrorType.InvalidInput, "Number of slots cannot be negative."));
                }

                // Attempt to add the clinic by passing the provided specialization and slot count to the service.
                string newClinicId = _ClinicService.AddClinic(new Clinic
                {
                    spe = spec,         // Clinic specialization
                    num_of_slots = num  // Number of slots available in the clinic
                });

                // Return a Created response with the new clinic ID, indicating that the clinic was successfully created.
                return Created(string.Empty, newClinicId);
            }
            catch (ArgumentException ex)
            {
                // Handle specific argument errors (e.g., invalid specialization or slot count).
                return BadRequest(CreateErrorResponse(ErrorType.InvalidInput,"check for number slot or check for spec " ));
            }
            catch (InvalidOperationException ex)
            {
                // Handle cases where the clinic could not be found or no clinics match the input.
                return NotFound(CreateErrorResponse(ErrorType.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                // Catch all unexpected errors and return a 500 Internal Server Error response.
                return StatusCode(500, CreateErrorResponse(ErrorType.InternalError, "An unexpected error occurred. " + ex.Message));
            }
        }

        // GET endpoint to fetch all clinics.
        [HttpGet("GetAllClinic")]
        public IActionResult GetAllClinic()
        {
            try
            {
                // Attempt to retrieve the list of clinics from the service.
                var clinic = _ClinicService.GetAllClinic();

                // Return the list of clinics with a 200 OK status code.
                return Ok(clinic);
            }
            catch (InvalidOperationException ex)
            {
                // Handle the case where no clinics are found (e.g., empty or invalid data).
                return NotFound(CreateErrorResponse(ErrorType.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                // Catch all unexpected errors and return a 500 Internal Server Error response.
                return StatusCode(500, CreateErrorResponse(ErrorType.InternalError, "An unexpected error occurred. " + ex.Message));
            }
        }

        // Helper method to generate a standardized error response.
        private object CreateErrorResponse(ErrorType errorType, string message)
        {
            // This method creates a structured error response that includes the error type and message.
            return new
            {
                ErrorType = errorType.ToString(),  // The type of error (from the ErrorType enum)
                ErrorMessage = message             // The error message providing details about the issue
            };
        }
    }

}

