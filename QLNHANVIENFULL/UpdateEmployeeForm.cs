using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNHANVIENFULL
{
    public partial class UpdateEmployeeForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        EmployeeDataContext db = new EmployeeDataContext();
        Employee employee = null;
        public UpdateEmployeeForm(Employee e = null)
        {
            employee = e;
            InitializeComponent();
        }
        void LoadDataDep()
        {
            cbbDepartment.Items.Clear();
            cbbDepartment.DataSource = db.Departments;
            cbbDepartment.DisplayMember = "DepName";
            cbbDepartment.ValueMember = "DepId";
        }
        private void ptbClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void pnltop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            
            if(string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtSalary.Text))
            {
                MessageBox.Show("Miss Data", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Employee epl = new Employee();
                epl.EmpName = txtName.Text.Trim();
                epl.EmpSal = int.Parse(txtSalary.Text);
                epl.EmpDOB = dtpkDOB.Value;
                epl.EmpJDate = dtpkJDate.Value;
                epl.EmpGen = cbbgender.SelectedItem.ToString();
                epl.EmpDep = int.Parse(cbbDepartment.SelectedValue.ToString());
                epl.EmpImage = "avatar.jpg";
                db.Employees.InsertOnSubmit(epl);
                db.SubmitChanges();
                MessageBox.Show("Added sucessfully !", "Notification",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Dispose();
            } 
                
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtName.Text)||string.IsNullOrEmpty(txtSalary.Text)||string.IsNullOrEmpty (cbbgender.Text)||string.IsNullOrEmpty(cbbDepartment.Text))
            {
                MessageBox.Show("Miss Data","Notification",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                Employee empl = db.Employees.SingleOrDefault(m=> m.EmpID==employee.EmpID);
                empl.EmpName = txtName.Text.Trim();
                empl.EmpSal = int.Parse(txtSalary.Text);
                empl.EmpDOB = dtpkDOB.Value;
                empl.EmpJDate = dtpkJDate.Value;
                empl.EmpGen = cbbgender.SelectedItem.ToString();
                empl.EmpDep = int.Parse(cbbDepartment.SelectedValue.ToString());
                db.SubmitChanges();
                MessageBox.Show("Updated sucessfully !", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
            }
        }

        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void UpdateEmployeeForm_Load(object sender, EventArgs e)
        {
            LoadDataDep();
            if(employee!=null)
            {
                txtName.Text = employee.EmpName;
                txtSalary.Text = employee.EmpSal.ToString();
                cbbgender.Text = employee.EmpGen.ToString();
                string text= employee.Department.DepName.ToString().Trim();
                cbbDepartment.Text = text;
                //int index = cbbDepartment.Items.IndexOf(text);
                //cbbDepartment.SelectedIndex = index;
                dtpkDOB.Text = employee.EmpDOB.ToString();
                dtpkJDate.Text = employee.EmpJDate.ToString();
            }    
        }
    }
}
