using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace webapi.DTOs
{
    //THis DTO is most commonly used for wrapping Http Responses with HttpStatusCodes. If not the case, return just plain DTO's
    public  class APIResponseDTO<T> where T: class,new()
    {
        public bool Success { get; set; }
        public HttpStatusCode Status { get; set; }
        public T? Response { get; set; }
    }
}
