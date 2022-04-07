
using System.Diagnostics.CodeAnalysis;

namespace PatientPortal.Common
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        #region base

        public const int SuccessCode = 200;
        public const string PatientServiceRunning = "Patient Service is running";
        public const string ErrorOccurred = "Some Error Occurred";
        public const string InvalidRequest = "Invalid Request";
        public const string CouldNotCaptureRequest = "Could not capture the request";
        public const string NoRequestHeadersFound = "No request headers found";
        public const string PatientServiceURL = "PatientServiceURL";
        public const string Post = "POST";
        public const string Get = "GET";
        public const string Put = "PUT";
        public const string Authorization = "Authorization";
        public const string CacheControl = "Cache-control";
        public const string NoCache = "no-cache";
        public const string MediaTypeJson = "application/json";
        public const bool True = true;
        public const bool False = false;
        public const string ForwardSlash = "/";
        public const string UnAuthorizedUser = "401 UnAuthorized User";
        public const string InvalidUser = "InvalidUserAttempted";
        public const string AllowOrigin = "AllowOrigin";
        public const string Id = "id";
        public const string PatientPortal = "PatientPortal";
        public const string PatientPortalUI = "http://localhost:3000/";
        public const string SwaggerJson = "/swagger/v1/swagger.json";
        public const string CurrentNameSpace = "PatientPortal";
        public const string CurrentDescription = "PatientPortal Microservices";
        public const string SwaggerVersion = "v1";
        public const string XmlExtension = ".xml";
        public const string Space = " ";
        public const string JwtBearer = "JwtBearer";

        #endregion base

        #region API Routes

        public const string PatientRootRoute = "api";
        public const string LoginRootRoute = "login";
        public const string GetAllPatientsRootRoute = "getAllPatients";
        public const string GetPatientByPatientIdRootRoute = "getPatientDetailsById/{patientId}";
        public const string GetPatientByPatientId = "patient";
        public const string Patients = "patients";
        public const string AuthorizationSecret = "AuthSecret";
        public const string AuthorizationUser = "AuthUser";
        #endregion API Roots
    }
}
