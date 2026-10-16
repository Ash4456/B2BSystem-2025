using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.IO;

namespace ITP4915M_Project
{
    public partial class Production : Form
    {
        private projectEntities3 db = new projectEntities3();
        private string staffName;
        private string employeeID;
        private string employeePhone;

      
        private class CartItem
        {
            public rawmaterials Material { get; set; }
            public int Quantity { get; set; }
        }

        private List<CartItem> cartItems = new List<CartItem>();
        private Panel cartItemsContainer;
        private Label cartTotalLabel;
        private bool isInitialized = false;

       
        private DataGridView dgvPRPending;
        private Panel panelPRPending;

        public class PRPendingRequestDto
        {
            public int RequestID { get; set; }
            public string OrderID { get; set; }
            public string RequestType { get; set; }
            public string Reason { get; set; }
            public string Status { get; set; }
            public DateTime RequestDate { get; set; }
            public string HandledBy { get; set; }
            public string ForwardTo { get; set; }
            public string PhotoPath { get; set; }  
        }

       
        private Button currentSelectedButton = null;

        public Production(string name)
        {
            InitializeComponent();
            this.staffName = name;
            this.label2.Text = name;

            
            LoadEmployeeData(name);

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            
            InitializePRPendingPanel();

           
            button7.Click -= Button7_Click;
            button7.Click += Button7_Click;

            
            this.dataGridView1.CellClick -= dataGridView1_CellClick;
            this.dataGridView1.CellClick += dataGridView1_CellClick;

            
            button1.Click -= button1_Click;
            button1.Click += button1_Click;

            button2.Click -= button2_Click;
            button2.Click += button2_Click;

            button3.Click -= button3_Click;
            button3.Click += button3_Click;

            button4.Click -= button4_Click;
            button4.Click += button4_Click;

            button5.Click -= button5_Click;
            button5.Click += button5_Click;

            button6.Click -= button6_Click;
            button6.Click += button6_Click;

            button8.Click -= button8_Click;
            button8.Click += button8_Click;

            textBoxProductQty.KeyPress -= textBoxProductQty_KeyPress;
            textBoxProductQty.KeyPress += textBoxProductQty_KeyPress;
        }

       
        private void SetActiveButton(Button selectedButton)
        {
           
            ResetAllButtonColors();

           
            if (selectedButton != null)
            {
                selectedButton.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
                selectedButton.ForeColor = System.Drawing.Color.White;
                currentSelectedButton = selectedButton;
            }
        }

