using System;
using System.Net;
using System.Net.Http;

namespace LicenseValidator.Core.Exceptions
{
    public class LicenseValidationException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public HttpResponseMessage HttpResponse { get; set; }

        public LicenseValidationException()
        {
            
        }

        public LicenseValidationException(Exception ex) : base ("License Exception", ex)
        {
            
        }
        
    }
}
