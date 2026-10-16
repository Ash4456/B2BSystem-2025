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
using System.Data.Entity;
using System.IO;

namespace ITP4915M_Project
{
    public partial class Logistics : Form
    {
        private projectEntities3 db = new projectEntities3();
        private string staffName;
        private bool isPrinting = false;

       
        public class OrderItemDto
        {
            public int OrderItemID { get; set; }
            public string ProductName { get; set; }
            public int? Qty { get; set; }
            public decimal? UnitPrice { get; set; }
            public decimal? SubTotal { get; set; }
        }

        public class DeliveryNoteDto
        {
            public string DeliveryID { get; set; }
            public string OrderID { get; set; }
            public string CustomerName { get; set; }
            public DateTime? DeliveryDate { get; set; }
            public string DeliveryStatus { get; set; }
            public string HandledBy { get; set; }
            public string DeliveryAddress { get; set; }
            public string CustomerPhone { get; set; }
        }

        public class ReplySlipDto
        {
            public string ReplyID { get; set; }
            public string DeliveryID { get; set; }
            public string OrderID { get; set; }
            public string CustomerName { get; set; }
            public DateTime? DeliveryDate { get; set; }
            public string Remarks { get; set; }
            public string CreatedBy { get; set; }
        }

      
        public class LOPendingRequestDto
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

       
        public Logistics(string name)
        {
            InitializeComponent();
            this.staffName = name;
            label1.Text = name;

            this.dataGridView1.CellContentClick -= dataGridView1_CellContentClick;
            this.dataGridView1.CellContentClick += dataGridView1_CellContentClick;

            this.dataGridView2.CellContentClick -= dataGridView2_CellContentClick;
            this.dataGridView2.CellContentClick += dataGridView2_CellContentClick;

            this.dataGridView3.CellContentClick -= dataGridView3_CellContentClick;
            this.dataGridView3.CellContentClick += dataGridView3_CellContentClick;

            
            this.button1.Click -= button1_Click;
            this.button1.Click += button1_Click;

            this.button2.Click -= button2_Click;
            this.button2.Click += button2_Click;

            this.button3.Click -= button3_Click;
            this.button3.Click += button3_Click;

            this.button4.Click -= button4_Click;
            this.button4.Click += button4_Click;

            this.button5.Click -= button5_Click;
            this.button5.Click += button5_Click;

            this.button6.Click -= button6_Click;
            this.button6.Click += button6_Click;

           
            this.buttonViewDeliveryNotes.Click -= buttonViewDeliveryNotes_Click;
            this.buttonViewDeliveryNotes.Click += buttonViewDeliveryNotes_Click;

           
            this.buttonUploadDeliveryStatus.Click -= buttonUploadDeliveryStatus_Click;
            this.buttonUploadDeliveryStatus.Click += buttonUploadDeliveryStatus_Click;

           
            this.dataGridViewUploadDeliveryStatus.DataError -= dataGridViewUploadDeliveryStatus_DataError;
            this.dataGridViewUploadDeliveryStatus.DataError += dataGridViewUploadDeliveryStatus_DataError;

            
            panel1.Visible = true;
            panel1.BringToFront();
            panel2.Visible = false;
            panel3.Visible = false;
            panelUploadDeliveryStatus.Visible = false;

            
            SetupDataGridView();
            SetupReplySlipDataGridView();
            SetupLOPendingDataGridView();

           
            LoadDeliveryNotes();
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

            button3.BackColor = defaultColor;
            button3.ForeColor = defaultForeColor;

            button4.BackColor = defaultColor;
            button4.ForeColor = defaultForeColor;

            button5.BackColor = defaultColor;
            button5.ForeColor = defaultForeColor;

            button6.BackColor = defaultColor;
            button6.ForeColor = defaultForeColor;

            buttonViewDeliveryNotes.BackColor = defaultColor;
            buttonViewDeliveryNotes.ForeColor = defaultForeColor;

            buttonUploadDeliveryStatus.BackColor = defaultColor;
            buttonUploadDeliveryStatus.ForeColor = defaultForeColor;
        }

      
        private void ShowDeliveryNotes()
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panelUploadDeliveryStatus.Visible = false;
            panel1.BringToFront();
        }

        private void ShowReplySlip()
        {
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            panelUploadDeliveryStatus.Visible = false;
            panel2.BringToFront();
        }

