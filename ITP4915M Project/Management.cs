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
    public partial class Management : Form
    {
        private string staffName;

        
        private Button currentSelectedButton = null;

        public Management(string name)
        {
            InitializeComponent();
            this.staffName = name;
            label1.Text = name;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

           
            button1.Click -= button1_Click;
            button1.Click += button1_Click;

            button2.Click -= button2_Click;
            button2.Click += button2_Click;
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
        }

        private void Management_Load(object sender, EventArgs e)
        {
            
            panel1.AutoScroll = true;
            panel1.AutoScrollMinSize = new Size(0, 0);

           
            panel2.Visible = false;
            panel1.Visible = true;

            LoadSalesPieChart();

            
            SetActiveButton(button1);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetActiveButton(button1);  
            panel1.Visible = true;
            panel2.Visible = false;
            LoadSalesPieChart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetActiveButton(button2);  
            panel1.Visible = false;
            panel2.Visible = true;
            panel2.BringToFront();
            LoadDailyOrderChart();
        }

        private void LoadSalesPieChart()
        {
            panel1.Controls.Clear();
            panel1.AutoScrollMinSize = new Size(0, 0);  

            try
            {
                var db = new projectEntities3();

               
                var salesData = (from item in db.order_items
                                 join product in db.product on item.ProductID equals product.ProductID
                                 where item.ProductID != null && item.ProductID != ""
                                 group item by new { item.ProductID, item.ProductName } into g
                                 select new
                                 {
                                     ProductName = g.Key.ProductName,
                                     TotalQuantity = g.Sum(x => x.Qty ?? 0)
                                 })
                                 .OrderByDescending(x => x.TotalQuantity)
                                 .ToList();

                int totalItems = salesData.Sum(x => x.TotalQuantity);

               
                Panel mainContainer = new Panel();
                mainContainer.AutoSize = true;
                mainContainer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                mainContainer.BackColor = Color.White;
                mainContainer.Location = new Point(0, 0);
                mainContainer.Padding = new Padding(10);

                
                PictureBox picChart = new PictureBox();
                int chartWidth = panel1.Width - 60;
                int chartHeight = 400;
                picChart.Size = new Size(chartWidth, chartHeight);
                picChart.BackColor = Color.White;
                picChart.SizeMode = PictureBoxSizeMode.AutoSize;
                picChart.Paint += (s, e) =>
                {
                   
                    DrawPieChart(e.Graphics, salesData, totalItems, picChart.Width, picChart.Height);
                };

                mainContainer.Controls.Add(picChart);

                FlowLayoutPanel legendPanel = new FlowLayoutPanel();
                legendPanel.FlowDirection = FlowDirection.TopDown;
                legendPanel.WrapContents = true;
                legendPanel.AutoSize = true;
                legendPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                legendPanel.Padding = new Padding(20, 15, 20, 20);
                legendPanel.BackColor = Color.White;
                legendPanel.Location = new Point(0, chartHeight + 15);
                legendPanel.Width = chartWidth;
                legendPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                
                Color[] colors = {
                    Color.FromArgb(52, 152, 219),   
                    Color.FromArgb(46, 204, 113),   
                    Color.FromArgb(231, 76, 60),    
                    Color.FromArgb(241, 196, 15),   
                    Color.FromArgb(155, 89, 182),   
                    Color.FromArgb(230, 126, 34),   
                    Color.FromArgb(26, 188, 156),   
                    Color.FromArgb(149, 165, 166), 
                    Color.FromArgb(192, 57, 43),    
                    Color.FromArgb(44, 62, 80)     
                };

                int colorIndex = 0;
                int legendItemWidth = chartWidth - 60;

                
                Label lblLegendTitle = new Label();
                lblLegendTitle.Text = "📋 Product Legend";
                lblLegendTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblLegendTitle.ForeColor = Color.FromArgb(44, 62, 80);
                lblLegendTitle.AutoSize = true;
                lblLegendTitle.Location = new Point(20, 0);
                legendPanel.Controls.Add(lblLegendTitle);

                foreach (var item in salesData)
                {
                    float percentage = (float)item.TotalQuantity / totalItems * 100;
                    Color sliceColor = colors[colorIndex % colors.Length];

                    
                    Panel legendItem = new Panel();
                    legendItem.Width = legendItemWidth;
                    legendItem.Height = 35;
                    legendItem.Margin = new Padding(3, 2, 3, 2);
                    legendItem.BackColor = Color.White;
                    legendItem.BorderStyle = BorderStyle.None;

                    
                    Panel colorBox = new Panel();
                    colorBox.Size = new Size(20, 20);
                    colorBox.Location = new Point(5, 7);
                    colorBox.BackColor = sliceColor;
                    colorBox.BorderStyle = BorderStyle.FixedSingle;
                    legendItem.Controls.Add(colorBox);

                    
                    Label lblName = new Label();
                    string name = item.ProductName;
                    if (name.Length > 20) name = name.Substring(0, 18) + "...";
                    lblName.Text = name;
                    lblName.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    lblName.AutoSize = true;
                    lblName.Location = new Point(32, 2);
                    lblName.ForeColor = Color.FromArgb(44, 62, 80);
                    legendItem.Controls.Add(lblName);

                    
                    Label lblQty = new Label();
                    lblQty.Text = $"{item.TotalQuantity} units";
                    lblQty.Font = new Font("Segoe UI", 8, FontStyle.Regular);
                    lblQty.AutoSize = true;
                    lblQty.Location = new Point(32, 20);
                    lblQty.ForeColor = Color.FromArgb(100, 100, 100);
                    legendItem.Controls.Add(lblQty);

                    
                    Label lblPercent = new Label();
                    lblPercent.Text = $"{percentage:F1}%";
                    lblPercent.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    lblPercent.AutoSize = true;
                    lblPercent.Location = new Point(legendItemWidth - 70, 8);
                    lblPercent.ForeColor = sliceColor;
                    lblPercent.TextAlign = ContentAlignment.MiddleRight;
                    legendItem.Controls.Add(lblPercent);

                    
                    Panel progressBar = new Panel();
                    progressBar.Width = 80;
                    progressBar.Height = 6;
                    progressBar.Location = new Point(legendItemWidth - 155, 22);
                    progressBar.BackColor = Color.FromArgb(230, 230, 230);
                    progressBar.BorderStyle = BorderStyle.None;

                    Panel progressFill = new Panel();
                    int fillWidth = (int)((double)item.TotalQuantity / totalItems * 80);
                    if (fillWidth < 1) fillWidth = 1;
                    progressFill.Width = fillWidth;
                    progressFill.Height = 6;
                    progressFill.Location = new Point(0, 0);
                    progressFill.BackColor = sliceColor;
                    progressBar.Controls.Add(progressFill);

                    legendItem.Controls.Add(progressBar);

                    legendPanel.Controls.Add(legendItem);
                    colorIndex++;
                }

                mainContainer.Controls.Add(legendPanel);

                
                panel1.Controls.Add(mainContainer);

                
                panel1.AutoScrollMinSize = new Size(0, 0);

               
                Button refreshBtn = new Button();
                refreshBtn.Text = "🔄 Refresh";
                refreshBtn.BackColor = Color.FromArgb(52, 152, 219);
                refreshBtn.ForeColor = Color.White;
                refreshBtn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                refreshBtn.FlatStyle = FlatStyle.Flat;
                refreshBtn.FlatAppearance.BorderSize = 0;
                refreshBtn.Size = new Size(120, 35);
                refreshBtn.Location = new Point(panel1.Width - 140, 10);
                refreshBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                refreshBtn.Click += (s, ev) => LoadSalesPieChart();
                panel1.Controls.Add(refreshBtn);
                refreshBtn.BringToFront();

                
                panel1.PerformLayout();
                panel1.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load sales data: " + ex.Message);
            }
        }

       
        private void DrawPieChart(Graphics g, IEnumerable<dynamic> salesData, int totalItems, int width, int height)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.Clear(Color.White);

            if (!salesData.Any() || totalItems == 0)
            {
                using (Font font = new Font("Arial", 18, FontStyle.Bold))
                using (Brush brush = new SolidBrush(Color.Gray))
                {
                    string msg = "No Sales Data Available";
                    SizeF size = g.MeasureString(msg, font);
                    float x = (width - size.Width) / 2;
                    float y = (height - size.Height) / 2 - 30;
                    g.DrawString(msg, font, brush, x, y);
                }
                return;
            }

            // Define colors
            Color[] colors = {
                Color.FromArgb(52, 152, 219),   
                Color.FromArgb(46, 204, 113),  
                Color.FromArgb(231, 76, 60),    
                Color.FromArgb(241, 196, 15),   
                Color.FromArgb(155, 89, 182),   
                Color.FromArgb(230, 126, 34),   
                Color.FromArgb(26, 188, 156),   
                Color.FromArgb(149, 165, 166),  
                Color.FromArgb(192, 57, 43),    
                Color.FromArgb(44, 62, 80)      
            };

           
            int pieSize = Math.Min(width - 100, height - 160);
            int centerX = width / 2;
            int centerY = height / 2 + 30;
            Rectangle rect = new Rectangle(centerX - pieSize / 2, centerY - pieSize / 2, pieSize, pieSize);

            
            using (Font titleFont = new Font("Segoe UI", 16, FontStyle.Bold))
            using (Brush titleBrush = new SolidBrush(Color.FromArgb(44, 62, 80)))
            {
                string title = "📊 Product Sales Distribution";
                SizeF titleSize = g.MeasureString(title, titleFont);
                float titleX = (width - titleSize.Width) / 2;
                g.DrawString(title, titleFont, titleBrush, titleX, 20);
            }

            
            using (Font subFont = new Font("Segoe UI", 10, FontStyle.Regular))
            using (Brush subBrush = new SolidBrush(Color.FromArgb(100, 100, 100)))
            {
                string subText = $"Total Products Sold: {totalItems} units";
                SizeF subSize = g.MeasureString(subText, subFont);
                float subX = (width - subSize.Width) / 2;
                g.DrawString(subText, subFont, subBrush, subX, 50);
            }

            float startAngle = 0;
            int colorIndex = 0;

            foreach (var item in salesData)
            {
                float percentage = (float)item.TotalQuantity / totalItems * 100;
                float sweepAngle = percentage * 360 / 100;

                Color sliceColor = colors[colorIndex % colors.Length];

                using (Pen pen = new Pen(Color.White, 2))
                using (Brush brush = new SolidBrush(sliceColor))
                {
                    
                    g.FillPie(brush, rect, startAngle, sweepAngle);
                    g.DrawPie(pen, rect, startAngle, sweepAngle);

                   
                    float midAngle = startAngle + sweepAngle / 2;
                    float midAngleRad = midAngle * (float)Math.PI / 180;

                    float labelRadius = pieSize * 0.48f;
                    float labelX = centerX + (float)(labelRadius * Math.Cos(midAngleRad));
                    float labelY = centerY + (float)(labelRadius * Math.Sin(midAngleRad));

                    string labelText = $"{percentage:F1}%";

                    
                    int fontSize = 12;
                    if (percentage < 8) fontSize = 10;
                    if (percentage < 5) fontSize = 8;
                    if (percentage < 3) fontSize = 7;

                    using (Font labelFont = new Font("Arial", fontSize, FontStyle.Bold))
                    using (Brush labelBrush = new SolidBrush(Color.White))
                    using (Brush shadowBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
                    {
                        SizeF labelSize = g.MeasureString(labelText, labelFont);
                        float lx = labelX - labelSize.Width / 2;
                        float ly = labelY - labelSize.Height / 2;

                        g.DrawString(labelText, labelFont, shadowBrush, lx + 1.5f, ly + 1.5f);
                        g.DrawString(labelText, labelFont, labelBrush, lx, ly);
                    }

                   
                    if (percentage >= 10)
                    {
                        float nameRadius = pieSize * 0.60f;
                        float nameX = centerX + (float)(nameRadius * Math.Cos(midAngleRad));
                        float nameY = centerY + (float)(nameRadius * Math.Sin(midAngleRad));

                        string nameText = item.ProductName;
                        if (nameText.Length > 12) nameText = nameText.Substring(0, 10) + "...";

                        using (Font nameFont = new Font("Arial", 6, FontStyle.Bold))
                        using (Brush nameBrush = new SolidBrush(Color.FromArgb(220, 255, 255, 255)))
                        using (Brush nameShadow = new SolidBrush(Color.FromArgb(80, 0, 0, 0)))
                        {
                            SizeF nameSize = g.MeasureString(nameText, nameFont);
                            float nx = nameX - nameSize.Width / 2;
                            float ny = nameY - nameSize.Height / 2;
                            g.DrawString(nameText, nameFont, nameShadow, nx + 1, ny + 1);
                            g.DrawString(nameText, nameFont, nameBrush, nx, ny);
                        }
                    }
                }

                startAngle += sweepAngle;
                colorIndex++;
            }

            
            g.DrawRectangle(new Pen(Color.FromArgb(220, 220, 220), 1), 0, 0, width - 1, height - 1);
        }

        
        private void LoadDailyOrderChart()
        {
            panel2.Controls.Clear();

            try
            {
                var db = new projectEntities3();

               
                var orders = db.orders
                    .Where(o => o.OrderDate != null)
                    .ToList();

                
                var dailyOrders = orders
                    .GroupBy(o => o.OrderDate.Value.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        OrderCount = g.Count()
                    })
                    .OrderBy(x => x.Date)
                    .ToList();

                
                if (dailyOrders.Count == 0)
                {
                    Label lblNoData = new Label();
                    lblNoData.Text = "No order data available";
                    lblNoData.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                    lblNoData.ForeColor = Color.Gray;
                    lblNoData.Dock = DockStyle.Fill;
                    lblNoData.TextAlign = ContentAlignment.MiddleCenter;
                    panel2.Controls.Add(lblNoData);
                    return;
                }

               
                PictureBox picChart = new PictureBox();
                picChart.Dock = DockStyle.Fill;
                picChart.BackColor = Color.White;
                picChart.Paint += (s, e) =>
                {
                    DrawLineChart(e.Graphics, dailyOrders, picChart.Width, picChart.Height);
                };

                panel2.Controls.Add(picChart);

                
                Button refreshBtn = new Button();
                refreshBtn.Text = "🔄 Refresh";
                refreshBtn.BackColor = Color.FromArgb(52, 152, 219);
                refreshBtn.ForeColor = Color.White;
                refreshBtn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                refreshBtn.FlatStyle = FlatStyle.Flat;
                refreshBtn.FlatAppearance.BorderSize = 0;
                refreshBtn.Size = new Size(120, 35);
                refreshBtn.Location = new Point(panel2.Width - 140, 10);
                refreshBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                refreshBtn.Click += (s, ev) => LoadDailyOrderChart();
                panel2.Controls.Add(refreshBtn);
                refreshBtn.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load daily order data: " + ex.Message);
            }
        }

        
        private void DrawLineChart(Graphics g, IEnumerable<dynamic> dailyOrders, int width, int height)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.Clear(Color.White);

            if (dailyOrders == null || !dailyOrders.Any())
            {
                using (Font font = new Font("Arial", 18, FontStyle.Bold))
                using (Brush brush = new SolidBrush(Color.Gray))
                {
                    string msg = "No Order Data Available";
                    SizeF size = g.MeasureString(msg, font);
                    float x = (width - size.Width) / 2;
                    float y = (height - size.Height) / 2;
                    g.DrawString(msg, font, brush, x, y);
                }
                return;
            }

           
            var orderList = dailyOrders.ToList();

            int paddingLeft = 80;
            int paddingRight = 40;
            int paddingTop = 80;        
            int paddingBottom = 60;

            int chartWidth = width - paddingLeft - paddingRight;
            int chartHeight = height - paddingTop - paddingBottom;

            
            int maxOrders = orderList.Max(x => x.OrderCount);
            if (maxOrders == 0) maxOrders = 1;

            
            using (Font titleFont = new Font("Segoe UI", 16, FontStyle.Bold))
            using (Brush titleBrush = new SolidBrush(Color.FromArgb(44, 62, 80)))
            {
                string title = "📈 Daily Order Rate";
                SizeF titleSize = g.MeasureString(title, titleFont);
                float titleX = (width - titleSize.Width) / 2;
                g.DrawString(title, titleFont, titleBrush, titleX, 20); 
            }

            
            int totalOrders = orderList.Sum(x => x.OrderCount);
            using (Font subFont = new Font("Segoe UI", 10, FontStyle.Regular))
            using (Brush subBrush = new SolidBrush(Color.FromArgb(100, 100, 100)))
            {
                string subText = $"Total Orders: {totalOrders}  |  Date Range: {orderList.First().Date:yyyy-MM-dd} to {orderList.Last().Date:yyyy-MM-dd}";
                SizeF subSize = g.MeasureString(subText, subFont);
                float subX = (width - subSize.Width) / 2;
                g.DrawString(subText, subFont, subBrush, subX, 48);  
            }

            
            int ySteps = 5;
            if (maxOrders < 5) ySteps = maxOrders;
            if (ySteps < 3) ySteps = 3;

            using (Pen gridPen = new Pen(Color.FromArgb(220, 220, 220), 1))
            using (Pen axisPen = new Pen(Color.FromArgb(100, 100, 100), 1))
            using (Font labelFont = new Font("Segoe UI", 8))
            using (Brush labelBrush = new SolidBrush(Color.FromArgb(80, 80, 80)))
            {
                for (int i = 0; i <= ySteps; i++)
                {
                    float yPos = paddingTop + chartHeight - (i / (float)ySteps) * chartHeight;
                    int value = (int)(i / (float)ySteps * maxOrders);

                    
                    g.DrawString(value.ToString(), labelFont, labelBrush, 10, yPos - 8);

                    
                    if (i > 0)
                    {
                        g.DrawLine(gridPen, paddingLeft, yPos, paddingLeft + chartWidth, yPos);
                    }
                }

                
                g.DrawLine(axisPen, paddingLeft, paddingTop + chartHeight, paddingLeft + chartWidth, paddingTop + chartHeight);

                
                g.DrawLine(axisPen, paddingLeft, paddingTop, paddingLeft, paddingTop + chartHeight);
            }

            
            if (orderList.Count == 1)
            {
                
                float xPos = paddingLeft + chartWidth / 2;
                float yPos = paddingTop + chartHeight - (orderList[0].OrderCount / (float)maxOrders) * chartHeight;

                using (Pen linePen = new Pen(Color.FromArgb(52, 152, 219), 3))
                using (Brush circleBrush = new SolidBrush(Color.FromArgb(52, 152, 219)))
                {
                    g.DrawEllipse(linePen, xPos - 6, yPos - 6, 12, 12);
                    g.FillEllipse(circleBrush, xPos - 5, yPos - 5, 10, 10);
                }

                
                using (Font labelFont2 = new Font("Segoe UI", 8, FontStyle.Bold))
                using (Brush labelBrush2 = new SolidBrush(Color.FromArgb(44, 62, 80)))
                {
                    string label = $"{orderList[0].Date:yyyy-MM-dd}\n{orderList[0].OrderCount} orders";
                    g.DrawString(label, labelFont2, labelBrush2, xPos - 30, yPos - 35);
                }
            }
            else
            {
               
                List<PointF> points = new List<PointF>();
                float xStep = chartWidth / (float)(orderList.Count - 1);

                for (int i = 0; i < orderList.Count; i++)
                {
                    float xPos = paddingLeft + i * xStep;
                    float yPos = paddingTop + chartHeight - (orderList[i].OrderCount / (float)maxOrders) * chartHeight;
                    points.Add(new PointF(xPos, yPos));
                }

                
                using (Pen linePen = new Pen(Color.FromArgb(52, 152, 219), 3))
                {
                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        g.DrawLine(linePen, points[i], points[i + 1]);
                    }
                }

                
                using (Brush areaBrush = new SolidBrush(Color.FromArgb(50, 52, 152, 219)))
                {
                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        PointF p1 = points[i];
                        PointF p2 = points[i + 1];
                        PointF p3 = new PointF(p2.X, paddingTop + chartHeight);
                        PointF p4 = new PointF(p1.X, paddingTop + chartHeight);

                        PointF[] polyPoints = new PointF[] { p1, p2, p3, p4 };
                        g.FillPolygon(areaBrush, polyPoints);
                    }
                }

                
                using (Pen circlePen = new Pen(Color.FromArgb(52, 152, 219), 2))
                using (Brush circleBrush = new SolidBrush(Color.White))
                using (Brush innerBrush = new SolidBrush(Color.FromArgb(52, 152, 219)))
                using (Font labelFont2 = new Font("Segoe UI", 7, FontStyle.Bold))
                using (Brush labelBrush2 = new SolidBrush(Color.FromArgb(44, 62, 80)))
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        PointF p = points[i];

                        
                        g.DrawEllipse(circlePen, p.X - 6, p.Y - 6, 12, 12);
                        g.FillEllipse(innerBrush, p.X - 4, p.Y - 4, 8, 8);

                       
                        string dateLabel = orderList[i].Date.ToString("MM-dd");
                        SizeF labelSize = g.MeasureString(dateLabel, labelFont2);
                        g.DrawString(dateLabel, labelFont2, labelBrush2, p.X - labelSize.Width / 2, paddingTop + chartHeight + 8);

                       
                        if (orderList[i].OrderCount > 0)
                        {
                            string valueLabel = orderList[i].OrderCount.ToString();
                            SizeF valueSize = g.MeasureString(valueLabel, labelFont2);
                            g.DrawString(valueLabel, labelFont2, labelBrush2, p.X - valueSize.Width / 2, p.Y - 22);
                        }
                    }
                }
            }

            
            g.DrawRectangle(new Pen(Color.FromArgb(220, 220, 220), 1), 0, 0, width - 1, height - 1);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelMain_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}