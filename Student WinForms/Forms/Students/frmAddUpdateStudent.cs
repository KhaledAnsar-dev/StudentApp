using Student_client_wf.Models;
using Student_client_wf.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_client_wf
{
    public partial class frmAddUpdateStudent : Form
    {
        public enum enMode { Add = 0, Update = 1 }
        private enMode mode;
        private int _studentID;

        public frmAddUpdateStudent(enMode mode = enMode.Add)
        {
            InitializeComponent();
            this.mode = mode;
        }
        public frmAddUpdateStudent(int studetID, enMode mode = enMode.Update)
        {
            InitializeComponent();
            this.mode = mode;
            _studentID = studetID;
        }
        private StudentDTO GetStudentDataObject()
        {
            int studentID = txtID.Text != "---" ? Convert.ToInt32(txtID.Text) : 0;
            string studentName = txtName.Text;
            int studentAge = Convert.ToInt32(txtAge.Text);
            int studentGrade = Convert.ToInt32(txtGrade.Text);

            return new StudentDTO(studentID, studentName, studentGrade, studentAge);
        }
        private void UpdateForm(StudentDTO student)
        {
            txtID.Text = student.studentID.ToString();
        }
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("الاسم لا يمكن أن يكون فارغًا", "تحقق من البيانات");
                txtName.Focus();
                return false;
            }

            if (!int.TryParse(txtAge.Text, out int age) || age < 18 || age > 70)
            {
                MessageBox.Show("يرجى إدخال عمر صحيح بين 18 و 70", "تحقق من البيانات");
                txtAge.Focus();
                return false;
            }

            if (!int.TryParse(txtGrade.Text, out int grade) || grade < 1 || grade > 101)
            {
                MessageBox.Show("يرجى إدخال مستوى دراسي صحيح بين 1 و 100", "تحقق من البيانات");
                txtGrade.Focus();
                return false;
            }

            return true;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {

            if (!ValidateInputs())
                return;

            var student = GetStudentDataObject();

            var service = new StudentService();

            if (mode == enMode.Add)
            {
                var result = await service.AddStudentAsync(student);

                if (result != null)
                {
                    MessageBox.Show("Added successfully");
                    UpdateForm(result);
                    mode = enMode.Update;
                }
                else
                    MessageBox.Show("Error");
            }
            else
            {
                if (Convert.ToBoolean(await service.UpdateStudentAsync(_studentID, student)))
                    MessageBox.Show("Updated successfully");
                else
                    MessageBox.Show("Error");
            }


        }

        private async void frmAddUpdateStudent_Load(object sender, EventArgs e)
        {
            if(mode == enMode.Update)
            {
                var student = await StudentService.FindAsync(_studentID);

                if (student != null)
                {
                    txtID.Text = student.studentID != 0 ? student.studentID.ToString() : "---";
                    txtName.Text = student.name ?? string.Empty;
                    txtAge.Text = student.age.ToString();
                    txtGrade.Text = student.grade.ToString();
                }
            }
        }
    }
}
