using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.DomainServices.Exceptions
{
    /// <summary>
    /// This is an exception that will be thrown if a error occurs while mapping DTO to Entities and vice versa
    /// </summary>
    public class FactoryDataMappingException : Exception
    {
        public FactoryDataMappingException(Exception ex, string methodName) : base($"FactoryDataMappingException thrown at {methodName}()", ex)
        {
        }
    }
}
