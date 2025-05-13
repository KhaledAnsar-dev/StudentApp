using Student_client_wf.Services;

namespace Student_client_wf
{
    public partial class frmStudentManagement : Form
    {
        public frmStudentManagement()
        {
            InitializeComponent();
        }
        private async void frmStudentManagement_Load(object sender, EventArgs e)
        {
            var service = new StudentService();
            var students = await service.GetAllStudentsAsync();

            if (students != null)
            {
                dgvStudentTable.DataSource = students;
            }
            else
            {
                MessageBox.Show("No data uploaded", "Error");
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            frmAddUpdateStudent frmAddUpdateStudent = new frmAddUpdateStudent();
            frmAddUpdateStudent.ShowDialog();

            // update interface
            frmStudentManagement_Load(null, null);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            int studnetID = (int)dgvStudentTable.CurrentRow.Cells[0].Value;
            frmAddUpdateStudent frmAddUpdateStudent = new frmAddUpdateStudent(studnetID);
            frmAddUpdateStudent.ShowDialog();

            // update interface
            frmStudentManagement_Load(null, null);
        }
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            var service = new StudentService();
            int studnetID = (int)dgvStudentTable.CurrentRow.Cells[0].Value;
            if (Convert.ToBoolean(await service.DeleteStudentAsync(studnetID)))
                MessageBox.Show("Deleted successfully");
            else
                MessageBox.Show("Error");
            // update interface
            frmStudentManagement_Load(null, null);
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            int studnetID = (int)dgvStudentTable.CurrentRow.Cells[0].Value;
            frmStudentDetails studentDetails = new frmStudentDetails(studnetID);
            studentDetails.ShowDialog();
        }
    }
}
