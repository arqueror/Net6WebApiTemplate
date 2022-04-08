using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.DTOs;

namespace webapi.DomainServices.Interfaces
{
    public interface IAccountService
    {
        UserDto GetUserProfile(string userAuthId);
        int GetUserId(string userAuthId);
        bool IboUplineExists(string ibonumber);
        bool IsUserNameAvailable(string username);
        bool IsEmailAvailable(string email);
        bool IsClientNumberAvailable(string clientNumber);
        UserDto CreateClient(UserDto user);
        UserDto SaveNewPassword(string id, string newPassword);
        UserDto GetUserProfileById(int id);
    }
}
