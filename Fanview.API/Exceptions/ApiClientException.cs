using System;

namespace Fanview.API.Exceptions
{
    public class ApiClientException : Exception
    {
        public ApiClientException(string message) : base(message) { }
    }
}
