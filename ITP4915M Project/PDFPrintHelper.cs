using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ITP4915M_Project
{
    public partial class PDFPrintHelper : Form
    {
        public static void ExportToPDF(string reportTitle, Dictionary<string, string> headerInfo, DataGridView dgv)
        {
            PrintDocument pd = new PrintDocument();
            pd.DocumentName = reportTitle;

            // 設定 A4 紙張與邊距
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

                // 1. 畫報表主標題
                g.DrawString(reportTitle, titleFont, Brushes.Black, startX, currentY);
                currentY += 40;
                g.DrawLine(new Pen(Color.Black, 2), startX, currentY, 750, currentY);
                currentY += 20;

                // 2. 畫上方摘要資訊 (Date, Staff 等)
                if (headerInfo != null)
                {
                    foreach (var info in headerInfo)
                    {
                        g.DrawString($"{info.Key}: {info.Value}", bodyBoldFont, Brushes.Black, startX, currentY);
                        currentY += 20;
                    }
                    currentY += 15;
                }

                // 3. 繪製 DataGridView 資料表格
                if (dgv != null && dgv.Columns.Count > 0)
                {
                    int totalColumns = 0;
                    foreach (DataGridViewColumn col in dgv.Columns) if (col.Visible) totalColumns++;

                    if (totalColumns > 0)
                    {
                        int colWidth = 700 / totalColumns;
                        int currentX = startX;

                        // 畫表頭 Header
                        foreach (DataGridViewColumn col in dgv.Columns)
                        {
                            if (!col.Visible) continue;
                            g.FillRectangle(Brushes.LightGray, currentX, currentY, colWidth, 25);
                            g.DrawRectangle(Pens.Black, currentX, currentY, colWidth, 25);
                            g.DrawString(col.HeaderText, headerFont, Brushes.Black, currentX + 3, currentY + 5);
                            currentX += colWidth;
                        }
                        currentY += 25;

                        // 畫表格內容 Row Data
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (row.IsNewRow) continue;
                            currentX = startX;

                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (!cell.OwningColumn.Visible) continue;
                                g.DrawRectangle(Pens.Black, currentX, currentY, colWidth, 22);
                                string cellValue = cell.Value?.ToString() ?? "";
                                g.DrawString(cellValue, bodyFont, Brushes.Black, currentX + 3, currentY + 4);
                                currentX += colWidth;
                            }
                            currentY += 22;

                            if (currentY > 1100) break; // 超出邊界防護
                        }
                    }
                }

                // 4. 頁尾簽署欄位
                currentY += 40;
                g.DrawString("Prepared By: __________________", bodyFont, Brushes.Black, startX, currentY);
                g.DrawString("Received By: __________________", bodyFont, Brushes.Black, 500, currentY);
            };

            // 彈出全螢幕預覽視窗
            PrintPreviewDialog ppd = new PrintPreviewDialog { Document = pd };
            ppd.WindowState = FormWindowState.Maximized;
            ppd.ShowDialog();
        }
    }
}
