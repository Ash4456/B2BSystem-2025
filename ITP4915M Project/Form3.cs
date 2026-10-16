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

namespace ITP4915M_Project
{
    public partial class Form3 : Form
    {
        private projectEntities3 db = new projectEntities3();
        private string staffName;
        private bool isSaving = false;
        private bool isLoading = false;
        private bool isClosing = false;  

        public Form3(string name)
        {
            InitializeComponent();
            this.staffName = name;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.cmbOrderID.SelectedIndexChanged += new System.EventHandler(this.cmbOrderID_SelectedIndexChanged);
            this.button4.Click += new System.EventHandler(this.button4_Click);
            this.button5.Click += new System.EventHandler(this.button5_Click);

            this.button4.DialogResult = DialogResult.None;
            this.button5.DialogResult = DialogResult.None;

            
            this.FormClosing += new FormClosingEventHandler(this.Form3_FormClosing);

            LoadPendingOrders();
            SetupOrderItemsDataGridView();
            SetReadOnlyFields();
        }

       
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;
        }

        private void SetReadOnlyFields()
        {
            textBox2.ReadOnly = true;
            textBox2.BackColor = Color.LightGray;

            textBox4.ReadOnly = true;
            textBox4.BackColor = Color.LightGray;

            textBox1.ReadOnly = true;
            textBox1.BackColor = Color.LightGray;

            dateTimePicker1.Enabled = false;
            dateTimePicker1.Value = DateTime.Now;
        }

