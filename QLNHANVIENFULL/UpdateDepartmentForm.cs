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
    public partial class UpdateDepartmentForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        Department department = null;
        EmployeeDataContext db = new EmployeeDataContext();
        public UpdateDepartmentForm(Department dep=null)
        {
            InitializeComponent();
        }

        private void ptbClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Miss Data", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Department dep = new Department(); 
                dep.DepName=txtName.Text.Trim();
                db.Departments.InsertOnSubmit(dep);
                db.SubmitChanges();
                MessageBox.Show("Inserted Sucessfully", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
            } 
                
        }

        private void UpdateDepartmentForm_Load(object sender, EventArgs e)
        {
            if(department!=null)
            {
                txtName.Text = department.DepName;
            }    
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Miss Data", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                Department dep = db.Departments.SingleOrDefault(d => d.DepId == department.DepId);
                dep.DepName = txtName.Text;
                db.SubmitChanges();
                MessageBox.Show("Updated Sucessfully", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
            } 
                
        }

        private void pnltop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
