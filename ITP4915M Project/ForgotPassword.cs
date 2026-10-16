using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITP4915M_Project
{
    public partial class ForgotPassword : Form
    {
        public ForgotPassword()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
           
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Please enter EmployeeID");
                return;
            }

            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Please enter New Password");
                return;
            }

            try
            {
                var db = new projectEntities3();   // ⚠️ 用你真正的 Entities 名

                // ✅ 找 user
                var user = db.employee
                             .Where(x => x.EmployeeID.Trim() == textBox1.Text)
                             .FirstOrDefault();

                if (user != null)
                {
                    // ✅ 更新 password
                    user.Password = textBox2.Text;

                    db.SaveChanges();   // ✅ save 到 database

                    MessageBox.Show("Password updated successfully!");

                    // ✅ 回 Login
                    Application.OpenForms["Login"].Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("EmployeeID not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();   // 清 EmployeeID
            textBox2.Clear();   // 清 Password
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Close();
        }

        private void ForgotPassword_Load(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
