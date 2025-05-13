using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentData
{
    public class clsDataSettings
    {
        static public string ConnectionString
        {
            get
            {
                return "Data Source=.;Initial Catalog=StudentsDB;Integrated Security=True;TrustServerCertificate=True";
            }
        }
    }
}
