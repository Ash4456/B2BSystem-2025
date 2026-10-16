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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.AcceptButton = button1;
        }

        private void label1_Click(object sender, EventArgs e)
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
                MessageBox.Show("Please enter Password");
                return;
            }

            try
            {
                var db = new projectEntities3();

                var user = db.employee
                             .Where(x => x.EmployeeID.Trim() == textBox1.Text
                                      && x.Password == textBox2.Text)
                             .FirstOrDefault();

                if (user != null)
                {
              
                    switch (user.Department.Trim())
                    {

                        case "SA":
                            new Sales(user.EmployeeName).Show();   
                            break;


                        case "PR":
                            new Production(user.EmployeeName).Show();
                            break;

                        case "IV":
                            new Inventory(user.EmployeeName).Show();
                            break;

                        case "LO":
                            new Logistics(user.EmployeeName).Show();
                            break;
                        case "CS":
                            new Customer_Service(user.EmployeeName).Show();
                            break;

                        case "FI":
                            new Finance(user.EmployeeName).Show();
                            break;
                        case "MA":
                            new Management(user.EmployeeName).Show();
                            break;
                    }

                    this.Hide();  
                }
                else
                {
                    MessageBox.Show("Invalid ID or Password");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            textBox2.UseSystemPasswordChar = true;
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPassword f = new ForgotPassword();  // 新頁面
            f.Show();  // 顯示新頁面
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();   // 清 EmployeeID
            textBox2.Clear();   // 清 Password
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
