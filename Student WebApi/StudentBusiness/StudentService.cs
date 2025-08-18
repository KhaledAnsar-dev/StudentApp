using StuudentData;
using StudentShared.Dtos;

namespace StudentBusiness
{
    public class StudentService
    {
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

        private bool _AddNewStudent()
        {
            //call DataAccess Layer 

            this.studentID = StudentRepository.AddNewStudent(SDTO);

            return (this.studentID != -1);
        }
        private bool _UpdateStudent()
        {
            return StudentRepository.UpdateStudent(SDTO);
        }
        public static List<StudentDTO> GetAllStudents()
        {
            return StudentRepository.GetAllStudents();
        }
        public static List<StudentDTO> GetPassedStudents()
        {
            return StudentRepository.GetPassedStudents();
        }
        public static StudentService Find(int studentID)
        {
            var studentDTO = StudentRepository.GetStudentByID(studentID);

            if (studentDTO != null)
            {
                return new StudentService(studentDTO,enMode.Update);
            }
            else
            {
                return null;
            }
        }
        public bool Save()
        {
            switch (mode)
            {
                case enMode.Add:
                    if (_AddNewStudent())
                    {

                        mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateStudent();

            }

            return false;
        }
        public static bool DeleteStudent(int ID)
        {
            return StudentRepository.DeleteStudent(ID);
        }
        public static double GetAverageGrade()
        {
            return StudentRepository.GetAverageGrade();
        }
    }
}
