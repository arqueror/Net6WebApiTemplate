using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.DomainServices.Exceptions
{
    /// <summary>
    /// This is an exception that will be thrown if a remote WebApi call fails
    /// </summary>
    public class WepApiCallException : Exception
    {
        public WepApiCallException(Exception ex, string methodName) : base($"WebApiCallException thrown at {methodName}()", ex)
        {
        }
    }
}
