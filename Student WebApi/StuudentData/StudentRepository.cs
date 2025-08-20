using Microsoft.Data.SqlClient;
using StudentData;
using System.Data;
using System.Reflection;
using System.Reflection.PortableExecutable;
using StudentShared.Dtos;
using StudentData.DataModels;
namespace StuudentData
{


    public class StudentRepository
    {
        public static StudentDTO? GetStudentByID(int? studentID)
        {
            if (studentID is null) return null;

            try
            {
                using (var ctx = new StudentDbContext())
                {
                    return ctx.Students
                        .Where(x => x.StudentId == studentID.Value)
                        .Select(s => new StudentDTO(
                            s.StudentId,
                            s.Name ?? string.Empty,
                            s.Grade ?? 0,
                            s.Age ?? 0
                        ))
                        .FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }
        public static int AddNewStudent(StudentDTO newStudent)
        {
            try
            {
                using (var ctx = new StudentDbContext())
                {
                    var entity = new Student
                    {
                        Name = newStudent.name,
                        Grade = newStudent.grade,
                        Age = newStudent.age
                    };

                    ctx.Students.Add(entity);
                    ctx.SaveChanges();
                    return entity.StudentId; // DB-generated
                }
            }
            catch (Exception)
            {
                // optional: log
                return 0;
            }
        }
        public static bool UpdateStudent(StudentDTO updatedStudent)
        {
            try
            {
                using (var ctx = new StudentDbContext())
                {
                    var entity = ctx.Students.FirstOrDefault(s => s.StudentId == updatedStudent.studentID);
                    if (entity is null) return false;

                    entity.Name = updatedStudent.name;
                    entity.Grade = updatedStudent.grade;
                    entity.Age = updatedStudent.age;

                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                // optional: log
                return false;
            }
        }
        public static bool DeleteStudent(int studentID)
        {
            try
            {
                using (var ctx = new StudentDbContext())
                {
                    var entity = ctx.Students.FirstOrDefault(s => s.StudentId == studentID);
                    if (entity is null) return false;

                    ctx.Students.Remove(entity);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                // optional: log
                return false;
            }
        }
        public static List<StudentDTO> GetAllStudents()
        {
            try
            {
                using (var ctx = new StudentDbContext())
                {
                    return ctx.Students
                        .Select(s => new StudentDTO(
                            s.StudentId,
                            s.Name ?? string.Empty,
                            s.Grade ?? 0,
                            s.Age ?? 0
                        ))
                        .ToList();
                }
            }
            catch
            {
                return new List<StudentDTO>();
            }
        }
    }
}
