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
    public partial class Form2 : Form
    {
        private projectEntities3 db = new projectEntities3();
        private string staffName;
        private bool isSaving = false;  

        public Form2(string name)
        {
            InitializeComponent();
            this.staffName = name;

            this.cmbDeliveryID.SelectedIndexChanged += new System.EventHandler(this.cmbDeliveryID_SelectedIndexChanged);
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button2.Click += new System.EventHandler(this.button2_Click);  // Save 按鈕

            LoadDeliveryOrders();
            SetupOrderItemsDataGridView();
            SetReadOnlyFields();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void SetReadOnlyFields()
        {
            txtOrderID.ReadOnly = true;
            txtOrderID.BackColor = Color.LightGray;

            textBox2.ReadOnly = true;
            textBox2.BackColor = Color.LightGray;

            textBox3.ReadOnly = true;
            textBox3.BackColor = Color.LightGray;

            textBox4.ReadOnly = true;
            textBox4.BackColor = Color.LightGray;

            textBox5.ReadOnly = true;
            textBox5.BackColor = Color.LightGray;

            textBox1.ReadOnly = true;
            textBox1.BackColor = Color.LightGray;
        }

        private void LoadDeliveryOrders()
        {
            try
            {
                var deliveryOrders = (from d in db.delivery
                                      join o in db.orders on d.OrderID equals o.OrderID
                                      where !db.replyslip.Any(r => r.DeliveryID == d.DeliveryID)
                                      select new DeliveryOrderDto
                                      {
                                          DeliveryID = d.DeliveryID,
                                          OrderID = d.OrderID,
                                          CustomerName = o.CustomerName,
                                          CustomerPhone = o.CustomerPhone,
                                          CustomerEmail = o.CustomerEmail,
                                          DeliveryAddress = o.DeliveryAddress,
                                          DeliveryDate = d.DeliveryDate,
                                          HandledBy = d.HandledBy
                                      }).ToList();

                if (deliveryOrders.Count() == 0)
                {
                    MessageBox.Show("No delivery notes available. All delivery notes already have reply slips.");
                    cmbDeliveryID.Enabled = false;
                    button2.Enabled = false;
                    return;
                }

                cmbDeliveryID.DataSource = deliveryOrders;
                cmbDeliveryID.DisplayMember = "Display";
                cmbDeliveryID.ValueMember = "DeliveryID";
                cmbDeliveryID.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbDeliveryID.SelectedIndex = -1;

                cmbDeliveryID.Enabled = true;
                button2.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load delivery orders: " + ex.Message);
            }
        }

        public class DeliveryOrderDto
        {
            public string DeliveryID { get; set; }
            public string OrderID { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public string CustomerEmail { get; set; }
            public string DeliveryAddress { get; set; }
            public DateTime? DeliveryDate { get; set; }
            public string HandledBy { get; set; }

            public string Display
            {
                get { return DeliveryID + " - " + OrderID + " - " + CustomerName; }
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
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView1.Columns.Clear();

            DataGridViewTextBoxColumn colProductName = new DataGridViewTextBoxColumn();
            colProductName.Name = "ProductName";
            colProductName.HeaderText = "Product Name";
            colProductName.DataPropertyName = "ProductName";
            colProductName.Width = 250;
            dataGridView1.Columns.Add(colProductName);

            DataGridViewTextBoxColumn colQty = new DataGridViewTextBoxColumn();
            colQty.Name = "Qty";
            colQty.HeaderText = "Quantity";
            colQty.DataPropertyName = "Qty";
            colQty.Width = 80;
            colQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns.Add(colQty);

            DataGridViewTextBoxColumn colUnitPrice = new DataGridViewTextBoxColumn();
            colUnitPrice.Name = "UnitPrice";
            colUnitPrice.HeaderText = "Unit Price";
            colUnitPrice.DataPropertyName = "UnitPrice";
            colUnitPrice.Width = 100;
            colUnitPrice.DefaultCellStyle.Format = "C2";
            colUnitPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns.Add(colUnitPrice);

            DataGridViewTextBoxColumn colSubTotal = new DataGridViewTextBoxColumn();
            colSubTotal.Name = "SubTotal";
            colSubTotal.HeaderText = "Subtotal";
            colSubTotal.DataPropertyName = "SubTotal";
            colSubTotal.Width = 100;
            colSubTotal.DefaultCellStyle.Format = "C2";
            colSubTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns.Add(colSubTotal);
        }

        
        private void cmbDeliveryID_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (isSaving) return;

            if (cmbDeliveryID.SelectedItem == null || cmbDeliveryID.SelectedIndex == -1)
            {
                ClearForm();
                return;
            }

            var selectedOrder = (DeliveryOrderDto)cmbDeliveryID.SelectedItem;

            textBox5.Text = selectedOrder.DeliveryDate?.ToString("yyyy-MM-dd") ?? "";
            textBox2.Text = selectedOrder.CustomerName;
            textBox4.Text = selectedOrder.CustomerPhone;
            textBox1.Text = selectedOrder.CustomerEmail;
            textBox3.Text = selectedOrder.DeliveryAddress;
            txtOrderID.Text = selectedOrder.OrderID;

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

                dataGridView1.DataSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load order items: " + ex.Message);
                dataGridView1.DataSource = null;
            }
        }

        private void ClearForm()
        {
            textBox5.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox1.Text = "";
            textBox3.Text = "";
            textBox6.Text = "";
            txtOrderID.Text = "";
            dataGridView1.DataSource = null;
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            
            if (isSaving) return;

            if (cmbDeliveryID.SelectedItem == null || cmbDeliveryID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a delivery order first", "Warning",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedOrder = (DeliveryOrderDto)cmbDeliveryID.SelectedItem;

                
                bool existingReply = db.replyslip.Any(r => r.DeliveryID == selectedOrder.DeliveryID);
                if (existingReply)
                {
                    MessageBox.Show($"Delivery ID {selectedOrder.DeliveryID} already has a reply slip!",
                                   "Duplicate Reply Slip",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                    LoadDeliveryOrders();
                    ClearForm();
                    return;
                }

                
                isSaving = true;

                string replyID = GenerateReplyID();

                
                bool exists = db.replyslip.Any(r => r.ReplyID == replyID);
                while (exists)
                {
                    replyID = GenerateReplyID();
                    exists = db.replyslip.Any(r => r.ReplyID == replyID);
                }

                
                replyslip newReply = new replyslip
                {
                    ReplyID = replyID,
                    DeliveryID = selectedOrder.DeliveryID,
                    OrderID = selectedOrder.OrderID,
                    Remarks = textBox6.Text,
                    CreatedBy = staffName
                };

                db.replyslip.Add(newReply);
                db.SaveChanges();

                
                MessageBox.Show($"✅ Reply Slip saved successfully!\n\n" +
                               $"📄 Reply ID: {replyID}\n" +
                               $"📦 Delivery ID: {selectedOrder.DeliveryID}\n" +
                               $"📦 Order ID: {selectedOrder.OrderID}\n" +
                               $"👤 Customer: {selectedOrder.CustomerName}\n" +
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

        
        private void button1_Click(object sender, EventArgs e)
        {
            if (isSaving) return;  

            if (cmbDeliveryID.SelectedItem == null || cmbDeliveryID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a delivery order first");
                return;
            }

            var selectedOrder = (DeliveryOrderDto)cmbDeliveryID.SelectedItem;

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

            Dictionary<string, string> headerInfo = new Dictionary<string, string>
            {
                { "Reply Slip ID", GenerateReplyID() },
                { "Delivery ID", selectedOrder.DeliveryID },
                { "Order ID", selectedOrder.OrderID },
                { "Delivery Date", textBox5.Text },
                { "Customer Name", textBox2.Text },
                { "Customer Phone", textBox4.Text },
                { "Customer Email", textBox1.Text },
                { "Delivery Address", textBox3.Text },
                { "Remarks", textBox6.Text }
            };

            PrintDocument pd = new PrintDocument();
            pd.DocumentName = "Reply Slip";
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

                g.DrawString("Reply Slip", titleFont, Brushes.Black, startX, currentY);
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

                int signatureX = startX + 180;
                int dateX = 500 + 50;

                g.DrawString("Customer Signature: ", bodyFont, Brushes.Black, startX, currentY);
                g.DrawLine(new Pen(Color.Black, 1), signatureX, currentY + 12, signatureX + 250, currentY + 12);

                g.DrawString("Date: ", bodyFont, Brushes.Black, dateX, currentY);
                g.DrawLine(new Pen(Color.Black, 1), dateX + 45, currentY + 12, dateX + 195, currentY + 12);
            };

            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = pd;
            ppd.WindowState = FormWindowState.Maximized;
            ppd.ShowDialog();
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

        private string GenerateReplyID()
        {
            string prefix = "RS";
            Random random = new Random();
            int randomNumber = random.Next(10000000, 99999999);
            return prefix + randomNumber.ToString();
        }

        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void Form2_Load(object sender, EventArgs e) { }

        private void Form2_Load_1(object sender, EventArgs e) { }

        private void pictureBox2_Click(object sender, EventArgs e) { }
    }
}