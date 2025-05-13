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
    public partial class ctrlStudentCard : UserControl
    {
        public ctrlStudentCard()
        {
            InitializeComponent();
        }

        public async Task<bool> LoadStudentInfo(int studentID)
        {
            var student = await StudentService.FindAsync(studentID);

            if (student != null)
            {
                stuID.Text = student.studentID.ToString();
                stuName.Text = student.name.ToString();
                stuAge.Text = student.age.ToString();
                stuGrade.Text = student.grade.ToString();
                return true;
            }
            else
                return false;
        }
    }
}
