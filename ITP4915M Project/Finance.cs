using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ITP4915M_Project
{
    public partial class Finance : Form
    {
        private projectEntities3 db = new projectEntities3();
        private string staffName;

       
        public class AfterServiceRequestDto
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

        public Finance(string name)
        {
            InitializeComponent();
            label2.Text = name;
            staffName = name;

            this.Load += new System.EventHandler(this.Finance_Load);

           
            this.dataGridView1.CellClick -= new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);

            this.dgvFIPending.CellClick -= new DataGridViewCellEventHandler(this.dgvFIPending_CellClick);
            this.dgvFIPending.CellClick += new DataGridViewCellEventHandler(this.dgvFIPending_CellClick);

            
            button1.Click -= button1_Click;
            button1.Click += button1_Click;

            button2.Click -= button2_Click;
            button2.Click += button2_Click;

            buttonBack.Click -= buttonBack_Click;
            buttonBack.Click += buttonBack_Click;

            
            buttonViewInvoice.Click -= buttonViewInvoice_Click;
            buttonViewInvoice.Click += buttonViewInvoice_Click;
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

            button2.BackColor = defaultColor;
            button2.ForeColor = defaultForeColor;

            buttonViewInvoice.BackColor = defaultColor;
            buttonViewInvoice.ForeColor = defaultForeColor;
        }

       
        private void Finance_Load(object sender, EventArgs e)
        {
            LoadInvoices();

            
            SetActiveButton(buttonViewInvoice);
        }

      
        private void buttonViewInvoice_Click(object sender, EventArgs e)
        {
            SetActiveButton(buttonViewInvoice);  

            
            panelContent.Visible = true;
            panelFIPending.Visible = false;
            panelContent.BringToFront();
            LoadInvoices();
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            SetActiveButton(button1);  

            
            using (Form3 form = new Form3(staffName))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    
                    LoadInvoices();
                }
            }
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            SetActiveButton(button2);  

            ShowFIPending();
        }

        
        private void buttonBack_Click(object sender, EventArgs e)
        {
            ShowInvoiceList();
        }

       
        private void ShowInvoiceList()
        {
            SetActiveButton(buttonViewInvoice);  

            panelContent.Visible = true;
            panelFIPending.Visible = false;
            panelContent.BringToFront();
            LoadInvoices();
        }

       
        private void ShowFIPending()
        {
            panelContent.Visible = false;
            panelFIPending.Visible = true;
            panelFIPending.BringToFront();
            LoadFIPendingRequests();
        }

        
        private void SetupFIPendingDataGridView()
        {
            dgvFIPending.AutoGenerateColumns = false;
            dgvFIPending.AllowUserToAddRows = false;
            dgvFIPending.ReadOnly = true;
            dgvFIPending.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFIPending.MultiSelect = false;
            dgvFIPending.Columns.Clear();

            dgvFIPending.ScrollBars = ScrollBars.Both;
            dgvFIPending.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            
            DataGridViewButtonColumn viewButton = new DataGridViewButtonColumn();
            viewButton.Name = "View";
            viewButton.HeaderText = "";
            viewButton.Text = "🔍 View";
            viewButton.UseColumnTextForButtonValue = true;
            viewButton.Width = 80;
            viewButton.Frozen = true;
            viewButton.FlatStyle = FlatStyle.Popup;
            viewButton.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            viewButton.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            viewButton.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            dgvFIPending.Columns.Add(viewButton);

            
            DataGridViewTextBoxColumn colRequestID = new DataGridViewTextBoxColumn();
            colRequestID.Name = "RequestID";
            colRequestID.HeaderText = "Request ID";
            colRequestID.DataPropertyName = "RequestID";
            colRequestID.Width = 100;
            dgvFIPending.Columns.Add(colRequestID);

           
            DataGridViewTextBoxColumn colOrderID = new DataGridViewTextBoxColumn();
            colOrderID.Name = "OrderID";
            colOrderID.HeaderText = "Order ID";
            colOrderID.DataPropertyName = "OrderID";
            colOrderID.Width = 100;
            dgvFIPending.Columns.Add(colOrderID);

           
            DataGridViewTextBoxColumn colRequestType = new DataGridViewTextBoxColumn();
            colRequestType.Name = "RequestType";
            colRequestType.HeaderText = "Request Type";
            colRequestType.DataPropertyName = "RequestType";
            colRequestType.Width = 120;
            dgvFIPending.Columns.Add(colRequestType);

            
            DataGridViewTextBoxColumn colReason = new DataGridViewTextBoxColumn();
            colReason.Name = "Reason";
            colReason.HeaderText = "Reason";
            colReason.DataPropertyName = "Reason";
            colReason.Width = 200;
            dgvFIPending.Columns.Add(colReason);

            
            DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
            colStatus.Name = "Status";
            colStatus.HeaderText = "Status";
            colStatus.DataPropertyName = "Status";
            colStatus.Width = 100;
            dgvFIPending.Columns.Add(colStatus);

            
            DataGridViewTextBoxColumn colRequestDate = new DataGridViewTextBoxColumn();
            colRequestDate.Name = "RequestDate";
            colRequestDate.HeaderText = "Request Date";
            colRequestDate.DataPropertyName = "RequestDate";
            colRequestDate.Width = 140;
            colRequestDate.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            dgvFIPending.Columns.Add(colRequestDate);

           
            DataGridViewTextBoxColumn colHandledBy = new DataGridViewTextBoxColumn();
            colHandledBy.Name = "HandledBy";
            colHandledBy.HeaderText = "Handled By";
            colHandledBy.DataPropertyName = "HandledBy";
            colHandledBy.Width = 100;
            dgvFIPending.Columns.Add(colHandledBy);

           
            DataGridViewTextBoxColumn colForwardTo = new DataGridViewTextBoxColumn();
            colForwardTo.Name = "ForwardTo";
            colForwardTo.HeaderText = "Forward To";
            colForwardTo.DataPropertyName = "ForwardTo";
            colForwardTo.Visible = false;
            dgvFIPending.Columns.Add(colForwardTo);

            
            DataGridViewTextBoxColumn colPhotoPath = new DataGridViewTextBoxColumn();
            colPhotoPath.Name = "PhotoPath";
            colPhotoPath.HeaderText = "PhotoPath";
            colPhotoPath.DataPropertyName = "PhotoPath";
            colPhotoPath.Visible = false;
            dgvFIPending.Columns.Add(colPhotoPath);

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            headerStyle.ForeColor = System.Drawing.Color.White;
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            headerStyle.WrapMode = DataGridViewTriState.True;
            dgvFIPending.ColumnHeadersDefaultCellStyle = headerStyle;

            dgvFIPending.EnableHeadersVisualStyles = false;
            dgvFIPending.GridColor = System.Drawing.Color.FromArgb(230, 230, 230);
        }

        
        private void LoadFIPendingRequests()
        {
            try
            {
                var list = db.afterservicerequest
                    .Where(r => r.ForwardTo == "FI" && r.Status == "Pending")
                    .Select(r => new AfterServiceRequestDto
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

                dgvFIPending.Columns.Clear();
                dgvFIPending.DataSource = null;
                dgvFIPending.Refresh();

                SetupFIPendingDataGridView();

                if (list.Count == 0)
                {
                    dgvFIPending.DataSource = list;
                    dgvFIPending.Refresh();
                    MessageBox.Show("No pending requests for FI.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dgvFIPending.DataSource = list;
                dgvFIPending.Refresh();

                
                if (dgvFIPending.Columns.Contains("PhotoPath"))
                {
                    dgvFIPending.Columns["PhotoPath"].Visible = false;
                }

                dgvFIPending.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load FI pending requests.\n" + ex.Message);
            }
        }

       
        private void dgvFIPending_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (e.ColumnIndex != 0) 
                return;

            if (dgvFIPending.Rows.Count == 0 || e.RowIndex >= dgvFIPending.Rows.Count)
                return;

            if (dgvFIPending.Rows[e.RowIndex].IsNewRow)
                return;

            if (dgvFIPending.Rows[e.RowIndex].Cells["RequestID"].Value == null)
                return;

            try
            {
                DataGridViewRow row = dgvFIPending.Rows[e.RowIndex];
                int requestID = Convert.ToInt32(row.Cells["RequestID"].Value);
                string orderID = row.Cells["OrderID"].Value?.ToString() ?? "";
                string requestType = row.Cells["RequestType"].Value?.ToString() ?? "";
                string reason = row.Cells["Reason"].Value?.ToString() ?? "";
                string status = row.Cells["Status"].Value?.ToString() ?? "";
                string photoPath = row.Cells["PhotoPath"].Value?.ToString() ?? "";  

                ShowFIPendingDetailDialog(requestID, orderID, requestType, reason, status, photoPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error viewing request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void ShowFIPendingDetailDialog(int requestID, string orderID, string requestType, string reason, string status, string photoPath = null)
        {
            Form dialog = new Form();
            dialog.Text = "📋 FI Request Details";
            dialog.Size = new System.Drawing.Size(550, 520);
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.BackColor = System.Drawing.Color.White;
            
            dialog.AutoScroll = true;
            dialog.AutoScrollMinSize = new System.Drawing.Size(550, 600);

            Label lblTitle = new Label();
            lblTitle.Text = "FI Request Details";
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
                UpdateFIRequestStatus(requestID, "Confirmed");
                dialog.Close();
                LoadFIPendingRequests();
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
                UpdateFIRequestStatus(requestID, "Rejected");
                dialog.Close();
                LoadFIPendingRequests();
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

       
        private void UpdateFIRequestStatus(int requestID, string newStatus)
        {
            try
            {
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

        
        private void LoadInvoices()
        {
            try
            {
                var invoices = (from i in db.invoice
                                join o in db.orders on i.OrderID equals o.OrderID
                                orderby i.CreatedDate descending
                                select new InvoiceDto
                                {
                                    InvoiceID = i.InvoiceID,
                                    OrderID = i.OrderID,
                                    CustomerName = o.CustomerName,
                                    CustomerPhone = o.CustomerPhone,
                                    TotalAmount = i.TotalAmount,
                                    PaymentMethod = i.PaymentMethod,
                                    CreatedBy = i.CreatedBy,
                                    CreatedDate = i.CreatedDate
                                }).ToList();

                dataGridView1.Columns.Clear();
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.RowHeadersVisible = false;


                dataGridView1.ScrollBars = ScrollBars.Both;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                AddViewButton();
                AddDataColumns();
                dataGridView1.DataSource = invoices;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load invoices: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        public class InvoiceDto
        {
            public string InvoiceID { get; set; }
            public string OrderID { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public decimal? TotalAmount { get; set; }
            public string PaymentMethod { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? CreatedDate { get; set; }
        }

        
        private void AddViewButton()
        {
            DataGridViewButtonColumn btnView = new DataGridViewButtonColumn();
            btnView.Name = "ViewPDF";
            btnView.HeaderText = "";
            btnView.Text = "View PDF";
            btnView.UseColumnTextForButtonValue = true;
            btnView.Width = 80;
            btnView.Frozen = true;  
            dataGridView1.Columns.Add(btnView);
        }

        
        private void AddDataColumns()
        {
           
            DataGridViewTextBoxColumn colInvoiceID = new DataGridViewTextBoxColumn();
            colInvoiceID.Name = "InvoiceID";
            colInvoiceID.HeaderText = "Invoice ID";
            colInvoiceID.DataPropertyName = "InvoiceID";
            colInvoiceID.Width = 120;
            colInvoiceID.Frozen = false;
            dataGridView1.Columns.Add(colInvoiceID);

           
            DataGridViewTextBoxColumn colOrderID = new DataGridViewTextBoxColumn();
            colOrderID.Name = "OrderID";
            colOrderID.HeaderText = "Order ID";
            colOrderID.DataPropertyName = "OrderID";
            colOrderID.Width = 100;
            colOrderID.Frozen = false;
            dataGridView1.Columns.Add(colOrderID);

            
            DataGridViewTextBoxColumn colCustomerName = new DataGridViewTextBoxColumn();
            colCustomerName.Name = "CustomerName";
            colCustomerName.HeaderText = "Customer Name";
            colCustomerName.DataPropertyName = "CustomerName";
            colCustomerName.Width = 150;
            colCustomerName.Frozen = false;
            dataGridView1.Columns.Add(colCustomerName);

            
            DataGridViewTextBoxColumn colCustomerPhone = new DataGridViewTextBoxColumn();
            colCustomerPhone.Name = "CustomerPhone";
            colCustomerPhone.HeaderText = "Phone";
            colCustomerPhone.DataPropertyName = "CustomerPhone";
            colCustomerPhone.Width = 100;
            colCustomerPhone.Frozen = false;
            dataGridView1.Columns.Add(colCustomerPhone);

            
            DataGridViewTextBoxColumn colTotalAmount = new DataGridViewTextBoxColumn();
            colTotalAmount.Name = "TotalAmount";
            colTotalAmount.HeaderText = "Total Amount";
            colTotalAmount.DataPropertyName = "TotalAmount";
            colTotalAmount.Width = 120;
            colTotalAmount.DefaultCellStyle.Format = "C2";
            colTotalAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colTotalAmount.Frozen = false;
            dataGridView1.Columns.Add(colTotalAmount);

            
            DataGridViewTextBoxColumn colPaymentMethod = new DataGridViewTextBoxColumn();
            colPaymentMethod.Name = "PaymentMethod";
            colPaymentMethod.HeaderText = "Payment Method";
            colPaymentMethod.DataPropertyName = "PaymentMethod";
            colPaymentMethod.Width = 120;
            colPaymentMethod.Frozen = false;
            dataGridView1.Columns.Add(colPaymentMethod);

            
            DataGridViewTextBoxColumn colCreatedBy = new DataGridViewTextBoxColumn();
            colCreatedBy.Name = "CreatedBy";
            colCreatedBy.HeaderText = "Created By";
            colCreatedBy.DataPropertyName = "CreatedBy";
            colCreatedBy.Width = 120;
            colCreatedBy.Frozen = false;
            dataGridView1.Columns.Add(colCreatedBy);

            
            DataGridViewTextBoxColumn colCreatedDate = new DataGridViewTextBoxColumn();
            colCreatedDate.Name = "CreatedDate";
            colCreatedDate.HeaderText = "Created Date";
            colCreatedDate.DataPropertyName = "CreatedDate";
            colCreatedDate.Width = 150;
            colCreatedDate.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            colCreatedDate.Frozen = false;
            dataGridView1.Columns.Add(colCreatedDate);
        }

        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dataGridView1.Columns[e.ColumnIndex].Name == "ViewPDF")
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string invoiceID = row.Cells["InvoiceID"].Value?.ToString() ?? "";
                string orderID = row.Cells["OrderID"].Value?.ToString() ?? "";
                string customerName = row.Cells["CustomerName"].Value?.ToString() ?? "";
                string customerPhone = row.Cells["CustomerPhone"].Value?.ToString() ?? "";
                decimal? totalAmount = row.Cells["TotalAmount"].Value as decimal?;
                string paymentMethod = row.Cells["PaymentMethod"].Value?.ToString() ?? "";
                string createdBy = row.Cells["CreatedBy"].Value?.ToString() ?? "";
                DateTime? createdDate = row.Cells["CreatedDate"].Value as DateTime?;

                ViewInvoicePDF(invoiceID, orderID, customerName, customerPhone,
                              totalAmount, paymentMethod, createdBy, createdDate);
            }
        }

        
        private void ViewInvoicePDF(string invoiceID, string orderID, string customerName,
                                    string customerPhone, decimal? totalAmount,
                                    string paymentMethod, string createdBy, DateTime? createdDate)
        {
            try
            {
                var orderItems = GetOrderItems(orderID);

                DataTable dt = new DataTable();
                dt.Columns.Add("Product Name", typeof(string));
                dt.Columns.Add("Quantity", typeof(int));
                dt.Columns.Add("Unit Price", typeof(decimal));
                dt.Columns.Add("Subtotal", typeof(decimal));

                decimal total = 0;
                foreach (var item in orderItems)
                {
                    dt.Rows.Add(
                        item.ProductName ?? "",
                        item.Qty ?? 0,
                        item.UnitPrice ?? 0,
                        item.SubTotal ?? 0
                    );
                    total += item.SubTotal ?? 0;
                }

                Dictionary<string, string> headerInfo = new Dictionary<string, string>
                {
                    { "Invoice ID", invoiceID },
                    { "Order ID", orderID },
                    { "Customer Name", customerName },
                    { "Customer Phone", customerPhone },
                    { "Payment Method", paymentMethod },
                    { "Created By", createdBy },
                    { "Created Date", createdDate?.ToString("yyyy-MM-dd HH:mm") ?? "Not set" }
                };

                PrintDocument pd = new PrintDocument();
                pd.DocumentName = "Invoice";
                pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
                pd.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);

                DataTable localDt = dt;
                decimal localTotal = total;

                pd.PrintPage += (sender2, e2) =>
                {
                    Graphics g = e2.Graphics;
                    Font titleFont = new Font("Arial", 18, FontStyle.Bold);
                    Font headerFont = new Font("Arial", 10, FontStyle.Bold);
                    Font bodyFont = new Font("Arial", 9);
                    Font bodyBoldFont = new Font("Arial", 9, FontStyle.Bold);

                    int currentY = 50;
                    int startX = 50;

                    g.DrawString("INVOICE", titleFont, Brushes.Black, startX, currentY);
                    currentY += 40;
                    g.DrawLine(new Pen(Color.Black, 2), startX, currentY, 750, currentY);
                    currentY += 20;

                    if (headerInfo != null)
                    {
                        foreach (var info in headerInfo)
                        {
                            g.DrawString($"{info.Key}: {info.Value}", bodyBoldFont, Brushes.Black, startX, currentY);
                            currentY += 20;
                        }
                        currentY += 15;
                    }

                    g.DrawString("Order Items:", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, startX, currentY);
                    currentY += 25;

                    if (localDt != null && localDt.Rows.Count > 0)
                    {
                        int totalColumns = localDt.Columns.Count;
                        int colWidth = 700 / totalColumns;
                        int currentX = startX;

                        for (int i = 0; i < totalColumns; i++)
                        {
                            DataColumn col = localDt.Columns[i];
                            string headerText = col.ColumnName;

                            g.FillRectangle(Brushes.LightGray, currentX, currentY, colWidth, 25);
                            g.DrawRectangle(Pens.Black, currentX, currentY, colWidth, 25);
                            g.DrawString(headerText, headerFont, Brushes.Black, currentX + 3, currentY + 5);
                            currentX += colWidth;
                        }
                        currentY += 25;

                        for (int i = 0; i < localDt.Rows.Count; i++)
                        {
                            DataRow row = localDt.Rows[i];
                            currentX = startX;

                            for (int j = 0; j < totalColumns; j++)
                            {
                                g.DrawRectangle(Pens.Black, currentX, currentY, colWidth, 22);

                                object cellValueObj = row[j];
                                string cellValue = "";

                                if (cellValueObj != null && cellValueObj != DBNull.Value)
                                {
                                    if (cellValueObj is decimal || cellValueObj is double || cellValueObj is float)
                                    {
                                        cellValue = string.Format("{0:C2}", Convert.ToDecimal(cellValueObj));
                                    }
                                    else
                                    {
                                        cellValue = cellValueObj.ToString();
                                    }
                                }

                                g.DrawString(cellValue, bodyFont, Brushes.Black, currentX + 3, currentY + 4);
                                currentX += colWidth;
                            }
                            currentY += 22;
                            if (currentY > 1100) break;
                        }

                        currentY += 10;
                        g.DrawString($"Total Amount: {localTotal:C2}", bodyBoldFont, Brushes.Black, startX, currentY);
                        currentY += 20;
                        g.DrawString($"Total Items: {localDt.Rows.Count}", bodyFont, Brushes.Black, startX, currentY);
                        currentY += 20;
                    }
                    else
                    {
                        g.DrawString("No order items found.", bodyFont, Brushes.Black, startX, currentY);
                        currentY += 30;
                    }

                    currentY += 40;
                    g.DrawString("Authorized Signature: __________________", bodyFont, Brushes.Black, startX, currentY);
                    g.DrawString("Date: __________________", bodyFont, Brushes.Black, 500, currentY);
                };

                PrintPreviewDialog ppd = new PrintPreviewDialog();
                ppd.Document = pd;
                ppd.WindowState = FormWindowState.Maximized;
                ppd.ShowDialog();
                ppd.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate PDF: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        public class OrderItemDto
        {
            public string ProductName { get; set; }
            public int? Qty { get; set; }
            public decimal? UnitPrice { get; set; }
            public decimal? SubTotal { get; set; }
        }

        
        private List<OrderItemDto> GetOrderItems(string orderId)
        {
            try
            {
                var items = (from oi in db.order_items
                             where oi.OrderID == orderId
                             select new OrderItemDto
                             {
                                 ProductName = oi.ProductName,
                                 Qty = oi.Qty,
                                 UnitPrice = oi.UnitPrice,
                                 SubTotal = oi.SubTotal
                             }).ToList();
                return items;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load order items: " + ex.Message);
                return new List<OrderItemDto>();
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void panelSidebar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelContent_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}