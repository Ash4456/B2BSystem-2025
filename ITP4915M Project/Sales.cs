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
    public partial class Sales : Form
    {
        List<product> productList = new List<product>();

        private class CartItem
        {
            public product Product { get; set; }
            public int Quantity { get; set; }
        }

        private List<CartItem> cartItems = new List<CartItem>();
        private Panel cartItemsContainer;
        private Label cartTotalLabel;
        private bool isInitialized = false;
        private bool productsLoaded = false;

        private DataGridView dgvSAPending;
        private Panel panelSAPending;

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

        public Sales(string name)
        {
            InitializeComponent();
            label1.Text = name;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            InitializeSAPendingPanel();

            button5.Click -= Button5_Click;
            button5.Click += Button5_Click;

            buttonOrderHistory.Click -= buttonOrderHistory_Click;
            buttonOrderHistory.Click += buttonOrderHistory_Click;

            buttonOrderHistorySearch.Click -= buttonOrderHistorySearch_Click;
            buttonOrderHistorySearch.Click += buttonOrderHistorySearch_Click;

            textBoxOrderHistorySearch.TextChanged -= textBoxOrderHistorySearch_TextChanged;
            textBoxOrderHistorySearch.TextChanged += textBoxOrderHistorySearch_TextChanged;

            dataGridView2.CellClick -= dataGridView2_CellClick;
            dataGridView2.CellClick += dataGridView2_CellClick;

            button2.Click -= button2_Click;
            button2.Click += button2_Click;

            button3.Click -= button3_Click;
            button3.Click += button3_Click;

            button4.Click -= button4_Click;
            button4.Click += button4_Click;

            button1.Click -= button1_Click_1;
            button1.Click += button1_Click_1;

            button6.Click -= button6_Click;
            button6.Click += button6_Click;

            button7.Click -= button7_Click;
            button7.Click += button7_Click;

            button9.Click -= button9_Click;
            button9.Click += button9_Click;

            buttonBackToSelect.Click -= buttonBackToSelect_Click;
            buttonBackToSelect.Click += buttonBackToSelect_Click;
        }

        private void SetDefaultOrderDates()
        {
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now.AddDays(10);
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
            Color defaultColor = System.Drawing.Color.FromArgb(52, 73, 94);
            Color defaultForeColor = System.Drawing.Color.White;

            button2.BackColor = defaultColor;
            button2.ForeColor = defaultForeColor;

            button3.BackColor = defaultColor;
            button3.ForeColor = defaultForeColor;

            button4.BackColor = defaultColor;
            button4.ForeColor = defaultForeColor;

            button5.BackColor = defaultColor;
            button5.ForeColor = defaultForeColor;

            buttonOrderHistory.BackColor = defaultColor;
            buttonOrderHistory.ForeColor = defaultForeColor;
        }

        private void InitializeSAPendingPanel()
        {
            panelSAPending = new Panel();
            panelSAPending.BackColor = System.Drawing.Color.White;
            panelSAPending.BorderStyle = BorderStyle.FixedSingle;
            panelSAPending.Location = new System.Drawing.Point(127, 69);
            panelSAPending.Name = "panelSAPending";
            panelSAPending.Size = new System.Drawing.Size(687, 347);
            panelSAPending.TabIndex = 30;
            panelSAPending.Visible = false;

            dgvSAPending = new DataGridView();
            dgvSAPending.Name = "dgvSAPending";
            dgvSAPending.Location = new System.Drawing.Point(10, 10);
            dgvSAPending.Size = new System.Drawing.Size(665, 325);
            dgvSAPending.TabIndex = 0;
            dgvSAPending.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            dgvSAPending.AllowUserToAddRows = false;
            dgvSAPending.AllowUserToDeleteRows = false;
            dgvSAPending.BackgroundColor = System.Drawing.Color.White;
            dgvSAPending.BorderStyle = BorderStyle.None;
            dgvSAPending.ReadOnly = true;
            dgvSAPending.RowHeadersVisible = false;
            dgvSAPending.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSAPending.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSAPending.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dgvSAPending.CellClick -= DgvSAPending_CellClick;
            dgvSAPending.CellClick += DgvSAPending_CellClick;

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            headerStyle.ForeColor = System.Drawing.Color.White;
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            headerStyle.WrapMode = DataGridViewTriState.True;
            dgvSAPending.ColumnHeadersDefaultCellStyle = headerStyle;

            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            cellStyle.BackColor = System.Drawing.SystemColors.Window;
            cellStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            cellStyle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            cellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            cellStyle.SelectionForeColor = System.Drawing.Color.White;
            cellStyle.WrapMode = DataGridViewTriState.False;
            dgvSAPending.DefaultCellStyle = cellStyle;

            DataGridViewCellStyle altCellStyle = new DataGridViewCellStyle();
            altCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            altCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            altCellStyle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            altCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            altCellStyle.SelectionForeColor = System.Drawing.Color.White;
            altCellStyle.WrapMode = DataGridViewTriState.False;
            dgvSAPending.AlternatingRowsDefaultCellStyle = altCellStyle;

            dgvSAPending.EnableHeadersVisualStyles = false;
            dgvSAPending.GridColor = System.Drawing.Color.FromArgb(230, 230, 230);

            panelSAPending.Controls.Add(dgvSAPending);
            this.Controls.Add(panelSAPending);
        }

        private void LoadSAPendingRequests()
        {
            try
            {
                var db = new projectEntities3();

                var list = db.afterservicerequest
                    .Where(r => r.ForwardTo == "SA" && r.Status == "Pending")
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

                dgvSAPending.Columns.Clear();
                dgvSAPending.DataSource = null;
                dgvSAPending.Refresh();

                if (list.Count == 0)
                {
                    dgvSAPending.DataSource = list;
                    dgvSAPending.Refresh();
                    MessageBox.Show("No pending requests for SA.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dgvSAPending.DataSource = list;
                dgvSAPending.Refresh();

                DataGridViewButtonColumn btnView = new DataGridViewButtonColumn();
                btnView.Name = "View";
                btnView.HeaderText = "";
                btnView.Text = "View";
                btnView.UseColumnTextForButtonValue = true;
                btnView.Width = 80;
                btnView.FlatStyle = FlatStyle.Popup;
                btnView.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
                btnView.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                btnView.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
                dgvSAPending.Columns.Insert(0, btnView);

                if (dgvSAPending.Columns.Contains("PhotoPath"))
                {
                    dgvSAPending.Columns["PhotoPath"].Visible = false;
                }

                dgvSAPending.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvSAPending.Refresh();
                dgvSAPending.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load SA pending requests.\n" + ex.Message);
            }
        }

        private void DgvSAPending_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (e.ColumnIndex != 0)
                return;

            if (dgvSAPending.Rows.Count == 0 || e.RowIndex >= dgvSAPending.Rows.Count)
                return;

            if (dgvSAPending.Rows[e.RowIndex].IsNewRow)
                return;

            if (dgvSAPending.Rows[e.RowIndex].Cells["RequestID"].Value == null)
                return;

            try
            {
                DataGridViewRow row = dgvSAPending.Rows[e.RowIndex];
                int requestID = Convert.ToInt32(row.Cells["RequestID"].Value);
                string orderID = row.Cells["OrderID"].Value?.ToString() ?? "";
                string requestType = row.Cells["RequestType"].Value?.ToString() ?? "";
                string reason = row.Cells["Reason"].Value?.ToString() ?? "";
                string status = row.Cells["Status"].Value?.ToString() ?? "";
                string photoPath = row.Cells["PhotoPath"].Value?.ToString() ?? "";

                ShowSAPendingDetailDialog(requestID, orderID, requestType, reason, status, photoPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error viewing request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowSAPendingDetailDialog(int requestID, string orderID, string requestType, string reason, string status, string photoPath = null)
        {
            Form dialog = new Form();
            dialog.Text = "Request Details (SA)";
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
                        string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".tif" };
                        string extension = Path.GetExtension(photoPath).ToLower();
                        bool isImage = imageExtensions.Contains(extension);

                        Label lblPhotoTitle = new Label();
                        lblPhotoTitle.Text = isImage ? "Attached Photo:" : "Attached File:";
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
                            btnViewPhoto.Text = "View Full Size";
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
                            btnOpenFile.Text = "Open File";
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
                        lblNoPhoto.Text = "File not found";
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
                    lblNoPhoto.Text = $"Cannot load file: {ex.Message}";
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
            btnConfirm.Text = "Confirm";
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
                UpdateSARequestStatus(requestID, "Confirmed");
                dialog.Close();
                LoadSAPendingRequests();
            };
            dialog.Controls.Add(btnConfirm);

            Button btnReject = new Button();
            btnReject.Text = "Reject";
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
                UpdateSARequestStatus(requestID, "Rejected");
                dialog.Close();
                LoadSAPendingRequests();
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
                previewForm.Text = $"{fileName}";
                previewForm.Size = new System.Drawing.Size(600, 600);
                previewForm.StartPosition = FormStartPosition.CenterParent;
                previewForm.BackColor = System.Drawing.Color.Black;

                Label lblFileName = new Label();
                lblFileName.Text = $"{fileName}";
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

        private void UpdateSARequestStatus(int requestID, string newStatus)
        {
            try
            {
                var db = new projectEntities3();
                var request = db.afterservicerequest.FirstOrDefault(r => r.RequestID == requestID);

                if (request != null)
                {
                    request.Status = newStatus;
                    db.SaveChanges();
                    MessageBox.Show($"Request #{requestID} has been {newStatus}");
                }
                else
                {
                    MessageBox.Show("Request not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update request status: " + ex.Message);
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            SetActiveButton(button5);

            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panelOrderHistory.Visible = false;

            panelSAPending.Visible = true;
            panelSAPending.BringToFront();

            LoadSAPendingRequests();
        }

        private void Sales_Load(object sender, EventArgs e)
        {
            var db = new projectEntities3();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.Columns.Clear();

            var list = db.product
                .Select(x => new
                {
                    x.ProductID,
                    x.ProductName,
                    UnitPrice = x.unitPrice,
                    Stock = x.Qty
                })
                .ToList();

            dataGridView1.DataSource = list;

            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panelOrderHistory.Visible = false;
            panelSAPending.Visible = false;

            AddOrderRow();

            SetDefaultOrderDates();

            if (!isInitialized)
            {
                LoadProductsToDisplay();
                LoadCartPanel();
                isInitialized = true;
                productsLoaded = true;
            }
            panelLeft.BringToFront();

            SetActiveButton(button2);
        }

        private void LoadProductsToDisplay()
        {
            flowLayoutPanelProducts.Controls.Clear();
            flowLayoutPanelProducts.SuspendLayout();

            using (var db = new projectEntities3())
            {
                var products = db.product.ToList();

                flowLayoutPanelProducts.FlowDirection = FlowDirection.LeftToRight;
                flowLayoutPanelProducts.WrapContents = true;
                flowLayoutPanelProducts.AutoSize = false;
                flowLayoutPanelProducts.Size = new Size(445, 1315);

                foreach (var product in products)
                {
                    Panel productPanel = CreateProductPanel(product);
                    flowLayoutPanelProducts.Controls.Add(productPanel);
                }
            }

            flowLayoutPanelProducts.ResumeLayout();
            flowLayoutPanelProducts.PerformLayout();
        }

        private Panel CreateProductPanel(product product)
        {
            Panel productPanel = new Panel();
            productPanel.Width = 210;
            productPanel.Height = 255;
            productPanel.BorderStyle = BorderStyle.FixedSingle;
            productPanel.Tag = product;
            productPanel.BackColor = Color.White;
            productPanel.Margin = new Padding(3);

            PictureBox pic = new PictureBox();
            pic.Image = GetProductImage(product.ProductID, product.ProductName);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Size = new Size(180, 110);
            pic.Location = new Point(20, 8);
            productPanel.Controls.Add(pic);

            Label lblName = new Label();
            lblName.Text = product.ProductName;
            lblName.Font = new Font("Arial", 8, FontStyle.Bold);
            lblName.TextAlign = ContentAlignment.MiddleCenter;
            lblName.Size = new Size(190, 35);
            lblName.Location = new Point(15, 125);
            productPanel.Controls.Add(lblName);

            Label lblPrice = new Label();
            lblPrice.Text = $"${product.unitPrice:F0}";
            lblPrice.ForeColor = Color.Red;
            lblPrice.Font = new Font("Arial", 9, FontStyle.Bold);
            lblPrice.TextAlign = ContentAlignment.MiddleCenter;
            lblPrice.Size = new Size(190, 22);
            lblPrice.Location = new Point(15, 165);
            productPanel.Controls.Add(lblPrice);

            int stockQty = product.Qty;

            Label lblQty = new Label();
            lblQty.Text = "Qty:";
            lblQty.Font = new Font("Arial", 8);
            lblQty.Size = new Size(35, 25);
            lblQty.Location = new Point(15, 192);
            productPanel.Controls.Add(lblQty);

            NumericUpDown numQty = new NumericUpDown();
            numQty.Minimum = 1;

            if (stockQty > 0)
            {
                numQty.Maximum = stockQty;
                numQty.Value = 1;
                numQty.Enabled = true;
            }
            else
            {
                numQty.Maximum = 1;
                numQty.Value = 1;
                numQty.Enabled = false;
            }

            numQty.Size = new Size(50, 22);
            numQty.Location = new Point(55, 190);
            productPanel.Controls.Add(numQty);

            Label lblStock = new Label();
            if (stockQty > 0)
            {
                lblStock.Text = $"S:{stockQty}";
                lblStock.ForeColor = stockQty < 10 ? Color.Red : Color.Green;
            }
            else
            {
                lblStock.Text = "SOLD OUT";
                lblStock.ForeColor = Color.Red;
            }
            lblStock.Font = new Font("Arial", 7, FontStyle.Bold);
            lblStock.Size = new Size(50, 22);
            lblStock.Location = new Point(110, 192);
            productPanel.Controls.Add(lblStock);

            if (stockQty > 0)
            {
                Button btnAddToCart = new Button();
                btnAddToCart.Text = "+ Add";
                btnAddToCart.BackColor = Color.LightBlue;
                btnAddToCart.Font = new Font("Arial", 8, FontStyle.Bold);
                btnAddToCart.Size = new Size(80, 28);
                btnAddToCart.Location = new Point(65, 218);
                btnAddToCart.Tag = product;

                var capturedProduct = product;
                var capturedNumQty = numQty;

                btnAddToCart.Click += (s, ev) =>
                {
                    using (var db = new projectEntities3())
                    {
                        var currentProduct = db.product.FirstOrDefault(p => p.ProductID == capturedProduct.ProductID);
                        if (currentProduct == null || currentProduct.Qty <= 0)
                        {
                            MessageBox.Show($"Product {capturedProduct.ProductName} is out of stock, cannot add to cart",
                                           "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        int qtyToAdd = (int)capturedNumQty.Value;
                        if (qtyToAdd > currentProduct.Qty)
                        {
                            MessageBox.Show($"Only {currentProduct.Qty} units available for {capturedProduct.ProductName}",
                                           "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            capturedNumQty.Value = currentProduct.Qty;
                            return;
                        }

                        AddToCart(capturedProduct, qtyToAdd);
                    }
                };
                productPanel.Controls.Add(btnAddToCart);
            }
            else
            {
                Label lblSold = new Label();
                lblSold.Text = "SOLD";
                lblSold.Font = new Font("Arial", 12, FontStyle.Bold);
                lblSold.ForeColor = Color.White;
                lblSold.BackColor = Color.Red;
                lblSold.TextAlign = ContentAlignment.MiddleCenter;
                lblSold.Size = new Size(80, 28);
                lblSold.Location = new Point(65, 218);
                lblSold.BorderStyle = BorderStyle.FixedSingle;
                productPanel.Controls.Add(lblSold);

                productPanel.BackColor = Color.FromArgb(240, 240, 240);
            }

            return productPanel;
        }

        private void LoadCartPanel()
        {
            panelCart.Controls.Clear();

            Label lblCartTitle = new Label();
            lblCartTitle.Text = "CART";
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
            cartTotalLabel.Text = "Total: $0";
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
            btnCheckout.Text = "Checkout";
            btnCheckout.Size = new Size(90, 25);
            btnCheckout.Location = new Point(90, 30);
            btnCheckout.Click += (s, ev) => Checkout();

            bottomPanel.Controls.Add(cartTotalLabel);
            bottomPanel.Controls.Add(btnClearCart);
            bottomPanel.Controls.Add(btnCheckout);
            panelCart.Controls.Add(bottomPanel);

            FlowLayoutPanel cartFlowPanel = new FlowLayoutPanel();
            cartFlowPanel.Location = new Point(5, 35);
            cartFlowPanel.Size = new Size(170, 180);
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

        private void AddToCart(product product, int quantity)
        {
            if (product.Qty <= 0)
            {
                MessageBox.Show($"Product {product.ProductName} is out of stock, cannot add to cart", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existingItem = cartItems.FirstOrDefault(c => c.Product.ProductID == product.ProductID);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                if (existingItem.Quantity > product.Qty)
                {
                    existingItem.Quantity = product.Qty;
                    MessageBox.Show($"Insufficient stock for {product.ProductName}. Maximum available: {product.Qty}", "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                cartItems.Add(new CartItem { Product = product, Quantity = quantity });
            }

            UpdateCartDisplay();
        }

        private void UpdateCartDisplay()
        {
            if (cartItemsContainer == null) return;

            cartItemsContainer.Controls.Clear();
            decimal total = 0;

            foreach (var item in cartItems)
            {
                Panel itemPanel = new Panel();
                itemPanel.Size = new Size(155, 55);
                itemPanel.Margin = new Padding(3, 3, 3, 3);
                itemPanel.BorderStyle = BorderStyle.FixedSingle;
                itemPanel.BackColor = Color.White;

                Label lblName = new Label();
                lblName.Text = item.Product.ProductName;
                lblName.Font = new Font("Arial", 7, FontStyle.Bold);
                lblName.AutoSize = true;
                lblName.MaximumSize = new Size(110, 0);
                lblName.Location = new Point(3, 2);
                itemPanel.Controls.Add(lblName);

                decimal subtotal = item.Product.unitPrice * item.Quantity;
                Label lblInfo = new Label();
                lblInfo.Text = $"{item.Quantity} x ${item.Product.unitPrice:F0}";
                lblInfo.Font = new Font("Arial", 6);
                lblInfo.Size = new Size(90, 14);
                lblInfo.Location = new Point(3, 25);
                itemPanel.Controls.Add(lblInfo);

                Label lblSubtotal = new Label();
                lblSubtotal.Text = $"${subtotal:F0}";
                lblSubtotal.Font = new Font("Arial", 7, FontStyle.Bold);
                lblSubtotal.ForeColor = Color.Red;
                lblSubtotal.Size = new Size(50, 14);
                lblSubtotal.Location = new Point(95, 22);
                lblSubtotal.TextAlign = ContentAlignment.MiddleRight;
                itemPanel.Controls.Add(lblSubtotal);

                Button btnRemove = new Button();
                btnRemove.Text = "X";
                btnRemove.Size = new Size(18, 18);
                btnRemove.Location = new Point(135, 35);
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
                total += subtotal;
            }

            if (cartTotalLabel != null)
            {
                cartTotalLabel.Text = $"Total: ${total:F0}";
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
                MessageBox.Show("Please add at least one product to cart first!", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GoToCreateOrderWithCart();
        }

        private void GoToCreateOrderWithCart()
        {
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
            panelOrderHistory.Visible = false;

            SetDefaultOrderDates();

            flowLayoutPanel1.Controls.Clear();

            if (cartItems.Count > 0)
            {
                foreach (var cartItem in cartItems)
                {
                    AddOrderRowWithProduct(cartItem.Product, cartItem.Quantity);
                }
            }
            else
            {
                AddOrderRow();
            }

            CalculateTotal();

            cartItems.Clear();
            UpdateCartDisplay();
        }

        private void AddOrderRowWithProduct(product selectedProduct, int quantity)
        {
            var db = new projectEntities3();
            var productList = db.product.ToList();

            Panel row = new Panel();
            row.Width = 380;
            row.Height = 35;

            ComboBox cb = new ComboBox();
            cb.DataSource = productList;
            cb.DisplayMember = "ProductName";
            cb.Width = 150;
            cb.Location = new Point(0, 5);
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.Enabled = false;

            NumericUpDown num = new NumericUpDown();
            num.Minimum = 1;
            num.Maximum = Math.Max(1, selectedProduct.Qty);
            num.Value = Math.Min(quantity, Math.Max(1, selectedProduct.Qty));
            num.Location = new Point(160, 5);
            num.Width = 60;
            num.Enabled = false;

            num.MouseWheel += (s, ev) =>
            {
                ((HandledMouseEventArgs)ev).Handled = true;
            };

            Label lblUnitPrice = new Label();
            lblUnitPrice.Text = $"${selectedProduct.unitPrice:F0}";
            lblUnitPrice.Font = new Font("Arial", 8, FontStyle.Bold);
            lblUnitPrice.ForeColor = Color.Blue;
            lblUnitPrice.Size = new Size(60, 20);
            lblUnitPrice.Location = new Point(230, 8);
            lblUnitPrice.TextAlign = ContentAlignment.MiddleLeft;

            Label lblSubtotal = new Label();
            decimal initialSubtotal = selectedProduct.unitPrice * num.Value;
            lblSubtotal.Text = $"${initialSubtotal:F0}";
            lblSubtotal.Font = new Font("Arial", 8, FontStyle.Bold);
            lblSubtotal.ForeColor = Color.Red;
            lblSubtotal.Size = new Size(70, 20);
            lblSubtotal.Location = new Point(300, 8);
            lblSubtotal.TextAlign = ContentAlignment.MiddleLeft;

            row.Controls.Add(cb);
            row.Controls.Add(num);
            row.Controls.Add(lblUnitPrice);
            row.Controls.Add(lblSubtotal);

            flowLayoutPanel1.Controls.Add(row);

            for (int i = 0; i < cb.Items.Count; i++)
            {
                product p = cb.Items[i] as product;
                if (p != null && p.ProductID == selectedProduct.ProductID)
                {
                    cb.SelectedIndex = i;
                    break;
                }
            }
        }

        private void AddOrderRow()
        {
            var db = new projectEntities3();
            var productList = db.product.ToList();

            productList.Insert(0, new product
            {
                ProductID = "-1",
                ProductName = "Select Product"
            });

            Panel row = new Panel();
            row.Width = 650;
            row.Height = 35;

            ComboBox cb = new ComboBox();
            cb.DataSource = productList;
            cb.DisplayMember = "ProductName";
            cb.SelectedIndex = -1;
            cb.Width = 150;
            cb.Location = new Point(0, 5);
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.Enabled = false;

            NumericUpDown num = new NumericUpDown();
            num.Minimum = 1;
            num.Maximum = 100;
            num.Value = 1;
            num.Location = new Point(160, 5);
            num.Width = 60;
            num.Enabled = false;

            Label lblUnitPrice = new Label();
            lblUnitPrice.Text = "$0";
            lblUnitPrice.Font = new Font("Arial", 8, FontStyle.Bold);
            lblUnitPrice.ForeColor = Color.Blue;
            lblUnitPrice.Size = new Size(60, 20);
            lblUnitPrice.Location = new Point(230, 8);

            Label lblSubtotal = new Label();
            lblSubtotal.Text = "$0";
            lblSubtotal.Font = new Font("Arial", 8, FontStyle.Bold);
            lblSubtotal.ForeColor = Color.Red;
            lblSubtotal.Size = new Size(70, 20);
            lblSubtotal.Location = new Point(300, 8);

            row.Controls.Add(cb);
            row.Controls.Add(num);
            row.Controls.Add(lblUnitPrice);
            row.Controls.Add(lblSubtotal);

            flowLayoutPanel1.Controls.Add(row);
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            decimal total = 0;
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                Panel row = ctrl as Panel;
                if (row == null) continue;

                ComboBox cb = row.Controls.OfType<ComboBox>().FirstOrDefault();
                NumericUpDown num = row.Controls.OfType<NumericUpDown>().FirstOrDefault();
                if (cb == null || num == null) continue;

                var selected = cb.SelectedItem as product;
                if (selected != null && selected.ProductID != null)
                {
                    total += selected.unitPrice * (int)num.Value;
                }
            }
            label10.Text = "$" + total.ToString("0.00");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var db = new projectEntities3();
            string keyword = textBox1.Text;
            dataGridView1.Columns.Clear();

            var result = db.product
                .Where(x => string.IsNullOrEmpty(keyword) || x.ProductName.Contains(keyword))
                .Select(x => new
                {
                    x.ProductID,
                    x.ProductName,
                    UnitPrice = x.unitPrice,
                    Stock = x.Qty
                })
                .ToList();

            dataGridView1.DataSource = result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetActiveButton(button2);

            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panelOrderHistory.Visible = false;
            panelSAPending.Visible = false;

            SetDefaultOrderDates();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetActiveButton(button3);

            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = true;
            panelOrderHistory.Visible = false;
            panelSAPending.Visible = false;

            LoadProductsToDisplay();
            productsLoaded = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetActiveButton(button4);

            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            panel4.Visible = false;
            panelOrderHistory.Visible = false;
            panelSAPending.Visible = false;
            LoadOrders();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveOrder();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = true;
            panelOrderHistory.Visible = false;
            panelSAPending.Visible = false;

            SetDefaultOrderDates();

            LoadProductsToDisplay();
            productsLoaded = true;

            flowLayoutPanel1.Controls.Clear();
            AddOrderRow();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            label10.Text = "$0.00";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Items can only be added from the product selection page.\nPlease go back to select products.",
                            "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            button4_Click(null, null);
        }

        private void buttonBackToSelect_Click(object sender, EventArgs e)
        {
            AddProductsToCartFromOrder();

            panel2.Visible = false;
            panel4.Visible = true;
            panel1.Visible = false;
            panel3.Visible = false;
            panelOrderHistory.Visible = false;
            panelSAPending.Visible = false;

            SetDefaultOrderDates();

            LoadProductsToDisplay();
            productsLoaded = true;

            UpdateCartDisplay();
        }

        private List<CartItem> GetProductsFromOrder()
        {
            List<CartItem> items = new List<CartItem>();

            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                Panel row = ctrl as Panel;
                if (row == null) continue;

                ComboBox cb = row.Controls.OfType<ComboBox>().FirstOrDefault();
                NumericUpDown num = row.Controls.OfType<NumericUpDown>().FirstOrDefault();

                if (cb != null && cb.SelectedItem != null && num != null)
                {
                    var product = cb.SelectedItem as product;
                    if (product != null && product.ProductID != null && product.ProductID != "-1")
                    {
                        items.Add(new CartItem
                        {
                            Product = product,
                            Quantity = (int)num.Value
                        });
                    }
                }
            }

            return items;
        }

        private void AddProductsToCartFromOrder()
        {
            var products = GetProductsFromOrder();

            cartItems.Clear();

            foreach (var item in products)
            {
                cartItems.Add(new CartItem
                {
                    Product = item.Product,
                    Quantity = item.Quantity
                });
            }

            UpdateCartDisplay();
        }

        private void SaveOrder()
        {
            string name = textBox2.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter Customer Name");
                return;
            }

            string phone = textBox3.Text;
            if (string.IsNullOrWhiteSpace(phone) || phone.Length != 8 || !phone.All(char.IsDigit))
            {
                MessageBox.Show("Invalid Phone Number");
                return;
            }

            string email = textBox4.Text;
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                MessageBox.Show("Invalid Email");
                return;
            }

            string address = textBox5.Text;
            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Please enter Address");
                return;
            }


            DateTime expectedDelivery = DateTime.Now.AddDays(10);

            if (dateTimePicker2.Value.Date < expectedDelivery.Date)
            {
                MessageBox.Show(
                    "Delivery date must be at least 10 days from order date.\n\n" +
                    "Order date: " + DateTime.Now.ToShortDateString() + "\n" +
                    "Minimum delivery date: " + expectedDelivery.ToShortDateString() + "\n" +
                    "Selected delivery date: " + dateTimePicker2.Value.ToShortDateString(),
                    "Invalid Delivery Date",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                dateTimePicker2.Value = expectedDelivery;
                return;
            }


            var db = new projectEntities3();

            if (flowLayoutPanel1.Controls.Count == 0)
            {
                MessageBox.Show("Please add product");
                return;
            }

            decimal total = 0;

            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                Panel row = ctrl as Panel;
                ComboBox cb = row.Controls.OfType<ComboBox>().FirstOrDefault();
                NumericUpDown num = row.Controls.OfType<NumericUpDown>().FirstOrDefault();
                var product = cb?.SelectedItem as product;

                if (product == null || product.ProductID == null)
                {
                    MessageBox.Show("Please select product");
                    return;
                }

                var dbProduct = db.product.FirstOrDefault(p => p.ProductID == product.ProductID);
                if (dbProduct == null || dbProduct.Qty < num.Value)
                {
                    MessageBox.Show($"Insufficient stock for {product.ProductName}");
                    return;
                }
            }

            orders order = new orders
            {
                OrderID = "OR" + Guid.NewGuid().ToString().Substring(0, 5),
                CustomerName = name,
                CustomerPhone = phone,
                CustomerEmail = email,
                DeliveryAddress = address,
                OrderDate = DateTime.Now,
                DeliveryDate = dateTimePicker2.Value,
                Status = "Pending",
                CreatedBy = label1.Text
            };

            db.orders.Add(order);

            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                Panel row = ctrl as Panel;
                ComboBox cb = row.Controls.OfType<ComboBox>().FirstOrDefault();
                NumericUpDown num = row.Controls.OfType<NumericUpDown>().FirstOrDefault();
                var product = cb.SelectedItem as product;

                decimal subtotal = product.unitPrice * (int)num.Value;
                total += subtotal;

                order_items item = new order_items
                {
                    OrderID = order.OrderID,
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Qty = (int)num.Value,
                    UnitPrice = product.unitPrice,
                    SubTotal = subtotal
                };

                db.order_items.Add(item);

                var dbProduct = db.product.FirstOrDefault(p => p.ProductID == product.ProductID);
                if (dbProduct != null)
                {
                    dbProduct.Qty -= (int)num.Value;
                    if (dbProduct.Qty < 0) dbProduct.Qty = 0;
                }
            }

            order.TotalAmount = total;
            db.SaveChanges();

            MessageBox.Show("Order Saved");

            flowLayoutPanel1.Controls.Clear();
            AddOrderRow();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();

            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            panel4.Visible = false;
            panelOrderHistory.Visible = false;
            panelSAPending.Visible = false;
            LoadOrders();
        }

        private void LoadOrders()
        {
            var db = new projectEntities3();
            string keyword = textBox6.Text;

            var result = db.orders
                .Where(o =>
                    o.Status != "Cancel" &&
                    (string.IsNullOrEmpty(keyword) ||
                     o.OrderID.Contains(keyword) ||
                     o.CustomerName.Contains(keyword)))
                .Select(o => new
                {
                    o.OrderID,
                    o.CustomerName,
                    o.CustomerPhone,
                    o.TotalAmount,
                    o.OrderDate
                })
                .ToList();

            dataGridView2.DataSource = result;
            AddActionButtons();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string colName = dataGridView2.Columns[e.ColumnIndex].Name;
            string orderID = dataGridView2.Rows[e.RowIndex].Cells["OrderID"].Value.ToString();

            if (colName == "View")
            {
                new OrderDetails(orderID).Show();
            }

            if (colName == "Cancel")
            {
                var result = MessageBox.Show(
                    "Are you sure you want to cancel this order?",
                    "Confirm Cancel",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    CancelOrder(orderID);
                    LoadOrders();
                }
            }
        }

        private void CancelOrder(string orderID)
        {
            try
            {
                var db = new projectEntities3();
                var order = db.orders.FirstOrDefault(o => o.OrderID == orderID);

                if (order == null) return;

                if (order.Status == "Cancel")
                {
                    MessageBox.Show("Order already cancelled");
                    return;
                }

                var items = db.order_items.Where(i => i.OrderID == orderID).ToList();

                int restoredCount = 0;
                foreach (var item in items)
                {
                    var product = db.product.FirstOrDefault(p => p.ProductID == item.ProductID);
                    if (product != null)
                    {
                        int qtyToRestore = item.Qty ?? 0;
                        product.Qty += qtyToRestore;
                        restoredCount += qtyToRestore;
                    }
                }

                order.Status = "Cancel";
                db.SaveChanges();

                MessageBox.Show($"Order cancelled\n\nStock restored: {restoredCount} units");

                if (panel4.Visible)
                {
                    LoadProductsToDisplay();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to cancel order: " + ex.Message);
            }
        }

        private void AddActionButtons()
        {
            if (dataGridView2.Columns.Contains("View"))
                dataGridView2.Columns.Remove("View");

            if (dataGridView2.Columns.Contains("Cancel"))
                dataGridView2.Columns.Remove("Cancel");

            DataGridViewButtonColumn btnView = new DataGridViewButtonColumn();
            btnView.Name = "View";
            btnView.Text = "View";
            btnView.UseColumnTextForButtonValue = true;

            DataGridViewButtonColumn btnCancel = new DataGridViewButtonColumn();
            btnCancel.Name = "Cancel";
            btnCancel.Text = "Cancel";
            btnCancel.UseColumnTextForButtonValue = true;

            dataGridView2.Columns.Insert(0, btnView);
            dataGridView2.Columns.Insert(1, btnCancel);

            foreach (DataGridViewColumn col in dataGridView2.Columns)
            {
                if (col.Index <= 1)
                {
                    col.Frozen = true;
                }
            }
        }

        private void buttonOrderHistory_Click(object sender, EventArgs e)
        {
            SetActiveButton(buttonOrderHistory);

            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panelSAPending.Visible = false;

            panelOrderHistory.Visible = true;
            panelOrderHistory.BringToFront();

            LoadOrderHistory();
        }

        private void LoadOrderHistory()
        {
            try
            {
                var db = new projectEntities3();
                string keyword = textBoxOrderHistorySearch.Text.Trim();

                var result = db.orders
                    .Where(o =>
                        (string.IsNullOrEmpty(keyword) ||
                         o.OrderID.Contains(keyword) ||
                         o.CustomerName.Contains(keyword) ||
                         o.Status.Contains(keyword) ||
                         o.CustomerPhone.Contains(keyword)))
                    .OrderByDescending(o => o.OrderDate)
                    .Select(o => new
                    {
                        o.OrderID,
                        o.CustomerName,
                        o.CustomerPhone,
                        o.TotalAmount,
                        o.OrderDate,
                        o.DeliveryDate,
                        o.Status,
                        o.CreatedBy
                    })
                    .ToList();

                dataGridViewOrderHistory.DataSource = result;

                if (dataGridViewOrderHistory.Columns.Contains("OrderID"))
                    dataGridViewOrderHistory.Columns["OrderID"].HeaderText = "Order ID";
                if (dataGridViewOrderHistory.Columns.Contains("CustomerName"))
                    dataGridViewOrderHistory.Columns["CustomerName"].HeaderText = "Customer Name";
                if (dataGridViewOrderHistory.Columns.Contains("CustomerPhone"))
                    dataGridViewOrderHistory.Columns["CustomerPhone"].HeaderText = "Phone";
                if (dataGridViewOrderHistory.Columns.Contains("TotalAmount"))
                {
                    dataGridViewOrderHistory.Columns["TotalAmount"].HeaderText = "Total Amount";
                    dataGridViewOrderHistory.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";
                }
                if (dataGridViewOrderHistory.Columns.Contains("OrderDate"))
                    dataGridViewOrderHistory.Columns["OrderDate"].HeaderText = "Order Date";
                if (dataGridViewOrderHistory.Columns.Contains("DeliveryDate"))
                    dataGridViewOrderHistory.Columns["DeliveryDate"].HeaderText = "Delivery Date";
                if (dataGridViewOrderHistory.Columns.Contains("Status"))
                {
                    dataGridViewOrderHistory.Columns["Status"].HeaderText = "Status";
                    dataGridViewOrderHistory.Columns["Status"].DefaultCellStyle.Font =
                        new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                if (dataGridViewOrderHistory.Columns.Contains("CreatedBy"))
                    dataGridViewOrderHistory.Columns["CreatedBy"].HeaderText = "Created By";

                dataGridViewOrderHistory.CellFormatting -= dataGridViewOrderHistory_CellFormatting;
                dataGridViewOrderHistory.CellFormatting += dataGridViewOrderHistory_CellFormatting;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load order history: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewOrderHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridViewOrderHistory.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();
                switch (status)
                {
                    case "Pending":
                        e.CellStyle.ForeColor = System.Drawing.Color.Orange;
                        break;
                    case "Paid":
                        e.CellStyle.ForeColor = System.Drawing.Color.Green;
                        break;
                    case "Cancel":
                        e.CellStyle.ForeColor = System.Drawing.Color.Red;
                        break;
                    case "Delivery":
                        e.CellStyle.ForeColor = System.Drawing.Color.Blue;
                        break;
                    case "Complete":
                        e.CellStyle.ForeColor = System.Drawing.Color.Purple;
                        break;
                    default:
                        e.CellStyle.ForeColor = System.Drawing.Color.Black;
                        break;
                }
            }
        }

        private void buttonOrderHistorySearch_Click(object sender, EventArgs e)
        {
            LoadOrderHistory();
        }

        private void textBoxOrderHistorySearch_TextChanged(object sender, EventArgs e)
        {
            LoadOrderHistory();
        }

        private Image GetProductImage(string productId, string productName)
        {
            string imagePath = Path.Combine(Application.StartupPath, "Images", $"{productId}.jpg");

            if (!File.Exists(imagePath))
            {
                imagePath = Path.Combine(Application.StartupPath, "Images", $"{productId}.png");
            }

            if (File.Exists(imagePath))
            {
                try
                {
                    return Image.FromFile(imagePath);
                }
                catch
                {
                    return GetDefaultProductImage(productId, productName);
                }
            }

            return GetDefaultProductImage(productId, productName);
        }

        private Image GetDefaultProductImage(string productId, string productName)
        {
            Bitmap bmp = new Bitmap(150, 120);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                int colorIndex = Math.Abs(productId.GetHashCode()) % 6;
                Color[] colors = {
                    Color.FromArgb(173, 216, 230),
                    Color.FromArgb(144, 238, 144),
                    Color.FromArgb(255, 182, 193),
                    Color.FromArgb(255, 255, 153),
                    Color.FromArgb(221, 160, 221),
                    Color.FromArgb(255, 218, 185)
                };
                g.Clear(colors[colorIndex]);
                g.DrawRectangle(Pens.DarkGray, 0, 0, 149, 119);

                string icon = GetProductIcon(productName);
                using (Font iconFont = new Font("Segoe UI", 40))
                {
                    SizeF textSize = g.MeasureString(icon, iconFont);
                    float x = (150 - textSize.Width) / 2;
                    g.DrawString(icon, iconFont, Brushes.DarkGray, x, 45);
                }

                string shortName = productName.Length > 12 ? productName.Substring(0, 10) + "..." : productName;
                using (Font textFont = new Font("Arial", 8, FontStyle.Bold))
                {
                    SizeF textSize = g.MeasureString(shortName, textFont);
                    float x = (150 - textSize.Width) / 2;
                    g.DrawString(shortName, textFont, Brushes.Black, x, 100);
                }
            }
            return bmp;
        }

        private string GetProductIcon(string productName)
        {
            if (productName.Contains("Sofa"))
                return "S";
            if (productName.Contains("Table"))
                return "T";
            if (productName.Contains("Chair"))
                return "C";
            if (productName.Contains("Wardrobe"))
                return "W";
            if (productName.Contains("Desk"))
                return "D";
            if (productName.Contains("Bed"))
                return "B";
            if (productName.Contains("Cabinet"))
                return "C";
            if (productName.Contains("Bookshelf"))
                return "B";
            return "P";
        }

        public void RefreshProductStock()
        {
            if (panel4.Visible)
            {
                LoadProductsToDisplay();
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click_3(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void label13_Click(object sender, EventArgs e) { }
        private void panel1_Paint_1(object sender, PaintEventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox6_TextChanged(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}