        private void ResetAllButtonColors()
        {
            System.Drawing.Color defaultColor = System.Drawing.Color.FromArgb(52, 73, 94);
            System.Drawing.Color defaultForeColor = System.Drawing.Color.White;

            button1.BackColor = defaultColor;
            button1.ForeColor = defaultForeColor;

            button3.BackColor = defaultColor;
            button3.ForeColor = defaultForeColor;

            button6.BackColor = defaultColor;
            button6.ForeColor = defaultForeColor;

            button7.BackColor = defaultColor;
            button7.ForeColor = defaultForeColor;
        }

        
        private void InitializePRPendingPanel()
        {
            
            panelPRPending = new Panel();
            panelPRPending.BackColor = System.Drawing.Color.White;
            panelPRPending.BorderStyle = BorderStyle.FixedSingle;
            panelPRPending.Location = new System.Drawing.Point(131, 69);
            panelPRPending.Name = "panelPRPending";
            panelPRPending.Size = new System.Drawing.Size(681, 347);
            panelPRPending.TabIndex = 31;
            panelPRPending.Visible = false;

            
            dgvPRPending = new DataGridView();
            dgvPRPending.Name = "dgvPRPending";
            dgvPRPending.Location = new System.Drawing.Point(10, 10);
            dgvPRPending.Size = new System.Drawing.Size(659, 325);
            dgvPRPending.TabIndex = 0;
            dgvPRPending.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            dgvPRPending.AllowUserToAddRows = false;
            dgvPRPending.AllowUserToDeleteRows = false;
            dgvPRPending.BackgroundColor = System.Drawing.Color.White;
            dgvPRPending.BorderStyle = BorderStyle.None;
            dgvPRPending.ReadOnly = true;
            dgvPRPending.RowHeadersVisible = false;
            dgvPRPending.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPRPending.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPRPending.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            
            dgvPRPending.CellClick -= DgvPRPending_CellClick;
            dgvPRPending.CellClick += DgvPRPending_CellClick;

            
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            headerStyle.ForeColor = System.Drawing.Color.White;
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            headerStyle.WrapMode = DataGridViewTriState.True;
            dgvPRPending.ColumnHeadersDefaultCellStyle = headerStyle;

          
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.BackColor = System.Drawing.SystemColors.Window;
            cellStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            cellStyle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            cellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            cellStyle.SelectionForeColor = System.Drawing.Color.White;
            cellStyle.WrapMode = DataGridViewTriState.False;
            dgvPRPending.DefaultCellStyle = cellStyle;

          
            DataGridViewCellStyle altCellStyle = new DataGridViewCellStyle();
            altCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            altCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            altCellStyle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            altCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            altCellStyle.SelectionForeColor = System.Drawing.Color.White;
            altCellStyle.WrapMode = DataGridViewTriState.False;
            dgvPRPending.AlternatingRowsDefaultCellStyle = altCellStyle;

            dgvPRPending.EnableHeadersVisualStyles = false;
            dgvPRPending.GridColor = System.Drawing.Color.FromArgb(230, 230, 230);

            panelPRPending.Controls.Add(dgvPRPending);
            this.Controls.Add(panelPRPending);
        }

        
        private void SetupPRPendingDataGridView()
        {
            dgvPRPending.AutoGenerateColumns = false;
            dgvPRPending.AllowUserToAddRows = false;
            dgvPRPending.ReadOnly = true;
            dgvPRPending.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPRPending.MultiSelect = false;
            dgvPRPending.Columns.Clear();

            dgvPRPending.ScrollBars = ScrollBars.Both;
            dgvPRPending.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

           
            DataGridViewButtonColumn viewButton = new DataGridViewButtonColumn();
            viewButton.Name = "View";
            viewButton.HeaderText = "";
            viewButton.Text = "🔍 View";
            viewButton.UseColumnTextForButtonValue = true;
            viewButton.Width = 80;
            viewButton.FlatStyle = FlatStyle.Popup;
            viewButton.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            viewButton.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            viewButton.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            dgvPRPending.Columns.Add(viewButton);

           
            DataGridViewTextBoxColumn colRequestID = new DataGridViewTextBoxColumn();
            colRequestID.Name = "RequestID";
            colRequestID.HeaderText = "Request ID";
            colRequestID.DataPropertyName = "RequestID";
            colRequestID.Width = 100;
            dgvPRPending.Columns.Add(colRequestID);

           
            DataGridViewTextBoxColumn colOrderID = new DataGridViewTextBoxColumn();
            colOrderID.Name = "OrderID";
            colOrderID.HeaderText = "Order ID";
            colOrderID.DataPropertyName = "OrderID";
            colOrderID.Width = 100;
            dgvPRPending.Columns.Add(colOrderID);

            
            DataGridViewTextBoxColumn colRequestType = new DataGridViewTextBoxColumn();
            colRequestType.Name = "RequestType";
            colRequestType.HeaderText = "Request Type";
            colRequestType.DataPropertyName = "RequestType";
            colRequestType.Width = 120;
            dgvPRPending.Columns.Add(colRequestType);

           
            DataGridViewTextBoxColumn colReason = new DataGridViewTextBoxColumn();
            colReason.Name = "Reason";
            colReason.HeaderText = "Reason";
            colReason.DataPropertyName = "Reason";
            colReason.Width = 200;
            dgvPRPending.Columns.Add(colReason);

            
            DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
            colStatus.Name = "Status";
            colStatus.HeaderText = "Status";
            colStatus.DataPropertyName = "Status";
            colStatus.Width = 100;
            dgvPRPending.Columns.Add(colStatus);

           
            DataGridViewTextBoxColumn colRequestDate = new DataGridViewTextBoxColumn();
            colRequestDate.Name = "RequestDate";
            colRequestDate.HeaderText = "Request Date";
            colRequestDate.DataPropertyName = "RequestDate";
            colRequestDate.Width = 140;
            colRequestDate.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            dgvPRPending.Columns.Add(colRequestDate);

            
            DataGridViewTextBoxColumn colHandledBy = new DataGridViewTextBoxColumn();
            colHandledBy.Name = "HandledBy";
            colHandledBy.HeaderText = "Handled By";
            colHandledBy.DataPropertyName = "HandledBy";
            colHandledBy.Width = 100;
            dgvPRPending.Columns.Add(colHandledBy);

           
            DataGridViewTextBoxColumn colForwardTo = new DataGridViewTextBoxColumn();
            colForwardTo.Name = "ForwardTo";
            colForwardTo.HeaderText = "Forward To";
            colForwardTo.DataPropertyName = "ForwardTo";
            colForwardTo.Visible = false;
            dgvPRPending.Columns.Add(colForwardTo);

            
            DataGridViewTextBoxColumn colPhotoPath = new DataGridViewTextBoxColumn();
            colPhotoPath.Name = "PhotoPath";
            colPhotoPath.HeaderText = "PhotoPath";
            colPhotoPath.DataPropertyName = "PhotoPath";
            colPhotoPath.Visible = false;
            dgvPRPending.Columns.Add(colPhotoPath);

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            headerStyle.ForeColor = System.Drawing.Color.White;
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            headerStyle.WrapMode = DataGridViewTriState.True;
            dgvPRPending.ColumnHeadersDefaultCellStyle = headerStyle;

            dgvPRPending.EnableHeadersVisualStyles = false;
            dgvPRPending.GridColor = System.Drawing.Color.FromArgb(230, 230, 230);
        }

        
        private void LoadPRPendingRequests()
        {
            try
            {
                var db = new projectEntities3();

                var list = db.afterservicerequest
                    .Where(r => r.ForwardTo == "PR" && r.Status == "Pending")
                    .Select(r => new PRPendingRequestDto
                    {
                        RequestID = r.RequestID,
                        OrderID = r.OrderID,
                        RequestType = r.RequestType,
                        Reason = r.Reason,
                        Status = r.Status,
                        RequestDate = r.RequestDate ?? DateTime.Now,
                        HandledBy = r.HandledBy,
                        ForwardTo = r.ForwardTo,
                        PhotoPath = r.PhotoPath  
                    })
                    .OrderByDescending(r => r.RequestDate)
                    .ToList();

                dgvPRPending.Columns.Clear();
                dgvPRPending.DataSource = null;
                dgvPRPending.Refresh();

                SetupPRPendingDataGridView();

                if (list.Count == 0)
                {
                    dgvPRPending.DataSource = list;
                    dgvPRPending.Refresh();
                    MessageBox.Show("No pending requests for PR.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dgvPRPending.DataSource = list;
                dgvPRPending.Refresh();

                
                if (dgvPRPending.Columns.Contains("PhotoPath"))
                {
                    dgvPRPending.Columns["PhotoPath"].Visible = false;
                }

                dgvPRPending.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load PR pending requests.\n" + ex.Message);
            }
        }

        
        private void DgvPRPending_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (e.ColumnIndex != 0) 
                return;

            if (dgvPRPending.Rows.Count == 0 || e.RowIndex >= dgvPRPending.Rows.Count)
                return;

            if (dgvPRPending.Rows[e.RowIndex].IsNewRow)
                return;

            if (dgvPRPending.Rows[e.RowIndex].Cells["RequestID"].Value == null)
                return;

            try
            {
                DataGridViewRow row = dgvPRPending.Rows[e.RowIndex];
                int requestID = Convert.ToInt32(row.Cells["RequestID"].Value);
                string orderID = row.Cells["OrderID"].Value?.ToString() ?? "";
                string requestType = row.Cells["RequestType"].Value?.ToString() ?? "";
                string reason = row.Cells["Reason"].Value?.ToString() ?? "";
                string status = row.Cells["Status"].Value?.ToString() ?? "";
                string photoPath = row.Cells["PhotoPath"].Value?.ToString() ?? "";  

                ShowPRPendingDetailDialog(requestID, orderID, requestType, reason, status, photoPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error viewing request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void ShowPRPendingDetailDialog(int requestID, string orderID, string requestType, string reason, string status, string photoPath = null)
        {
            Form dialog = new Form();
            dialog.Text = "📋 PR Request Details";
            dialog.Size = new System.Drawing.Size(550, 520);
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.BackColor = System.Drawing.Color.White;
            
            dialog.AutoScroll = true;
            dialog.AutoScrollMinSize = new System.Drawing.Size(550, 600);

            Label lblTitle = new Label();
            lblTitle.Text = "PR Request Details";
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblTitle.Location = new System.Drawing.Point(20, 20);
            lblTitle.Size = new System.Drawing.Size(250, 32);
            dialog.Controls.Add(lblTitle);

            Panel line = new Panel();
            line.BackColor = System.Drawing.Color.FromArgb(220, 220, 220);
            line.Location = new System.Drawing.Point(20, 55);
            line.Size = new System.Drawing.Size(490, 2);
            dialog.Controls.Add(line);

            int yPos = 75;
            int labelWidth = 120;

            
            Label lblOrderIDTitle = new Label();
            lblOrderIDTitle.Text = "Order ID:";
            lblOrderIDTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblOrderIDTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblOrderIDTitle.Location = new System.Drawing.Point(20, yPos);
            lblOrderIDTitle.Size = new System.Drawing.Size(labelWidth, 25);
            dialog.Controls.Add(lblOrderIDTitle);

            Label lblOrderIDValue = new Label();
            lblOrderIDValue.Text = orderID;
            lblOrderIDValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblOrderIDValue.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblOrderIDValue.Location = new System.Drawing.Point(140, yPos);
            lblOrderIDValue.Size = new System.Drawing.Size(200, 25);
            dialog.Controls.Add(lblOrderIDValue);

            yPos += 35;

            
            Label lblTypeTitle = new Label();
            lblTypeTitle.Text = "Request Type:";
            lblTypeTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblTypeTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblTypeTitle.Location = new System.Drawing.Point(20, yPos);
            lblTypeTitle.Size = new System.Drawing.Size(labelWidth, 25);
            dialog.Controls.Add(lblTypeTitle);

            Label lblTypeValue = new Label();
            lblTypeValue.Text = requestType;
            lblTypeValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblTypeValue.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblTypeValue.Location = new System.Drawing.Point(140, yPos);
            lblTypeValue.Size = new System.Drawing.Size(200, 25);
            dialog.Controls.Add(lblTypeValue);

            yPos += 35;

           
            Label lblStatusTitle = new Label();
            lblStatusTitle.Text = "Status:";
            lblStatusTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblStatusTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblStatusTitle.Location = new System.Drawing.Point(20, yPos);
            lblStatusTitle.Size = new System.Drawing.Size(labelWidth, 25);
            dialog.Controls.Add(lblStatusTitle);

            Label lblStatusValue = new Label();
            lblStatusValue.Text = status;
            lblStatusValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblStatusValue.ForeColor = status == "Pending" ? System.Drawing.Color.Orange : System.Drawing.Color.Green;
            lblStatusValue.Location = new System.Drawing.Point(140, yPos);
            lblStatusValue.Size = new System.Drawing.Size(200, 25);
            dialog.Controls.Add(lblStatusValue);

            yPos += 40;

           
            Label lblReasonTitle = new Label();
            lblReasonTitle.Text = "Reason:";
            lblReasonTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblReasonTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblReasonTitle.Location = new System.Drawing.Point(20, yPos);
            lblReasonTitle.Size = new System.Drawing.Size(labelWidth, 25);
            dialog.Controls.Add(lblReasonTitle);

            TextBox txtReasonDisplay = new TextBox();
            txtReasonDisplay.Text = reason;
            txtReasonDisplay.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtReasonDisplay.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            txtReasonDisplay.Location = new System.Drawing.Point(20, yPos + 30);
            txtReasonDisplay.Size = new System.Drawing.Size(490, 80);
            txtReasonDisplay.Multiline = true;
            txtReasonDisplay.ReadOnly = true;
            txtReasonDisplay.ScrollBars = ScrollBars.Vertical;
            txtReasonDisplay.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            dialog.Controls.Add(txtReasonDisplay);

            yPos += 120;

           
            if (!string.IsNullOrEmpty(photoPath))
            {
                try
                {
                    string fullPath = Path.Combine(Application.StartupPath, photoPath);
                    if (File.Exists(fullPath))
                    {
                        
                        string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".tif" };
                        string extension = Path.GetExtension(photoPath).ToLower();
                        bool isImage = imageExtensions.Contains(extension);

                        Label lblPhotoTitle = new Label();
                        lblPhotoTitle.Text = isImage ? "📷 Attached Photo:" : "📎 Attached File:";
                        lblPhotoTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                        lblPhotoTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
                        lblPhotoTitle.Location = new System.Drawing.Point(20, yPos);
                        lblPhotoTitle.Size = new System.Drawing.Size(150, 25);
                        dialog.Controls.Add(lblPhotoTitle);

                        Label lblPhotoName = new Label();
                        lblPhotoName.Text = Path.GetFileName(photoPath);
                        lblPhotoName.Font = new System.Drawing.Font("Segoe UI", 9F);
                        lblPhotoName.ForeColor = System.Drawing.Color.DarkBlue;
                        lblPhotoName.Location = new System.Drawing.Point(180, yPos);
                        lblPhotoName.Size = new System.Drawing.Size(300, 25);
                        dialog.Controls.Add(lblPhotoName);

                        if (isImage)
                        {
                           
                            PictureBox picPhoto = new PictureBox();
                            picPhoto.Image = Image.FromFile(fullPath);
                            picPhoto.SizeMode = PictureBoxSizeMode.Zoom;
                            picPhoto.Location = new System.Drawing.Point(20, yPos + 30);
                            picPhoto.Size = new System.Drawing.Size(200, 150);
                            picPhoto.BorderStyle = BorderStyle.FixedSingle;
                            picPhoto.Cursor = Cursors.Hand;
                            dialog.Controls.Add(picPhoto);

                            
                            picPhoto.Click += (s, ev) =>
                            {
                                ShowPhotoPreview(fullPath, Path.GetFileName(photoPath));
                            };

                            Button btnViewPhoto = new Button();
                            btnViewPhoto.Text = "🔍 View Full Size";
                            btnViewPhoto.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
                            btnViewPhoto.ForeColor = System.Drawing.Color.White;
                            btnViewPhoto.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
                            btnViewPhoto.FlatStyle = FlatStyle.Flat;
                            btnViewPhoto.FlatAppearance.BorderSize = 0;
                            btnViewPhoto.Size = new System.Drawing.Size(120, 28);
                            btnViewPhoto.Location = new System.Drawing.Point(230, yPos + 80);
                            btnViewPhoto.Cursor = Cursors.Hand;
                            btnViewPhoto.Click += (s, ev) =>
                            {
                                ShowPhotoPreview(fullPath, Path.GetFileName(photoPath));
                            };
                            dialog.Controls.Add(btnViewPhoto);

                            yPos += 190;
                        }
                        else
                        {
                            
                            Button btnOpenFile = new Button();
                            btnOpenFile.Text = "📂 Open File";
                            btnOpenFile.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
                            btnOpenFile.ForeColor = System.Drawing.Color.White;
                            btnOpenFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                            btnOpenFile.FlatStyle = FlatStyle.Flat;
                            btnOpenFile.FlatAppearance.BorderSize = 0;
                            btnOpenFile.Size = new System.Drawing.Size(120, 30);
                            btnOpenFile.Location = new System.Drawing.Point(20, yPos + 30);
                            btnOpenFile.Cursor = Cursors.Hand;
                            btnOpenFile.Click += (s, ev) =>
                            {
                                try
                                {
                                    System.Diagnostics.Process.Start(fullPath);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Cannot open file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            };
                            dialog.Controls.Add(btnOpenFile);

                            
                            FileInfo fileInfo = new FileInfo(fullPath);
                            Label lblFileSize = new Label();
                            lblFileSize.Text = $"Size: {fileInfo.Length / 1024} KB";
                            lblFileSize.Font = new System.Drawing.Font("Segoe UI", 8F);
                            lblFileSize.ForeColor = System.Drawing.Color.Gray;
                            lblFileSize.Location = new System.Drawing.Point(150, yPos + 38);
                            lblFileSize.Size = new System.Drawing.Size(150, 20);
                            dialog.Controls.Add(lblFileSize);

                            yPos += 70;
                        }
                    }
                    else
                    {
                        Label lblNoPhoto = new Label();
                        lblNoPhoto.Text = "⚠️ File not found";
                        lblNoPhoto.Font = new System.Drawing.Font("Segoe UI", 9F);
                        lblNoPhoto.ForeColor = System.Drawing.Color.Gray;
                        lblNoPhoto.Location = new System.Drawing.Point(20, yPos);
                        lblNoPhoto.Size = new System.Drawing.Size(250, 25);
                        dialog.Controls.Add(lblNoPhoto);
                        yPos += 35;
                    }
                }
                catch (Exception ex)
                {
                    Label lblNoPhoto = new Label();
                    lblNoPhoto.Text = $"⚠️ Cannot load file: {ex.Message}";
                    lblNoPhoto.Font = new System.Drawing.Font("Segoe UI", 9F);
                    lblNoPhoto.ForeColor = System.Drawing.Color.Gray;
                    lblNoPhoto.Location = new System.Drawing.Point(20, yPos);
                    lblNoPhoto.Size = new System.Drawing.Size(400, 25);
                    dialog.Controls.Add(lblNoPhoto);
                    yPos += 35;
                }
            }

            int buttonYPos = yPos + 20;

            
            Button btnConfirm = new Button();
            btnConfirm.Text = "✅ Confirm";
            btnConfirm.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            btnConfirm.ForeColor = System.Drawing.Color.White;
            btnConfirm.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Size = new System.Drawing.Size(120, 40);
            btnConfirm.Location = new System.Drawing.Point(150, buttonYPos);
            btnConfirm.Cursor = Cursors.Hand;
            btnConfirm.Click += (s, ev) =>
            {
                UpdatePRRequestStatus(requestID, "Confirmed");
                dialog.Close();
                LoadPRPendingRequests();
            };
            dialog.Controls.Add(btnConfirm);

           
            Button btnReject = new Button();
            btnReject.Text = "❌ Reject";
            btnReject.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            btnReject.ForeColor = System.Drawing.Color.White;
            btnReject.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnReject.FlatStyle = FlatStyle.Flat;
            btnReject.FlatAppearance.BorderSize = 0;
            btnReject.Size = new System.Drawing.Size(120, 40);
            btnReject.Location = new System.Drawing.Point(290, buttonYPos);
            btnReject.Cursor = Cursors.Hand;
            btnReject.Click += (s, ev) =>
            {
                UpdatePRRequestStatus(requestID, "Rejected");
                dialog.Close();
                LoadPRPendingRequests();
            };
            dialog.Controls.Add(btnReject);

            Panel spacer = new Panel();
            spacer.Size = new System.Drawing.Size(1, 30);
            spacer.Location = new System.Drawing.Point(0, buttonYPos + 50);
            dialog.Controls.Add(spacer);

            dialog.ShowDialog();
        }

       
        private void ShowPhotoPreview(string filePath, string fileName)
        {
            try
            {
                Form previewForm = new Form();
                previewForm.Text = $"📷 {fileName}";
                previewForm.Size = new System.Drawing.Size(600, 600);
                previewForm.StartPosition = FormStartPosition.CenterParent;
                previewForm.BackColor = System.Drawing.Color.Black;

                Label lblFileName = new Label();
                lblFileName.Text = $"📷 {fileName}";
                lblFileName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                lblFileName.ForeColor = System.Drawing.Color.White;
                lblFileName.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                lblFileName.Dock = DockStyle.Top;
                lblFileName.Height = 40;
                lblFileName.TextAlign = ContentAlignment.MiddleCenter;
                previewForm.Controls.Add(lblFileName);

                PictureBox bigPic = new PictureBox();
                bigPic.Image = Image.FromFile(filePath);
                bigPic.SizeMode = PictureBoxSizeMode.Zoom;
                bigPic.Dock = DockStyle.Fill;
                previewForm.Controls.Add(bigPic);

                bigPic.Click += (ss, evv) => previewForm.Close();

                previewForm.KeyDown += (ss, evv) =>
                {
                    if (evv.KeyCode == Keys.Escape)
                        previewForm.Close();
                };
                previewForm.KeyPreview = true;

                previewForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot preview file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void UpdatePRRequestStatus(int requestID, string newStatus)
        {
            try
            {
                var db = new projectEntities3();
                var request = db.afterservicerequest.FirstOrDefault(r => r.RequestID == requestID);

                if (request != null)
                {
                    request.Status = newStatus;
                    db.SaveChanges();
                    MessageBox.Show($"Request #{requestID} has been {newStatus} ✅");
                }
                else
                {
                    MessageBox.Show("Request not found ❌");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update request status: " + ex.Message);
            }
        }

        
        private void Button7_Click(object sender, EventArgs e)
        {
            SetActiveButton(button7);

           
            panel1.Visible = false;
            panel2.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;

            
            panelPRPending.Visible = true;
            panelPRPending.BringToFront();

            LoadPRPendingRequests();
        }

        
        private void LoadEmployeeData(string name)
        {
            try
            {
                var employee = db.employee.FirstOrDefault(emp => emp.EmployeeName == name);
                if (employee != null)
                {
                    employeeID = employee.EmployeeID;
                    employeePhone = employee.PhoneNumber;
                }
                else
                {
                    employeeID = "N/A";
                    employeePhone = "N/A";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load employee data: " + ex.Message);
                employeeID = "N/A";
                employeePhone = "N/A";
            }
        }

       
        private void Production_Load(object sender, EventArgs e)
        {
            
            panel1.Visible = true;
            panel2.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panelPRPending.Visible = false;
            panel1.BringToFront();

            
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            
            LoadRawMaterials();

            
            LoadProductDropdowns();

            panelProducts.Size = new Size(453, 342);
            panelProducts.AutoScroll = true;

           
            flowLayoutPanelProducts.Size = new Size(panelProducts.Width - 15, panelProducts.Height - 15);
            flowLayoutPanelProducts.AutoSize = false;

            if (!isInitialized)
            {
                LoadMaterialsToDisplay();
                LoadCartPanel();
                isInitialized = true;
            }

           
            SetEmployeeInfo();

           
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;

            
            SetActiveButton(button1);
        }

        
        private void SetEmployeeInfo()
        {
            textBox2.Text = employeeID;
            textBox3.Text = staffName;
            textBox4.Text = employeePhone;
        }

       
        private void LoadRawMaterials()
        {
            try
            {
                var requests = (from r in db.rawmaterialrequest
                                join emp in db.employee on r.EmployeeID equals emp.EmployeeID
                                orderby r.CreatedDate descending
                                select new
                                {
                                    r.RequestID,
                                    EmployeeName = emp.EmployeeName,
                                    r.TakesTime,
                                    r.Status,
                                    r.CreatedBy,
                                    r.CreatedDate
                                }).ToList();

                dataGridView1.DataSource = requests;

               
                if (dataGridView1.Columns.Contains("View"))
                {
                    dataGridView1.Columns.Remove("View");
                }

                DataGridViewButtonColumn btnView = new DataGridViewButtonColumn();
                btnView.Name = "View";
                btnView.HeaderText = "";
                btnView.Text = "View";
                btnView.UseColumnTextForButtonValue = true;
                btnView.Width = 80;
                dataGridView1.Columns.Insert(0, btnView);

               
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.ScrollBars = ScrollBars.Vertical;

                
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load raw material requests: " + ex.Message);
            }
        }

        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dataGridView1.Columns[e.ColumnIndex].Name == "View")
            {
                string requestID = dataGridView1.Rows[e.RowIndex].Cells["RequestID"].Value?.ToString() ?? "";

                if (!string.IsNullOrEmpty(requestID))
                {
                    ShowRequestDetails(requestID);
                }
            }
        }

       
        private void ShowRequestDetails(string requestID)
        {
            try
            {
                var request = (from r in db.rawmaterialrequest
                               join emp in db.employee on r.EmployeeID equals emp.EmployeeID
                               where r.RequestID == requestID
                               select new
                               {
                                   r.RequestID,
                                   EmployeeName = emp.EmployeeName,
                                   EmployeeID = r.EmployeeID,
                                   r.TakesTime,
                                   r.Status,
                                   r.CreatedBy,
                                   r.CreatedDate
                               }).FirstOrDefault();

                if (request == null)
                {
                    MessageBox.Show("Request not found.");
                    return;
                }

                var items = (from i in db.rawmaterialrequestitem
                             where i.RequestID == requestID
                             select new
                             {
                                 i.MaterialID,
                                 i.MaterialName,
                                 i.Quantity
                             }).ToList();

                StringBuilder message = new StringBuilder();
                message.AppendLine($"=== Raw Material Request Details ===");
                message.AppendLine();
                message.AppendLine($"Request ID: {request.RequestID}");
                message.AppendLine($"Employee: {request.EmployeeName} ({request.EmployeeID})");
                message.AppendLine($"Takes Time: {request.TakesTime?.ToString("yyyy-MM-dd HH:mm") ?? "Not set"}");
                message.AppendLine($"Status: {request.Status}");
                message.AppendLine($"Created By: {request.CreatedBy}");
                message.AppendLine($"Created Date: {request.CreatedDate?.ToString("yyyy-MM-dd HH:mm") ?? "Not set"}");
                message.AppendLine();
                message.AppendLine("--- Materials ---");

                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        message.AppendLine($"  • {item.MaterialID} - {item.MaterialName} x {item.Quantity}");
                    }
                }
                else
                {
                    message.AppendLine("  No materials found.");
                }

                MessageBox.Show(message.ToString(), "Request Details",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load request details: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void LoadProductDropdowns()
        {
            try
            {
                var db = new projectEntities3();
                var products = db.product
                    .Select(p => new { p.ProductID, p.ProductName, p.Qty })
                    .ToList();

                comboBoxProduct.DataSource = products;
                comboBoxProduct.DisplayMember = "ProductName";
                comboBoxProduct.ValueMember = "ProductID";
                comboBoxProduct.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

       
        private void RefreshProductGrid()
        {
            try
            {
                var db = new projectEntities3();
                var products = db.product
                    .Select(p => new
                    {
                        p.ProductID,
                        p.ProductName,
                        UnitPrice = p.unitPrice,
                        Stock = p.Qty
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        
        private void SearchProduct(string keyword)
        {
            try
            {
                var db = new projectEntities3();

                var result = db.product
                    .Where(p => string.IsNullOrEmpty(keyword) ||
                                p.ProductID.Contains(keyword) ||
                                p.ProductName.Contains(keyword))
                    .Select(p => new
                    {
                        p.ProductID,
                        p.ProductName,
                        UnitPrice = p.unitPrice,
                        Stock = p.Qty
                    })
                    .ToList();

                if (result.Count == 0)
                {
                    MessageBox.Show("No products found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching: " + ex.Message);
            }
        }

        
        private void LoadMaterialsToDisplay()
        {
            
            flowLayoutPanelProducts.Controls.Clear();

            
            using (var dbContext = new projectEntities3())
            {
                
                var materials = dbContext.rawmaterials.ToList();

                
                flowLayoutPanelProducts.FlowDirection = FlowDirection.LeftToRight;
                flowLayoutPanelProducts.WrapContents = true;
                flowLayoutPanelProducts.AutoSize = false;

                
                flowLayoutPanelProducts.Size = new Size(panelProducts.Width - 20, panelProducts.Height - 20);
                flowLayoutPanelProducts.Padding = new Padding(5);

                
                int itemsPerRow = Math.Max(1, (flowLayoutPanelProducts.Width - 10) / 210);
                int totalRows = (int)Math.Ceiling((double)materials.Count / itemsPerRow);
                int totalHeight = totalRows * 220 + 20;

               
                flowLayoutPanelProducts.Height = Math.Max(totalHeight, panelProducts.Height - 20);

                foreach (var material in materials)
                {
                    Panel materialPanel = CreateMaterialPanel(material);
                    flowLayoutPanelProducts.Controls.Add(materialPanel);
                }

                
                flowLayoutPanelProducts.PerformLayout();
                flowLayoutPanelProducts.Invalidate();
                flowLayoutPanelProducts.Refresh();
            }
        }

       
        private Panel CreateMaterialPanel(rawmaterials material)
        {
            Panel materialPanel = new Panel();
            materialPanel.Width = 200;
            materialPanel.Height = 210;
            materialPanel.BorderStyle = BorderStyle.FixedSingle;
            materialPanel.Tag = material;
            materialPanel.BackColor = Color.White;
            materialPanel.Margin = new Padding(3);

            
            PictureBox pic = new PictureBox();
            pic.Image = GetMaterialImage(material.MaterialID, material.MaterialName);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Size = new Size(180, 80);
            pic.Location = new Point(10, 8);
            materialPanel.Controls.Add(pic);

           
            Label lblName = new Label();
            lblName.Text = material.MaterialName;
            lblName.Font = new Font("Arial", 10, FontStyle.Bold);
            lblName.TextAlign = ContentAlignment.MiddleCenter;
            lblName.Size = new Size(180, 25);
            lblName.Location = new Point(10, 95);
            materialPanel.Controls.Add(lblName);

            
            Label lblCategory = new Label();
            lblCategory.Text = $"Category: {material.Category}";
            lblCategory.Font = new Font("Arial", 8);
            lblCategory.Size = new Size(180, 18);
            lblCategory.Location = new Point(10, 122);
            materialPanel.Controls.Add(lblCategory);

           
            Label lblStock = new Label();
            lblStock.Text = $"Stock: {material.StockQty}";
            lblStock.Font = new Font("Arial", 8, FontStyle.Bold);
            lblStock.ForeColor = material.StockQty < 10 ? Color.Red : Color.Green;
            lblStock.Size = new Size(180, 18);
            lblStock.Location = new Point(10, 142);
            materialPanel.Controls.Add(lblStock);

            
            Label lblQty = new Label();
            lblQty.Text = "Qty:";
            lblQty.Font = new Font("Arial", 8);
            lblQty.Size = new Size(35, 25);
            lblQty.Location = new Point(10, 165);
            materialPanel.Controls.Add(lblQty);

            NumericUpDown numQty = new NumericUpDown();
            numQty.Minimum = 1;
           
            int maxQty = material.StockQty > 0 ? material.StockQty : 1;
            numQty.Maximum = maxQty;
            numQty.Value = 1;
            numQty.Size = new Size(50, 22);
            numQty.Location = new Point(50, 163);

           
            numQty.MouseWheel += (s, ev) =>
            {
                ((HandledMouseEventArgs)ev).Handled = true;
            };

            materialPanel.Controls.Add(numQty);

           
            Button btnAddToCart = new Button();
            btnAddToCart.Text = "+ Add";
            btnAddToCart.BackColor = Color.LightBlue;
            btnAddToCart.Font = new Font("Arial", 8, FontStyle.Bold);
            btnAddToCart.Size = new Size(80, 28);
            btnAddToCart.Location = new Point(110, 162);
            btnAddToCart.Tag = material;
            btnAddToCart.Click += (s, ev) =>
            {
                
                if (material.StockQty <= 0)
                {
                    MessageBox.Show($"Material {material.MaterialName} is out of stock!", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                AddToCart(material, (int)numQty.Value);
            };
            materialPanel.Controls.Add(btnAddToCart);

            return materialPanel;
        }

        
        private Image GetMaterialImage(string materialId, string materialName)
        {
           
            string imagePath = Path.Combine(Application.StartupPath, "rm_images", $"{materialId}.jpg");

           
            if (!File.Exists(imagePath))
            {
                imagePath = Path.Combine(Application.StartupPath, "rm_images", $"{materialId}.png");
            }

            
            if (File.Exists(imagePath))
            {
                try
                {
                    return Image.FromFile(imagePath);
                }
                catch
                {
                    
                    return GetDefaultMaterialImage(materialId, materialName);
                }
            }

           
            return GetDefaultMaterialImage(materialId, materialName);
        }

        
        private Image GetDefaultMaterialImage(string materialId, string materialName)
        {
            Bitmap bmp = new Bitmap(180, 80);
            using (Graphics g = Graphics.FromImage(bmp))
            {
               
                int colorIndex = Math.Abs(materialId.GetHashCode()) % 8;
                Color[] colors = {
                    Color.FromArgb(52, 152, 219),   
                    Color.FromArgb(46, 204, 113),   
                    Color.FromArgb(231, 76, 60),    
                    Color.FromArgb(155, 89, 182),   
                    Color.FromArgb(241, 196, 15),   
                    Color.FromArgb(230, 126, 34),   
                    Color.FromArgb(26, 188, 156),   
                    Color.FromArgb(149, 165, 166)   
                };

                g.Clear(colors[colorIndex]);
                g.DrawRectangle(Pens.DarkGray, 0, 0, 179, 79);

                
                string icon = GetMaterialIcon(materialName);
                using (Font iconFont = new Font("Segoe UI", 32))
                {
                    SizeF textSize = g.MeasureString(icon, iconFont);
                    float x = (180 - textSize.Width) / 2;
                    float y = (80 - textSize.Height) / 2;
                    g.DrawString(icon, iconFont, Brushes.White, x, y);
                }

               
                string shortName = materialName.Length > 12 ? materialName.Substring(0, 10) + "..." : materialName;
                using (Font textFont = new Font("Arial", 7, FontStyle.Bold))
                {
                    SizeF textSize = g.MeasureString(shortName, textFont);
                    float x = (180 - textSize.Width) / 2;
                    g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 0)), x - 5, 55, textSize.Width + 10, 20);
                    g.DrawString(shortName, textFont, Brushes.White, x, 57);
                }
            }
            return bmp;
        }

        
        private string GetMaterialIcon(string materialName)
        {
            if (materialName.Contains("Wood"))
                return "🪵";
            if (materialName.Contains("Steel"))
                return "⚙️";
            if (materialName.Contains("Plastic"))
                return "🧪";
            if (materialName.Contains("Glass"))
                return "🔮";
            if (materialName.Contains("Metal"))
                return "🔩";
            if (materialName.Contains("Fabric"))
                return "🧵";
            if (materialName.Contains("Leather"))
                return "👜";
            if (materialName.Contains("Rubber"))
                return "🔄";
            return "📦";
        }

       
        private void LoadCartPanel()
        {
            panelCart.Controls.Clear();

            
            Label lblCartTitle = new Label();
            lblCartTitle.Text = "🛒 CART";
            lblCartTitle.Font = new Font("Arial", 9, FontStyle.Bold);
            lblCartTitle.Dock = DockStyle.Top;
            lblCartTitle.Height = 30;
            lblCartTitle.TextAlign = ContentAlignment.MiddleCenter;
            panelCart.Controls.Add(lblCartTitle);

            
            Panel line = new Panel();
            line.Height = 2;
            line.Dock = DockStyle.Top;
            line.BackColor = Color.Gray;
            panelCart.Controls.Add(line);

            
            Panel bottomPanel = new Panel();
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = 70;

            cartTotalLabel = new Label();
            cartTotalLabel.Name = "lblCartTotal";
            cartTotalLabel.Text = "Total: 0 items";
            cartTotalLabel.Font = new Font("Arial", 9, FontStyle.Bold);
            cartTotalLabel.ForeColor = Color.Red;
            cartTotalLabel.Location = new Point(10, 5);
            cartTotalLabel.Width = 200;

            Button btnClearCart = new Button();
            btnClearCart.Text = "Clear";
            btnClearCart.Size = new Size(70, 25);
            btnClearCart.Location = new Point(10, 30);
            btnClearCart.Click += (s, ev) => ClearCart();

            Button btnCheckout = new Button();
            btnCheckout.Text = "Request";
            btnCheckout.Size = new Size(90, 25);
            btnCheckout.Location = new Point(90, 30);
            btnCheckout.Click += (s, ev) => Checkout();

            bottomPanel.Controls.Add(cartTotalLabel);
            bottomPanel.Controls.Add(btnClearCart);
            bottomPanel.Controls.Add(btnCheckout);
            panelCart.Controls.Add(bottomPanel);

            
            FlowLayoutPanel cartFlowPanel = new FlowLayoutPanel();
            cartFlowPanel.Location = new Point(5, 35);
            cartFlowPanel.Size = new Size(260, 200);
            cartFlowPanel.FlowDirection = FlowDirection.TopDown;
            cartFlowPanel.WrapContents = false;
            cartFlowPanel.AutoScroll = true;
            cartFlowPanel.HorizontalScroll.Enabled = false;
            cartFlowPanel.HorizontalScroll.Visible = false;
            cartFlowPanel.BackColor = Color.WhiteSmoke;
            cartFlowPanel.Padding = new Padding(3);
            panelCart.Controls.Add(cartFlowPanel);

            cartItemsContainer = cartFlowPanel;
        }

        
        private void AddToCart(rawmaterials material, int quantity)
        {
            var existingItem = cartItems.FirstOrDefault(c => c.Material.MaterialID == material.MaterialID);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                if (existingItem.Quantity > material.StockQty)
                {
                    existingItem.Quantity = material.StockQty;
                    MessageBox.Show($"Stock limit reached for {material.MaterialName}. Max {material.StockQty} available.");
                }
            }
            else
            {
                cartItems.Add(new CartItem { Material = material, Quantity = quantity });
            }

            UpdateCartDisplay();
        }

      
        private void UpdateCartDisplay()
        {
            if (cartItemsContainer == null) return;

            cartItemsContainer.Controls.Clear();
            int totalItems = 0;

            foreach (var item in cartItems)
            {
                Panel itemPanel = new Panel();
                itemPanel.Size = new Size(240, 50);
                itemPanel.Margin = new Padding(3, 3, 3, 3);
                itemPanel.BorderStyle = BorderStyle.FixedSingle;
                itemPanel.BackColor = Color.White;

               
                Label lblName = new Label();
                lblName.Text = item.Material.MaterialName;
                lblName.Font = new Font("Arial", 8, FontStyle.Bold);
                lblName.AutoSize = true;
                lblName.MaximumSize = new Size(140, 0);
                lblName.Location = new Point(3, 2);
                itemPanel.Controls.Add(lblName);

                
                Label lblInfo = new Label();
                lblInfo.Text = $"Qty: {item.Quantity}  (Stock: {item.Material.StockQty})";
                lblInfo.Font = new Font("Arial", 7);
                lblInfo.Size = new Size(150, 14);
                lblInfo.Location = new Point(3, 25);
                itemPanel.Controls.Add(lblInfo);

                
                Button btnRemove = new Button();
                btnRemove.Text = "✖";
                btnRemove.Size = new Size(18, 18);
                btnRemove.Location = new Point(220, 15);
                btnRemove.BackColor = Color.LightCoral;
                btnRemove.Font = new Font("Arial", 7);
                btnRemove.Tag = item;
                btnRemove.Click += (s, ev) =>
                {
                    cartItems.Remove(item);
                    UpdateCartDisplay();
                };
                itemPanel.Controls.Add(btnRemove);

                cartItemsContainer.Controls.Add(itemPanel);
                totalItems += item.Quantity;
            }

            if (cartTotalLabel != null)
            {
                cartTotalLabel.Text = $"Total: {totalItems} items";
            }

            cartItemsContainer.PerformLayout();
            cartItemsContainer.Invalidate();
        }

       
        private void ClearCart()
        {
            cartItems.Clear();
            UpdateCartDisplay();
        }

        private void Checkout()
        {
            if (cartItems.Count == 0)
            {
                MessageBox.Show("Please add at least one material to cart first!", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GoToCreateRequest();
        }

       
        private void GoToCreateRequest()
        {
            
            panel1.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel2.Visible = true;

            
            panel2.BringToFront();

            
            flowLayoutPanel1.Controls.Clear();

            foreach (var cartItem in cartItems)
            {
                AddRequestRowWithMaterial(cartItem.Material, cartItem.Quantity);
            }

            
            SetEmployeeInfo();
        }

        
        private void AddRequestRowWithMaterial(rawmaterials selectedMaterial, int quantity)
        {
            var materialList = db.rawmaterials.ToList();

            Panel row = new Panel();
            row.Width = 650;
            row.Height = 35;

            ComboBox cb = new ComboBox();
            cb.DataSource = materialList;
            cb.DisplayMember = "MaterialName";
            cb.Width = 180;
            cb.Location = new Point(0, 5);
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.Enabled = false;  
            NumericUpDown num = new NumericUpDown();
            num.Minimum = 1;
            num.Maximum = selectedMaterial.StockQty;
            num.Value = Math.Min(quantity, selectedMaterial.StockQty);
            num.Location = new Point(190, 5);
            num.Width = 60;
            num.Enabled = false;  
           
            num.MouseWheel += (s, ev) =>
            {
                ((HandledMouseEventArgs)ev).Handled = true;
            };

            
            Label lblStock = new Label();
            lblStock.Text = $"Stock: {selectedMaterial.StockQty}";
            lblStock.Font = new Font("Arial", 8);
            lblStock.ForeColor = selectedMaterial.StockQty < 10 ? Color.Red : Color.Green;
            lblStock.Size = new Size(100, 20);
            lblStock.Location = new Point(260, 8);
            lblStock.TextAlign = ContentAlignment.MiddleLeft;

            row.Controls.Add(cb);
            row.Controls.Add(num);
            row.Controls.Add(lblStock);

            flowLayoutPanel1.Controls.Add(row);

            
            for (int i = 0; i < cb.Items.Count; i++)
            {
                rawmaterials m = cb.Items[i] as rawmaterials;
                if (m != null && m.MaterialID == selectedMaterial.MaterialID)
                {
                    cb.SelectedIndex = i;
                    break;
                }
            }
        }

        
        private void SaveFlowLayoutToCart()
        {
           
            cartItems.Clear();

            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                Panel row = ctrl as Panel;
                if (row == null) continue;

                ComboBox cb = row.Controls.OfType<ComboBox>().FirstOrDefault();
                NumericUpDown num = row.Controls.OfType<NumericUpDown>().FirstOrDefault();

                if (cb != null && cb.SelectedItem != null && num != null && num.Value > 0)
                {
                    var material = cb.SelectedItem as rawmaterials;
                    if (material != null && material.MaterialID != null)
                    {
                        cartItems.Add(new CartItem
                        {
                            Material = material,
                            Quantity = (int)num.Value
                        });
                    }
                }
            }
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            SetActiveButton(button1);

           
            panel2.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panelPRPending.Visible = false;
            panel1.Visible = true;
            panel1.BringToFront();

            
            LoadRawMaterials();
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = textBox1.Text.Trim();

                var requests = (from r in db.rawmaterialrequest
                                join emp in db.employee on r.EmployeeID equals emp.EmployeeID
                                where string.IsNullOrEmpty(keyword) ||
                                      r.RequestID.Contains(keyword) ||
                                      emp.EmployeeName.Contains(keyword) ||
                                      r.Status.Contains(keyword)
                                orderby r.CreatedDate descending
                                select new
                                {
                                    r.RequestID,
                                    EmployeeName = emp.EmployeeName,
                                    r.TakesTime,
                                    r.Status,
                                    r.CreatedBy,
                                    r.CreatedDate
                                }).ToList();

                dataGridView1.DataSource = requests;

                
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.ScrollBars = ScrollBars.Vertical;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message);
            }
        }

       
        private void button3_Click(object sender, EventArgs e)
        {
            SetActiveButton(button3);

            
            panel1.Visible = false;
            panel2.Visible = false;
            panel5.Visible = false;
            panelPRPending.Visible = false;
            panel4.Visible = true;
            panel4.BringToFront();

           
            LoadMaterialsToDisplay();

            
            UpdateCartDisplay();
        }

        
        private void button4_Click(object sender, EventArgs e)
        {
            
            SaveFlowLayoutToCart();

            
            flowLayoutPanel1.Controls.Clear();

            
            panel2.Visible = false;
            panel4.Visible = true;
            panel1.Visible = false;
            panel5.Visible = false;
            panelPRPending.Visible = false;
            panel4.BringToFront();

           
            panelProducts.Visible = true;
            panelProducts.AutoScroll = true;

           
            UpdateCartDisplay();

            
            LoadMaterialsToDisplay();

            
            panel4.Refresh();
            panelProducts.Refresh();
            flowLayoutPanelProducts.Refresh();
            Application.DoEvents();
        }

        
        private void button5_Click(object sender, EventArgs e)
        {
            
            List<RequestMaterialDto> selectedMaterials = new List<RequestMaterialDto>();
            bool hasMaterial = false;

            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                Panel row = ctrl as Panel;
                if (row == null) continue;

                ComboBox cb = row.Controls.OfType<ComboBox>().FirstOrDefault();
                NumericUpDown num = row.Controls.OfType<NumericUpDown>().FirstOrDefault();

                if (cb != null && cb.SelectedItem != null && num != null && num.Value > 0)
                {
                    var material = cb.SelectedItem as rawmaterials;
                    if (material != null && material.MaterialID != null)
                    {
                        selectedMaterials.Add(new RequestMaterialDto
                        {
                            MaterialID = material.MaterialID,
                            MaterialName = material.MaterialName,
                            Quantity = (int)num.Value
                        });
                        hasMaterial = true;
                    }
                }
            }

            if (!hasMaterial)
            {
                MessageBox.Show("Please select at least one material.", "Warning",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                
                foreach (var material in selectedMaterials)
                {
                    var dbMaterial = db.rawmaterials.FirstOrDefault(m => m.MaterialID == material.MaterialID);
                    if (dbMaterial == null)
                    {
                        MessageBox.Show($"Material {material.MaterialName} not found in database!", "Error",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (dbMaterial.StockQty < material.Quantity)
                    {
                        MessageBox.Show($"Insufficient stock for {material.MaterialName}!\n\n" +
                                       $"Requested: {material.Quantity}\n" +
                                       $"Available: {dbMaterial.StockQty}",
                                       "Insufficient Stock",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Warning);
                        return;
                    }
                }

                
                string requestID = GenerateRequestID();

                
                rawmaterialrequest newRequest = new rawmaterialrequest
                {
                    RequestID = requestID,
                    EmployeeID = employeeID,
                    TakesTime = dateTimePicker1.Value,
                    Status = "Pending",
                    CreatedBy = staffName,
                    CreatedDate = DateTime.Now
                };

                db.rawmaterialrequest.Add(newRequest);

               
                foreach (var material in selectedMaterials)
                {
                    rawmaterialrequestitem detail = new rawmaterialrequestitem
                    {
                        RequestID = requestID,
                        MaterialID = material.MaterialID,
                        MaterialName = material.MaterialName,
                        Quantity = material.Quantity
                    };

                    db.rawmaterialrequestitem.Add(detail);

                    
                    var dbMaterial = db.rawmaterials.FirstOrDefault(m => m.MaterialID == material.MaterialID);
                    if (dbMaterial != null)
                    {
                        dbMaterial.StockQty -= material.Quantity;
                        
                        if (dbMaterial.StockQty < 0) dbMaterial.StockQty = 0;
                    }
                }

                
                db.SaveChanges();

                MessageBox.Show($"Request saved successfully! ✅\n\n" +
                               $"Request ID: {requestID}\n" +
                               $"Employee: {staffName}\n" +
                               $"Materials: {selectedMaterials.Count} items\n\n" +
                               "⚠️ Material stock has been deducted.",
                               "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                panel2.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
                panelPRPending.Visible = false;
                panel1.Visible = true;
                panel1.BringToFront();

               
                flowLayoutPanel1.Controls.Clear();
                cartItems.Clear();
                UpdateCartDisplay();

                
                LoadRawMaterials();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void button6_Click(object sender, EventArgs e)
        {
            SetActiveButton(button6);

            panel1.Visible = false;
            panel2.Visible = false;
            panel4.Visible = false;
            panel5.Visible = true;
            panelPRPending.Visible = false;
            panel5.BringToFront();
            RefreshProductGrid();
        }

        
        private void button8_Click(object sender, EventArgs e)
        {
            if (comboBoxProduct.SelectedValue == null)
            {
                MessageBox.Show("Please select a product! ❌", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBoxProductQty.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Please enter a valid positive quantity! ❌", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string selectedProductID = comboBoxProduct.SelectedValue.ToString();
                var db = new projectEntities3();

                var product = db.product.FirstOrDefault(p => p.ProductID == selectedProductID);

                if (product != null)
                {
                    product.Qty += qty;
                    db.SaveChanges();

                    MessageBox.Show(
                        $"Product Quantity Updated! ✅\n" +
                        $"Product: {product.ProductName}\n" +
                        $"Qty Added: {qty}\n" +
                        $"New Stock Level: {product.Qty}",
                        "Update Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    textBoxProductQty.Clear();
                    comboBoxProduct.SelectedIndex = -1;

                    RefreshProductGrid();
                }
                else
                {
                    MessageBox.Show("Product not found! ❌", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update product quantity: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void textBoxProductQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button8_Click(sender, e);
                e.Handled = true;
            }
        }

        
        private string GenerateRequestID()
        {
            string prefix = "RQ";
            Random random = new Random();
            int randomNumber = random.Next(10000000, 99999999);
            return prefix + randomNumber.ToString();
        }

        
        public class RequestMaterialDto
        {
            public string MaterialID { get; set; }
            public string MaterialName { get; set; }
            public int Quantity { get; set; }
        }

       
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

      
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
    }
}