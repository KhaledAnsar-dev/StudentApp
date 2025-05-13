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
    public partial class frmStudentDetails : Form
    {
        private int _studentID;
        public frmStudentDetails(int studentID)
        {
            InitializeComponent();
            _studentID = studentID;
        }

        private async void frmStudentDetails_Load(object sender, EventArgs e)
        {
            bool result = await ctrlStudentCard1.LoadStudentInfo(_studentID);

            if (!result)
                MessageBox.Show("Error occured : Student not found");
        }
    }
}