        private void ShowLOPending()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            panelUploadDeliveryStatus.Visible = false;
            panel3.BringToFront();
            LoadLOPendingRequests();
        }

        private void ShowUploadDeliveryStatus()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panelUploadDeliveryStatus.Visible = true;
            panelUploadDeliveryStatus.BringToFront();
            LoadUploadDeliveryStatusOrders();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            SetActiveButton(button1);  

            if (isForm1Opening) return;
            isForm1Opening = true;

            using (Form1 form = new Form1(staffName))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadDeliveryNotes();
                }
            }

            isForm1Opening = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetActiveButton(button2);  

            using (Form2 form = new Form2(staffName))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadReplySlips();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetActiveButton(button3); 

            ShowLOPending();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetActiveButton(button4);  

            ShowReplySlip();
            LoadReplySlips();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetActiveButton(button5);  

            ShowDeliveryNotes();
            LoadDeliveryNotes();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SetActiveButton(button6);  

            ShowDeliveryNotes();
            LoadDeliveryNotes();
        }

       
        private void buttonViewDeliveryNotes_Click(object sender, EventArgs e)
        {
            SetActiveButton(buttonViewDeliveryNotes);  

            ShowDeliveryNotes();
            LoadDeliveryNotes();
        }

       
        private void buttonUploadDeliveryStatus_Click(object sender, EventArgs e)
        {
            SetActiveButton(buttonUploadDeliveryStatus);  

           
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panelUploadDeliveryStatus.Visible = true;
            panelUploadDeliveryStatus.BringToFront();

            LoadUploadDeliveryStatusOrders();
        }

       
        private void SetupDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.Columns.Clear();

            dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            DataGridViewButtonColumn viewButton = new DataGridViewButtonColumn();
            viewButton.Name = "ViewButton";
            viewButton.HeaderText = "";
            viewButton.Text = "View PDF";
            viewButton.UseColumnTextForButtonValue = true;
            viewButton.Width = 80;
            dataGridView1.Columns.Add(viewButton);

            DataGridViewTextBoxColumn colDeliveryID = new DataGridViewTextBoxColumn();
            colDeliveryID.Name = "DeliveryID";
            colDeliveryID.HeaderText = "Delivery ID";
            colDeliveryID.DataPropertyName = "DeliveryID";
            colDeliveryID.Width = 120;
            dataGridView1.Columns.Add(colDeliveryID);

            DataGridViewTextBoxColumn colOrderID = new DataGridViewTextBoxColumn();
            colOrderID.Name = "OrderID";
            colOrderID.HeaderText = "Order ID";
            colOrderID.DataPropertyName = "OrderID";
            colOrderID.Width = 100;
            dataGridView1.Columns.Add(colOrderID);

            DataGridViewTextBoxColumn colCustomerName = new DataGridViewTextBoxColumn();
            colCustomerName.Name = "CustomerName";
            colCustomerName.HeaderText = "Customer Name";
            colCustomerName.DataPropertyName = "CustomerName";
            colCustomerName.Width = 150;
            dataGridView1.Columns.Add(colCustomerName);

            DataGridViewTextBoxColumn colDeliveryDate = new DataGridViewTextBoxColumn();
            colDeliveryDate.Name = "DeliveryDate";
            colDeliveryDate.HeaderText = "Delivery Date";
            colDeliveryDate.DataPropertyName = "DeliveryDate";
            colDeliveryDate.Width = 120;
            colDeliveryDate.DefaultCellStyle.Format = "yyyy-MM-dd";
            dataGridView1.Columns.Add(colDeliveryDate);

            DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
            colStatus.Name = "DeliveryStatus";
            colStatus.HeaderText = "Status";
            colStatus.DataPropertyName = "DeliveryStatus";
            colStatus.Width = 80;
            dataGridView1.Columns.Add(colStatus);

            DataGridViewTextBoxColumn colHandledBy = new DataGridViewTextBoxColumn();
            colHandledBy.Name = "HandledBy";
            colHandledBy.HeaderText = "Handled By";
            colHandledBy.DataPropertyName = "HandledBy";
            colHandledBy.Width = 100;
            dataGridView1.Columns.Add(colHandledBy);

            DataGridViewTextBoxColumn colDeliveryAddress = new DataGridViewTextBoxColumn();
            colDeliveryAddress.Name = "DeliveryAddress";
            colDeliveryAddress.HeaderText = "Delivery Address";
            colDeliveryAddress.DataPropertyName = "DeliveryAddress";
            colDeliveryAddress.Width = 150;
            dataGridView1.Columns.Add(colDeliveryAddress);

            DataGridViewTextBoxColumn colCustomerPhone = new DataGridViewTextBoxColumn();
            colCustomerPhone.Name = "CustomerPhone";
            colCustomerPhone.HeaderText = "Customer Phone";
            colCustomerPhone.DataPropertyName = "CustomerPhone";
            colCustomerPhone.Width = 120;
            dataGridView1.Columns.Add(colCustomerPhone);
        }

        private void SetupReplySlipDataGridView()
        {
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = false;
            dataGridView2.Columns.Clear();

            dataGridView2.ScrollBars = ScrollBars.Both;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            DataGridViewButtonColumn viewButton = new DataGridViewButtonColumn();
            viewButton.Name = "ViewButton";
            viewButton.HeaderText = "";
            viewButton.Text = "View PDF";
            viewButton.UseColumnTextForButtonValue = true;
            viewButton.Width = 80;
            dataGridView2.Columns.Add(viewButton);

            DataGridViewTextBoxColumn colReplyID = new DataGridViewTextBoxColumn();
            colReplyID.Name = "ReplyID";
            colReplyID.HeaderText = "Reply ID";
            colReplyID.DataPropertyName = "ReplyID";
            colReplyID.Width = 120;
            dataGridView2.Columns.Add(colReplyID);

            DataGridViewTextBoxColumn colDeliveryID = new DataGridViewTextBoxColumn();
            colDeliveryID.Name = "DeliveryID";
            colDeliveryID.HeaderText = "Delivery ID";
            colDeliveryID.DataPropertyName = "DeliveryID";
            colDeliveryID.Width = 120;
            dataGridView2.Columns.Add(colDeliveryID);

            DataGridViewTextBoxColumn colOrderID = new DataGridViewTextBoxColumn();
            colOrderID.Name = "OrderID";
            colOrderID.HeaderText = "Order ID";
            colOrderID.DataPropertyName = "OrderID";
            colOrderID.Width = 100;
            dataGridView2.Columns.Add(colOrderID);

            DataGridViewTextBoxColumn colCustomerName = new DataGridViewTextBoxColumn();
            colCustomerName.Name = "CustomerName";
            colCustomerName.HeaderText = "Customer Name";
            colCustomerName.DataPropertyName = "CustomerName";
            colCustomerName.Width = 150;
            dataGridView2.Columns.Add(colCustomerName);

            DataGridViewTextBoxColumn colDeliveryDate = new DataGridViewTextBoxColumn();
            colDeliveryDate.Name = "DeliveryDate";
            colDeliveryDate.HeaderText = "Delivery Date";
            colDeliveryDate.DataPropertyName = "DeliveryDate";
            colDeliveryDate.Width = 120;
            colDeliveryDate.DefaultCellStyle.Format = "yyyy-MM-dd";
            dataGridView2.Columns.Add(colDeliveryDate);

            DataGridViewTextBoxColumn colRemarks = new DataGridViewTextBoxColumn();
            colRemarks.Name = "Remarks";
            colRemarks.HeaderText = "Remarks";
            colRemarks.DataPropertyName = "Remarks";
            colRemarks.Width = 200;
            dataGridView2.Columns.Add(colRemarks);

            DataGridViewTextBoxColumn colCreatedBy = new DataGridViewTextBoxColumn();
            colCreatedBy.Name = "CreatedBy";
            colCreatedBy.HeaderText = "Created By";
            colCreatedBy.DataPropertyName = "CreatedBy";
            colCreatedBy.Width = 100;
            dataGridView2.Columns.Add(colCreatedBy);
        }

        
        private void SetupLOPendingDataGridView()
        {
            dataGridView3.AutoGenerateColumns = false;
            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.ReadOnly = true;
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.MultiSelect = false;
            dataGridView3.Columns.Clear();

            dataGridView3.ScrollBars = ScrollBars.Both;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            
            DataGridViewButtonColumn viewButton = new DataGridViewButtonColumn();
            viewButton.Name = "ViewButton";
            viewButton.HeaderText = "";
            viewButton.Text = "🔍 View";
            viewButton.UseColumnTextForButtonValue = true;
            viewButton.Width = 80;
            viewButton.FlatStyle = FlatStyle.Popup;
            viewButton.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            viewButton.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            viewButton.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            dataGridView3.Columns.Add(viewButton);

            
            DataGridViewTextBoxColumn colRequestID = new DataGridViewTextBoxColumn();
            colRequestID.Name = "RequestID";
            colRequestID.HeaderText = "Request ID";
            colRequestID.DataPropertyName = "RequestID";
            colRequestID.Width = 100;
            dataGridView3.Columns.Add(colRequestID);

            
            DataGridViewTextBoxColumn colOrderID = new DataGridViewTextBoxColumn();
            colOrderID.Name = "OrderID";
            colOrderID.HeaderText = "Order ID";
            colOrderID.DataPropertyName = "OrderID";
            colOrderID.Width = 100;
            dataGridView3.Columns.Add(colOrderID);

            
            DataGridViewTextBoxColumn colRequestType = new DataGridViewTextBoxColumn();
            colRequestType.Name = "RequestType";
            colRequestType.HeaderText = "Request Type";
            colRequestType.DataPropertyName = "RequestType";
            colRequestType.Width = 120;
            dataGridView3.Columns.Add(colRequestType);

           
            DataGridViewTextBoxColumn colReason = new DataGridViewTextBoxColumn();
            colReason.Name = "Reason";
            colReason.HeaderText = "Reason";
            colReason.DataPropertyName = "Reason";
            colReason.Width = 200;
            dataGridView3.Columns.Add(colReason);

            
            DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
            colStatus.Name = "Status";
            colStatus.HeaderText = "Status";
            colStatus.DataPropertyName = "Status";
            colStatus.Width = 100;
            dataGridView3.Columns.Add(colStatus);

            
            DataGridViewTextBoxColumn colRequestDate = new DataGridViewTextBoxColumn();
            colRequestDate.Name = "RequestDate";
            colRequestDate.HeaderText = "Request Date";
            colRequestDate.DataPropertyName = "RequestDate";
            colRequestDate.Width = 140;
            colRequestDate.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            dataGridView3.Columns.Add(colRequestDate);

            
            DataGridViewTextBoxColumn colHandledBy = new DataGridViewTextBoxColumn();
            colHandledBy.Name = "HandledBy";
            colHandledBy.HeaderText = "Handled By";
            colHandledBy.DataPropertyName = "HandledBy";
            colHandledBy.Width = 100;
            dataGridView3.Columns.Add(colHandledBy);

            
            DataGridViewTextBoxColumn colForwardTo = new DataGridViewTextBoxColumn();
            colForwardTo.Name = "ForwardTo";
            colForwardTo.HeaderText = "Forward To";
            colForwardTo.DataPropertyName = "ForwardTo";
            colForwardTo.Visible = false;
            dataGridView3.Columns.Add(colForwardTo);

            
            DataGridViewTextBoxColumn colPhotoPath = new DataGridViewTextBoxColumn();
            colPhotoPath.Name = "PhotoPath";
            colPhotoPath.HeaderText = "PhotoPath";
            colPhotoPath.DataPropertyName = "PhotoPath";
            colPhotoPath.Visible = false;
            dataGridView3.Columns.Add(colPhotoPath);

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            headerStyle.ForeColor = System.Drawing.Color.White;
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            headerStyle.WrapMode = DataGridViewTriState.True;
            dataGridView3.ColumnHeadersDefaultCellStyle = headerStyle;

            dataGridView3.EnableHeadersVisualStyles = false;
            dataGridView3.GridColor = System.Drawing.Color.FromArgb(230, 230, 230);
        }

        
        private void LoadDeliveryNotes()
        {
            try
            {
                var deliveryNotes = (from d in db.delivery
                                     join o in db.orders on d.OrderID equals o.OrderID
                                     orderby d.DeliveryDate descending
                                     select new DeliveryNoteDto
                                     {
                                         DeliveryID = d.DeliveryID,
                                         OrderID = d.OrderID,
                                         CustomerName = o.CustomerName,
                                         DeliveryDate = d.DeliveryDate,
                                         DeliveryStatus = d.DeliveryStatus,
                                         HandledBy = d.HandledBy,
                                         DeliveryAddress = o.DeliveryAddress,
                                         CustomerPhone = o.CustomerPhone
                                     }).ToList();

                dataGridView1.DataSource = deliveryNotes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load delivery notes: " + ex.Message);
            }
        }

        private void LoadReplySlips()
        {
            try
            {
                var replySlips = (from r in db.replyslip
                                  join d in db.delivery on r.DeliveryID equals d.DeliveryID
                                  join o in db.orders on r.OrderID equals o.OrderID
                                  orderby r.ReplyID descending
                                  select new ReplySlipDto
                                  {
                                      ReplyID = r.ReplyID,
                                      DeliveryID = r.DeliveryID,
                                      OrderID = r.OrderID,
                                      CustomerName = o.CustomerName,
                                      DeliveryDate = d.DeliveryDate,
                                      Remarks = r.Remarks,
                                      CreatedBy = r.CreatedBy
                                  }).ToList();

                dataGridView2.DataSource = replySlips;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load reply slips: " + ex.Message);
            }
        }

       
        private void LoadLOPendingRequests()
        {
            try
            {
                var list = db.afterservicerequest
                    .Where(r => r.ForwardTo == "LO" && r.Status == "Pending")
                    .Select(r => new LOPendingRequestDto
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

                dataGridView3.Columns.Clear();
                dataGridView3.DataSource = null;
                dataGridView3.Refresh();

                SetupLOPendingDataGridView();

                if (list.Count == 0)
                {
                    dataGridView3.DataSource = list;
                    dataGridView3.Refresh();
                    MessageBox.Show("No pending requests for LO.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dataGridView3.DataSource = list;
                dataGridView3.Refresh();

                
                if (dataGridView3.Columns.Contains("PhotoPath"))
                {
                    dataGridView3.Columns["PhotoPath"].Visible = false;
                }

                dataGridView3.Invalidate();

                System.Diagnostics.Debug.WriteLine($"Loaded {list.Count} LO pending requests");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load LO pending requests.\n" + ex.Message);
            }
        }

       
        private List<OrderItemDto> GetOrderItems(string orderId)
        {
            try
            {
                var items = (from oi in db.order_items
                             where oi.OrderID == orderId
                             select new OrderItemDto
                             {
                                 OrderItemID = oi.OrderItemID,
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

        
        private void ViewDeliveryNotePDF(string deliveryID, string orderID, string customerName,
                                  string customerPhone, string deliveryAddress,
                                  DateTime? deliveryDate, string handledBy)
        {
            try
            {
                var orderItems = GetOrderItems(orderID);

                DataTable dt = new DataTable();
                dt.Columns.Add("Order Item ID", typeof(int));
                dt.Columns.Add("Product Name", typeof(string));
                dt.Columns.Add("Quantity", typeof(int));
                dt.Columns.Add("Unit Price", typeof(decimal));
                dt.Columns.Add("Subtotal", typeof(decimal));

                foreach (var item in orderItems)
                {
                    dt.Rows.Add(
                        item.OrderItemID,
                        item.ProductName ?? "",
                        item.Qty ?? 0,
                        item.UnitPrice ?? 0,
                        item.SubTotal ?? 0
                    );
                }

                Dictionary<string, string> headerInfo = new Dictionary<string, string>
                {
                    { "Delivery Note ID", deliveryID },
                    { "Order ID", orderID },
                    { "Customer Name", customerName },
                    { "Customer Phone", customerPhone },
                    { "Delivery Address", deliveryAddress },
                    { "Delivery Date", deliveryDate?.ToString("yyyy-MM-dd") ?? "Not set" },
                    { "Created By", handledBy }
                };

                PrintDocument pd = new PrintDocument();
                pd.DocumentName = "Delivery Note";
                pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
                pd.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);

                DataTable localDt = dt;

                pd.PrintPage += (sender, e) =>
                {
                    Graphics g = e.Graphics;
                    Font titleFont = new Font("Arial", 18, FontStyle.Bold);
                    Font headerFont = new Font("Arial", 10, FontStyle.Bold);
                    Font bodyFont = new Font("Arial", 9);
                    Font bodyBoldFont = new Font("Arial", 9, FontStyle.Bold);

                    int currentY = 50;
                    int startX = 50;

                    g.DrawString("Delivery Note", titleFont, Brushes.Black, startX, currentY);
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
                        g.DrawString($"Total Items: {localDt.Rows.Count}", bodyFont, Brushes.Black, startX, currentY);
                        currentY += 20;
                    }
                    else
                    {
                        g.DrawString("No order items found.", bodyFont, Brushes.Black, startX, currentY);
                        currentY += 30;
                    }

                    currentY += 40;
                    g.DrawString("Prepared By: __________________", bodyFont, Brushes.Black, startX, currentY);
                    g.DrawString("Received By: __________________", bodyFont, Brushes.Black, 500, currentY);
                };

                PrintPreviewDialog ppd = new PrintPreviewDialog();
                ppd.Document = pd;
                ppd.WindowState = FormWindowState.Maximized;
                ppd.ShowDialog();
                ppd.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to view PDF: " + ex.Message);
            }
        }

        private void ViewReplySlipPDF(string replyID, string deliveryID, string orderID,
                              string customerName, DateTime? deliveryDate,
                              string remarks, string createdBy)
        {
            try
            {
                var orderItems = GetOrderItems(orderID);

                DataTable dt = new DataTable();
                dt.Columns.Add("Product Name", typeof(string));
                dt.Columns.Add("Quantity", typeof(int));
                dt.Columns.Add("Unit Price", typeof(decimal));
                dt.Columns.Add("Subtotal", typeof(decimal));

                decimal totalAmount = 0;
                foreach (var item in orderItems)
                {
                    dt.Rows.Add(
                        item.ProductName ?? "",
                        item.Qty ?? 0,
                        item.UnitPrice ?? 0,
                        item.SubTotal ?? 0
                    );
                    totalAmount += item.SubTotal ?? 0;
                }

                Dictionary<string, string> headerInfo = new Dictionary<string, string>
                {
                    { "Reply Slip ID", replyID },
                    { "Delivery ID", deliveryID },
                    { "Order ID", orderID },
                    { "Customer Name", customerName },
                    { "Delivery Date", deliveryDate?.ToString("yyyy-MM-dd") ?? "Not set" },
                    { "Remarks", remarks },
                    { "Created By", createdBy }
                };

                PrintDocument pd = new PrintDocument();
                pd.DocumentName = "Reply Slip";
                pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
                pd.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);

                pd.PrintPage += (sender, e) =>
                {
                    Graphics g = e.Graphics;
                    Font titleFont = new Font("Arial", 18, FontStyle.Bold);
                    Font headerFont = new Font("Arial", 10, FontStyle.Bold);
                    Font bodyFont = new Font("Arial", 9);
                    Font bodyBoldFont = new Font("Arial", 9, FontStyle.Bold);

                    int currentY = 50;
                    int startX = 50;

                    g.DrawString("Reply Slip", titleFont, Brushes.Black, startX, currentY);
                    currentY += 40;
                    g.DrawLine(new Pen(Color.Black, 2), startX, currentY, 750, currentY);
                    currentY += 20;

                    foreach (var info in headerInfo)
                    {
                        g.DrawString($"{info.Key}: {info.Value}", bodyBoldFont, Brushes.Black, startX, currentY);
                        currentY += 20;
                    }
                    currentY += 15;

                    g.DrawString("Order Items:", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, startX, currentY);
                    currentY += 25;

                    if (dt.Rows.Count > 0)
                    {
                        int colWidth = 700 / dt.Columns.Count;
                        int currentX = startX;

                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            g.FillRectangle(Brushes.LightGray, currentX, currentY, colWidth, 25);
                            g.DrawRectangle(Pens.Black, currentX, currentY, colWidth, 25);
                            g.DrawString(dt.Columns[i].ColumnName, headerFont, Brushes.Black, currentX + 3, currentY + 5);
                            currentX += colWidth;
                        }
                        currentY += 25;

                        foreach (DataRow row in dt.Rows)
                        {
                            currentX = startX;
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                g.DrawRectangle(Pens.Black, currentX, currentY, colWidth, 22);
                                string value = row[i]?.ToString() ?? "";
                                if (i >= 2 && decimal.TryParse(value, out decimal decValue))
                                    value = $"{decValue:C2}";
                                g.DrawString(value, bodyFont, Brushes.Black, currentX + 3, currentY + 4);
                                currentX += colWidth;
                            }
                            currentY += 22;
                        }

                        currentY += 10;
                        g.DrawString($"Total Amount: {totalAmount:C2}", bodyBoldFont, Brushes.Black, startX, currentY);
                        currentY += 20;
                        g.DrawString($"Total Items: {dt.Rows.Count}", bodyFont, Brushes.Black, startX, currentY);
                    }
                    else
                    {
                        g.DrawString("No order items found.", bodyFont, Brushes.Black, startX, currentY);
                    }

                    currentY += 40;
                    g.DrawString("Customer Signature: __________________", bodyFont, Brushes.Black, startX, currentY);
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
                MessageBox.Show("Failed to view Reply Slip PDF: " + ex.Message);
            }
        }

        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (isPrinting) return;

            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["ViewButton"].Index)
            {
                isPrinting = true;

                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                string deliveryID = row.Cells["DeliveryID"].Value?.ToString() ?? "";
                string orderID = row.Cells["OrderID"].Value?.ToString() ?? "";
                string customerName = row.Cells["CustomerName"].Value?.ToString() ?? "";
                string customerPhone = row.Cells["CustomerPhone"].Value?.ToString() ?? "";
                string deliveryAddress = row.Cells["DeliveryAddress"].Value?.ToString() ?? "";
                DateTime? deliveryDate = row.Cells["DeliveryDate"].Value as DateTime?;
                string handledBy = row.Cells["HandledBy"].Value?.ToString() ?? "";

                ViewDeliveryNotePDF(deliveryID, orderID, customerName, customerPhone,
                                   deliveryAddress, deliveryDate, handledBy);

                this.BeginInvoke(new Action(() => { isPrinting = false; }));
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (isPrinting) return;

            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView2.Columns["ViewButton"].Index)
            {
                isPrinting = true;

                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                string replyID = row.Cells["ReplyID"].Value?.ToString() ?? "";
                string deliveryID = row.Cells["DeliveryID"].Value?.ToString() ?? "";
                string orderID = row.Cells["OrderID"].Value?.ToString() ?? "";
                string customerName = row.Cells["CustomerName"].Value?.ToString() ?? "";
                DateTime? deliveryDate = row.Cells["DeliveryDate"].Value as DateTime?;
                string remarks = row.Cells["Remarks"].Value?.ToString() ?? "";
                string createdBy = row.Cells["CreatedBy"].Value?.ToString() ?? "";

                ViewReplySlipPDF(replyID, deliveryID, orderID, customerName,
                                 deliveryDate, remarks, createdBy);

                this.BeginInvoke(new Action(() => { isPrinting = false; }));
            }
        }

       
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (e.ColumnIndex != 0) 
                return;

            if (dataGridView3.Rows.Count == 0 || e.RowIndex >= dataGridView3.Rows.Count)
                return;

            if (dataGridView3.Rows[e.RowIndex].IsNewRow)
                return;

            if (dataGridView3.Rows[e.RowIndex].Cells["RequestID"].Value == null)
                return;

            try
            {
                DataGridViewRow row = dataGridView3.Rows[e.RowIndex];
                int requestID = Convert.ToInt32(row.Cells["RequestID"].Value);
                string orderID = row.Cells["OrderID"].Value?.ToString() ?? "";
                string requestType = row.Cells["RequestType"].Value?.ToString() ?? "";
                string reason = row.Cells["Reason"].Value?.ToString() ?? "";
                string status = row.Cells["Status"].Value?.ToString() ?? "";
                string photoPath = row.Cells["PhotoPath"].Value?.ToString() ?? "";  

                ShowLOPendingRequestDetailDialog(requestID, orderID, requestType, reason, status, photoPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error viewing request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void ShowLOPendingRequestDetailDialog(int requestID, string orderID, string requestType, string reason, string status, string photoPath = null)
        {
            Form dialog = new Form();
            dialog.Text = "📋 LO Pending Request Details";
            dialog.Size = new System.Drawing.Size(550, 520);
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.BackColor = System.Drawing.Color.White;
            
            dialog.AutoScroll = true;
            dialog.AutoScrollMinSize = new System.Drawing.Size(550, 600);

            Label lblTitle = new Label();
            lblTitle.Text = "LO Request Details";
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
                UpdateLORequestStatus(requestID, "Confirmed");
                dialog.Close();
                LoadLOPendingRequests();
                LoadDeliveryNotes();
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
                UpdateLORequestStatus(requestID, "Rejected");
                dialog.Close();
                LoadLOPendingRequests();
                LoadDeliveryNotes();
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

       
        private void UpdateLORequestStatus(int requestID, string newStatus)
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

        private void LoadUploadDeliveryStatusOrders()
        {
            try
            {
                var orders = (from o in db.orders
                              where db.replyslip.Any(r => r.OrderID == o.OrderID)
                              && (o.Status == "Paid" || o.Status == "Delivery")
                              orderby o.OrderDate descending
                              select new
                              {
                                  o.OrderID,
                                  o.CustomerName,
                                  o.CustomerPhone,
                                  o.DeliveryAddress,
                                  o.TotalAmount,
                                  o.OrderDate,
                                  o.DeliveryDate,
                                  o.Status,
                                  HasDeliveryNote = db.delivery.Any(d => d.OrderID == o.OrderID),
                                  ReplyCount = db.replyslip.Count(r => r.OrderID == o.OrderID)
                              })
                              .ToList();

                dataGridViewUploadDeliveryStatus.DataSource = orders;

                if (dataGridViewUploadDeliveryStatus.Columns.Contains("OrderID"))
                    dataGridViewUploadDeliveryStatus.Columns["OrderID"].HeaderText = "Order ID";
                if (dataGridViewUploadDeliveryStatus.Columns.Contains("CustomerName"))
                    dataGridViewUploadDeliveryStatus.Columns["CustomerName"].HeaderText = "Customer Name";
                if (dataGridViewUploadDeliveryStatus.Columns.Contains("CustomerPhone"))
                    dataGridViewUploadDeliveryStatus.Columns["CustomerPhone"].HeaderText = "Phone";
                if (dataGridViewUploadDeliveryStatus.Columns.Contains("DeliveryAddress"))
                    dataGridViewUploadDeliveryStatus.Columns["DeliveryAddress"].HeaderText = "Delivery Address";
                if (dataGridViewUploadDeliveryStatus.Columns.Contains("TotalAmount"))
                {
                    dataGridViewUploadDeliveryStatus.Columns["TotalAmount"].HeaderText = "Total Amount";
                    dataGridViewUploadDeliveryStatus.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";
                }
                if (dataGridViewUploadDeliveryStatus.Columns.Contains("OrderDate"))
                    dataGridViewUploadDeliveryStatus.Columns["OrderDate"].HeaderText = "Order Date";
                if (dataGridViewUploadDeliveryStatus.Columns.Contains("DeliveryDate"))
                    dataGridViewUploadDeliveryStatus.Columns["DeliveryDate"].HeaderText = "Delivery Date";
                if (dataGridViewUploadDeliveryStatus.Columns.Contains("Status"))
                    dataGridViewUploadDeliveryStatus.Columns["Status"].HeaderText = "Status";
                if (dataGridViewUploadDeliveryStatus.Columns.Contains("HasDeliveryNote"))
                    dataGridViewUploadDeliveryStatus.Columns["HasDeliveryNote"].HeaderText = "Has Delivery Note";
                if (dataGridViewUploadDeliveryStatus.Columns.Contains("ReplyCount"))
                    dataGridViewUploadDeliveryStatus.Columns["ReplyCount"].HeaderText = "Reply Count";

                dataGridViewUploadDeliveryStatus.ReadOnly = true;
                dataGridViewUploadDeliveryStatus.AllowUserToAddRows = false;
                dataGridViewUploadDeliveryStatus.AllowUserToDeleteRows = false;

                AddUploadStatusButtonColumn();

                dataGridViewUploadDeliveryStatus.CellFormatting -= dataGridViewUploadDeliveryStatus_CellFormatting;
                dataGridViewUploadDeliveryStatus.CellFormatting += dataGridViewUploadDeliveryStatus_CellFormatting;

                dataGridViewUploadDeliveryStatus.CellFormatting -= dataGridViewUploadDeliveryStatus_StatusFormatting;
                dataGridViewUploadDeliveryStatus.CellFormatting += dataGridViewUploadDeliveryStatus_StatusFormatting;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load orders: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void AddUploadStatusButtonColumn()
        {
            
            if (dataGridViewUploadDeliveryStatus.Columns.Contains("UploadStatus"))
            {
                dataGridViewUploadDeliveryStatus.Columns.Remove("UploadStatus");
            }

            DataGridViewButtonColumn btnUpload = new DataGridViewButtonColumn();
            btnUpload.Name = "UploadStatus";
            btnUpload.HeaderText = "";
            btnUpload.Text = "📤 Upload";
            btnUpload.UseColumnTextForButtonValue = true;
            btnUpload.Width = 80;
            btnUpload.FlatStyle = FlatStyle.Popup;
            btnUpload.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            btnUpload.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            btnUpload.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            dataGridViewUploadDeliveryStatus.Columns.Insert(0, btnUpload);

            
            dataGridViewUploadDeliveryStatus.CellClick -= dataGridViewUploadDeliveryStatus_CellClick;
            dataGridViewUploadDeliveryStatus.CellClick += dataGridViewUploadDeliveryStatus_CellClick;
        }

       
        private void dataGridViewUploadDeliveryStatus_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dataGridViewUploadDeliveryStatus.Columns[e.ColumnIndex].Name != "UploadStatus")
                return;

            if (dataGridViewUploadDeliveryStatus.Rows.Count == 0 || e.RowIndex >= dataGridViewUploadDeliveryStatus.Rows.Count)
                return;

            if (dataGridViewUploadDeliveryStatus.Rows[e.RowIndex].IsNewRow)
                return;

            if (dataGridViewUploadDeliveryStatus.Rows[e.RowIndex].Cells["OrderID"].Value == null)
                return;

            try
            {
                DataGridViewRow row = dataGridViewUploadDeliveryStatus.Rows[e.RowIndex];
                string orderID = row.Cells["OrderID"].Value?.ToString() ?? "";
                string customerName = row.Cells["CustomerName"].Value?.ToString() ?? "";
                string customerPhone = row.Cells["CustomerPhone"].Value?.ToString() ?? "";
                string deliveryAddress = row.Cells["DeliveryAddress"].Value?.ToString() ?? "";
                DateTime? orderDate = row.Cells["OrderDate"].Value as DateTime?;

                
                DateTime? deliveryDate = null;
                var order = db.orders.FirstOrDefault(o => o.OrderID == orderID);
                if (order != null)
                {
                    deliveryDate = order.DeliveryDate;
                }

                ShowUploadDeliveryStatusDialog(orderID, customerName, customerPhone, deliveryAddress, orderDate, deliveryDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void ShowUploadDeliveryStatusDialog(string orderID, string customerName, string customerPhone, string deliveryAddress, DateTime? orderDate, DateTime? deliveryDate)
        {
            Form dialog = new Form();
            dialog.Text = "📤 Upload Delivery Status";
            dialog.Size = new System.Drawing.Size(450, 420);
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.BackColor = System.Drawing.Color.White;

            int yPos = 20;

            
            Label lblTitle = new Label();
            lblTitle.Text = "📤 Upload Delivery Status";
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblTitle.Location = new System.Drawing.Point(20, yPos);
            lblTitle.Size = new System.Drawing.Size(300, 32);
            dialog.Controls.Add(lblTitle);

            yPos += 45;

            
            Panel line = new Panel();
            line.BackColor = System.Drawing.Color.FromArgb(220, 220, 220);
            line.Location = new System.Drawing.Point(20, yPos);
            line.Size = new System.Drawing.Size(400, 2);
            dialog.Controls.Add(line);

            yPos += 20;

           
            Label lblOrderIDTitle = new Label();
            lblOrderIDTitle.Text = "Order ID:";
            lblOrderIDTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblOrderIDTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblOrderIDTitle.Location = new System.Drawing.Point(20, yPos);
            lblOrderIDTitle.Size = new System.Drawing.Size(120, 25);
            dialog.Controls.Add(lblOrderIDTitle);

            Label lblOrderIDValue = new Label();
            lblOrderIDValue.Text = orderID;
            lblOrderIDValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblOrderIDValue.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblOrderIDValue.Location = new System.Drawing.Point(150, yPos);
            lblOrderIDValue.Size = new System.Drawing.Size(200, 25);
            dialog.Controls.Add(lblOrderIDValue);

            yPos += 35;

            
            Label lblCustomerNameTitle = new Label();
            lblCustomerNameTitle.Text = "Customer Name:";
            lblCustomerNameTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblCustomerNameTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblCustomerNameTitle.Location = new System.Drawing.Point(20, yPos);
            lblCustomerNameTitle.Size = new System.Drawing.Size(120, 25);
            dialog.Controls.Add(lblCustomerNameTitle);

            Label lblCustomerNameValue = new Label();
            lblCustomerNameValue.Text = customerName;
            lblCustomerNameValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblCustomerNameValue.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblCustomerNameValue.Location = new System.Drawing.Point(150, yPos);
            lblCustomerNameValue.Size = new System.Drawing.Size(200, 25);
            dialog.Controls.Add(lblCustomerNameValue);

            yPos += 35;

            
            Label lblCustomerPhoneTitle = new Label();
            lblCustomerPhoneTitle.Text = "Customer Phone:";
            lblCustomerPhoneTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblCustomerPhoneTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblCustomerPhoneTitle.Location = new System.Drawing.Point(20, yPos);
            lblCustomerPhoneTitle.Size = new System.Drawing.Size(120, 25);
            dialog.Controls.Add(lblCustomerPhoneTitle);

            Label lblCustomerPhoneValue = new Label();
            lblCustomerPhoneValue.Text = customerPhone;
            lblCustomerPhoneValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblCustomerPhoneValue.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblCustomerPhoneValue.Location = new System.Drawing.Point(150, yPos);
            lblCustomerPhoneValue.Size = new System.Drawing.Size(200, 25);
            dialog.Controls.Add(lblCustomerPhoneValue);

            yPos += 35;

            
            Label lblDeliveryAddressTitle = new Label();
            lblDeliveryAddressTitle.Text = "Delivery Address:";
            lblDeliveryAddressTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblDeliveryAddressTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblDeliveryAddressTitle.Location = new System.Drawing.Point(20, yPos);
            lblDeliveryAddressTitle.Size = new System.Drawing.Size(120, 25);
            dialog.Controls.Add(lblDeliveryAddressTitle);

            Label lblDeliveryAddressValue = new Label();
            lblDeliveryAddressValue.Text = deliveryAddress;
            lblDeliveryAddressValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblDeliveryAddressValue.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            lblDeliveryAddressValue.Location = new System.Drawing.Point(150, yPos);
            lblDeliveryAddressValue.Size = new System.Drawing.Size(250, 25);
            dialog.Controls.Add(lblDeliveryAddressValue);

            yPos += 35;

            
            Label lblDeliveryDateTitle = new Label();
            lblDeliveryDateTitle.Text = "Delivery Time:";
            lblDeliveryDateTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblDeliveryDateTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblDeliveryDateTitle.Location = new System.Drawing.Point(20, yPos);
            lblDeliveryDateTitle.Size = new System.Drawing.Size(120, 25);
            dialog.Controls.Add(lblDeliveryDateTitle);

            DateTimePicker dtpDeliveryDate = new DateTimePicker();
            dtpDeliveryDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            dtpDeliveryDate.Location = new System.Drawing.Point(150, yPos);
            dtpDeliveryDate.Size = new System.Drawing.Size(200, 31);

            
            if (deliveryDate.HasValue)
            {
                dtpDeliveryDate.Value = deliveryDate.Value;
            }
            else
            {
                dtpDeliveryDate.Value = DateTime.Now;
            }
            dialog.Controls.Add(dtpDeliveryDate);

            yPos += 45;

            
            Label lblStatusTitle = new Label();
            lblStatusTitle.Text = "Status:";
            lblStatusTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblStatusTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblStatusTitle.Location = new System.Drawing.Point(20, yPos);
            lblStatusTitle.Size = new System.Drawing.Size(120, 25);
            dialog.Controls.Add(lblStatusTitle);

            ComboBox cmbStatus = new ComboBox();
            cmbStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbStatus.Location = new System.Drawing.Point(150, yPos);
            cmbStatus.Size = new System.Drawing.Size(200, 33);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.AddRange(new object[] { "Delivery", "Complete" });
            cmbStatus.SelectedIndex = 0;
            dialog.Controls.Add(cmbStatus);

            yPos += 55;

            
            Button btnUpload = new Button();
            btnUpload.Text = "📤 Upload";
            btnUpload.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            btnUpload.ForeColor = System.Drawing.Color.White;
            btnUpload.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnUpload.FlatStyle = FlatStyle.Flat;
            btnUpload.FlatAppearance.BorderSize = 0;
            btnUpload.Size = new System.Drawing.Size(120, 40);
            btnUpload.Location = new System.Drawing.Point(150, yPos);
            btnUpload.Cursor = Cursors.Hand;
            btnUpload.Click += (s, ev) =>
            {
                try
                {
                    string selectedStatus = cmbStatus.SelectedItem?.ToString() ?? "Delivery";
                    DateTime deliveryDateValue = dtpDeliveryDate.Value;

                    
                    var order = db.orders.FirstOrDefault(o => o.OrderID == orderID);
                    if (order != null)
                    {
                        order.Status = selectedStatus;

                        
                        var existingDelivery = db.delivery.FirstOrDefault(d => d.OrderID == orderID);
                        if (existingDelivery != null)
                        {
                            existingDelivery.DeliveryDate = deliveryDateValue;
                            existingDelivery.DeliveryStatus = selectedStatus;
                            existingDelivery.HandledBy = staffName;
                        }
                        else
                        {
                            string deliveryID = GenerateDeliveryID();
                            delivery newDelivery = new delivery
                            {
                                DeliveryID = deliveryID,
                                OrderID = orderID,
                                DeliveryDate = deliveryDateValue,
                                DeliveryStatus = selectedStatus,
                                HandledBy = staffName
                            };
                            db.delivery.Add(newDelivery);
                        }

                        db.SaveChanges();

                        MessageBox.Show($"✅ Order {orderID} status updated to: {selectedStatus}\n\n" +
                                       $"📦 Order ID: {orderID}\n" +
                                       $"👤 Customer: {customerName}\n" +
                                       $"📅 Delivery Date: {deliveryDateValue:yyyy-MM-dd HH:mm}",
                                       "Success",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Information);

                        dialog.Close();
                        LoadUploadDeliveryStatusOrders();
                        LoadDeliveryNotes();
                    }
                    else
                    {
                        MessageBox.Show("Order not found ❌", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            dialog.Controls.Add(btnUpload);

            
            Button btnCancel = new Button();
            btnCancel.Text = "✖ Cancel";
            btnCancel.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            btnCancel.ForeColor = System.Drawing.Color.White;
            btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Size = new System.Drawing.Size(120, 40);
            btnCancel.Location = new System.Drawing.Point(280, yPos);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += (s, ev) => dialog.Close();
            dialog.Controls.Add(btnCancel);

            dialog.ShowDialog();
        }

       
        private string GenerateDeliveryID()
        {
            string prefix = "DL";
            Random random = new Random();
            int randomNumber = random.Next(10000000, 99999999);
            return prefix + randomNumber.ToString();
        }

       
        private void dataGridViewUploadDeliveryStatus_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridViewUploadDeliveryStatus.Columns[e.ColumnIndex].Name == "HasDeliveryNote" && e.Value != null)
            {
                try
                {
                    bool hasDelivery = Convert.ToBoolean(e.Value);
                    e.Value = hasDelivery ? "✅ Yes" : "❌ No";
                    e.CellStyle.ForeColor = hasDelivery ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                    e.FormattingApplied = true;
                }
                catch
                {
                    e.Value = "❌ No";
                    e.CellStyle.ForeColor = System.Drawing.Color.Red;
                    e.FormattingApplied = true;
                }
            }
        }

        
        private void dataGridViewUploadDeliveryStatus_StatusFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridViewUploadDeliveryStatus.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
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
                e.FormattingApplied = true;
            }
        }

        
        private void dataGridViewUploadDeliveryStatus_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        
        private bool isForm1Opening = false;

        private void Logistics_Load(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }

        private void label4_Click(object sender, EventArgs e) { }

        private void label5_Click(object sender, EventArgs e) { }

        private void textBox4_TextChanged(object sender, EventArgs e) { }

        private void label6_Click(object sender, EventArgs e) { }

        private void panel2_Paint(object sender, PaintEventArgs e) { }

        private void panelMain_Paint(object sender, PaintEventArgs e) { }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}