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
    public partial class DepartmentForm : Form
    {
        EmployeeDataContext db = new EmployeeDataContext();
        public DepartmentForm()
        {
            InitializeComponent();
        }
        public void LoaddgvDepartment()
        {
            dgvDepartment.DataSource = db.Departments.Select(d => new { d.DepId, d.DepName });
            lbltotal.Text = dgvDepartment.RowCount.ToString();
        }
        private void DepartmentForm_Load(object sender, EventArgs e)
        {
            LoaddgvDepartment();

        }

        private void dgvDepartment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int posrow = e.RowIndex;
            int poscol = e.ColumnIndex;
            if (posrow >= 0 && poscol >= 0) // Delete
            {
                if (poscol == 1)
                {
                    if (MessageBox.Show("Are you sure delete this Department ? If you delete this department , employee use this department will be deleted .", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Department dep = db.Departments.SingleOrDefault(m => m.DepId == int.Parse(dgvDepartment[2, posrow].Value.ToString()));
                        db.Salaries.DeleteAllOnSubmit(db.Salaries.Where(m => m.EmployeeID == m.Employee.EmpID && m.Employee.EmpDep == dep.DepId));
                        db.Employees.DeleteAllOnSubmit(db.Employees.Where(m => m.EmpDep == dep.DepId));
                        
                        db.Departments.DeleteOnSubmit(dep);
                        db.SubmitChanges();
                        LoaddgvDepartment();
                    }

                }
                else if (poscol == 0) //Update
                {
                    Department dep = db.Departments.SingleOrDefault(m => m.DepId == int.Parse(dgvDepartment[2, posrow].Value.ToString()));
                    UpdateDepartmentForm f = new UpdateDepartmentForm(dep);
                    f.btnAdd.Enabled = false;
                    f.btnUpdate.Enabled = true;
                    f.ShowDialog();
                    LoaddgvDepartment();
                }

            }
        }

        private void ptbAdd_Click(object sender, EventArgs e)
        {
            UpdateDepartmentForm f = new UpdateDepartmentForm();
            f.btnAdd.Enabled = true;
            f.btnUpdate.Enabled = false;
            f.ShowDialog();
            LoaddgvDepartment();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                LoaddgvDepartment();
            }
            else
            {
                dgvDepartment.DataSource = db.Departments.Where(m => m.DepName.Contains(txtSearch.Text.Trim())).Select(p => new { p.DepId, p.DepName });
                lbltotal.Text = dgvDepartment.RowCount.ToString();
            }

        }

        /*private void dgvDepartment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int vitri = e.RowIndex;
            if(vitri>=0&& dgvDepartment[1, vitri].Value!=null)
            {
                txtDepartmentName.Text = dgvDepartment[1, vitri].Value.ToString();
            }    
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtDepartmentName.Text.Trim();
            if (name != "")
            {
                Department dep = new Department();
                dep.DepName = name;
                db.Departments.InsertOnSubmit(dep);
                db.SubmitChanges();
                dgvDepartment.DataSource = db.Departments;
                dgvDepartment.Refresh();
                txtDepartmentName.ResetText();
                MessageBox.Show("Insert Successfully");
            }
            else
            {
                MessageBox.Show("Miss Data");
            } 
                
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string name = txtDepartmentName.Text.Trim();
            int vitri = dgvDepartment.CurrentRow.Index;
            if(name !="" &&vitri>=0 && dgvDepartment[0,vitri].Value!=null )
            {
                int IDpart = int.Parse(dgvDepartment[0, vitri].Value.ToString());
                Department dep = db.Departments.SingleOrDefault(p => p.DepId == IDpart);
                if(dep!=null)
                {
                    dep.DepName=name;
                    db.SubmitChanges();
                    txtDepartmentName.ResetText();
                    MessageBox.Show("Update Successfully");
                    LoaddgvDepartment();

                }    
            }    
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int vitri = dgvDepartment.CurrentRow.Index;
            if ( vitri >= 0 && dgvDepartment[0, vitri].Value != null)
            {
                int IDpart = int.Parse(dgvDepartment[0, vitri].Value.ToString());
                Department dep = db.Departments.SingleOrDefault(p => p.DepId == IDpart);
                if (dep != null)
                {
                    db.Departments.DeleteOnSubmit(dep);
                    db.SubmitChanges();
                    txtDepartmentName.ResetText();
                    MessageBox.Show("Delete Successfully");
                    LoaddgvDepartment();

                }
            }
        }*/
    }
}
