using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PatientPortal.Common;
using PatientPortal.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PatientPortal.Services
{
    public class Patient : IPatient
    {
        private IConfiguration _configuration { get; }
        public string PatientRequestUri { get; set; }
        public string UserSecret { get; set; }
        public string User { get; set; }
        private IDictionary _dictionary { get; }
        public Patient(IConfiguration configuration, IDictionary dictionary)
        {
            _configuration = configuration;
            _dictionary = dictionary;
            if(_dictionary==null)
            {
                _dictionary = new Dictionary<int, string>();
            }
            PatientRequestUri = _configuration.GetValue<string>(Constants.PatientServiceURL);
            UserSecret = _configuration.GetValue<string>(Constants.AuthorizationSecret);
            User = _configuration.GetValue<string>(Constants.AuthorizationUser);
        }
        
        /// <summary>
        /// Get all the patient details
        /// </summary>
        /// <returns></returns>
        public async Task<List<PatientDetails>> GetAllPatients()
        {
            try
            {
                var patientRestRequest = ConstructRestRequest(string.Concat(PatientRequestUri, Constants.ForwardSlash, Constants.Patients));
                var patientInformation = CustomMappingForPatient(await GetApiClientRequest<List<PatientDetails>>(patientRestRequest));
                return patientInformation;
            }
            catch (Exception ex)
            {
                var newException = new Exception(string.Format(Constants.ErrorOccurred, MethodBase.GetCurrentMethod().DeclaringType.FullName), ex);
                throw newException;
            }
        }
        /// <summary>
        /// Get the patient details by PatientId
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public async Task<List<PatientDetails>> PatientDetailsById(string patientId)
        {
            var patientResult = new List<PatientDetails>();
            try
            {
                var patientRestRequest = ConstructRestRequest(string.Concat(PatientRequestUri, Constants.ForwardSlash, Constants.GetPatientByPatientId, Constants.ForwardSlash, _dictionary[Convert.ToInt32(patientId)]));
                var patientInformation = await GetApiClientRequest<PatientDetails>(patientRestRequest);
                patientResult.Add(patientInformation);
                return patientResult;
            }
            catch (Exception ex)
            {
                var newException = new Exception(string.Format(Constants.ErrorOccurred, MethodBase.GetCurrentMethod().DeclaringType.FullName), ex);
                throw newException;
            }
        }

        /// <summary>
        /// Generic method to make a async get request call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getRequest"></param>
        /// <returns></returns>
        public async static Task<T> GetApiClientRequest<T>(RestRequest getRequest)
        {
            dynamic response = null;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(getRequest.RequestUri);
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            httpWebRequest.Method = getRequest.HttpWebRequestMethod;
            httpWebRequest.ContentType = getRequest.MediaType;
            if (getRequest.HeaderKeyValue != null)
            {
                foreach (var keyValuePair in getRequest.HeaderKeyValue)
                {
                    httpWebRequest.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            httpWebRequest.Headers.Set(Constants.CacheControl, Constants.NoCache);
            try
            {
                httpWebRequest.ServerCertificateValidationCallback += (sender, cert, chain, error) => Constants.True;
                string responseText;
                using (var httpResponse = ((HttpWebResponse)await httpWebRequest.GetResponseAsync().ConfigureAwait(false)))
                {
                    var encoding = Encoding.ASCII;
                    using (var reader = new StreamReader(httpResponse.GetResponseStream(), encoding))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                return JsonConvert.DeserializeObject<T>(responseText); ;
            }
            catch (Exception ex)
            {
                return (T)response;
            }
        }

        /// <summary>
        /// Generate Jwt token based on user details
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        public async Task<dynamic> GenerateToken(User userDetails)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();

            if ((userDetails.UserName.Equals(User) && userDetails.Password.Equals(UserSecret)))
                {
                var key = Encoding.ASCII.GetBytes(userDetails.Password);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim(Constants.Id, userDetails.UserName)}),
                    Expires = DateTime.UtcNow.AddSeconds(60),
                    Issuer = Constants.PatientPortal,
                    Audience = Constants.PatientPortalUI,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var accessToken = tokenHandler.WriteToken(token);
                return await Task.Run(() => accessToken);
            }
            else
            {
                return Constants.UnAuthorizedUser;
            }
        }
        #region Private Methods
        /// <summary>
        /// Construct the custom patient list to do not expose the patientIds
        /// </summary>
        /// <param name="patientDetails"></param>
        /// <returns></returns>
        private List<PatientDetails> CustomMappingForPatient(List<PatientDetails> patientDetails)
        {
            var patientInformationList = new List<PatientDetails>();
            patientInformationList.AddRange(patientDetails);
            int counter = 1;
            _dictionary.Clear();
            foreach (var patient in patientInformationList)
            {
                _dictionary.Add(counter, patient.PatientId);
                patient.PatientId = Convert.ToString(counter);
                counter++;
            }
            return patientInformationList;
        }
        /// <summary>
        /// Construct the Get rest request.
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        private RestRequest ConstructRestRequest(string requestUrl)
        {
            var patientRestRequest = new RestRequest
            {
                RequestUri = requestUrl,
                MediaType = Constants.MediaTypeJson,
                HttpWebRequestMethod = Constants.Get,
                HeaderKeyValue = new Dictionary<string, string>(),
                PostData = null
            };

            return patientRestRequest;

        }

        #endregion
    }
}
