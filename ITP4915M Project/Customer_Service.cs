using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.Entity;
using System.IO;
using System.Drawing;

namespace ITP4915M_Project
{
    public partial class Customer_Service : Form
    {
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

        private string staffName;
        private string uploadedPhotoPath = null;

        
        private Button currentSelectedButton = null;

        public Customer_Service(string name)
        {
            InitializeComponent();
            this.staffName = name;
            label2.Text = "👤 " + name;

          
            button4.Click -= Button4_Click;
            button4.Click += Button4_Click;

           
            dataGridView1.CellClick -= DataGridView1_CellClick;
            dataGridView1.CellClick += DataGridView1_CellClick;

           
            button1.Click -= button1_Click;
            button1.Click += button1_Click;

            button2.Click -= button2_Click;
            button2.Click += button2_Click;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
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
            button4.BackColor = defaultColor;
            button4.ForeColor = defaultForeColor;
        }

        private void Customer_Service_Load(object sender, EventArgs e)
        {
           
            cmbRequestType.Items.Clear();
            cmbRequestType.Items.Add("Return");
            cmbRequestType.Items.Add("Replacement");
            cmbRequestType.Items.Add("Refund");
            cmbRequestType.SelectedIndex = 0;

           
            comboBox1.Items.Clear();
            comboBox1.Items.Add("None (Keep in CS)");
            comboBox1.Items.Add("SA (Sales)");
            comboBox1.Items.Add("LO (Logistics)");
            comboBox1.Items.Add("PR (Production)");
            comboBox1.Items.Add("FI (Finance)");
            comboBox1.SelectedIndex = 0;

            panelRequestInput.Visible = false;
            dgvHistory.Visible = true;
            panel1.Visible = false;
            lblModuleTitle.Text = "Request History Logs";

           
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvHistory.ScrollBars = ScrollBars.Both;
            dgvHistory.BackgroundColor = System.Drawing.Color.White;
            dgvHistory.BorderStyle = BorderStyle.None;
            dgvHistory.ReadOnly = true;
            dgvHistory.RowHeadersVisible = false;
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.BackgroundColor = System.Drawing.Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            LoadOrders();
            LoadHistoryData();

            
            SetActiveButton(button2);
        }

