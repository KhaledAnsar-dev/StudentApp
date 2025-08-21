using StuudentData;
using StudentShared.Dtos;
using StudentData.Abstractions;
using StudentBusiness.Abstractions;

namespace StudentBusiness
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;

        public enum enMode { Add, Update};
        enMode mode = enMode.Add;

        public int studentID { get; set; }
        public string name { get; set; }
        public int grade { get; set; }
        public int age { get; set; }

        public StudentDTO SDTO
        {
            get { return new StudentDTO(this.studentID,this.name,this.grade ,this.age); }
        }

        public StudentService(StudentDTO studentDTO, enMode mode = enMode.Add)
        {
            this.studentID = studentDTO.studentID;
            this.name = studentDTO.name;
            this.grade = studentDTO.grade;
            this.age = studentDTO.age;

           this.mode = mode;
        }
        public StudentService(IStudentRepository repo)
        {
            _repo = repo;
        }

        public StudentDTO? AddStudent(StudentDTO studentDTO)
        {
            var id = _repo.AddNewStudent(studentDTO);
            if (id == -1) return null;

            studentDTO.studentID = id;
            return studentDTO;
        }

        public StudentDTO? UpdateStudent(int studentID, StudentDTO studentDTO)
        {
            var existing = _repo.GetStudentByID(studentID);
            if (existing == null) return null;

            studentDTO.studentID = studentID;
            var success = _repo.UpdateStudent(studentDTO);
            return success ? studentDTO : null;
        }
        public List<StudentDTO> GetAllStudents() => _repo.GetAllStudents();
        public StudentDTO? Find(int studentID) => _repo.GetStudentByID(studentID);

        public bool DeleteStudent(int ID)
        {
            return _repo.DeleteStudent(ID);
        }
    }
}
