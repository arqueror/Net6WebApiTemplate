using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.DTOs
{
    public class UserDto
    {
        public string UserId { get; set; } = string.Empty;
        public string ReferalMethod { get; set; } = string.Empty;
        public string UpLine { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Datetime { get; set; } = DateTime.Now;
        public string Picture { get; set; } = string.Empty;
        public DateTime? Birthday { get; set; } = DateTime.Now;
        public string Phone { get; set; } = string.Empty;
        public bool Newsletteroption { get; set; } = false;
    }
}
