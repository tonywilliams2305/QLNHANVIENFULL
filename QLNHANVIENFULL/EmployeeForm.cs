using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNHANVIENFULL
{
    public partial class EmployeeForm : Form
    {

        EmployeeDataContext db = new EmployeeDataContext();
        public EmployeeForm()
        {
            InitializeComponent();
        }

       
        public void LoaddgvEmployee()
        {
            

            //dgvEmployee.Columns["imagecol"].DataPropertyName = "EmpImage";
            dgvEmployee.RowTemplate.Height = 50;
            string t = Application.StartupPath + @"\AddressImage\";
            dgvEmployee.DataSource = db.Employees.Select(p=> new {p.EmpID,p.EmpGen,p.Department.DepName,p.EmpName,p.EmpDOB,p.EmpJDate,p.EmpSal, EmpImage= Image.FromFile(t + p.EmpImage.ToString())  });
            
            var tmp = db.Employees.Select(p=> t + p.EmpImage.ToString());
            lbltotal.Text = dgvEmployee.RowCount.ToString();
        }
        
        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            LoaddgvEmployee();
            
        }

        private void dgvEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int posrow = e.RowIndex;
            int poscol = e.ColumnIndex;
            if(posrow>=0 && poscol>=0) // Delete
            {
                if (dgvEmployee.Columns[poscol].Name == "Delete")
                {
                    if (MessageBox.Show("Are you sure delete this Employee ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Employee epl = db.Employees.SingleOrDefault(m => m.EmpID == int.Parse(dgvEmployee["ID", posrow].Value.ToString()));
                        Salary sal = db.Salaries.SingleOrDefault(m => m.EmployeeID == int.Parse(dgvEmployee["ID", posrow].Value.ToString()));
                        db.Salaries.DeleteOnSubmit(sal);
                        db.Employees.DeleteOnSubmit(epl);
                        db.SubmitChanges();
                    }
                    this.Refresh();
                }
                else if(dgvEmployee.Columns[poscol].Name == "Edit")//Update
                {
                    Employee epl = db.Employees.SingleOrDefault(m => m.EmpID == int.Parse(dgvEmployee["ID", posrow].Value.ToString()));
                    UpdateEmployeeForm f = new UpdateEmployeeForm(epl);
                    f.btnAdd.Enabled = false;
                    f.btnUpdate.Enabled = true;
                    f.ShowDialog();
                    LoaddgvEmployee();
                }
                else if (dgvEmployee.Columns[poscol].Name == "imagecol") //UpLoad Image
                {
                    string addressimage = "";
                    string namefile = "";
                    OpenFileDialog dal = new OpenFileDialog();
                    dal.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp;)|*.jpg; *.jpeg; *.gif; *.bmp;";
                   
                   
                    if (dal.ShowDialog() == DialogResult.OK)
                    {
                        addressimage = dal.FileName;
                        namefile = Path.GetFileName(dal.FileName);

                        if(File.Exists(Application.StartupPath + "\\AddressImage\\" + namefile))
                        {
                            File.Delete(Application.StartupPath + "\\AddressImage\\" + namefile);
                        }    

                        File.Copy(dal.FileName, Application.StartupPath + "\\AddressImage\\" + namefile);
                        Employee epl = db.Employees.SingleOrDefault(m => m.EmpID == int.Parse(dgvEmployee["ID", posrow].Value.ToString()));
                        epl.EmpImage =  namefile;
                        db.SubmitChanges();
                    }
                    LoaddgvEmployee();
                }


            }    
        }

        private void ptbAdd_Click(object sender, EventArgs e)
        {
            UpdateEmployeeForm f = new UpdateEmployeeForm();
            f.btnAdd.Enabled = true;
            f.btnUpdate.Enabled = false;
            f.ShowDialog();
            LoaddgvEmployee();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if(txtSearch.Text=="")
            {
                LoaddgvEmployee();
            }
            else
            {
                dgvEmployee.DataSource = db.Employees.Where(m=>m.EmpName.Contains(txtSearch.Text.Trim())).Select(p => new { p.EmpID, p.EmpGen, p.Department.DepName, p.EmpName, p.EmpDOB, p.EmpJDate, p.EmpSal });
                lbltotal.Text = dgvEmployee.RowCount.ToString();
            }
        }
    }
}
