using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentBusiness;
using StudentBusiness.Abstractions;
using StudentData.Abstractions;
using StudentShared.Dtos;

namespace Student_API_Project.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService) =>
            _studentService = studentService;
        

        [HttpGet("All", Name = "GetAllStudents")]
        [Authorize(Roles = "Admin,Editor,Viewer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        {
            List<StudentDTO> studentList = _studentService.GetAllStudents();
            if (studentList.Count == 0)
                return NotFound("No student found");

            return Ok(studentList);
        }


        [HttpGet("Find/{studentID}", Name = "GetStudentById")]
        [Authorize(Roles = "Admin,Editor,Viewer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<StudentDTO> GetStudentByID(int studentID)
        {
            if (studentID <= 0)
                return BadRequest($"Wrong input, {studentID} is not acceptable.");

            var student = _studentService.Find(studentID);

            if (student == null)
                return NotFound($"No student found with ID {studentID}");

            //we return the DTO not the student object.
            return Ok(student);
        }


        [HttpPost("New",Name = "AddNewStudent")]
        [Authorize(Roles = "Admin,Editor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<StudentDTO> AddNewStudent(StudentDTO newStudent)
        {
            if (newStudent == null || string.IsNullOrEmpty(newStudent.name) || newStudent.age < 0 || newStudent.grade < 0)
                return BadRequest("Invalid student data.");

            var addedStudent = _studentService.AddStudent(newStudent);
            if (addedStudent == null)
                return BadRequest("Failed to add student.");

            return CreatedAtRoute("GetStudentById", new { studentID = addedStudent.studentID }, addedStudent);
        }


        [HttpDelete("{studentID}" ,Name = "DeleteStudent")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult DeleteStudent(int studentID)
        {
            //we validate the data here
            if (studentID <= 0)
            {
                return BadRequest("Invalid student Id.");
            }

            if (_studentService.DeleteStudent(studentID))
            {
                return Ok($"Student with ID {studentID} Has deleted");
            }
            else
            {
                return NotFound($"Student with ID {studentID} is not exists");
            }
        }


        [HttpPut("{studentID}",Name = "UpdateStudent")]
        [Authorize(Roles = "Admin,Editor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<StudentDTO> UpdateStudent(int studentID , StudentDTO updatedStudent)
        {
            if (studentID < 1 || updatedStudent == null || string.IsNullOrEmpty(updatedStudent.name) || updatedStudent.age < 0 || updatedStudent.grade < 0)
                return BadRequest("Invalid student data.");

            var student = _studentService.UpdateStudent(studentID, updatedStudent);
            if (student == null)
                return NotFound("Student not found.");

            return Ok(student);
        }
    }
}
