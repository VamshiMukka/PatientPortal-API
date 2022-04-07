using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientPortal.Common
{
    public class ApplicationLog
    {
        /// <summary>
        /// Return the incoming request as a formatted string
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> GetLoggableRequestBody(HttpRequest request, string errorMessage = "", string stackTrace = "", bool isError = false)
        {
            string returnString = string.Empty;
            try
            {
                #region Request Headers

                var headers = request.Headers;
                var headerString = new System.Text.StringBuilder();
                if (headers.Keys.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        if (headerString.Length == 0)
                            headerString.Append($"{header.Key}: {header.Value} ");
                        else
                            headerString.Append($" || {header.Key}: {header.Value} ");
                    }
                }
                else
                    headerString.Append(Constants.NoRequestHeadersFound);

                #endregion

                request.Body.Position = 0;
                using (var reader = new System.IO.StreamReader(request.Body))
                {
                    var newLine = Environment.NewLine;

                    if (isError)
                        returnString = $"ERROR: Message: {errorMessage}, StackTrace: {stackTrace}, Scheme: {request.Scheme}, Host: {request.Host}, Action: {request.Method.ToUpperInvariant()}, Path: {request.Path}, Headers: [{headerString}], QueryString {request.QueryString}, RequestBody: {await reader.ReadToEndAsync()}";
                    else
                        returnString = $"INFORMATION: Scheme: {request.Scheme}, Host: {request.Host}, Action: {request.Method.ToUpperInvariant()}, Path: {request.Path}, Headers: [{headerString}], QueryString {request.QueryString}, RequestBody: {await reader.ReadToEndAsync()}";
                }
            }
            catch (Exception ex)
            {
                return ($"ERROR IN LOGGING: {Constants.CouldNotCaptureRequest}");
            }
            return returnString;
        }

    }
}
