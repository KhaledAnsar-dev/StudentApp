using StudentShared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentBusiness.Abstractions
{
    public interface IStudentService
    {
        List<StudentDTO> GetAllStudents();
        StudentDTO? Find(int studentID);
        StudentDTO? AddStudent(StudentDTO studentDTO);
        StudentDTO? UpdateStudent(int studentID, StudentDTO studentDTO);
        bool DeleteStudent(int ID);
    }
}
