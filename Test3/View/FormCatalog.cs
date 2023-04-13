using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test3
{
    public partial class FormCatalog : Form
    {
        public FormCatalog()
        {
            InitializeComponent();
        }

        private static string picName;
        private static string picPath = Application.StartupPath + "/Pict/";
        private static Bitmap bitmap;
        private void LoadGrid()
        {
            dataGridView1.Rows.Clear();

            using(Models.DB01Entities DB = new Models.DB01Entities())
            {
                var products = DB.Product.ToList();

                if (comboBoxCost.SelectedIndex == 0)
                    products = products.OrderByDescending(x => x.ProductCost).ToList();
                else if (comboBoxCost.SelectedIndex == 1)
                    products = products.OrderBy(x => x.ProductCost).ToList();

                if (comboBoxDiscount.SelectedIndex == 1)
                    products = products.Where(x => x.ProductDiscount < 10).ToList();
                else if (comboBoxDiscount.SelectedIndex == 2)
                    products = products.Where(x => x.ProductDiscount >= 10 && x.ProductDiscount < 15).ToList();
                else if (comboBoxDiscount.SelectedIndex == 3)
                    products = products.Where(x => x.ProductDiscount >= 15).ToList();

                int i = 0;
                foreach(var item in products)
                {
                    dataGridView1.Rows.Add();

                    picName = item.ProductPhoto;
                    if (String.IsNullOrEmpty(picName))
                        dataGridView1.Rows[i].Cells[0].Value = Properties.Resources.nophoto;
                    else
                    {
                        bitmap = new Bitmap(picPath + picName);
                        dataGridView1.Rows[i].Cells[0].Value = bitmap;
                    }

                    if (item.ProductDiscount != null)
                    {
                        dataGridView1.Rows[i].Cells[1].Value =
                            $"Наименование: {item.ProductName}" +
                            $"Описание: {item.ProductDescription}" +
                            $"Производитель: {item.Manufacturer.ManufacturerName}";
                        dataGridView1.Rows[i].Cells[2].Value = item.ProductCost;
                        dataGridView1.Rows[i].Cells[3].Value = item.ProductDiscount;
                        dataGridView1.Rows[i].Cells[4].Value = item.ProductCost * (100 - item.ProductDiscount) / 100;
                        dataGridView1.Rows[i].Cells[2].Style = new DataGridViewCellStyle
                        {
                            Font = new Font(this.Font, FontStyle.Strikeout)
                        };
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[1].Value =
                            $"Наименование: {item.ProductName}" +
                            $"Описание: {item.ProductDescription}" +
                            $"Производитель: {item.Manufacturer.ManufacturerName}";
                        dataGridView1.Rows[i].Cells[2].Value = item.ProductCost;
                        dataGridView1.Rows[i].Cells[3].Value = "-";
                        dataGridView1.Rows[i].Cells[4].Value = "-";
                    }
                    i++;
                }
                labelCount.Text = "Количество записей: " + i.ToString();
            }
        }

        private void FormCatalog_Load(object sender, EventArgs e)
        {
            comboBoxCost.SelectedIndex = 0;
            comboBoxDiscount.SelectedIndex = 0;
            LoadGrid();
        }

        private void comboBoxCost_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void comboBoxDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите выйти?", "Подтверждение выхода", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
                this.Close();
        }
    }
}
