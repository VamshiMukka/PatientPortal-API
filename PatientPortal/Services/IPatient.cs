using PatientPortal.Common;
using PatientPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientPortal.Services
{
    public interface IPatient
    {
        /// <summary>
        /// Get the patient details by PatientId
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        Task<List<PatientDetails>> PatientDetailsById(string patientId);
        /// <summary>
        /// Get all the patient details
        /// </summary>
        /// <returns></returns>
        Task<List<PatientDetails>> GetAllPatients();
        /// <summary>
        /// Generate Jwt token based on user details
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        Task<dynamic> GenerateToken(User userDetails);
    }
}