        private void LoadPendingOrders()
        {
            try
            {
                
                if (isClosing) return;

                isLoading = true;

                var pendingOrders = (from o in db.orders
                                     where o.Status == "Pending"
                                     && !db.invoice.Any(i => i.OrderID == o.OrderID)
                                     select new OrderDto
                                     {
                                         OrderID = o.OrderID,
                                         CustomerName = o.CustomerName,
                                         CustomerPhone = o.CustomerPhone,
                                         CustomerEmail = o.CustomerEmail,
                                         DeliveryAddress = o.DeliveryAddress,
                                         DeliveryDate = o.DeliveryDate,
                                         TotalAmount = o.TotalAmount,
                                         Status = o.Status
                                     }).ToList();

                if (pendingOrders.Count() == 0)
                {
                    MessageBox.Show("No pending orders available.", "Information",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbOrderID.Enabled = false;
                    cmbOrderID.DataSource = null;
                    cmbOrderID.Text = "No orders available";
                    isLoading = false;
                    return;
                }

                
                this.cmbOrderID.SelectedIndexChanged -= new System.EventHandler(this.cmbOrderID_SelectedIndexChanged);

                cmbOrderID.DataSource = pendingOrders;
                cmbOrderID.DisplayMember = "DisplayText";
                cmbOrderID.ValueMember = "OrderID";
                cmbOrderID.DropDownStyle = ComboBoxStyle.DropDownList;

                if (pendingOrders.Count() > 0)
                {
                    cmbOrderID.SelectedIndex = 0;
                }
                else
                {
                    cmbOrderID.SelectedIndex = -1;
                }

                cmbOrderID.Enabled = true;

                
                this.cmbOrderID.SelectedIndexChanged += new System.EventHandler(this.cmbOrderID_SelectedIndexChanged);

                if (pendingOrders.Count() > 0)
                {
                    cmbOrderID_SelectedIndexChanged(null, null);
                }

                isLoading = false;
            }
            catch (Exception ex)
            {
                isLoading = false;
                MessageBox.Show("Failed to load pending orders: " + ex.Message);
            }
        }

        public class OrderDto
        {
            public string OrderID { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public string CustomerEmail { get; set; }
            public string DeliveryAddress { get; set; }
            public DateTime? DeliveryDate { get; set; }
            public decimal? TotalAmount { get; set; }
            public string Status { get; set; }

            public string DisplayText
            {
                get { return OrderID + " - " + CustomerName; }
            }
        }

        public class OrderItemDto
        {
            public string ProductName { get; set; }
            public int? Qty { get; set; }
            public decimal? UnitPrice { get; set; }
            public decimal? SubTotal { get; set; }
        }

        private void SetupOrderItemsDataGridView()
        {
            dgvOrderItems.AutoGenerateColumns = false;
            dgvOrderItems.AllowUserToAddRows = false;
            dgvOrderItems.ReadOnly = true;
            dgvOrderItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrderItems.Columns.Clear();

            DataGridViewTextBoxColumn colProductName = new DataGridViewTextBoxColumn();
            colProductName.Name = "ProductName";
            colProductName.HeaderText = "Product Name";
            colProductName.DataPropertyName = "ProductName";
            colProductName.Width = 250;
            dgvOrderItems.Columns.Add(colProductName);

            DataGridViewTextBoxColumn colQty = new DataGridViewTextBoxColumn();
            colQty.Name = "Qty";
            colQty.HeaderText = "Quantity";
            colQty.DataPropertyName = "Qty";
            colQty.Width = 80;
            colQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOrderItems.Columns.Add(colQty);

            DataGridViewTextBoxColumn colUnitPrice = new DataGridViewTextBoxColumn();
            colUnitPrice.Name = "UnitPrice";
            colUnitPrice.HeaderText = "Unit Price";
            colUnitPrice.DataPropertyName = "UnitPrice";
            colUnitPrice.Width = 100;
            colUnitPrice.DefaultCellStyle.Format = "C2";
            colUnitPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvOrderItems.Columns.Add(colUnitPrice);

            DataGridViewTextBoxColumn colSubTotal = new DataGridViewTextBoxColumn();
            colSubTotal.Name = "SubTotal";
            colSubTotal.HeaderText = "Subtotal";
            colSubTotal.DataPropertyName = "SubTotal";
            colSubTotal.Width = 100;
            colSubTotal.DefaultCellStyle.Format = "C2";
            colSubTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvOrderItems.Columns.Add(colSubTotal);
        }

        private void cmbOrderID_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (isSaving || isLoading || isClosing) return;

            if (cmbOrderID.SelectedItem == null || cmbOrderID.SelectedIndex == -1)
            {
                ClearForm();
                return;
            }

            var selectedOrder = (OrderDto)cmbOrderID.SelectedItem;

            textBox2.Text = selectedOrder.CustomerName;
            textBox4.Text = selectedOrder.CustomerPhone;
            textBox1.Text = selectedOrder.DeliveryAddress;

            if (selectedOrder.DeliveryDate.HasValue)
            {
                dateTimePicker1.Value = selectedOrder.DeliveryDate.Value;
            }

            LoadOrderItems(selectedOrder.OrderID);
        }

        private void LoadOrderItems(string orderId)
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

                dgvOrderItems.DataSource = items;

                decimal total = items.Sum(i => i.SubTotal ?? 0);
                lblTotalAmount.Text = $"Total: ${total:F2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load order items: " + ex.Message);
                dgvOrderItems.DataSource = null;
            }
        }

        private void ClearForm()
        {
            textBox2.Text = "";
            textBox4.Text = "";
            textBox1.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            dgvOrderItems.DataSource = null;
            lblTotalAmount.Text = "Total: $0.00";
        }

        private string GenerateInvoiceID()
        {
            string prefix = "IV";
            Random random = new Random();
            int randomNumber = random.Next(10000000, 99999999);
            return prefix + randomNumber.ToString();
        }

        private string GetSelectedPaymentMethod()
        {
            if (radioButton1.Checked) return "Cash";
            if (radioButton2.Checked) return "Octopus";
            if (radioButton3.Checked) return "Alipayhk";
            if (radioButton4.Checked) return "PayMe";
            if (radioButton5.Checked) return "Visa";
            return "";
        }

        
        private void button4_Click(object sender, EventArgs e)
        {
            
            if (isSaving || isClosing) return;

            if (cmbOrderID.SelectedItem == null || cmbOrderID.SelectedIndex == -1)
            {
                if (cmbOrderID.Items.Count == 0)
                {
                    MessageBox.Show("No pending orders available.\n\n" +
                                   "Please create an order first before creating an invoice.",
                                   "No Orders Available",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Please select an order from the dropdown list first.",
                                   "Warning",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                }
                return;
            }

            if (!radioButton1.Checked && !radioButton2.Checked &&
                !radioButton3.Checked && !radioButton4.Checked && !radioButton5.Checked)
            {
                MessageBox.Show("Please select a payment method.",
                               "Warning",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            
            isSaving = true;

            try
            {
                var selectedOrder = (OrderDto)cmbOrderID.SelectedItem;
                string paymentMethod = GetSelectedPaymentMethod();

                
                bool existingInvoice = db.invoice.Any(i => i.OrderID == selectedOrder.OrderID);
                if (existingInvoice)
                {
                    MessageBox.Show($"Order {selectedOrder.OrderID} already has an invoice!",
                                   "Duplicate Invoice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isSaving = false;
                    LoadPendingOrders();
                    ClearForm();
                    return;
                }

                string invoiceID = GenerateInvoiceID();

                invoice newInvoice = new invoice
                {
                    InvoiceID = invoiceID,
                    OrderID = selectedOrder.OrderID,
                    PaymentMethod = paymentMethod,
                    TotalAmount = selectedOrder.TotalAmount ?? 0,
                    CreatedBy = staffName,
                    CreatedDate = DateTime.Now
                };

                db.invoice.Add(newInvoice);

                var orderToUpdate = db.orders.FirstOrDefault(o => o.OrderID == selectedOrder.OrderID);
                if (orderToUpdate != null)
                {
                    orderToUpdate.Status = "Paid";
                }

                db.SaveChanges();

                
                MessageBox.Show($"✅ Invoice saved successfully!\n\n" +
                               $"📄 Invoice ID: {invoiceID}\n" +
                               $"📦 Order ID: {selectedOrder.OrderID}\n" +
                               $"👤 Customer: {selectedOrder.CustomerName}\n" +
                               $"💳 Payment Method: {paymentMethod}\n" +
                               $"💰 Total Amount: ${selectedOrder.TotalAmount:F2}\n" +
                               $"✍️ Created By: {staffName}",
                               "Success",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);

                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                isSaving = false;
                MessageBox.Show("❌ Save failed: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (isClosing) return;

            if (cmbOrderID.SelectedItem == null || cmbOrderID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an order first.", "Warning",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedOrder = (OrderDto)cmbOrderID.SelectedItem;
                var orderItems = GetOrderItems(selectedOrder.OrderID);

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

                string paymentMethod = GetSelectedPaymentMethod();
                if (string.IsNullOrEmpty(paymentMethod))
                {
                    paymentMethod = "Not selected";
                }

                string invoiceID = GenerateInvoiceID();

                Dictionary<string, string> headerInfo = new Dictionary<string, string>
                {
                    { "Invoice ID", invoiceID },
                    { "Order ID", selectedOrder.OrderID },
                    { "Customer Name", selectedOrder.CustomerName },
                    { "Customer Phone", selectedOrder.CustomerPhone },
                    { "Customer Email", selectedOrder.CustomerEmail },
                    { "Delivery Address", selectedOrder.DeliveryAddress },
                    { "Delivery Date", selectedOrder.DeliveryDate?.ToString("yyyy-MM-dd") ?? "Not set" },
                    { "Payment Method", paymentMethod },
                    { "Invoice Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm") }
                };

                PrintDocument pd = new PrintDocument();
                pd.DocumentName = "Invoice";
                pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
                pd.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);

                DataTable localDt = dt;
                decimal localTotal = totalAmount;

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

        private void panelContent_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}