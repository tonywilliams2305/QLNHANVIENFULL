using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNHANVIENFULL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Form activeForm = null;
        public void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(childForm);
            pnlMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private void ptbDepartment_Click(object sender, EventArgs e)
        {
            OpenChildForm(new DepartmentForm());
        }

        private void ptbEmployee_Click(object sender, EventArgs e)
        {
            OpenChildForm(new EmployeeForm());
        }

        private void ptbSalary_Click(object sender, EventArgs e)
        {
            OpenChildForm(new SalaryForm());
        }

        private void ptbLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want Logout ?", "Infor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void ptbAccount_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AccountForm());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OpenChildForm(new IntroForm());
        }
    }
}
