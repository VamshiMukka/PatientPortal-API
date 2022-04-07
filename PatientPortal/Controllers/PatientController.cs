using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PatientPortal.Common;
using PatientPortal.Models;
using PatientPortal.Services;
using System;
using System.Threading.Tasks;


namespace PatientPortal.Controllers
{
    [Route(Constants.PatientRootRoute)]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatient _patientService;
        private readonly ILogger<PatientController> _logger;
        private readonly Common.ApplicationLog _logHelper;
        public PatientController(IPatient patientService, ILogger<PatientController> logger)
        {
            _patientService = patientService;
            _logger = logger;
            _logHelper = new Common.ApplicationLog();
        }

        [HttpGet]
        public string Index()
        {
            return Constants.PatientServiceRunning;
        }

        /// <summary>
        /// Get the jwt token on succesful login
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route(Constants.LoginRootRoute)]
        public async Task<ActionResult<Response>> Login(User userDetails)
        {
            Response response = new();
            try
            {
                if (userDetails== null || (userDetails != null && string.IsNullOrEmpty(userDetails.UserName) && string.IsNullOrEmpty(userDetails.Password)))
                {
                    response.Errors = Constants.UnAuthorizedUser;
                }
                else
                {
                    response.Success = await _patientService.GenerateToken(userDetails);
                    if(response.Success == Constants.UnAuthorizedUser)
                    {
                        return Unauthorized();
                    }
                }
            }
            catch (Exception ex)
            {
                var loggableRequest = await _logHelper.GetLoggableRequestBody(Request, ex.Message, ex.StackTrace, true);
                _logger.LogError(ex, loggableRequest, null);
                response.Errors = Constants.ErrorOccurred;
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Get all the patient details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(Constants.GetAllPatientsRootRoute)]
        public async Task<ActionResult<Response>> GetAllPatients()
        {
            Response response = new();
            try
            {
               response.Success = await _patientService.GetAllPatients();
            }
            catch (Exception ex)
            {
                var loggableRequest = await _logHelper.GetLoggableRequestBody(Request, ex.Message, ex.StackTrace, true);
                _logger.LogError(ex, loggableRequest, null);
                response.Errors = Constants.ErrorOccurred;
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Get patient details for the requested patient
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(Constants.GetPatientByPatientIdRootRoute)]
        public async Task<ActionResult<PatientDetails>> PatientDetailsById(string patientId)
        {
            Response response = new();
            try
            {
                if (string.IsNullOrEmpty(patientId))
                {
                    response.Errors = Constants.InvalidRequest;
                }
                else
                {
                    response.Success = await _patientService.PatientDetailsById(patientId);
                }
            }
            catch (Exception ex)
            {
                var loggableRequest = await _logHelper.GetLoggableRequestBody(Request, ex.Message, ex.StackTrace, true);
                _logger.LogError(ex, loggableRequest, null);
                response.Errors = Constants.ErrorOccurred;
                return BadRequest(response);
            }
            return Ok(response);
        }

       
    }
}
