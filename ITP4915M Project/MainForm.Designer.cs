namespace ITP4915M_Project
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlLeftNavigation;
        private System.Windows.Forms.Label lblStaffName;
        private System.Windows.Forms.Label lblDept;

        // 導航按鈕群組
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnSalesMenu;
        private System.Windows.Forms.Button btnInventoryMenu;
        private System.Windows.Forms.Button btnCustomerServiceMenu;
        private System.Windows.Forms.Button btnLogout;

        private System.Windows.Forms.Panel pnlContentContainer;
        private System.Windows.Forms.Label lblContentTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlLeftNavigation = new System.Windows.Forms.Panel();
            this.lblDept = new System.Windows.Forms.Label();
            this.lblStaffName = new System.Windows.Forms.Label();
            this.btnDashboard = new System.Windows.Forms.Button(); // 確認按鈕物件已實例化
            this.btnSalesMenu = new System.Windows.Forms.Button();
            this.btnInventoryMenu = new System.Windows.Forms.Button();
            this.btnCustomerServiceMenu = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlContentContainer = new System.Windows.Forms.Panel();
            this.lblContentTitle = new System.Windows.Forms.Label();

            this.pnlLeftNavigation.SuspendLayout();
            this.SuspendLayout();

            // ... (前方位置設定程式碼保持不變)

            // 導航按鈕樣式與位置
            int startY = 100;

            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.Location = new System.Drawing.Point(5, startY);
            this.btnDashboard.Size = new System.Drawing.Size(150, 45);
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click); // 確保已綁定事件

            this.btnSalesMenu.Text = "Sales";
            this.btnSalesMenu.Location = new System.Drawing.Point(5, startY + 60);
            this.btnSalesMenu.Size = new System.Drawing.Size(150, 45);
            this.btnSalesMenu.Click += new System.EventHandler(this.btnSalesMenu_Click);

            this.btnInventoryMenu.Text = "Inventory";
            this.btnInventoryMenu.Location = new System.Drawing.Point(5, startY + 120);
            this.btnInventoryMenu.Size = new System.Drawing.Size(150, 45);
            this.btnInventoryMenu.Click += new System.EventHandler(this.btnInventoryMenu_Click);

            this.btnCustomerServiceMenu.Text = "Service";
            this.btnCustomerServiceMenu.Location = new System.Drawing.Point(5, startY + 180);
            this.btnCustomerServiceMenu.Size = new System.Drawing.Size(150, 45);
            this.btnCustomerServiceMenu.Click += new System.EventHandler(this.btnCustomerServiceMenu_Click);

            this.btnLogout.Text = "Logout";
            this.btnLogout.Location = new System.Drawing.Point(5, 535);
            this.btnLogout.Size = new System.Drawing.Size(150, 40);
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // 重要：確保 btnDashboard 被加入到 Controls 清單中
            this.pnlLeftNavigation.Controls.Add(this.lblDept);
            this.pnlLeftNavigation.Controls.Add(this.lblStaffName);
            this.pnlLeftNavigation.Controls.Add(this.btnDashboard);
            this.pnlLeftNavigation.Controls.Add(this.btnSalesMenu);
            this.pnlLeftNavigation.Controls.Add(this.btnInventoryMenu);
            this.pnlLeftNavigation.Controls.Add(this.btnCustomerServiceMenu);
            this.pnlLeftNavigation.Controls.Add(this.btnLogout);

            // ... (後方程式碼保持不變)
        }
    }
}