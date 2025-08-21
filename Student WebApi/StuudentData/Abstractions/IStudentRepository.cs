using StudentShared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentData.Abstractions
{
    public interface IStudentRepository
    {
        StudentDTO? GetStudentByID(int? studentID);
        int AddNewStudent(StudentDTO newStudent);
        bool UpdateStudent(StudentDTO updatedStudent);
        bool DeleteStudent(int studentID);
        List<StudentDTO> GetAllStudents();
    }
}
