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
    public partial class SalaryForm : Form
    {
        EmployeeDataContext db = new EmployeeDataContext();
        public SalaryForm()
        {
            InitializeComponent();
        }

        
        public void LoadSalary()
        {
            dgvSalary.DataSource = db.Salaries.Select(p => new {p.Scode,p.Employee.EmpName,p.Period,p.From,p.To,p.Paydate,p.totalsal,p.Salary1});
            lbltotal.Text = db.Salaries.Sum(m => m.totalsal).ToString()+"$";
        }
        private void SalaryForm_Load(object sender, EventArgs e)
        {
            
            LoadSalary();
        }

        private void dgvSalary_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int posrow = e.RowIndex;
            int poscol = e.ColumnIndex;
            if (posrow >= 0 && poscol >= 0) 
            {
                if (poscol == 1)// Delete
                {
                    if (MessageBox.Show("Are you sure delete this Salary ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        
                        Salary sal = db.Salaries.SingleOrDefault(m => m.Scode == int.Parse(dgvSalary[2, posrow].Value.ToString()));
                        db.Salaries.DeleteOnSubmit(sal);
                        
                        db.SubmitChanges();
                        LoadSalary();
                    }
                    
                }
                else if (poscol == 0) //Update
                {
                    Salary epl = db.Salaries.SingleOrDefault(m => m.Scode == int.Parse(dgvSalary[2, posrow].Value.ToString()));
                    UpdateSalaryForm f = new UpdateSalaryForm(epl);
                    f.btnAdd.Enabled = false;
                    f.btnUpdate.Enabled = true;
                    f.ShowDialog();
                    LoadSalary();
                }

            }
        }

        private void ptbAdd_Click(object sender, EventArgs e)
        {
            UpdateSalaryForm f = new UpdateSalaryForm();
            f.btnAdd.Enabled = true;
            f.btnUpdate.Enabled = false;
            f.ShowDialog();
            LoadSalary();

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if(txtSearch.Text=="")
            {
                LoadSalary();
            }    
            else
            {
                dgvSalary.DataSource = db.Salaries.Where(m => m.EmployeeName.Contains(txtSearch.Text.Trim())).Select(p => new { p.Scode, p.Employee.EmpName, p.Period, p.From, p.To, p.Paydate, p.totalsal, p.Salary1 });
                lbltotal.Text = db.Salaries.Sum(m => m.totalsal).ToString()+"$";
            }    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReportForm reportForm = new ReportForm();
            reportForm.ShowDialog();
        }





        /*private void cbbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            string namee = cbbEmployee.Text;
            Employee emp = db.Employees.SingleOrDefault(p => p.EmpName == namee);
            if (emp != null)
            {
                lblSal.Text = emp.EmpSal.ToString() + "  $";
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Salary sal = new Salary();
            string namee = cbbEmployee.Text;
            Employee emp = db.Employees.SingleOrDefault(p => p.EmpName == namee);
            sal.EmployeeID = emp.EmpID;
            sal.EmployeeName = emp.EmpName;
            sal.Period = (dtpkTo.Value.Day - dtpkFrom.Value.Day) + Math.Abs(dtpkTo.Value.Month - dtpkFrom.Value.Month) * 30 + Math.Abs(dtpkTo.Value.Year - dtpkFrom.Value.Year) * 12 * 30;
            sal.Paydate = dtpkPaydate.Value;
            sal.From = dtpkFrom.Value;
            sal.To = dtpkTo.Value;
            sal.Sal = emp.EmpSal;
            sal.totalsal = emp.EmpSal * sal.Period;
            db.Salaries.InsertOnSubmit(sal);
            db.SubmitChanges();
            LoadSalary();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int vitri = dgvSalary.CurrentRow.Index;
            if (vitri >= 0 && dgvSalary[0, vitri].Value != null)
            {
                int IDSal = int.Parse(dgvSalary[0, vitri].Value.ToString());

                Salary sal = db.Salaries.SingleOrDefault(p => p.Scode == IDSal);
                if (sal != null)
                {
                   sal.From=dtpkFrom.Value;
                    sal.To = dtpkTo.Value;
                    sal.Paydate = dtpkPaydate.Value;
                    sal.EmployeeName = cbbEmployee.Text;
                    sal.Sal = int.Parse(dgvSalary[3,vitri].Value.ToString());
                    db.SubmitChanges();
                }
            }


        }

        private void dgvSalary_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int vitri = e.RowIndex;
            if (vitri >= 0 && dgvSalary[0, vitri].Value != null)
            {
                int IdEmp = int.Parse(dgvSalary[1, vitri].Value.ToString());
                Employee emp = db.Employees.SingleOrDefault(p => p.EmpID == IdEmp);
                if (emp != null)
                {
                    cbbEmployee.Text = emp.EmpName;
                    cbbEmployee.SelectedValue = emp.EmpID;
                    dtpkFrom.Text = dgvSalary[6, vitri].Value.ToString();
                    dtpkTo.Text = dgvSalary[7, vitri].Value.ToString();
                    dtpkPaydate.Text = dgvSalary[4, vitri].Value.ToString();
                    LoadSalary();
                }

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int vitri = dgvSalary.CurrentRow.Index;
            if (vitri >= 0 && dgvSalary[0, vitri].Value != null)
            {
                if (MessageBox.Show("Do you want to delete this Salary ?", "Infor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int IDSal = int.Parse(dgvSalary[0, vitri].Value.ToString());
                    Salary sal = db.Salaries.SingleOrDefault(p => p.Scode == IDSal);
                    db.Salaries.DeleteOnSubmit(sal);
                    db.SubmitChanges();

                }
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            ReportForm reportForm = new ReportForm();
            reportForm.ShowDialog();
        }*/
    }
}
