using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Biocov
{
    public partial class Cartoning : Form
    {
        db db = new db();
        sqlite sqlite = new sqlite();
        string bnumber,hidscanner, hidscanner2,outboxQty;
        DataTable table = new DataTable();

        public string adminid;
        Dictionary<string, string> config = new Dictionary<string, string>();
        private class DataProduct
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Qty { get; set; }

        }
        BindingList<DataProduct> _comboProduct = new BindingList<DataProduct>();


        public Cartoning()
        {
            InitializeComponent();
            cbProduct.DisplayMember = "Name";
            cbProduct.ValueMember = "Value";
            getProductName();
            tbBatchnumber.Enabled = false;
            lblQty.Text = dataGridView1.RowCount.ToString();
            table.Columns.Add("GSI 1D");
            table.Columns.Add("Status");
            table.Columns.Add("Qty");
            table.Columns.Add("Time");
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            tbQty.Enabled = false;
            timer1.Interval = 1000;
            timer1.Start();
        }

        public void getProductName()
        {
            List<string> field = new List<string>();
            field.Add("product_model,model_name,outbox_qty");
            List<string[]> dss = db.selectList(field, "[product_model]", "");
            if (db.num_rows > 0)
            {
                
                int i = 0;
                _comboProduct.Add(new DataProduct { Name = "-Select Product-", Value = null, Qty=null });
                foreach (string[] Row in dss)
                {
                    i++;
                    _comboProduct.Add(new DataProduct { Name = Row[1], Value = Row[0] ,Qty = Row[2]});


                }
                cbProduct.DataSource = _comboProduct;

            }

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tbBatchnumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void Cartoning_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = false;
            tbCarton.Enabled = false;
            tbCarton.Enabled = false;
            tbBatchnumber.Enabled = false;
            dataGridView1.Enabled = false;
            config = sqlite.getConfig("MGI");
            hidscanner = config["hidscanner"];
            hidscanner2 = config["hidscanner2"];

        }

        private void panel3_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void tbBatchnumber_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void cbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbBatchnumber.Enabled = true;
            outboxQty = _comboProduct[cbProduct.SelectedIndex].Qty;
        }

        private void tbBatchnumber_Click(object sender, EventArgs e)
        {
            using (var listbatch = new ListBatch("('" + cbProduct.SelectedValue.ToString() + "')"))
            {
                var result = listbatch.ShowDialog();
                if (result == DialogResult.OK)
                {
                    bnumber = listbatch.batchNumber;
                    tbBatchnumber.Text = listbatch.batchNumber;
                    tbQty.Enabled = true;

                }
            }
        }

        private void tbQty_TextChanged(object sender, EventArgs e)
        {
            if (tbQty.Text != "")
            {
                button1.Enabled = true;
            }
            else {
                button1.Enabled = false;

                }
        }

        private void tbCarton_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbCarton_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyValue == 13)
            {
                Dictionary<string, string> field = new Dictionary<string, string>();
                int  substract= (int.Parse(outboxQty)- int.Parse(tbQty.Text));  
                field.Add("flag", "0");
                field.Add("isreject", "0");
                field.Add("substraction", substract.ToString());
                if (db.update(field, "[vaccine]", "batchnumber='" + bnumber + "' AND innerboxgsoneid='" + tbCarton.Text + "'") > 0)
                {
                    Dictionary<string, string> syslog = new Dictionary<string, string>();
                    syslog.Add("eventType", "12");
                    syslog.Add("eventName", "Validation GS1 ID Carton " + tbCarton.Text);
                    syslog.Add("[from]", "Packaging");
                    syslog.Add("uom", "Carton");
                    syslog.Add("userid", adminid);
                    db.insert(syslog, "[system_log]");
                    DataRow row = table.NewRow();
                    row[0] = tbCarton.Text;
                    row[1] = "PASS";
                    row[2] = tbQty.Text;
                    row[3] = DateTime.Now.ToLongTimeString();
                    table.Rows.Add(row);
                    lblQty.Text = table.Rows.Count.ToString();
                    MessageBox.Show("Success");
                    tbCarton.Text = "";
                    tbCarton.Focus();

                }
                else
                {
                    DataRow row = table.NewRow();
                    row[0] = tbCarton.Text;
                    row[1] = "FAIL";
                    row[2] = tbQty.Text;
                    row[3] = DateTime.Now.ToLongTimeString();
                    table.Rows.Add(row);
                    lblQty.Text = table.Rows.Count.ToString();
                    MessageBox.Show("Failed!");
                    tbCarton.Text = "";
                    tbCarton.Focus();
                }
                
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard dbd = new Dashboard();
            dbd.lblRole.Text = lblRole.Text;
            dbd.lblUserId.Text = lblUserId.Text;
            dbd.adminid = adminid;
            dbd.Show();
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToShortDateString();


        }

        private void btClear_Click(object sender, EventArgs e)
        {
            table.Rows.Clear();
            dataGridView1.DataSource = table;
            cbProduct.Enabled = true;
            cbProduct.SelectedIndex = 0;
            cbProduct.Focus();
            lblQty.Text = "";
            tbQty.Text = "";
            tbQty.Enabled = false;
            tbCarton.Text = "";
            tbCarton.Enabled = false;
            tbBatchnumber.Text = "";
            tbBatchnumber.Enabled = false;
        }

        private void Cartoning_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbCarton.Enabled = true;
            tbCarton.Focus();
            cbProduct.Enabled = false;
            tbBatchnumber.Enabled = false;
            tbQty.Enabled = false;
            button1.Enabled = false;
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (dataGridView1.Rows.Count > 1)
            {
                try
                {
                    if (dataGridView1[1, e.RowIndex].Value.ToString() == "FAIL")
                    {
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;

                    }
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

       
    }
}
