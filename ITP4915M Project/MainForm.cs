using System;
using System.Drawing;
using System.Windows.Forms;

namespace ITP4915M_Project
{
    public partial class MainForm : Form
    {
        private string staffName;
        private string department;

        public MainForm(string name, string dept)
        {
            InitializeComponent();
            this.staffName = name;
            this.department = dept;
            lblStaffName.Text = name;
            lblDept.Text = "Dept: " + dept;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupNavigationSecurity();
            ShowWelcomeMessage();
        }

        private void SetupNavigationSecurity()
        {
            btnSalesMenu.Enabled = (department == "SA" || department == "Manager");
            btnInventoryMenu.Enabled = (department == "IV" || department == "Manager");
            btnCustomerServiceMenu.Enabled = (department == "CS" || department == "SA" || department == "Manager");

            if (department == "Manager" || department == "SA")
            {
                lblDept.ForeColor = Color.Gold;
            }
        }

        private void ShowWelcomeMessage()
        {
            lblContentTitle.Text = "Welcome to Smart Production & Logistics System";
            pnlContentContainer.Controls.Clear();

            Label lblWelcome = new Label()
            {
                Text = $"Hello, {staffName}!\nPlease select a function from the left navigation bar.",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(40, 50)
            };
            pnlContentContainer.Controls.Add(lblWelcome);
            pnlLeftNavigation.BringToFront();
        }

        private void LoadChildForm(Form childForm)
        {
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // 強化版防遮擋邏輯：遞迴清理所有層級的圖片與背景
            CleanFormDecorations(childForm);

            pnlContentContainer.Controls.Clear();
            pnlContentContainer.Controls.Add(childForm);
            childForm.Show();

            pnlLeftNavigation.BringToFront();
        }

        // 遞迴移除子表單中會造成遮擋的裝飾物
        private void CleanFormDecorations(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is PictureBox pct)
                {
                    // 若發現是舊版設計的背景圖，強制隱藏
                    if (pct.Name.Contains("pictureBox"))
                    {
                        pct.Visible = false;
                    }
                }

                // 若發現有與容器大小重疊的黑色 Panel (常是舊版選單背景)，將其透明化或隱藏
                if (ctrl is Panel && ctrl.BackColor == Color.Black && ctrl.Size.Width > 500)
                {
                    ctrl.Visible = false;
                }

                // 修正佈局：確保內容控制項向左對齊 (扣除導覽列寬度)
                if (ctrl.Name.Contains("panel") || ctrl.Name.Contains("dgv") || ctrl.Name.Contains("dataGridView"))
                {
                    ctrl.Left = 0;
                }

                if (ctrl.HasChildren)
                {
                    CleanFormDecorations(ctrl);
                }
            }
        }

        private void btnSalesMenu_Click(object sender, EventArgs e)
        {
            lblContentTitle.Text = "Sales Order Management";
            Sales salesForm = new Sales(staffName);
            LoadChildForm(salesForm);
        }

        private void btnInventoryMenu_Click(object sender, EventArgs e)
        {
            lblContentTitle.Text = "Inventory & Warehouse Control";
            Inventory inventoryForm = new Inventory(staffName);
            LoadChildForm(inventoryForm);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            // 如果您需要功能，請在此處撰寫；若不需要，留空即可
            MessageBox.Show("Dashboard is under construction!");
        }

        private void btnCustomerServiceMenu_Click(object sender, EventArgs e)
        {
            lblContentTitle.Text = "Customer Service Support";
            Customer_Service csForm = new Customer_Service(staffName);
            LoadChildForm(csForm);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}