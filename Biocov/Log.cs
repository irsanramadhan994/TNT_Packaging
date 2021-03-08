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

    public partial class Log : Form
    {

        DataTable table = new DataTable();
        class DataValidation
        {
            public string innerboxgsoneid { get; set; }
            public string status { get; set; }
            public string date { get; set; }
            public string time { get; set; }

        }

        class DataVialValidation
        {
            public string innerboxgsoneid { get; set; }
            public string gsoneidvial { get; set; }
            public string status { get; set; }
            public string date { get; set; }
            public string time { get; set; }

        }

        List<DataValidation> tablelog = new List<DataValidation>();
        List<DataVialValidation> tablelog2 = new List<DataVialValidation>();

        public Log(string from, List<string> validationLog)
        {
            InitializeComponent();
            label1.Text = from;

            if (from == "Log Carton Validation")
            {
                table.Columns.Add("GSI 1D");
                table.Columns.Add("Status");
                table.Columns.Add("Date");
                table.Columns.Add("Time");
                foreach (string items in validationLog)
                {
                    string[] array = items.Split('&');
                    tablelog.Add(new DataValidation { innerboxgsoneid = array[0], status = array[1], date = array[2], time = array[3] });

                }

                foreach (DataValidation items in tablelog)
                {
                    DataRow row = table.NewRow();
                    row[0] = items.innerboxgsoneid;
                    row[1] = items.status;
                    row[2] = items.date;
                    row[3] = items.time;
                    table.Rows.Add(row);

                }

                dataGridView1.DataSource = table;
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            }
            else
            {

                table.Columns.Add("GS1 ID Carton");
                table.Columns.Add("GS1 ID Vial");
                table.Columns.Add("Status");
                table.Columns.Add("Date");
                table.Columns.Add("Time");
                foreach (string items in validationLog)
                {
                    string[] array = items.Split('&');
                    tablelog2.Add(new DataVialValidation { innerboxgsoneid = array[0], gsoneidvial = array[1], status = array[2], date = array[3], time = array[4] });

                }

                foreach (DataVialValidation items in tablelog2)
                {
                    DataRow row = table.NewRow();
                    row[0] = items.innerboxgsoneid;
                    row[1] = items.gsoneidvial;
                    row[2] = items.date;
                    row[3] = items.time;
                    table.Rows.Add(row);

                }

                dataGridView1.DataSource = table;
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            }

        }

        private void Log_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = false;
        }







    }
  
    
}
