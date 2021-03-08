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
    public partial class ListBatch : Form
    {
        public string product,type;
        db database = new db();
        string linenumber;
        DataTable table = new DataTable();
        public string batchNumber { get; set; } 
        public ListBatch(string product)
        {
            InitializeComponent();
            this.product = product;
            this.linenumber = "38";
            SetDataGridListBatch("0", "");
           
        }

        public void SetDataGridListBatch(string status, string banyak)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string, string>(SetDataGridListBatch), new object[] { status, banyak });
                return;
            }
            List<string> field = new List<string>();
            if (product == "")
            {
                field.Add("a.batchNumber 'Batch Number', b.model_name 'Product', a.packagingQty 'Qty', COUNT(c.id) as 'Actual Qty', a.startdate 'Start Date', a.enddate 'End Date'");
                List<string[]> dss = database.selectList(field, "[packaging_order] a inner join [product_model] b on a.productModelId = b.product_model left outer join [vaccine_packaging] c on c.batchNumber = a.batchNumber ", "a.status = 1 group by a.batchNumber, b.model_name, a.packagingQty, a.startdate, a.enddate, a.packagingOrderNumber order by a.packagingOrderNumber desc");
                if (database.num_rows > 0)
                {
                    table = database.dataHeader;
                    table.Columns.Add("No").SetOrdinal(0);
                    int i = 0;
                    foreach (string[] Row in dss)
                    {
                        i++;
                        DataRow row = table.NewRow();
                        row[0] = i;
                        row[1] = Row[0];
                        row[2] = Row[1];
                        row[3] = Row[2];
                        string a = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(long.Parse(Row[3])).ToShortDateString();
                        string b = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(long.Parse(Row[4])).ToShortDateString();
                        row[4] = Row[3];
                        row[5] = a;
                        row[6] = b;
                        table.Rows.Add(row);
                    }
                    this.dataGridView1.DataSource = table;
                    this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                else
                {
                    refresh();
                }
            }
            else
            {
                field.Add("a.batchNumber 'Batch Number', b.model_name 'Product', a.packagingQty 'Qty',  a.startdate 'Start Date', a.enddate 'End Date'");
                List<string[]> dss = database.selectList(field, "[packaging_order] a inner join [product_model] b on a.productModelId = b.product_model ", "a.status = 1  and a.productModelId IN " + product + " group by a.batchNumber, b.model_name, a.packagingQty, a.startdate, a.enddate, a.packagingOrderNumber order by a.packagingOrderNumber desc");
                if (database.num_rows > 0)
                {
                    table = database.dataHeader;
                    table.Columns.Add("No").SetOrdinal(0);
                    int i = 0;
                    foreach (string[] Row in dss)
                    {
                        i++;
                        DataRow row = table.NewRow();
                        row[0] = i;
                        row[1] = Row[0];
                        row[2] = Row[1];
                        row[3] = Row[2];
                        string a = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(long.Parse(Row[3])).ToShortDateString();
                        string b = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(long.Parse(Row[4])).ToShortDateString();
                        row[4] = a;
                        row[5] = b;
                        table.Rows.Add(row);
                    }
                    this.dataGridView1.DataSource = table;
                    this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                else
                {
                    refresh();
                }
            }
          
        }

        internal void refresh()
        {
            table = new DataTable();
            table.Columns.Add("No");
            table.Columns.Add("Batch Number");
            table.Columns.Add("Product");
            table.Columns.Add("Status");
            table.Columns.Add("Qty");
            table.Columns.Add("Start Date");
            table.Columns.Add("End Date");
            dataGridView1.DataSource = table;
            this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                batchNumber = this.dataGridView1[1, e.RowIndex].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();

            }


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ListBatch_Load(object sender, EventArgs e)
        {

        }

    }
}
