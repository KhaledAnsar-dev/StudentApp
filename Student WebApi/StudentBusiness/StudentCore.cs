using StuudentData;

namespace StudentBusiness
{
    public class StudentCore
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

        public StudentCore(StudentDTO studentDTO, enMode mode = enMode.Add)
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

            this.studentID = StudentDataSource.AddNewStudent(SDTO);

            return (this.studentID != -1);
        }
        private bool _UpdateStudent()
        {
            return StudentDataSource.UpdateStudent(SDTO);
        }
        public static List<StudentDTO> GetAllStudents()
        {
            return StudentDataSource.GetAllStudents();
        }
        public static List<StudentDTO> GetPassedStudents()
        {
            return StudentDataSource.GetPassedStudents();
        }
        public static StudentCore Find(int studentID)
        {
            var studentDTO = StudentDataSource.GetStudentByID(studentID);

            if (studentDTO != null)
            {
                return new StudentCore(studentDTO,enMode.Update);
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
            return StudentDataSource.DeleteStudent(ID);
        }
        public static double GetAverageGrade()
        {
            return StudentDataSource.GetAverageGrade();
        }
    }
}
