using CleanCllinicSystem.models;
using CleanCllinicSystem.services;
using Microsoft.AspNetCore.Mvc;

namespace CleanCllinicSystem.Controllers
{
    // The PatientController class is responsible for handling HTTP requests related to patient operations.
    // It interacts with the IPateientService layer to perform the necessary business logic for adding and retrieving patients.
    [ApiController] // Indicates that this class handles HTTP requests and will automatically process model binding and validation.
    [Route("api/[Controller]")] // Defines the base route for this controller. It will be mapped to api/patient.
    public class PatientController : ControllerBase
    {
        // Private field to hold the service that contains the business logic for patient operations.
        private readonly IPateientService _PatientService;

        // Constructor that accepts an IPateientService object via dependency injection.
        public PatientController(IPateientService pa)
        {
            _PatientService = pa;  // Initializes the service with the injected instance.
        }

        // HTTP POST action to add a new patient to the system.
        // The method expects parameters for the patient's name, age, and gender.
        [HttpPost("AddPatient")] // Maps the route api/patient/AddPatient to this action.
        public IActionResult AddPatient(string name, int age, string gender)
        {
            try
            {
                // Attempt to parse the gender string into the GenderType enum.
                if (!Enum.TryParse<GenderType>(gender, true, out var genderEnum))
                {
                    // If gender parsing fails, return a BadRequest response with an error message.
                    return BadRequest("Invalid gender value. Please provide 'Male', 'Female', or 'Other'.");
                }

                // Call the AddPatient method of the service to add a new patient.
                string newPatient = _PatientService.AddPatient(new Patient
                {
                    pname = name,  // Set the patient's name.
                    age = age,     // Set the patient's age.
                    RGender = genderEnum  // Set the patient's gender from the parsed enum.
                });

                // Return a Created response, which indicates successful creation of the patient.
                return Created(string.Empty, newPatient);  // You can return a URI for the newly created patient here.
            }
            catch (Exception ex)
            {
                // If an error occurs, return a BadRequest response with the exception message.
                return BadRequest(ex.Message);
            }
        }

        // HTTP GET action to retrieve a list of all patients.
        [HttpGet("GetAllPatient")] // Maps the route api/patient/GetAllPatient to this action.
        public IActionResult GetAllPatient()
        {
            try
            {
                // Call the GetAllPatient method from the service to retrieve a list of patients.
                var patientList = _PatientService.GetAllPatient();

                // Return an OK response with the list of patients.
                return Ok(patientList);
            }
            catch (Exception ex)
            {
                // If an error occurs, return a BadRequest response with the exception message.
                return BadRequest(ex.Message);
            }
        }
    }

}