        private void LoadOrders()
        {
            var db = new projectEntities3();

            var orders = db.orders
                .Select(o => new
                {
                    o.OrderID,
                    Display = o.OrderID + " - " + o.CustomerName
                })
                .ToList();

            cmbOrderID.DataSource = orders;
            cmbOrderID.DisplayMember = "Display";
            cmbOrderID.ValueMember = "OrderID";
        }

       
        private void LoadHistoryData()
        {
            try
            {
                var db = new projectEntities3();

                var list = db.afterservicerequest
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

                dgvHistory.DataSource = list;

                
                SetStatusColumnColor(dgvHistory);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load history data logs.\n" + ex.Message);
            }
        }

        
        private void SetStatusColumnColor(DataGridView dgv)
        {
            if (dgv.Columns.Contains("Status") && dgv.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells["Status"].Value != null)
                    {
                        string status = row.Cells["Status"].Value.ToString();

                        switch (status)
                        {
                            case "Confirmed":
                                row.Cells["Status"].Style.BackColor = System.Drawing.Color.FromArgb(200, 255, 200);
                                row.Cells["Status"].Style.ForeColor = System.Drawing.Color.DarkGreen;
                                break;
                            case "Rejected":
                                row.Cells["Status"].Style.BackColor = System.Drawing.Color.FromArgb(255, 200, 200);
                                row.Cells["Status"].Style.ForeColor = System.Drawing.Color.DarkRed;
                                break;
                            case "Pending":
                                row.Cells["Status"].Style.BackColor = System.Drawing.Color.FromArgb(255, 255, 200);
                                row.Cells["Status"].Style.ForeColor = System.Drawing.Color.DarkOrange;
                                break;
                            default:
                                row.Cells["Status"].Style.BackColor = System.Drawing.Color.White;
                                row.Cells["Status"].Style.ForeColor = System.Drawing.Color.Black;
                                break;
                        }
                    }
                }
            }
        }

       
        private void Button4_Click(object sender, EventArgs e)
        {
            SetActiveButton(button4);

            try
            {
                panel1.Visible = true;
                panel1.BringToFront();

                panelRequestInput.Visible = false;
                dgvHistory.Visible = false;

                lblModuleTitle.Text = "📋 CS Pending Requests";

                var db = new projectEntities3();

                var list = db.afterservicerequest
                    .Where(r => r.ForwardTo == "CS" && r.Status == "Pending")
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

                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = null;
                dataGridView1.Refresh();

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridView1.ScrollBars = ScrollBars.Both;

                if (list.Count() == 0)
                {
                    dataGridView1.DataSource = list;
                    dataGridView1.Refresh();
                    MessageBox.Show("No pending requests for CS.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dataGridView1.DataSource = list;
                dataGridView1.Refresh();

                DataGridViewButtonColumn btnView = new DataGridViewButtonColumn();
                btnView.Name = "View";
                btnView.HeaderText = "";
                btnView.Text = "🔍 View";
                btnView.UseColumnTextForButtonValue = true;
                btnView.Width = 80;
                btnView.Frozen = true;
                btnView.FlatStyle = FlatStyle.Popup;
                btnView.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
                btnView.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                btnView.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
                dataGridView1.Columns.Insert(0, btnView);

                if (dataGridView1.Columns.Contains("RequestID"))
                    dataGridView1.Columns["RequestID"].Width = 100;
                if (dataGridView1.Columns.Contains("OrderID"))
                    dataGridView1.Columns["OrderID"].Width = 120;
                if (dataGridView1.Columns.Contains("RequestType"))
                    dataGridView1.Columns["RequestType"].Width = 120;
                if (dataGridView1.Columns.Contains("Reason"))
                    dataGridView1.Columns["Reason"].Width = 200;
                if (dataGridView1.Columns.Contains("Status"))
                    dataGridView1.Columns["Status"].Width = 100;
                if (dataGridView1.Columns.Contains("RequestDate"))
                    dataGridView1.Columns["RequestDate"].Width = 150;
                if (dataGridView1.Columns.Contains("HandledBy"))
                    dataGridView1.Columns["HandledBy"].Width = 120;
                if (dataGridView1.Columns.Contains("ForwardTo"))
                    dataGridView1.Columns["ForwardTo"].Width = 100;
                if (dataGridView1.Columns.Contains("PhotoPath"))
                    dataGridView1.Columns["PhotoPath"].Visible = false;

                dataGridView1.Refresh();
                dataGridView1.Invalidate();

                System.Diagnostics.Debug.WriteLine($"Loaded {list.Count()} CS pending requests");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load CS pending requests.\n" + ex.Message);
            }
        }

        
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (e.ColumnIndex != 0)
                return;

            if (dataGridView1.Rows.Count == 0 || e.RowIndex >= dataGridView1.Rows.Count)
                return;

            if (dataGridView1.Rows[e.RowIndex].IsNewRow)
                return;

            if (dataGridView1.Rows[e.RowIndex].Cells["RequestID"].Value == null)
                return;

            try
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                int requestID = Convert.ToInt32(row.Cells["RequestID"].Value);
                string orderID = row.Cells["OrderID"].Value?.ToString() ?? "";
                string requestType = row.Cells["RequestType"].Value?.ToString() ?? "";
                string reason = row.Cells["Reason"].Value?.ToString() ?? "";
                string status = row.Cells["Status"].Value?.ToString() ?? "";
                string photoPath = row.Cells["PhotoPath"].Value?.ToString() ?? "";

                ShowRequestDetailDialog(requestID, orderID, requestType, reason, status, photoPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error viewing request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void ShowRequestDetailDialog(int requestID, string orderID, string requestType, string reason, string status, string photoPath = null)
        {
            Form dialog = new Form();
            dialog.Text = "📋 Request Details";
            dialog.Size = new System.Drawing.Size(550, 520);
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.BackColor = System.Drawing.Color.White;
            
            dialog.AutoScroll = true;
            dialog.AutoScrollMinSize = new System.Drawing.Size(550, 600);

            
            Label lblTitle = new Label();
            lblTitle.Text = "Request Details";
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblTitle.Location = new System.Drawing.Point(20, 20);
            lblTitle.Size = new System.Drawing.Size(200, 32);
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
                        Label lblPhotoTitle = new Label();
                        lblPhotoTitle.Text = "📷 Attached Photo:";
                        lblPhotoTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                        lblPhotoTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
                        lblPhotoTitle.Location = new System.Drawing.Point(20, yPos);
                        lblPhotoTitle.Size = new System.Drawing.Size(150, 25);
                        dialog.Controls.Add(lblPhotoTitle);

                        Label lblPhotoName = new Label();
                        lblPhotoName.Text = Path.GetFileName(photoPath);
                        lblPhotoName.Font = new System.Drawing.Font("Segoe UI", 8F);
                        lblPhotoName.ForeColor = System.Drawing.Color.Gray;
                        lblPhotoName.Location = new System.Drawing.Point(180, yPos);
                        lblPhotoName.Size = new System.Drawing.Size(300, 25);
                        dialog.Controls.Add(lblPhotoName);

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
                            Form photoForm = new Form();
                            photoForm.Text = "📷 Photo Preview";
                            photoForm.Size = new System.Drawing.Size(600, 600);
                            photoForm.StartPosition = FormStartPosition.CenterParent;
                            photoForm.BackColor = System.Drawing.Color.Black;

                            PictureBox bigPic = new PictureBox();
                            bigPic.Image = Image.FromFile(fullPath);
                            bigPic.SizeMode = PictureBoxSizeMode.Zoom;
                            bigPic.Dock = DockStyle.Fill;
                            photoForm.Controls.Add(bigPic);

                            bigPic.Click += (ss, evv) => photoForm.Close();
                            photoForm.KeyDown += (ss, evv) =>
                            {
                                if (evv.KeyCode == Keys.Escape)
                                    photoForm.Close();
                            };
                            photoForm.KeyPreview = true;

                            photoForm.ShowDialog();
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
                            Form photoForm = new Form();
                            photoForm.Text = "📷 Photo Preview";
                            photoForm.Size = new System.Drawing.Size(600, 600);
                            photoForm.StartPosition = FormStartPosition.CenterParent;
                            photoForm.BackColor = System.Drawing.Color.Black;

                            PictureBox bigPic = new PictureBox();
                            bigPic.Image = Image.FromFile(fullPath);
                            bigPic.SizeMode = PictureBoxSizeMode.Zoom;
                            bigPic.Dock = DockStyle.Fill;
                            photoForm.Controls.Add(bigPic);

                            bigPic.Click += (ss, evv) => photoForm.Close();
                            photoForm.KeyDown += (ss, evv) =>
                            {
                                if (evv.KeyCode == Keys.Escape)
                                    photoForm.Close();
                            };
                            photoForm.KeyPreview = true;

                            photoForm.ShowDialog();
                        };
                        dialog.Controls.Add(btnViewPhoto);

                        yPos += 190;
                    }
                    else
                    {
                        Label lblNoPhoto = new Label();
                        lblNoPhoto.Text = "⚠️ Photo file not found";
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
                    lblNoPhoto.Text = $"⚠️ Cannot load photo: {ex.Message}";
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
                UpdateRequestStatus(requestID, "Confirmed");
                dialog.Close();
                Button4_Click(null, null);
                LoadHistoryData();
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
                UpdateRequestStatus(requestID, "Rejected");
                dialog.Close();
                Button4_Click(null, null);
                LoadHistoryData();
            };
            dialog.Controls.Add(btnReject);

            Panel spacer = new Panel();
            spacer.Size = new System.Drawing.Size(1, 30);
            spacer.Location = new System.Drawing.Point(0, buttonYPos + 50);
            dialog.Controls.Add(spacer);

            dialog.ShowDialog();
        }

        private void UpdateRequestStatus(int requestID, string newStatus)
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

        private void button1_Click(object sender, EventArgs e)
        {
            SetActiveButton(button1);

            panelRequestInput.Visible = true;
            dgvHistory.Visible = false;
            panel1.Visible = false;
            lblModuleTitle.Text = "Create After-Service Request";

            comboBox1.SelectedIndex = 0;
            uploadedPhotoPath = null;
            btnUploadPhoto.Text = "📷 Upload Photo";
            btnUploadPhoto.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetActiveButton(button2);

            panelRequestInput.Visible = false;
            dgvHistory.Visible = true;
            panel1.Visible = false;
            lblModuleTitle.Text = "Request History Logs";
            LoadHistoryData();
        }

        
        private void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Select Photo to Upload";
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        string fileName = Path.GetFileName(filePath);

                        uploadedPhotoPath = filePath;

                        btnUploadPhoto.Text = "✅ Photo Selected";
                        btnUploadPhoto.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);

                        MessageBox.Show($"Photo selected: {fileName}\n\nClick Submit to send your request with this photo attached.",
                                        "Photo Uploaded",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to upload photo: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string orderID = cmbOrderID.SelectedValue?.ToString();
            string reqType = cmbRequestType.SelectedItem?.ToString();
            string reason = txtReason.Text.Trim();

            string selectedForward = comboBox1.SelectedItem?.ToString() ?? "None (Keep in CS)";
            string forwardDept = GetForwardDepartment(selectedForward);

            if (string.IsNullOrEmpty(orderID) || string.IsNullOrEmpty(reason))
            {
                MessageBox.Show("Please select Order and enter Reason ❌");
                return;
            }

            try
            {
                var db = new projectEntities3();

                
                string savedPhotoPath = null;
                if (!string.IsNullOrEmpty(uploadedPhotoPath))
                {
                    try
                    {
                        string destFolder = Path.Combine(Application.StartupPath, "RequestPhotos");
                        if (!Directory.Exists(destFolder))
                        {
                            Directory.CreateDirectory(destFolder);
                        }

                        string fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Path.GetFileName(uploadedPhotoPath)}";
                        string destPath = Path.Combine(destFolder, fileName);
                        File.Copy(uploadedPhotoPath, destPath, true);

                      
                        savedPhotoPath = $"RequestPhotos\\{fileName}";
                    }
                    catch (Exception photoEx)
                    {
                        MessageBox.Show($"Failed to save photo: {photoEx.Message}\nRequest will still be submitted.",
                                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                afterservicerequest newRequest = new afterservicerequest
                {
                    OrderID = orderID,
                    RequestType = reqType,
                    Reason = reason,
                    Status = "Pending",
                    RequestDate = DateTime.Now,
                    HandledBy = staffName,
                    ForwardTo = forwardDept,
                    PhotoPath = savedPhotoPath 
                };

                db.afterservicerequest.Add(newRequest);
                db.SaveChanges();

                string photoMessage = string.IsNullOrEmpty(savedPhotoPath) ? "" : $"\n📷 Photo attached: {Path.GetFileName(savedPhotoPath)}";

                MessageBox.Show($"Request submitted ✅\nForward to: {forwardDept}{photoMessage}");

                txtReason.Clear();
                cmbRequestType.SelectedIndex = 0;
                comboBox1.SelectedIndex = 0;
                uploadedPhotoPath = null;
                btnUploadPhoto.Text = "📷 Upload Photo";
                btnUploadPhoto.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);

                button2_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ❌: " + ex.Message);
            }
        }

        private string GetForwardDepartment(string selectedItem)
        {
            switch (selectedItem)
            {
                case "SA (Sales)":
                    return "SA";
                case "LO (Logistics)":
                    return "LO";
                case "PR (Production)":
                    return "PR";
                case "FI (Finance)":
                    return "FI";
                case "None (Keep in CS)":
                default:
                    return "CS";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtReason.Clear();
            comboBox1.SelectedIndex = 0;
            uploadedPhotoPath = null;

            btnUploadPhoto.Text = "📷 Upload Photo";
            btnUploadPhoto.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);

            button2_Click(sender, e);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void panelLeft_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblModuleTitle_Click(object sender, EventArgs e)
        {

        }
    }
}