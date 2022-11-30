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
    public partial class AccountForm : Form
    {
        EmployeeDataContext db = new EmployeeDataContext();
        public AccountForm()
        {
            InitializeComponent();
        }

        public void LoaddgvAccount()
        {
            dgvAccount.DataSource = db.Accounts.Select(a=> new {a.ID,a.username,a.password,a.Email});
            lbltotal.Text = dgvAccount.RowCount.ToString();
        }
        private void AccountForm_Load(object sender, EventArgs e)
        {
            LoaddgvAccount();
        }

        private void ptbAdd_Click(object sender, EventArgs e)
        {
            UpdateAccountForm f = new UpdateAccountForm();
            f.ShowDialog();
            LoaddgvAccount();
        }

        private void dgvAccount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int posrow = e.RowIndex;
            int poscol = e.ColumnIndex;
            if (posrow >= 0 && poscol >= 0)
            {
                if (poscol == 1)// Delete
                {
                    if (MessageBox.Show("Are you sure delete this Account ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        Account acc = db.Accounts.SingleOrDefault(m => m.ID == int.Parse(dgvAccount[2, posrow].Value.ToString()));
                        db.Accounts.DeleteOnSubmit(acc);

                        db.SubmitChanges();
                        LoaddgvAccount();
                    }

                }
                else if (poscol == 0) //Update
                {
                    Account acc = db.Accounts.SingleOrDefault(m => m.ID == int.Parse(dgvAccount[2, posrow].Value.ToString()));
                    UpdateAccountForm f = new UpdateAccountForm(acc);
                    f.btnAdd.Enabled = false;
                    f.btnUpdate.Enabled = true;
                    f.ShowDialog();
                    LoaddgvAccount();
                }

            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if(txtSearch.Text=="")
            {
                LoaddgvAccount();
            }
            else
            {
                dgvAccount.DataSource = db.Accounts.Where(m => m.username.Contains(txtSearch.Text.Trim())).Select(p => new { p.ID,p.username,p.password});
                lbltotal.Text = dgvAccount.RowCount.ToString();
            } 
                
        }

        /* private void btnAdd_Click(object sender, EventArgs e)
         {
             string user = txtuser.Text.Trim();
             string password = txtpassword.Text.Trim();
             if(user=="" || password=="")
             {
                 MessageBox.Show("Miss data");
                 return;
             }   
             if(password.Length<6)
             {
                 MessageBox.Show("Password Lenght have to greater 6 character");
                 return;
             }    
             Account acc = new Account();
             acc.username = user;
             acc.password = password;
             db.Accounts.InsertOnSubmit(acc);
             db.SubmitChanges();
             MessageBox.Show("Insert Successfully");
             LoaddgvAccount();
         }

         private void dgvAccount_CellClick(object sender, DataGridViewCellEventArgs e)
         {
             int vitri = e.RowIndex;
             if(vitri>=0 && dgvAccount[0,vitri].Value!=null)
             {
                 txtuser.Text = dgvAccount[0, vitri].Value.ToString();
                 txtpassword.Text = dgvAccount[1, vitri].Value.ToString();
             }    
         }

         private void btnUpdate_Click(object sender, EventArgs e)
         {
             string user = txtuser.Text.Trim();
             string password = txtpassword.Text.Trim();
             int vitri = dgvAccount.CurrentRow.Index;
             int IDacc = int.Parse(dgvAccount[2, vitri].Value.ToString());
             if (user == "" || password == "")
             {
                 MessageBox.Show("Miss data");
                 return;
             }
             if (password.Length < 6)
             {
                 MessageBox.Show("Password Lenght have to greater 6 character");
                 return;
             }
             Account acc = db.Accounts.SingleOrDefault(p => p.ID == IDacc);
             if (acc != null)
             {
                 acc.username = user;
                 acc.password = password;

                 db.SubmitChanges();
                 MessageBox.Show("Update Successfully");
                 LoaddgvAccount();
             }
         }

         private void btnDelete_Click(object sender, EventArgs e)
         {
             string user = txtuser.Text.Trim();
             string password = txtpassword.Text.Trim();
             int IDacc = int.Parse(dgvAccount[2, dgvAccount.CurrentRow.Index].Value.ToString());
             Account acc = db.Accounts.SingleOrDefault(p => p.ID == IDacc);
             if (acc != null)
             {

                 db.Accounts.DeleteOnSubmit(acc);
                 db.SubmitChanges();
                 MessageBox.Show("Delete Successfully");
                 LoaddgvAccount();
             }
         }*/
    }
}
