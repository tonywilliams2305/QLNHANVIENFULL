using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

namespace QLNHANVIENFULL
{
    public partial class LoginForm : Form
    {
        EmployeeDataContext db = new EmployeeDataContext();
        public LoginForm()
        {
            InitializeComponent();
        }

        private void chkshow_CheckedChanged(object sender, EventArgs e)
        {
            if(chkshow.Checked)
            {
                txtpassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtpassword.UseSystemPasswordChar = true;
            } 
                
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            txtpassword.ResetText();
            txtusername.ResetText();
        }

        private void Login_Click(object sender, EventArgs e)
        {

            string user = txtusername.Text.Trim();
            string password = txtpassword.Text.Trim();
            if(user=="" || password=="")
            {
                MessageBox.Show("Miss data");
                return;
            }    
            Account account = db.Accounts.SingleOrDefault(p => p.username == user && p.password == password);
            if(account == null)
            {
                lblCheck.Visible = true;
                lblCheck.Text = "Account not correct !";
            }
            else
            {
                txtpassword.ResetText();
                txtusername.ResetText();
                Form1 frm = new Form1();
                frm.ShowDialog();
            } 
                

        }

        private void txtusername_KeyPress(object sender, KeyPressEventArgs e)
        {
            lblCheck.Visible = false;
        }

        private void btnForget_Click(object sender, EventArgs e)
        {
            ForgetPasswordForm f = new ForgetPasswordForm();
            f.ShowDialog();
        }
    }
}
