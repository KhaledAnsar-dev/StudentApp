using Microsoft.Data.SqlClient;
using StudentData;
using System.Data;
using System.Reflection;
using System.Reflection.PortableExecutable;
using StudentShared.Dtos;
using StudentData.DataModels;
using Microsoft.EntityFrameworkCore;
namespace StuudentData
{


    public class StudentRepository
    {
        // Factory method: creates a new DbContext instance for each call
        private static StudentDbContext CreateContext() => new StudentDbContext();

        // Helper method: convert Student entity into a StudentDTO
        private static StudentDTO ToDto(Student s) =>
            new StudentDTO(s.StudentId, s.Name ?? string.Empty, s.Grade ?? 0, s.Age ?? 0);

        // Get one student by ID (returns null if not found)
        public static StudentDTO? GetStudentByID(int? studentID)
        {
            if (studentID is null || studentID <= 0) return null;

            try
            {
                using var ctx = CreateContext(); // new DbContext for this call
                var s = ctx.Students.AsNoTracking()
                                    .FirstOrDefault(x => x.StudentId == studentID.Value);
                return s is null ? null : ToDto(s);
            }
            catch
            {
                // could log exception here
                return null;
            }
        }

        // Insert a new student and return the generated ID
        public static int AddNewStudent(StudentDTO newStudent)
        {
            if (newStudent is null || string.IsNullOrWhiteSpace(newStudent.name)) return 0;

            try
            {
                using var ctx = CreateContext(); // open context
                var entity = new Student
                {
                    Name = newStudent.name,
                    Grade = newStudent.grade,
                    Age = newStudent.age
                };

                ctx.Students.Add(entity);   // mark entity for insertion
                var saved = ctx.SaveChanges(); // commit changes to DB
                return saved > 0 ? entity.StudentId : 0;
            }
            catch
            {
                return 0;
            }
        }

        // Update an existing student (returns true if successful)
        public static bool UpdateStudent(StudentDTO updatedStudent)
        {
            if (updatedStudent is null || updatedStudent.studentID <= 0) return false;

            try
            {
                using var ctx = CreateContext(); // new DbContext
                var entity = ctx.Students.FirstOrDefault(s => s.StudentId == updatedStudent.studentID);
                if (entity is null) return false;

                // apply updates
                entity.Name = updatedStudent.name;
                entity.Grade = updatedStudent.grade;
                entity.Age = updatedStudent.age;

                return ctx.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        // Delete a student by ID
        public static bool DeleteStudent(int studentID)
        {
            if (studentID <= 0) return false;

            try
            {
                using var ctx = CreateContext(); // open context
                var entity = ctx.Students.FirstOrDefault(s => s.StudentId == studentID);
                if (entity is null) return false;

                ctx.Students.Remove(entity); // mark entity for deletion
                return ctx.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        // Return all students in the database
        public static List<StudentDTO> GetAllStudents()
        {
            try
            {
                using var ctx = CreateContext(); // new DbContext
                return ctx.Students.AsNoTracking()
                                   .OrderBy(s => s.Name) // optional sorting
                                   .Select(s => new StudentDTO(
                                        s.StudentId,
                                        s.Name ?? string.Empty,
                                        s.Grade ?? 0,
                                        s.Age ?? 0
                                    ))
                                   .ToList();
            }
            catch
            {
                return new List<StudentDTO>();
            }
        }
    }
}
