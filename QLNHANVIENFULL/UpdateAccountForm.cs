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
    public partial class UpdateAccountForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        Account account = null;
        EmployeeDataContext db = new EmployeeDataContext();
        public UpdateAccountForm(Account acc = null)
        {
            account = acc;
            InitializeComponent();
        }

        private void chkShowpass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowpass.Checked)
            {
                txtpassword.UseSystemPasswordChar = false;
                txtconfirm.UseSystemPasswordChar = false;
            }
            else
            {
                txtpassword.UseSystemPasswordChar = true;
                txtconfirm.UseSystemPasswordChar = true;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtusername.Text) || string.IsNullOrEmpty(txtpassword.Text) || string.IsNullOrEmpty(txtconfirm.Text) || string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Miss Data", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                if (txtconfirm.Text != txtpassword.Text)
                {
                    MessageBox.Show("Password not correct", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    Account acc = new Account();
                    acc.username = txtusername.Text.Trim();
                    acc.password = txtpassword.Text.Trim();
                    acc.Email = txtEmail.Text.Trim();
                    db.Accounts.InsertOnSubmit(acc);
                    db.SubmitChanges();
                    MessageBox.Show("Inserted Sucessfully", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }

            }

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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            
            if (account != null)
            {
                if (string.IsNullOrEmpty(txtusername.Text) || string.IsNullOrEmpty(txtpassword.Text) || string.IsNullOrEmpty(txtconfirm.Text) || string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Miss Data", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!txtEmail.Text.Contains("@gmail.com"))
                {
                    MessageBox.Show("Email havto format @gmail.com!", "Nofication",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }


                if (txtconfirm.Text != txtpassword.Text)
                {
                    MessageBox.Show("Password not correct", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    Account acc = db.Accounts.SingleOrDefault(m => m.ID == account.ID);
                    acc.username = txtusername.Text.Trim();
                    acc.password = txtpassword.Text.Trim();
                    acc.Email = txtEmail.Text.Trim();
                    db.SubmitChanges();
                    MessageBox.Show("Updated Sucessfully", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }


            }
        }

        private void UpdateAccountForm_Load(object sender, EventArgs e)
        {
            if (account != null)
            {
                txtusername.Text = account.username;
                txtpassword.Text = account.password;
                txtconfirm.Text = account.password;
                txtEmail.Text = account.Email;
            }
        }
    }
}
