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
using System.Net.Mime;
namespace QLNHANVIENFULL
{
    public partial class ForgetPasswordForm : Form
    {

        Random rd = new Random();
        int otp;
        EmployeeDataContext db = new EmployeeDataContext();
        public ForgetPasswordForm()
        {
            InitializeComponent();
        }

        void hide()
        {
            lblNewPassword.Visible = false;
            lblOTP.Visible = false;
            txtNewpassword.Visible = false;
            txtOTP.Visible = false;
            chkShowPass.Visible = false;
        }
        void show()
        {
            lblNewPassword.Visible = true;
            lblOTP.Visible = true;
            txtNewpassword.Visible = true;
            txtOTP.Visible = true;
            chkShowPass.Visible = true;
        }

        private void ForgetPasswordForm_Load(object sender, EventArgs e)
        {

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if(otp.ToString()==txtOTP.Text.Trim())
            {
                if(txtNewpassword.Text.Length>=6)
                {
                    Account acc = db.Accounts.SingleOrDefault(m=>m.Email==txtEmail.Text.Trim());
                    acc.password = txtNewpassword.Text;
                    db.SubmitChanges();
                    MessageBox.Show("Udated New Password !");
                }
                else
                {
                    MessageBox.Show("Password have to greater than or equals 6", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                } 
                    

            }
            else
            {
                MessageBox.Show("Not correct OTP!");
                return;
            } 
            
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {

            
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Miss Data", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                Account acc = db.Accounts.SingleOrDefault(a => a.Email == txtEmail.Text.Trim());
                if(acc == null)
                {
                    MessageBox.Show("Gmail not exists !", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }    
                try
                {
                    otp = rd.Next(10000, 99999);

                    var from = new MailAddress("trandoanhkhoa192837@gmail.com");
                    var to = new MailAddress(txtEmail.Text.Trim());

                    const string frompass = "dzxdjqkuolekvxqz"; //mở xác thực bằng 2 lớp rồi nhận mã 
                    const string subject = "OTP CODE";
                    string body = otp.ToString();

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(from.Address, frompass),
                        Timeout = 200000
                    };
                    using (var message = new MailMessage(from, to)
                    {
                        Subject = subject,
                        Body = body,

                    })
                    {
                        smtp.Send(message);
                    }
                    MessageBox.Show("OTP sent sucessfully !");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    hide();
                    return;
                }
                show();
            }
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if(chkShowPass.Checked)
            {
                txtNewpassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtNewpassword.UseSystemPasswordChar = true;
            } 

            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
