using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITP4915M_Project
{
    public partial class OrderDetails : Form
    {
        string orderID;
        public OrderDetails(string id)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            orderID = id;

        }


        private void OrderDetails_Load(object sender, EventArgs e)
        {
            var db = new projectEntities3();

            var order = db.orders.FirstOrDefault(o => o.OrderID == orderID);
            var items = db.order_items.Where(i => i.OrderID == orderID).ToList();


            if (order != null)
            {
                label1.Text = "Order ID: " + order.OrderID;
                label2.Text = "Customer: " + order.CustomerName;
                label3.Text = "Phone: " + order.CustomerPhone;


                textBox1.Text = order.DeliveryAddress;
                dateTimePicker1.Value = order.DeliveryDate ?? DateTime.Now;
            }


            flowLayoutPanel1.Controls.Clear();

            foreach (var item in items)
            {
                AddItemRow(item);
            }



            CalculateTotal();
        }


        private void CalculateTotal()
        {
            decimal total = 0;

            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                Panel row = ctrl as Panel;

                Label lblSub = row.Controls.OfType<Label>().Last();

                string text = lblSub.Text.Replace("$", "");

                if (decimal.TryParse(text, out decimal sub))
                {
                    total += sub;
                }
            }

            label7.Text = "Total: $" + total;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {
            var db = new projectEntities3();

            var order = db.orders.FirstOrDefault(o => o.OrderID == orderID);

            if (order != null)

            {

                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    Panel row = ctrl as Panel;

                    ComboBox cb = row.Controls.OfType<ComboBox>().First();
                    var p = cb.SelectedItem as product;

                    if (p == null || string.IsNullOrEmpty(p.ProductID))
                    {
                        MessageBox.Show("Please select a product ❗");
                        return;
                    }
                }

                DateTime orderDate = order.OrderDate ?? DateTime.Now;
                DateTime deliveryDate = dateTimePicker1.Value;

                if (deliveryDate < orderDate.AddDays(10))
                {
                    MessageBox.Show("Delivery date must be at least 10 days after order date ❌");
                    return;
                }

                order.DeliveryAddress = textBox1.Text;
                order.DeliveryDate = dateTimePicker1.Value;


                var oldItems = db.order_items.Where(i => i.OrderID == orderID);

                foreach (var item in oldItems)
                {
                    db.order_items.Remove(item);
                }


                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    Panel row = ctrl as Panel;

                    ComboBox cb = row.Controls.OfType<ComboBox>().First();
                    NumericUpDown num = row.Controls.OfType<NumericUpDown>().First();

                    var p = cb.SelectedItem as product;
                    if (p == null) continue;

                    order_items item = new order_items
                    {
                        OrderID = orderID,
                        ProductID = p.ProductID,
                        ProductName = p.ProductName,
                        Qty = (int)num.Value,
                        UnitPrice = p.unitPrice,
                        SubTotal = p.unitPrice * (int)num.Value
                    };

                    db.order_items.Add(item);
                }

                db.SaveChanges();

                MessageBox.Show("Updated ✅");
                this.Close();
            }
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        private void AddItemRow(order_items item = null)
        {
            Panel row = new Panel();
            row.Width = 550;
            row.Height = 40;
            row.Margin = new Padding(2);

            row.MouseWheel += (s, e) =>
            {
                flowLayoutPanel1.Focus();
            };


            ComboBox cb = new ComboBox();
            cb.Width = 150;
            cb.Location = new Point(10, 5);
            cb.DropDownStyle = ComboBoxStyle.DropDownList;


            Label lblQty = new Label();
            lblQty.Text = "Qty:";
            lblQty.AutoSize = true;
            lblQty.Location = new Point(170, 8);

            NumericUpDown num = new NumericUpDown();
            num.Width = 60;
            num.Minimum = 1;
            num.Location = new Point(210, 5);
            num.MouseWheel += (s, e) =>
            {
                ((HandledMouseEventArgs)e).Handled = true;
            };


            Label lblPrice = new Label();
            lblPrice.Width = 80;
            lblPrice.Location = new Point(290, 8);

            Label lblSub = new Label();
            lblSub.Width = 100;
            lblSub.Location = new Point(380, 8);

            Button btnDel = new Button();
            btnDel.Text = "-";
            btnDel.Width = 50;
            btnDel.Location = new Point(480, 5);


            var db = new projectEntities3();
            var productList = db.product.ToList();

            productList.Insert(0, new product
            {
                ProductID = "",
                ProductName = ""
            });


            cb.DataSource = productList;
            cb.DisplayMember = "ProductName";
            cb.ValueMember = "ProductID";
            cb.BindingContext = new BindingContext();
            cb.SelectedIndex = 0;


            if (item != null)
            {
                var selected = productList
                    .FirstOrDefault(p => p.ProductID == item.ProductID);

                cb.SelectedItem = selected;

                num.Value = item.Qty.HasValue && item.Qty.Value > 0
                            ? item.Qty.Value
                            : 1;

                lblPrice.Text = "$" + item.UnitPrice;
                lblSub.Text = "$" + (item.SubTotal ?? 0);


            }


            num.ValueChanged += (s, e) =>
            {
                var p = cb.SelectedItem as product;
                if (p == null) return;

                decimal sub = p.unitPrice * num.Value;
                lblSub.Text = "$" + sub;

                CalculateTotal();
            };



            cb.SelectedIndexChanged += (s, e) =>
            {
                var p = cb.SelectedItem as product;
                if (p == null) return;
                num.Maximum = p.Qty;
                if (num.Value > num.Maximum)
                {
                    num.Value = num.Maximum;
                }
                lblPrice.Text = "$" + p.unitPrice;
                lblSub.Text = "$" + (p.unitPrice * num.Value);
                CalculateTotal();
            };



            btnDel.Click += (s, e) =>
            {
                if (flowLayoutPanel1.Controls.Count <= 1)
                {
                    MessageBox.Show("At least 1 product required ❗");
                    return;
                }

                flowLayoutPanel1.Controls.Remove(row);
                CalculateTotal();
            };



            row.Controls.Add(cb);
            row.Controls.Add(lblQty);
            row.Controls.Add(num);
            row.Controls.Add(lblPrice);
            row.Controls.Add(lblSub);
            row.Controls.Add(btnDel);

            flowLayoutPanel1.Controls.Add(row);
        }


        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddItemRow();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {

        }
    }
}

