using StudentBusiness.Global;
using StudentData;
using StudentShared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentBusiness
{
    public class AuthService
    {
        public static UserDto? ValidateUser(string email, string password)
        {
            var user = UserRepository.GetByEmail(email);

            if (user == null) return null;

            if(!Helper.CheckPassword(password, user.PasswordHash))
                return null;

            return new UserDto
            {
                Id = user.UserId,
                Email = user.Email,
                Role = user.Role
            };

        }
    }
}
