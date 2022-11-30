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
    public partial class UpdateSalaryForm : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        Salary salary = null;
        public UpdateSalaryForm(Salary sal = null)
        {
            salary = sal;
            InitializeComponent();
        }
        EmployeeDataContext db = new EmployeeDataContext();
        private void LoadEmployee()
        {
            cbbEmployee.Items.Clear();
            cbbEmployee.DataSource = db.Employees;
            cbbEmployee.DisplayMember = "EmpName";
            cbbEmployee.ValueMember = "EmpId";
        }
        private void ptbClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void UpdateSalaryForm_Load(object sender, EventArgs e)
        {
            LoadEmployee();
            if (salary != null)
            {
                txtSalary.Text = salary.Salary1.ToString();
                cbbEmployee.Text = salary.EmployeeName;

                dtpkFrom.Text = salary.From.ToString();
                dtpkTo.Text = salary.To.ToString();
                dtpkPayDate.Text = salary.Paydate.ToString();

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbbEmployee.Text) || string.IsNullOrEmpty(txtSalary.Text) || string.IsNullOrEmpty(cbbEmployee.Text))
            {
                MessageBox.Show("Miss data","Notification",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            else
            {
                //Salary sal = new Salary();


                Salary sal = new Salary();
                Employee epl = db.Employees.SingleOrDefault(emp => emp.EmpID == int.Parse(cbbEmployee.SelectedValue.ToString()));
                sal.EmployeeID = int.Parse(cbbEmployee.SelectedValue.ToString());
                
                sal.EmployeeName = cbbEmployee.Text;
                sal.Period = (dtpkTo.Value.Day - dtpkFrom.Value.Day) + Math.Abs(dtpkTo.Value.Month - dtpkFrom.Value.Month) * 30 + Math.Abs(dtpkTo.Value.Year - dtpkFrom.Value.Year) * 12 * 30;
                sal.Paydate = dtpkPayDate.Value;
                sal.From = dtpkFrom.Value;
                sal.To = dtpkTo.Value;
                sal.Salary1 = epl.EmpSal;
                sal.totalsal = epl.EmpSal * sal.Period;
                db.Salaries.InsertOnSubmit(sal);
                db.SubmitChanges();
                MessageBox.Show("Inserted Sucessfully", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
                
            } 
                
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Salary tmp = db.Salaries.SingleOrDefault(m => m.Scode == salary.Scode);
            tmp.EmployeeID = int.Parse(cbbEmployee.SelectedValue.ToString());
            tmp.Salary1 = int.Parse(txtSalary.Text);
            tmp.EmployeeName = cbbEmployee.Text;
            tmp.From = dtpkFrom.Value;
            tmp.To = dtpkTo.Value;
            tmp.Paydate = dtpkPayDate.Value;
            tmp.Period= (dtpkTo.Value.Day - dtpkFrom.Value.Day) + Math.Abs(dtpkTo.Value.Month - dtpkFrom.Value.Month) * 30 + Math.Abs(dtpkTo.Value.Year - dtpkFrom.Value.Year) * 12 * 30;
            salary.totalsal=salary.Salary1 * salary.Period;
            db.SubmitChanges();
            MessageBox.Show("Updated Sucessfully", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
