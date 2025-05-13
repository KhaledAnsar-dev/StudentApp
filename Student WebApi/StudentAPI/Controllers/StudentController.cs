using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentBusiness;
using StuudentData;

namespace Student_API_Project.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        {
            List<StudentDTO> studentList = StudentCore.GetAllStudents();
            if (studentList.Count == 0)
                return NotFound("No student found");

            return Ok(studentList);
        }


        [HttpGet("AllRoles", Name = "GetAllRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllRoles()
        {

            List<StudentDTO> studentList = StudentCore.GetAllStudents();
            if (studentList.Count == 0)
                return NotFound("No student found");

            return Ok(studentList);

        }


        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetPassedStudents()
        {
            List<StudentDTO> studentList = StudentCore.GetPassedStudents();
            if (studentList.Count == 0)
                return NotFound("No student found");
            return Ok(studentList);
        }


        [HttpGet("Averge", Name = "GetGradeAVG")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<double> GetGradeAVG()
        {
            var gradeAVG = StudentCore.GetAverageGrade();
            return Ok(gradeAVG);
        }


        [HttpGet("Find/{studentID}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> GetStudentByID(int studentID)
        {
            if (studentID <= 0)
                return BadRequest($"Wrong input, {studentID} is not acceptable.");

            StudentCore student = StudentCore.Find(studentID);

            if (student == null)
                return NotFound($"No student found with ID {studentID}");

            //here we get only the DTO object to send it back.
            StudentDTO SDTO = student.SDTO;

            //we return the DTO not the student object.
            return Ok(SDTO);
        }


        [HttpPost(Name = "AddNewStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> AddNewStudent(StudentDTO newStudent)
        {
            //we validate the data here
            if (newStudent == null || string.IsNullOrEmpty(newStudent.name) || newStudent.age < 0 || newStudent.grade < 0)
            {
                return BadRequest("Invalid student data.");
            }

            var student = new StudentCore(newStudent);
            student.Save();
            newStudent.studentID = student.studentID;

            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetStudentById", new { studentID = newStudent.studentID}, newStudent);
        }


        [HttpDelete("{studentID}" ,Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteStudent(int studentID)
        {
            //we validate the data here
            if (studentID <= 0)
            {
                return BadRequest("Invalid student Id.");
            }

            if (StudentCore.DeleteStudent(studentID))
            {
                return Ok($"Student with ID {studentID} Has deleted");
            }
            else
            {
                return NotFound($"Student with ID {studentID} is not exists");
            }
        }


        [HttpPut("{studentID}",Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> UpdateStudent(int studentID , StudentDTO updatedStudent)
        {
            //we validate the data here
            if (studentID < 1 || updatedStudent == null || string.IsNullOrEmpty(updatedStudent.name) || updatedStudent.age < 0 || updatedStudent.grade < 0)
            {
                return BadRequest("Invalid student data.");
            }

            var student = StudentCore.Find(studentID);            
            if (student == null)
            {
                return NotFound("Student is not exists");               
            }

            student.age = updatedStudent.age;
            student.grade = updatedStudent.grade;
            student.name = updatedStudent.name;
            student.Save();
            return Ok(student.SDTO);
        }
    }
}
