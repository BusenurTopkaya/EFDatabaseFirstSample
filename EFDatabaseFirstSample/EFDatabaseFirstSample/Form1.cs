using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFDatabaseFirstSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ProductDal _productDal = new ProductDal();
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            dgvProducts.DataSource = _productDal.GetAll();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _productDal.Add(new Product
            {
                ProductName = txtName.Text,
                UnitPrice = Convert.ToDecimal(txtPrice.Text),
                UnitsInStock = Convert.ToInt16(txtStock.Text)
            });
            LoadProducts();
            MessageBox.Show("Added");
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dgvProducts.CurrentRow.Cells[1].Value.ToString();
            txtPrice.Text = dgvProducts.CurrentRow.Cells[2].Value.ToString();
            txtStock.Text = dgvProducts.CurrentRow.Cells[3].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            _productDal.Update(new Product
            {
                ProductID = Convert.ToInt32(dgvProducts.CurrentRow.Cells[0].Value.ToString()),
                ProductName = txtName.Text,
                UnitPrice = Convert.ToDecimal(txtPrice.Text),
                UnitsInStock = Convert.ToInt16(txtStock.Text)
            });
            LoadProducts();
            MessageBox.Show("Updated");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _productDal.Delete(new Product
            {
                ProductID = Convert.ToInt32(dgvProducts.CurrentRow.Cells[0].Value.ToString())
            });
            FormClear();
            LoadProducts();
            MessageBox.Show("Deleted");
        }

        private void FormClear()
        {
            foreach (Control item in groupBox1.Controls)
            {

                if (item is TextBox)
                {
                    item.Text = "";
                    //TextBox txt = (TextBox)item;
                    //txt.Clear();
                }
            }
        }

        private void SearchProduct(string key)
        {
            var result = _productDal.GetByName(key);
            dgvProducts.DataSource = result;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchProduct(txtSearch.Text);
        }

        private void btnGetById_Click(object sender, EventArgs e)
        {
            var result = _productDal.GetById(Convert.ToInt32(dgvProducts.CurrentRow.Cells[0].Value.ToString()));
            MessageBox.Show(result is null ? "Kayıt Bulunamadı" : result.ProductName);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                //Linq To Entities ile DB'e Select atıldı, tablodaki tüm kolonları aldık
                List<Product> result = (from p in context.Products
                                        select p).ToList();

                //List<Product> result = context.Products.ToList();

                //select'imize where cümleciği eklendi
                //List<Product> result = (from p in context.Products
                //                        where p.ProductName == "Chai" || p.UnitPrice > 50
                //                        select p).ToList();

                //List<Product> result = context.Products.Where(a => a.ProductName == "Chai" || a.UnitPrice > 50).ToList();

                //List<Product> result = (from p in context.Products
                //                        where p.ProductName.Contains("ch")
                //                        select p).ToList();

                //List<Product> result = context.Products.Where(a => a.ProductName.Contains("ch")).ToList();

                //List<Product> results = context.Products.ToList();

                DataFill(result);
            }
        }

        private void DataFill(List<Product> result)
        {
            dgvProducts.DataSource = null;
            dgvProducts.DataSource = result;
        }

        private void btnOrderBy_Click(object sender, EventArgs e)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                //List<Product> result = (from product in context.Products
                //                        orderby product.ProductID
                //                        select product).ToList();

                List<Product> result = context.Products.OrderBy(x => x.ProductName).ToList();

                //List<Product> result = (from product in context.Products
                //                        orderby product.ProductID descending
                //                        select product).ToList();

                //List<Product> result = context.Products.OrderByDescending(x => x.ProductID).ToList();


                DataFill(result);
            }
        }

        private void btnThenBy_Click(object sender, EventArgs e)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                //List<Product> result = context.Products.OrderByDescending(x => x.UnitPrice).ThenByDescending(x => x.UnitsInStock).ToList();

                List<Product> result = context.Products.OrderByDescending(x => x.UnitPrice).ThenBy(x => x.UnitsInStock).ToList();

                DataFill(result);
            }
        }

        private void btnGetCategory_Click(object sender, EventArgs e)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                List<Category> result = context.Categories.ToList();

                dgvProducts.DataSource = null;
                dgvProducts.DataSource = result;
            }
        }

        private void btnSingleColumn_Click(object sender, EventArgs e)
        {
            using (DatabaseContext context = new DatabaseContext())
            {             
                //Single Columns
                var result = (from p in context.Products
                              select p.ProductName).ToList();

                //TWO Columns
                //var result = from p in context.Products
                //             select new { Adi = p.ProductName, Fiyat = p.UnitPrice };

                dgvProducts.DataSource = null;
                dgvProducts.ColumnCount = 1;
                dgvProducts.Columns[0].Name = "Ürün Adı";

                foreach (var item in result)
                {
                    dgvProducts.Rows.Add(item);

                }

            }
        }
    }
}
