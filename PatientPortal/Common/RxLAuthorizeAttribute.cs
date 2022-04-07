using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace PatientPortal.Common
{
    public class RxLAuthorizeAttribute : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext actionContext)
        {
            actionContext.Result = new UnauthorizedResult();
            return;
        }
       /* /// <summary>
        /// Overridden method to handle OKTA Auth check
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                return;
            }

            //if (SkipAuthorization(actionContext))
            //{
            //    return;
            //}

            if (!IsAuthorized(actionContext))
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var isAuthorized = false;
            IEnumerable<string> tokenHdr;
            try
            {
                actionContext.Request.Headers.TryGetValues("Authorization", out tokenHdr);

                string accessToken = tokenHdr?.FirstOrDefault();

                if (string.IsNullOrEmpty(accessToken))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Token cannot be empty");
                }
                else
                {
                    
                    //dynamic IssuerURL = JObject.Parse(TenantConfiguration.IssuerURL);
                    //if (IssuerURL != null)
                    //{
                    //    var introspectResult = Introspect(IssuerURL.BaseAddress.ToString(), IssuerURL.IntrospectEndPoint.ToString(), TenantConfiguration.ExternalClientID, accessToken).Result;

                    //    if (introspectResult.Item1 == HttpStatusCode.OK && Convert.ToBoolean(((JObject)introspectResult.Item2).SelectToken("active")))
                    //    {
                    //        UserName = ((JObject)introspectResult.Item2).SelectToken("username").ToString();
                    //        isAuthorized = true;
                    //    }
                    //}
                    
                }
            }
            catch (Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Token");
                return isAuthorized;
            }
            return isAuthorized;
        }
        
        /// <summary>
        /// Indicates whether the specified control is allow anonymous.
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns>true if the control is allow anonymous; otherwise, false.</returns>
        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            Contract.Assert(actionContext != null);

            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                       || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }*/
    }
}
