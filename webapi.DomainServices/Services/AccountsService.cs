using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.Data.Interfaces;
using webapi.DomainServices.Interfaces;
using webapi.DTOs;

namespace webapi.DomainServices.Services
{
    public class AccountsService : IAccountService
    {
        public AccountsService(ISqlLiteContext db)
        {
        }

        public UserDto CreateClient(UserDto user)
        {
            throw new NotImplementedException();
        }

        public int GetUserId(string userAuthId)
        {
            throw new NotImplementedException();
        }

        public UserDto GetUserProfile(string userAuthId)
        {
            throw new NotImplementedException();
        }

        public UserDto GetUserProfileById(int id)
        {
            throw new NotImplementedException();
        }

        public bool IboUplineExists(string ibonumber)
        {
            throw new NotImplementedException();
        }

        public bool IsClientNumberAvailable(string clientNumber)
        {
            throw new NotImplementedException();
        }

        public bool IsEmailAvailable(string email)
        {
            throw new NotImplementedException();
        }

        public bool IsUserNameAvailable(string username)
        {
            throw new NotImplementedException();
        }

        public UserDto SaveNewPassword(string id, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
