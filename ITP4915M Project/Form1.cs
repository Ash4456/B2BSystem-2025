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

namespace ITP4915M_Project
{
    public partial class Form1 : Form
    {

        private projectEntities3 db = new projectEntities3();
        private string staffName;

        public Form1(string name)
        {
            InitializeComponent();
            this.staffName = name;
            LoadOrdersWithoutDeliveryNote();
            ClearForm();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            SetupOrderItemsDataGridView();
        }


        public Form1() : this("")
        {
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

        
        private void LoadOrdersWithoutDeliveryNote()
        {
            try
            {
                var ordersWithoutDelivery = (from o in db.orders
                                             join d in db.delivery on o.OrderID equals d.OrderID into deliveryGroup
                                             from d in deliveryGroup.DefaultIfEmpty()
                                             where d == null
                                                   && o.Status == "Paid"  
                                                   && o.Status != "Completed"
                                                   && o.Status != "Cancelled"
                                             select new OrderDto
                                             {
                                                 OrderID = o.OrderID,
                                                 CustomerName = o.CustomerName,
                                                 DeliveryAddress = o.DeliveryAddress,
                                                 CustomerPhone = o.CustomerPhone,
                                                 DeliveryDate = o.DeliveryDate
                                             }).ToList();

                if (ordersWithoutDelivery.Count == 0)
                {
                    MessageBox.Show("No orders available for delivery.\n\n" +
                                   "Please ensure:\n" +
                                   "1. Orders have 'Paid' status\n" +
                                   "2. Orders do not have an existing delivery note",
                                   "Information",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);

                    cmbOrderID.DataSource = null;
                    cmbOrderID.Enabled = false;
                    button4.Enabled = false;
                    return;
                }

                cmbOrderID.DataSource = ordersWithoutDelivery;
                cmbOrderID.DisplayMember = "Display";
                cmbOrderID.ValueMember = "OrderID";
                cmbOrderID.SelectedIndex = -1;
                cmbOrderID.Enabled = true;
                button4.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load orders: " + ex.Message);
            }
        }

        public class OrderDto
        {
            public string OrderID { get; set; }
            public string CustomerName { get; set; }
            public string DeliveryAddress { get; set; }
            public string CustomerPhone { get; set; }
            public DateTime? DeliveryDate { get; set; }

            public string Display
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

        private void cmbOrderID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOrderID.SelectedIndex == -1 || cmbOrderID.SelectedItem == null)
            {
                ClearForm();
                return;
            }

            var selectedOrder = (OrderDto)cmbOrderID.SelectedItem;

            textBox4.Text = selectedOrder.CustomerName;
            textBox2.Text = selectedOrder.CustomerPhone;
            textBox1.Text = selectedOrder.DeliveryAddress;

            if (selectedOrder.DeliveryDate != null)
            {
                dateTimePicker1.Value = selectedOrder.DeliveryDate.Value;
            }
            else
            {
                dateTimePicker1.Value = DateTime.Now.AddDays(7);
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

                if (items.Count() > 0)
                {
                    decimal total = items.Sum(i => i.SubTotal ?? 0);
                    lblTotalAmount.Text = $"Total Amount: {total:C2}";
                }
                else
                {
                    lblTotalAmount.Text = "Total Amount: $0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load order items: " + ex.Message);
                dgvOrderItems.DataSource = null;
                lblTotalAmount.Text = "Total Amount: $0.00";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (cmbOrderID.SelectedItem == null)
            {
                MessageBox.Show("Please select an order first");
                return;
            }

            var selectedOrder = (OrderDto)cmbOrderID.SelectedItem;

            Dictionary<string, string> headerInfo = new Dictionary<string, string>
            {
                { "Delivery Note ID", GenerateDeliveryID() },
                { "Order ID", selectedOrder.OrderID },
                { "Customer Name", textBox4.Text },
                { "Customer Phone", textBox2.Text },
                { "Delivery Address", textBox1.Text },
                { "Delivery Date", dateTimePicker1.Value.ToString("yyyy-MM-dd") },
                { "Created By", staffName }
            };

            PDFPrintHelper.ExportToPDF("Delivery Note", headerInfo, dgvOrderItems);
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

        private string GenerateDeliveryID()
        {
            string prefix = "DV";
            string deliveryID = "";
            Random random = new Random();
            bool isUnique = false;

            while (!isUnique)
            {
                int randomNumber = random.Next(10000000, 99999999);
                deliveryID = prefix + randomNumber.ToString();

                bool exists = db.delivery.Any(d => d.DeliveryID == deliveryID);

                if (!exists)
                {
                    isUnique = true;
                }
            }

            return deliveryID;
        }

        private void ClearForm()
        {
            if (cmbOrderID.SelectedIndex != -1)
            {
                cmbOrderID.SelectedIndex = -1;
            }

            textBox4.Text = "";
            textBox2.Text = "";
            textBox1.Text = "";
            dateTimePicker1.Value = DateTime.Now.AddDays(7);
            dgvOrderItems.DataSource = null;
            lblTotalAmount.Text = "Total Amount: $0.00";
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void dgvOrderItems_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (cmbOrderID.SelectedItem == null)
            {
                MessageBox.Show("Please select an order first");
                return;
            }

            button4.Enabled = false;

            try
            {
                var selectedOrder = (OrderDto)cmbOrderID.SelectedItem;

                
                var order = db.orders.FirstOrDefault(o => o.OrderID == selectedOrder.OrderID);
                if (order != null && order.Status != "Paid")
                {
                    MessageBox.Show($"Order {selectedOrder.OrderID} is not paid.\n\n" +
                                   "Only paid orders can create delivery notes.",
                                   "Invalid Order Status",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                    button4.Enabled = true;
                    return;
                }

                bool existingDelivery = db.delivery.Any(d => d.OrderID == selectedOrder.OrderID);

                if (existingDelivery)
                {
                    MessageBox.Show($"Order {selectedOrder.OrderID} already has a delivery note!\nEach order can only create one delivery note.",
                                   "Duplicate Delivery Note",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                    button4.Enabled = true;
                    return;
                }

                string deliveryID = GenerateDeliveryID();

                delivery newDelivery = new delivery
                {
                    DeliveryID = deliveryID,
                    OrderID = selectedOrder.OrderID,
                    DeliveryDate = dateTimePicker1.Value,
                    DeliveryStatus = "Pending",
                    HandledBy = staffName
                };

                db.delivery.Add(newDelivery);
                db.SaveChanges();

                MessageBox.Show($"✅ Delivery note saved!\n\n" +
                               $"📦 Delivery ID: {deliveryID}\n" +
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
                MessageBox.Show("❌ Save failed: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                button4.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }

        private void lblTotalAmount_Click(object sender, EventArgs e)
        {

        }
    }
}