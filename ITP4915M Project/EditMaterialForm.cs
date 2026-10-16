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

    public partial class EditMaterialForm : Form
    {
        
        public string MaterialID;
        public int NewQty;

        public EditMaterialForm(string name, int qty, string id)
        {
            InitializeComponent();
            this.AcceptButton = buttonSave;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            
            labelName.Text = name;
            textBoxQty.Text = qty.ToString();

            
            MaterialID = id;
        }

        private void EditMaterialForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxQty.Text, out int qty))
            {
                if (qty >= 0) // ✅ 限制 >= 0
                {
                    NewQty = qty;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Qty must be 0 or above ❌");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number ❌");
            }
        }



        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}
