using System;
using System.Net;

namespace IpLookup.Infrastructure.Api
{
    public class ApiException : Exception
    {
        public ApiException(HttpStatusCode statusCode, string response)
        {
            StatusCode = statusCode;
            Response = response;
        }

        public HttpStatusCode StatusCode { get; }
        public string Response { get; }
    }
}
