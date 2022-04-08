using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.Data.Exceptions
{
    /// <summary>
    /// This is an exception that will be thrown if something fails within SQLite service class
    /// </summary>
    public class SqLiteServiceException : Exception
    {
        public SqLiteServiceException(Exception ex) : base("SQLite Exception", ex)
        {
        }
    }
}
