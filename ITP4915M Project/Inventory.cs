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
    public partial class Inventory : Form
    {
        private string staffName;
        private string employeeID;
        private string employeePhone;

        
        private class ProcurementCartItem
        {
            public rawmaterials Material { get; set; }
            public int Quantity { get; set; }
        }

        private List<ProcurementCartItem> procurementCart = new List<ProcurementCartItem>();
        private Panel procurementCartContainer;
        private Label procurementCartTotalLabel;
        private bool isProcurementInitialized = false;

        
        private Button currentSelectedButton = null;

        public Inventory(string name)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.staffName = name;
            label2.Text = name;
            LoadEmployeeData(name);

            
            this.dataGridView3.CellClick -= new DataGridViewCellEventHandler(this.dataGridView3_CellClick);
            this.dataGridView3.CellClick += new DataGridViewCellEventHandler(this.dataGridView3_CellClick);

           
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

            button9.Click -= button9_Click;
            button9.Click += button9_Click;

            button10.Click -= button10_Click;
            button10.Click += button10_Click;

            button11.Click -= button11_Click;
            button11.Click += button11_Click;

            button12.Click -= button12_Click;
            button12.Click += button12_Click;

            button13.Click -= button13_Click;
            button13.Click += button13_Click;

            button14.Click -= button14_Click;
            button14.Click += button14_Click;

            
            textBox1.KeyPress -= textBox1_KeyPress;
            textBox1.KeyPress += textBox1_KeyPress;

            textBox2.KeyPress -= textBox2_KeyPress;
            textBox2.KeyPress += textBox2_KeyPress;
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

            button5.BackColor = defaultColor;
            button5.ForeColor = defaultForeColor;

            button9.BackColor = defaultColor;
            button9.ForeColor = defaultForeColor;
        }

        
        private void LoadEmployeeData(string name)
        {
            try
            {
                var db = new projectEntities3();
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
            catch
            {
                employeeID = "N/A";
                employeePhone = "N/A";
            }
        }

        private void Inventory_Load(object sender, EventArgs e)
        {
           
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;

            RefreshProductGrid();
            LoadInwardDropdowns();
            LoadProcurementDropdowns();

           
            SetActiveButton(button1);
        }

        
        private void LoadProcurementDropdowns()
        {
            try
            {
                var db = new projectEntities3();
                var materials = db.rawmaterials.ToList();
                comboBox3.DataSource = materials;
                comboBox3.DisplayMember = "MaterialName";
                comboBox3.ValueMember = "MaterialID";
                comboBox3.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading materials: " + ex.Message);
            }
        }

        
        private void button9_Click(object sender, EventArgs e)
        {
            SetActiveButton(button9);  

            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel5.Visible = true;
            panel6.Visible = false;
            panel7.Visible = false;
            LoadProcurementGrid();
        }

       
        private void LoadProcurementGrid()
        {
            try
            {
                var db = new projectEntities3();

              
                var procurements = db.procurement
                    .Select(p => new
                    {
                        p.ProcurementID,
                        p.EmployeeID,
                        p.DeliveryDate,
                        p.Status,
                        p.TotalItems,
                        p.CreatedDate,
                        p.CreatedBy
                    })
                    .OrderByDescending(p => p.CreatedDate)
                    .ToList();

                dataGridView3.DataSource = procurements;

               
                AddViewButtonToDataGridView3();

                if (procurements.Count == 0)
                {
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading procurement data: " + ex.Message);
            }
        }

        
        private void AddViewButtonToDataGridView3()
        {
            
            if (dataGridView3.Columns.Contains("View"))
            {
                dataGridView3.Columns.Remove("View");
            }

            DataGridViewButtonColumn btnView = new DataGridViewButtonColumn();
            btnView.Name = "View";
            btnView.HeaderText = "";
            btnView.Text = "📄 View";
            btnView.UseColumnTextForButtonValue = true;
            btnView.Width = 100;
            dataGridView3.Columns.Insert(0, btnView);

            
            btnView.Frozen = true;

           
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            
            if (dataGridView3.Columns[e.ColumnIndex].Name == "View")
            {
                string procurementID = dataGridView3.Rows[e.RowIndex]
                    .Cells["ProcurementID"].Value?.ToString();

                if (string.IsNullOrEmpty(procurementID)) return;

                ShowProcurementDetails(procurementID);
            }
        }

       
        private void ShowProcurementDetails(string procurementID)
        {
            try
            {
                var db = new projectEntities3();

                
                var procurement = db.procurement
                    .FirstOrDefault(p => p.ProcurementID == procurementID);

                if (procurement == null)
                {
                    MessageBox.Show("Procurement record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

               
                var items = db.procurement_items
                    .Where(i => i.ProcurementID == procurementID)
                    .Select(i => new
                    {
                        i.MaterialName,
                        i.Quantity
                    })
                    .ToList();

                
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"PROCUREMENT DETAILS");
                sb.AppendLine($"Procurement ID: {procurement.ProcurementID}");
                sb.AppendLine($"Employee ID:    {procurement.EmployeeID}");
                sb.AppendLine($"Status:         {procurement.Status}");
                sb.AppendLine($"Total Items:    {procurement.TotalItems}");
                sb.AppendLine($"Delivery Date:  {procurement.DeliveryDate?.ToString("yyyy-MM-dd") ?? "N/A"}");
                sb.AppendLine($"Created Date:   {procurement.CreatedDate?.ToString("yyyy-MM-dd HH:mm") ?? "N/A"}");
                sb.AppendLine($"Created By:     {procurement.CreatedBy}");

                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        sb.AppendLine($"{item.MaterialName} x{item.Quantity}");
                    }
                }
                else
                {
                    sb.AppendLine("No items found.");
                }

                MessageBox.Show(sb.ToString(), "Procurement Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load procurement details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void button11_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel5.Visible = false;
            panel6.Visible = true;
            panel7.Visible = false;

            if (!isProcurementInitialized)
            {
                LoadProcurementMaterialsToDisplay();
                LoadProcurementCartPanel();
                isProcurementInitialized = true;
            }
            else
            {
                RefreshProcurementMaterialsDisplay();
            }
            procurementCart.Clear();
            UpdateProcurementCartDisplay();
        }

        
        private void LoadProcurementMaterialsToDisplay()
        {
            flowLayoutPanel2.Controls.Clear();
            var db = new projectEntities3();
            var materials = db.rawmaterials.ToList();

            flowLayoutPanel2.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel2.WrapContents = true;
            flowLayoutPanel2.AutoSize = false;
            flowLayoutPanel2.Size = new Size(445, 878);

            foreach (var material in materials)
            {
                Panel materialPanel = CreateProcurementMaterialPanel(material);
                flowLayoutPanel2.Controls.Add(materialPanel);
            }
        }

        private void RefreshProcurementMaterialsDisplay()
        {
            LoadProcurementMaterialsToDisplay();
        }

        
        private Panel CreateProcurementMaterialPanel(rawmaterials material)
        {
            Panel materialPanel = new Panel();
            materialPanel.Width = 200;
            materialPanel.Height = 200;
            materialPanel.BorderStyle = BorderStyle.FixedSingle;
            materialPanel.Tag = material;
            materialPanel.BackColor = Color.White;
            materialPanel.Margin = new Padding(3);

           
            Label lblName = new Label();
            lblName.Text = material.MaterialName;
            lblName.Font = new Font("Arial", 10, FontStyle.Bold);
            lblName.TextAlign = ContentAlignment.MiddleCenter;
            lblName.Size = new Size(180, 30);
            lblName.Location = new Point(10, 10);
            materialPanel.Controls.Add(lblName);

            Label lblCategory = new Label();
            lblCategory.Text = $"Category: {material.Category}";
            lblCategory.Font = new Font("Arial", 8);
            lblCategory.Size = new Size(180, 20);
            lblCategory.Location = new Point(10, 45);
            materialPanel.Controls.Add(lblCategory);

            
            Label lblStock = new Label();
            lblStock.Text = $"Stock: {material.StockQty}";
            lblStock.Font = new Font("Arial", 8, FontStyle.Bold);
            lblStock.ForeColor = material.StockQty < 10 ? Color.Red : Color.Green;
            lblStock.Size = new Size(180, 20);
            lblStock.Location = new Point(10, 68);
            materialPanel.Controls.Add(lblStock);

           
            Label lblQty = new Label();
            lblQty.Text = "Qty:";
            lblQty.Font = new Font("Arial", 8);
            lblQty.Size = new Size(35, 25);
            lblQty.Location = new Point(10, 95);
            materialPanel.Controls.Add(lblQty);

            NumericUpDown numQty = new NumericUpDown();
            numQty.Minimum = 1;
            numQty.Maximum = 1000;
            numQty.Value = 1;
            numQty.Size = new Size(60, 22);
            numQty.Location = new Point(50, 93);
            numQty.MouseWheel += (s, ev) => { ((HandledMouseEventArgs)ev).Handled = true; };
            materialPanel.Controls.Add(numQty);

            
            Button btnAddToCart = new Button();
            btnAddToCart.Text = "+ Add";
            btnAddToCart.BackColor = Color.LightBlue;
            btnAddToCart.Font = new Font("Arial", 8, FontStyle.Bold);
            btnAddToCart.Size = new Size(80, 28);
            btnAddToCart.Location = new Point(110, 92);
            btnAddToCart.Tag = material;
            btnAddToCart.Click += (s, ev) =>
            {
                AddToProcurementCart(material, (int)numQty.Value);
            };
            materialPanel.Controls.Add(btnAddToCart);

            return materialPanel;
        }

       
        private void LoadProcurementCartPanel()
        {
            panelCartProcurement.Controls.Clear();

            
            Label lblCartTitle = new Label();
            lblCartTitle.Text = "🛒 Procurement Cart";
            lblCartTitle.Font = new Font("Arial", 9, FontStyle.Bold);
            lblCartTitle.Dock = DockStyle.Top;
            lblCartTitle.Height = 30;
            lblCartTitle.TextAlign = ContentAlignment.MiddleCenter;
            panelCartProcurement.Controls.Add(lblCartTitle);

            
            Panel line = new Panel();
            line.Height = 2;
            line.Dock = DockStyle.Top;
            line.BackColor = Color.Gray;
            panelCartProcurement.Controls.Add(line);

            
            Panel bottomPanel = new Panel();
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = 80;

            procurementCartTotalLabel = new Label();
            procurementCartTotalLabel.Name = "lblCartTotal";
            procurementCartTotalLabel.Text = "Total: 0 items";
            procurementCartTotalLabel.Font = new Font("Arial", 9, FontStyle.Bold);
            procurementCartTotalLabel.ForeColor = Color.Red;
            procurementCartTotalLabel.Location = new Point(10, 5);
            procurementCartTotalLabel.Width = 200;

            Button btnClearCart = new Button();
            btnClearCart.Text = "Clear";
            btnClearCart.Size = new Size(70, 25);
            btnClearCart.Location = new Point(10, 35);
            btnClearCart.Click += (s, ev) => ClearProcurementCart();

            Button btnConfirm = new Button();
            btnConfirm.Text = "Confirm";
            btnConfirm.Size = new Size(90, 25);
            btnConfirm.Location = new Point(90, 35);
            btnConfirm.BackColor = Color.LightGreen;
            btnConfirm.Click += (s, ev) => ConfirmProcurement();

            bottomPanel.Controls.Add(procurementCartTotalLabel);
            bottomPanel.Controls.Add(btnClearCart);
            bottomPanel.Controls.Add(btnConfirm);
            panelCartProcurement.Controls.Add(bottomPanel);

           
            FlowLayoutPanel cartFlowPanel = new FlowLayoutPanel();
            cartFlowPanel.Location = new Point(5, 35);
            cartFlowPanel.Size = new Size(260, 250);
            cartFlowPanel.FlowDirection = FlowDirection.TopDown;
            cartFlowPanel.WrapContents = false;
            cartFlowPanel.AutoScroll = true;
            cartFlowPanel.HorizontalScroll.Enabled = false;
            cartFlowPanel.HorizontalScroll.Visible = false;
            cartFlowPanel.BackColor = Color.WhiteSmoke;
            cartFlowPanel.Padding = new Padding(3);
            panelCartProcurement.Controls.Add(cartFlowPanel);

            procurementCartContainer = cartFlowPanel;
        }

       
        private void AddToProcurementCart(rawmaterials material, int quantity)
        {
            var existingItem = procurementCart.FirstOrDefault(c => c.Material.MaterialID == material.MaterialID);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                procurementCart.Add(new ProcurementCartItem { Material = material, Quantity = quantity });
            }
            UpdateProcurementCartDisplay();
        }

        
        private void UpdateProcurementCartDisplay()
        {
            if (procurementCartContainer == null) return;

            procurementCartContainer.Controls.Clear();
            int totalItems = 0;

            foreach (var item in procurementCart)
            {
                Panel itemPanel = new Panel();
                itemPanel.Size = new Size(150, 50);
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
                lblInfo.Text = $"Qty: {item.Quantity}";
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
                    procurementCart.Remove(item);
                    UpdateProcurementCartDisplay();
                };
                itemPanel.Controls.Add(btnRemove);

                procurementCartContainer.Controls.Add(itemPanel);
                totalItems += item.Quantity;
            }

            if (procurementCartTotalLabel != null)
            {
                procurementCartTotalLabel.Text = $"Total: {totalItems} items";
            }

            procurementCartContainer.PerformLayout();
            procurementCartContainer.Invalidate();
        }

        
        private void ClearProcurementCart()
        {
            procurementCart.Clear();
            UpdateProcurementCartDisplay();
        }

       
        private void ConfirmProcurement()
        {
            if (procurementCart.Count == 0)
            {
                MessageBox.Show("Please add at least one material to cart!", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            panel6.Visible = false;
            panel7.Visible = true;

            
            textBox6.Text = staffName;
            textBox7.Text = employeeID;
            textBox8.Text = employeePhone;
            dateTimePicker2.Value = DateTime.Now.AddDays(7);

            
            flowLayoutPanel3.Controls.Clear();

            foreach (var item in procurementCart)
            {
                Panel row = new Panel();
                row.Width = 500;
                row.Height = 35;
                row.Margin = new Padding(3);
                row.BackColor = Color.White;
                row.BorderStyle = BorderStyle.FixedSingle;

                Label lblName = new Label();
                lblName.Text = item.Material.MaterialName;
                lblName.Font = new Font("Arial", 9, FontStyle.Bold);
                lblName.Size = new Size(200, 25);
                lblName.Location = new Point(10, 5);
                row.Controls.Add(lblName);

                Label lblQty = new Label();
                lblQty.Text = $"Qty: {item.Quantity}";
                lblQty.Font = new Font("Arial", 9);
                lblQty.Size = new Size(100, 25);
                lblQty.Location = new Point(250, 5);
                row.Controls.Add(lblQty);

                flowLayoutPanel3.Controls.Add(row);
            }

            
            int total = procurementCart.Sum(c => c.Quantity);
            label14.Text = $"Total Items: {total}";
        }

        
        private void button12_Click(object sender, EventArgs e)
        {
            panel6.Visible = true;
            panel7.Visible = false;
            RefreshProcurementMaterialsDisplay();
        }

        
        private void button13_Click(object sender, EventArgs e)
        {
            if (procurementCart.Count == 0)
            {
                MessageBox.Show("No items to save! Please add materials to cart.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                
                string procurementID = GenerateProcurementID();

               
                int totalItems = procurementCart.Sum(c => c.Quantity);

                var db = new projectEntities3();

                
                procurement newProcurement = new procurement
                {
                    ProcurementID = procurementID,
                    EmployeeID = employeeID,
                    DeliveryDate = dateTimePicker2.Value,
                    Status = "Pending",
                    TotalItems = totalItems,
                    CreatedDate = DateTime.Now,
                    CreatedBy = staffName
                };

                db.procurement.Add(newProcurement);

                
                foreach (var item in procurementCart)
                {
                    procurement_items detail = new procurement_items
                    {
                        ProcurementID = procurementID,
                        MaterialID = item.Material.MaterialID,
                        MaterialName = item.Material.MaterialName,
                        Quantity = item.Quantity
                    };
                    db.procurement_items.Add(detail);
                }

                
                db.SaveChanges();

                MessageBox.Show(
                    $"Procurement order saved successfully! ✅\n\n" +
                    $"Procurement ID: {procurementID}\n" +
                    $"Employee: {staffName}\n" +
                    $"Total Items: {totalItems}\n" +
                    $"Delivery Date: {dateTimePicker2.Value:yyyy-MM-dd}",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                
                procurementCart.Clear();
                UpdateProcurementCartDisplay();
                isProcurementInitialized = false;

               
                panel6.Visible = false;
                panel7.Visible = false;
                panel5.Visible = true;
                LoadProcurementGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save procurement order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void button14_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel3.Controls.Count == 0)
            {
                MessageBox.Show("No items to export!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                
                Dictionary<string, string> headerInfo = new Dictionary<string, string>
                {
                    { "Procurement Order ID", GenerateProcurementID() },
                    { "Employee Name", staffName },
                    { "Employee ID", employeeID },
                    { "Phone Number", employeePhone },
                    { "Delivery Date", dateTimePicker2.Value.ToString("yyyy-MM-dd") },
                    { "Created Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm") },
                    { "Created By", staffName }
                };

                
                DataGridView tempGrid = new DataGridView();
                tempGrid.AutoGenerateColumns = true;
                tempGrid.Columns.Clear();

                
                DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
                colName.Name = "MaterialName";
                colName.HeaderText = "Material Name";
                tempGrid.Columns.Add(colName);

                DataGridViewTextBoxColumn colQty = new DataGridViewTextBoxColumn();
                colQty.Name = "Quantity";
                colQty.HeaderText = "Quantity";
                tempGrid.Columns.Add(colQty);

                
                int total = 0;
                foreach (var item in procurementCart)
                {
                    tempGrid.Rows.Add(item.Material.MaterialName, item.Quantity);
                    total += item.Quantity;
                }
                tempGrid.Rows.Add("TOTAL ITEMS:", total);

               
                PDFPrintHelper.ExportToPDF("PROCUREMENT ORDER", headerInfo, tempGrid);

                MessageBox.Show($"Procurement order exported to PDF successfully! ✅", "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private string GenerateProcurementID()
        {
            string prefix = "PO";
            Random random = new Random();
            int randomNumber = random.Next(10000000, 99999999);
            return prefix + randomNumber.ToString();
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
                dataGridView1.DataSource = products;
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

                dataGridView1.DataSource = result;

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

        
        private void button1_Click(object sender, EventArgs e)
        {
            SetActiveButton(button1);  

            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            RefreshProductGrid();
            textBox1.Clear();
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            string keyword = textBox1.Text.Trim();
            SearchProduct(keyword);
        }

       
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;
                panel5.Visible = false;
                panel6.Visible = false;
                panel7.Visible = false;
                string keyword = textBox1.Text.Trim();
                SearchProduct(keyword);
                e.Handled = true;
            }
        }

       

        private void RefreshRawMaterialGrid()
        {
            try
            {
                var db = new projectEntities3();
                var materials = db.rawmaterials
                    .Select(r => new
                    {
                        r.MaterialID,
                        r.MaterialName,
                        r.Category,
                        Stock = r.StockQty
                    })
                    .ToList();
                dataGridView2.DataSource = materials;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading raw materials: " + ex.Message);
            }
        }

        private void SearchRawMaterial(string keyword)
        {
            try
            {
                var db = new projectEntities3();

                var result = db.rawmaterials
                    .Where(r => string.IsNullOrEmpty(keyword) ||
                                r.MaterialID.Contains(keyword) ||
                                r.MaterialName.Contains(keyword))
                    .Select(r => new
                    {
                        r.MaterialID,
                        r.MaterialName,
                        r.Category,
                        Stock = r.StockQty
                    })
                    .ToList();

                dataGridView2.DataSource = result;

                if (result.Count == 0)
                {
                    MessageBox.Show("No raw materials found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching: " + ex.Message);
            }
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            SetActiveButton(button3); 

            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            RefreshRawMaterialGrid();
            textBox2.Clear();
        }

        
        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            string keyword = textBox2.Text.Trim();
            SearchRawMaterial(keyword);
        }

       
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                panel1.Visible = false;
                panel2.Visible = true;
                panel3.Visible = false;
                panel5.Visible = false;
                panel6.Visible = false;
                panel7.Visible = false;
                string keyword = textBox2.Text.Trim();
                SearchRawMaterial(keyword);
                e.Handled = true;
            }
        }

       

        private void LoadInwardDropdowns()
        {
            try
            {
                var db = new projectEntities3();
                var rawItems = db.rawmaterials
                    .Select(r => new { r.MaterialID, r.MaterialName, r.StockQty })
                    .ToList();

                comboBox1.DataSource = rawItems;
                comboBox1.DisplayMember = "MaterialName";
                comboBox1.ValueMember = "MaterialID";
                comboBox1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading raw materials: " + ex.Message);
            }
        }

        
        private void button5_Click(object sender, EventArgs e)
        {
            SetActiveButton(button5); 

            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            LoadInwardDropdowns();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
        }

        
        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select a raw material! ❌", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBox3.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Please enter a valid positive quantity! ❌", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string selectedMaterialID = comboBox1.SelectedValue.ToString();
                var db = new projectEntities3();

                var material = db.rawmaterials.FirstOrDefault(r => r.MaterialID == selectedMaterialID);

                if (material != null)
                {
                    material.StockQty += qty;
                    db.SaveChanges();

                    MessageBox.Show(
                        $"Raw Material Inward Recorded! ✅\n" +
                        $"Material: {material.MaterialName}\n" +
                        $"Qty Added: {qty}\n" +
                        $"New Stock Level: {material.StockQty}",
                        "Inward Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    textBox3.Clear();
                    comboBox1.SelectedIndex = -1;

                    if (panel2.Visible)
                    {
                        RefreshRawMaterialGrid();
                    }
                }
                else
                {
                    MessageBox.Show("Material not found! ❌", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to record inward transaction: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void button10_Click(object sender, EventArgs e)
        {
            string keyword = textBox5.Text.Trim();
            try
            {
                var db = new projectEntities3();

                
                var procurements = db.procurement
                    .Where(p => string.IsNullOrEmpty(keyword) ||
                                p.ProcurementID.Contains(keyword) ||
                                p.EmployeeID.Contains(keyword) ||
                                p.CreatedBy.Contains(keyword))
                    .Select(p => new
                    {
                        p.ProcurementID,
                        p.EmployeeID,
                        p.DeliveryDate,
                        p.Status,
                        p.TotalItems,
                        p.CreatedDate,
                        p.CreatedBy
                    })
                    .OrderByDescending(p => p.CreatedDate)
                    .ToList();

                dataGridView3.DataSource = procurements;

                
                AddViewButtonToDataGridView3();

                if (procurements.Count == 0)
                {
                    MessageBox.Show("No procurement records found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message);
            }
        }

        
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

       
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }


    }
}