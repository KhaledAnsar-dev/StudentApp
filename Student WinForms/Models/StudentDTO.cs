using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_client_wf.Models
{
    public class StudentDTO
    {
        public StudentDTO(int studentID, string name, int grade, int age)
        {
            this.studentID = studentID;
            this.name = name;
            this.grade = grade;
            this.age = age;
        }
        public int studentID { get; set; }
        public string name { get; set; }
        public int grade { get; set; }
        public int age { get; set; }

    }
}
