using StudentData.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentData
{
    public class UserRepository
    {
        public static User? GetByEmail(string email)
        {
			try
			{
				using (var context = new StudentDbContext())
				{
					return context.Users.Where(u => u.Email == email).FirstOrDefault();
				}
			}
			catch (Exception)
			{

				throw;
			}
        }
    }
}
