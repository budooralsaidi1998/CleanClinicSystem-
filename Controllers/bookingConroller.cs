using CleanCllinicSystem.services;
using Microsoft.AspNetCore.Mvc;

namespace CleanCllinicSystem.Controllers
{
    // Enum to define different types of errors for better error handling
    public enum errortype
    {
        InvalidInput = 1,  // Invalid input (e.g., missing parameters, incorrect data format)
        NotFound = 2,      // Item not found (e.g., no booking found, patient not found)
        InternalError = 3, // Unexpected server error
        SlotUnavailable = 4 // Slot already booked or unavailable
    }

    [ApiController]
    [Route("api/[Controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookService _bookingService;

        // Constructor to initialize the booking service
        public BookingController(IBookService bookService)
        {
            _bookingService = bookService;
        }

        // API endpoint to book an appointment
        [HttpPost("BookAppointment")]
        public IActionResult AddBook(string namep, string spec, DateTime date, int slotNumber)
        {
            try
            {
                // Validate the input parameters
                if (string.IsNullOrWhiteSpace(namep) || string.IsNullOrWhiteSpace(spec))
                {
                    return BadRequest(CreateErrorResponse(ErrorType.InvalidInput, "Patient name and specialization are required."));
                }

                // Call the service method to book an appointment
                _bookingService.BookAppointment(namep, spec, date, slotNumber);
                return Created("", "Appointment booked successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case where the patient or clinic is not found
                return NotFound(CreateErrorResponse(ErrorType.NotFound, ex.Message));
            }
            catch (ArgumentException ex)
            {
                // Handle invalid input cases (e.g., slot already booked or invalid data)
                return BadRequest(CreateErrorResponse(ErrorType.InvalidInput, ex.Message));
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, CreateErrorResponse(ErrorType.InternalError, "An unexpected error occurred. " + ex.Message));
            }
        }

        // API endpoint to get all bookings for a specific clinic
        [HttpGet("GetBookByClinic/{namesp}")]
        public IActionResult GetAppoiByClinic(string namesp)
        {
            try
            {
                // Get the appointments for the specified clinic
                var books = _bookingService.ViewAppointmentsBySpeClinic(namesp);
                return Ok(books);
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case where no clinics were found for the given specialization
                return NotFound(CreateErrorResponse(ErrorType.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, CreateErrorResponse(ErrorType.InternalError, "An unexpected error occurred. " + ex.Message));
            }
        }

        // API endpoint to get all bookings for a specific patient
        [HttpGet("GetBookByPatient/{name}")]
        public IActionResult GetAppoiByPatient(string name)
        {
            try
            {
                // Get the appointments for the specified patient
                var books = _bookingService.ViewAppointmentsByPatient(name);
                return Ok(books);
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case where no bookings were found for the patient
                return NotFound(CreateErrorResponse(ErrorType.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, CreateErrorResponse(ErrorType.InternalError, "An unexpected error occurred. " + ex.Message));
            }
        }

        // Helper method to create a structured error response
        private object CreateErrorResponse(ErrorType errorType, string message)
        {
            return new
            {
                ErrorType = errorType.ToString(),
                ErrorMessage = message
            };
        }
    }

}
