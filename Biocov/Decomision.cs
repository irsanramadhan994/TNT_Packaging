using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using RawInput_dll;
using System.Text;
using System.Windows.Forms;

namespace Biocov
{
    public partial class Decomision : Form
    {
        db db = new db();
        string bnumber,hidscanner,hidscanner2;
        public bool cartonvalidated = false;
        public string adminid,gs1carton1,gs1idvial1;
        DataTable table = new DataTable();
        DataTable table2 = new DataTable();
        sqlite sqlite = new sqlite();
        Dictionary<string, string> config = new Dictionary<string, string>();
        private readonly RawInput _rawinput;
        const bool CaptureOnlyInForeground = true;
        private bool isStart;
        private string notifMessage, notifCaption;
        Logger log = new Logger();

        private class DataProduct
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private class DataValidation
        {
            public string innerboxgsoneid { get; set; }
            public string status { get; set; }
            public string date { get; set; }
            public string time { get; set; }

        }

        List<DataValidation> decomissionLog = new List<DataValidation>();



        public Decomision()
        {
            InitializeComponent();
            var a = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(long.Parse("1595488622")).ToLocalTime();
            timer1.Interval = 1000;
            timer2.Interval = 500;
            timer1.Start();
            _rawinput = new RawInput(Handle, CaptureOnlyInForeground);
            isStart = false;
            cbProduct.DisplayMember = "Name";
            cbProduct.ValueMember = "Value";
            tbBatchnumber.Enabled = false;
            table.Columns.Add("GS1 ID");
            table.Columns.Add("Status");
            table.Columns.Add("Date");
            table.Columns.Add("Time");
            dataGridView4.DataSource = table;
            dataGridView4.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView4.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView4.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView4.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            table2.Columns.Add("GS1 ID");
            table2.Columns.Add("Status");
            table2.Columns.Add("Date");
            table2.Columns.Add("Time");
            dataGridView1.DataSource = table2;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
  
            getProductName();

        }

        private void OnKeyPressed(object sender, RawInputEventArg e)
        {

            if (tab.SelectedTab.Text == "Carton")
            {

                if (e.KeyPressEvent.VKeyName != "ENTER" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.VKeyName != "LSHIFT" && e.KeyPressEvent.VKeyName != "CAPITAL" && e.KeyPressEvent.KeyPressState == "MAKE" && (e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) || e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower())) && isStart)
                {
                    if (e.KeyPressEvent.VKeyName.Length > 1 && e.KeyPressEvent.VKeyName[0].ToString() == "D")
                    {
                        gs1carton1 += e.KeyPressEvent.VKeyName[1].ToString();
                    }
                    else
                    {

                        gs1carton1 += e.KeyPressEvent.VKeyName;

                    }



                }

                if (e.KeyPressEvent.VKeyName == "ENTER" && e.KeyPressEvent.KeyPressState == "MAKE" && (e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) || e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower())) &&  isStart)
                {
                    tbCarton.Text = gs1carton1;
                    cartonDecommision();
                }


            }
            else
            {
                if (e.KeyPressEvent.VKeyName != "ENTER" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.VKeyName != "LSHIFT" && e.KeyPressEvent.VKeyName != "CAPITAL" && e.KeyPressEvent.KeyPressState == "MAKE" && (e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) || e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower())) && !cartonvalidated && isStart)
                {
                    if (e.KeyPressEvent.VKeyName.Length > 1 && e.KeyPressEvent.VKeyName[0].ToString() == "D")
                    {
                        gs1carton1 += e.KeyPressEvent.VKeyName[1].ToString();
                    }
                    else
                    {

                        gs1carton1 += e.KeyPressEvent.VKeyName;

                    }



                }

                if (e.KeyPressEvent.VKeyName != "ENTER" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.VKeyName != "LSHIFT" && e.KeyPressEvent.VKeyName != "CAPITAL" && e.KeyPressEvent.KeyPressState == "MAKE" && (e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) || e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower())) && cartonvalidated && isStart)
                {
                    if (e.KeyPressEvent.VKeyName.Length > 1 && e.KeyPressEvent.VKeyName[0].ToString() == "D")
                    {
                        gs1idvial1 += e.KeyPressEvent.VKeyName[1].ToString();
                    }
                    else
                    {

                        gs1idvial1 += e.KeyPressEvent.VKeyName;

                    }



                }

                if (e.KeyPressEvent.VKeyName == "ENTER" && e.KeyPressEvent.KeyPressState == "MAKE" && (e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) || e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower())) && !cartonvalidated && isStart)
                {
                    tbCarton2.Text = gs1carton1;
                    cartonValidation();

                }

                if (e.KeyPressEvent.VKeyName == "ENTER" && e.KeyPressEvent.KeyPressState == "MAKE" && (e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) || e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower())) && cartonvalidated && isStart)
                {
                    tbVial1.Text = gs1idvial1;
                    vialDecommision();
                }
            
            }
        }


   
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        public void getProductName()
        {
            List<string> field = new List<string>();
            field.Add("product_model,model_name");
            List<string[]> dss = db.selectList(field, "[product_model]", "");
            if (db.num_rows > 0)
            {

                int i = 0;
                BindingList<DataProduct> _comboProduct = new BindingList<DataProduct>();
                _comboProduct.Add(new DataProduct { Name = "-Select Product-", Value = null });
                foreach (string[] Row in dss)
                {
                    i++;
                    _comboProduct.Add(new DataProduct { Name = Row[1], Value = Row[0] });


                }
                cbProduct.DataSource = _comboProduct;

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Decomision_Load(object sender, EventArgs e)
        {
            _rawinput.KeyPressed += OnKeyPressed;
            tbBatchnumber.Enabled = false;
            tbCarton.Enabled = false;
            tbCarton2.Enabled = false;
            dataGridView1.Enabled = false;
            dataGridView4.Enabled = false;
            btEnd.Enabled = false;
            btEnd1.Enabled = false;
            tbVial1.Enabled = false;
            btStart.Enabled = false;
            config = sqlite.getConfig(lblUserId.Text);
            hidscanner = config["hidscanner"];
            hidscanner2 = config["hidscanner2"];
            dataGridView4.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
        }

        private void label10_Click(object sender, EventArgs e)
        {


        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox8_Click(object sender, EventArgs e)
        {
            using (var listbatch = new ListBatch("('" + cbProduct.SelectedValue.ToString() + "')"))
            {
                var result = listbatch.ShowDialog();
                if (result == DialogResult.OK)
                {
                    bnumber = listbatch.batchNumber;
                    tbBatchnumber.Text = listbatch.batchNumber;
                    btStart.Enabled = true;
                }
            }  
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbBatchnumber.Enabled = true;
            tbBatchnumber.Focus();
        }

    


        private void cartonDecommision()
        {

            Dictionary<string, string> field2 = new Dictionary<string, string>();
            List<string> fields = new List<string>();
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            field2.Add("rejecttime", unixTimestamp.ToString());
            field2.Add("isreject", "2");
            fields.Add("innerboxgsoneid");
            if (db.selectList(fields, "[vaccine_packaging]", "basketid is not null AND innerboxgsoneid='" + tbCarton.Text + "'") == null)
            {
                if (db.update(field2, "[innerbox]", "gsoneinnerboxid='" + tbCarton.Text + "' AND batchnumber='" + bnumber + "' AND (isreject='0' OR isreject='1')") > 0)
                {

                    DataRow row = table.NewRow();
                    row[0] = tbCarton.Text;
                    row[1] = "Success";
                    row[2] = DateTime.Now.ToLongDateString();
                    row[3] = DateTime.Now.ToLongTimeString();
                    table.Rows.Add(row);
                    decomissionLog.Add(new DataValidation { innerboxgsoneid = tbCarton2.Text, status = "Success", date = DateTime.Now.ToLongDateString(), time = DateTime.Now.ToLongTimeString() });
                    Dictionary<string, string> syslog = new Dictionary<string, string>();
                    syslog.Add("eventType", "12");
                    syslog.Add("eventName", "Decommision Carton " + tbCarton.Text + " Batchnumber " + tbBatchnumber.Text);
                    syslog.Add("[from]", "Packaging");
                    syslog.Add("uom", "Carton");
                    syslog.Add("userid", adminid);
                    db.insert(syslog, "[system_log]");
                    dataGridView4.DataSource = table;
                    dataGridView4.Sort(dataGridView4.Columns[3], ListSortDirection.Descending);
                    Dictionary<string, string> field = new Dictionary<string, string>();
                    field.Add("innerboxid", "NULL");
                    field.Add("innerboxgsoneid", "NULL");
                    field.Add("isreject", "1");
                    field.Add("flag", "2");
                    if (db.update(field, "[vaccine_packaging]", "innerboxgsoneid='" + tbCarton.Text + "' AND batchnumber='" + bnumber + "'") > 0)
                    {
                        notifMessage = "Decomission Success";
                        notifCaption = "Success";
                        timer2.Start();

                    }
                    tbCarton.Text = "";
                    gs1carton1 = "";
                    tbCarton.Enabled = false;
                }
                else
                {
                    decomissionLog.Add(new DataValidation { innerboxgsoneid = tbCarton2.Text, status = "Failed", date = DateTime.Now.ToLongDateString(), time = DateTime.Now.ToLongTimeString() });
                    DataRow row = table.NewRow();
                    row[0] = tbCarton.Text;
                    row[1] = "FAIL";
                    row[2] = DateTime.Now.ToLongDateString();
                    row[3] = DateTime.Now.ToLongTimeString();
                    table.Rows.Add(row);
                    dataGridView4.DataSource = table;
                    tbCarton.Text = "";
                    gs1carton1 = "";
                    tbCarton.Enabled = false;
                    notifMessage = "Decomission Fail";
                    notifCaption = "Notification";
                    timer2.Start();

                }
            }
            else {

                notifMessage = "Innerbox already in Basket!";
                notifCaption = "Fail";
                timer2.Start();
            } 


                  
        
        }
        



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard dbd = new Dashboard();
            dbd.lblRole.Text = lblRole.Text;
            dbd.lblUserId.Text = lblUserId.Text;
            dbd.adminid = adminid;
            _rawinput.KeyPressed -= OnKeyPressed;
            _rawinput.RemoveMessageFilter();
            dbd.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> field = new Dictionary<string, string>();
            field.Add("innerboxid", "NULL");
            field.Add("innerboxgsoneid", "NULL");
            field.Add("isreject", "2");

            if (db.update(field, "[Vaccine]", "gsonevialid='" + tbVial1.Text + "' AND batchnumber='" + bnumber + "'") > 0)
            {
                Dictionary<string, string> field2 = new Dictionary<string, string>();
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                field2.Add("rejecttime", unixTimestamp.ToString());
                field2.Add("isreject", "2");

                if (db.update(field2, "[innerbox]", "gsoneinnerboxid='" + tbCarton2.Text + "' AND batchnumber='" + bnumber + "'") > 0)
                {
                    AutoClosingMessageBox.Show("Decommision Success !", "Notification", 2000);
                    List<string> fieldselect = new List<string>();
                    fieldselect.Add("gsonevialid 'GS1 Vial ID'");
                    List<string[]> dss = db.selectList(fieldselect, "[Vaccine]", "innerboxgsoneid='" + tbCarton2.Text + "' AND batchnumber='" + bnumber + "'");
                    if (db.num_rows > 0)
                    {

                        table = db.dataHeader;
                        foreach (string[] Row in dss)
                        {
                            DataRow row = table.NewRow();
                            row[0] = Row[0];
                            table.Rows.Add(row);


                        }
                        this.dataGridView1.DataSource = table;
                        this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        lblVialQty.Text = (dataGridView1.Rows.Count - 1).ToString();
                        Dictionary<string, string> syslog = new Dictionary<string, string>();
                        syslog.Add("eventType", "12");
                        syslog.Add("eventName", "Decommision Vial " + tbVial1.Text + " Batchnumber " + tbBatchnumber.Text);
                        syslog.Add("[from]", "Packaging");
                        syslog.Add("uom", "Vial");
                        syslog.Add("userid", adminid);
                        db.insert(syslog, "[system_log]");
                        tbVial1.Text = "";
                        tbVial1.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("Data not found!");
                    decomissionLog.Add(new DataValidation { innerboxgsoneid = tbCarton2.Text, status = "Failed", date = DateTime.Now.ToLongDateString(), time = DateTime.Now.ToLongTimeString() });

                }


            }
            else
            {
                MessageBox.Show("Operation Failed!");
            }

        }

        private void Decomision_FormClosed(object sender, FormClosedEventArgs e)
        {
            _rawinput.KeyPressed -= OnKeyPressed;
            Application.Exit();
        }



        private void cartonValidation()
        {

            List<string> field = new List<string>();
            field.Add("gsonevialid 'GS1 Vial ID'");
            List<string[]> dss = db.selectList(field, "[vaccine_packaging]", "innerboxgsoneid='" + tbCarton2.Text + "' AND batchnumber='" + bnumber + "'");
            if (db.num_rows > 0)
            {

                table2 = db.dataHeader;
                foreach (string[] Row in dss)
                {
                    DataRow row = table2.NewRow();
                    row[0] = Row[0];
                    table2.Rows.Add(row);


                }
                this.dataGridView1.DataSource = table2;
                this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                tbCarton2.Enabled = false;;
                lblVialQty.Text = (dataGridView1.Rows.Count - 1).ToString();
                cartonvalidated = true;

            }
            else
            {
                notifMessage = "Carton Not Found";
                notifCaption = "Notification";
                timer2.Start();
                tbCarton2.Text = "";
                gs1carton1 = "";
                tbCarton2.Focus();
            }
        }

        private void vialDecommision()
        {

            Dictionary<string, string> field = new Dictionary<string, string>();
            field.Add("innerboxid", "NULL");
            field.Add("innerboxgsoneid", "NULL");
            field.Add("isreject", "2");

            if (db.update(field, "[vaccine_packaging]", "gsonevialid='" + tbVial1.Text + "' AND batchnumber='" + bnumber + "'AND isreject='0'") > 0)
            {
                Dictionary<string, string> field2 = new Dictionary<string, string>();
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                field2.Add("rejecttime", unixTimestamp.ToString());
                field2.Add("isreject", "2");

                if (db.update(field2, "[innerbox]", "gsoneinnerboxid='" + tbCarton2.Text + "' AND batchnumber='" + bnumber + "'") > 0)
                {
                    notifMessage = "Decomision Success !";
                    notifCaption = "Notification";
                    timer2.Start(); Dictionary<string, string> syslog = new Dictionary<string, string>();
                    syslog.Add("eventType", "12");
                    syslog.Add("eventName", "Decommision Vial " + tbVial1.Text + " Batchnumber " + tbBatchnumber.Text);
                    syslog.Add("[from]", "Packaging");
                    syslog.Add("uom", "Vial");
                    syslog.Add("userid", adminid);
                    db.insert(syslog, "[system_log]");

                    log.LogWriter("Decommision Vial " + tbVial1.Text + " Batchnumber " + tbBatchnumber.Text + " Status Success");

                    List<string> fieldselect = new List<string>();
                    fieldselect.Add("gsonevialid 'GS1 Vial ID'");
                    List<string[]> dss = db.selectList(fieldselect, "[vaccine_packaging]", "innerboxgsoneid='" + tbCarton2.Text + "' AND batchnumber='" + bnumber + "'");
                    if (db.num_rows > 0)
                    {
                        table2.Clear();

                        foreach (string[] Row in dss)
                        {
                            DataRow row = table2.NewRow();
                            row[0] = Row[0];

                            table2.Rows.Add(row);


                        }
                        this.dataGridView1.DataSource = table2;
                        this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        lblVialQty.Text = (dataGridView1.Rows.Count - 1).ToString();
                        gs1idvial1 = "";
                        tbVial1.Text = "";
                        tbVial1.Focus();
                    }

                }
                else
                {
                    notifMessage = "Data Not Found";
                    notifCaption = "Notification";
                    timer2.Start(); decomissionLog.Add(new DataValidation { innerboxgsoneid = tbCarton2.Text, status = "Failed", date = DateTime.Now.ToLongDateString(), time = DateTime.Now.ToLongTimeString() });
                    log.LogWriter("Decommision Carton " + tbCarton.Text + " Batchnumber " + tbBatchnumber.Text + " Status Fail");

                }


            }
            else
            {
                notifMessage = "Vial Not Found";
                notifCaption = "Notification";
                timer2.Start();

                gs1idvial1 = "";
                tbVial1.Text = "";
                tbVial1.Enabled = false;
            }

        }


        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                MessageBox.Show(text, caption);
            }

            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }

            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow(null, _caption);
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToShortDateString();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            isStart = false;
            tbCarton2.Text = "";
            tbCarton2.Enabled = false;
            gs1carton1 = "";
            gs1idvial1 = "";
            tbVial1.Text = "";
            tbVial1.Enabled = false;
            btEnd.Enabled = false;
            btEnd1.Enabled = false;
            cbProduct.SelectedIndex = 0;
            cbProduct.Enabled = true;
            cartonvalidated = false;
            tbBatchnumber.Text = "";
            tbBatchnumber.Enabled = false;
            table2.Reset();
            dataGridView1.DataSource = table2;
            lblVialQty.Text = dataGridView1.Rows.Count.ToString();

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void btEnd_Click(object sender, EventArgs e)
        {
            isStart = false;
            cbProduct.SelectedIndex = 0;
            cbProduct.Enabled = true;
            btEnd.Enabled = false;
            btEnd1.Enabled = false;
            tbCarton.Text = "";
            tbCarton.Enabled = false;
            tbBatchnumber.Text = "";
            tbBatchnumber.Enabled = false;
            table.Reset();
            dataGridView4.DataSource = table;
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            tbBatchnumber.Enabled = false;
            cbProduct.Enabled = false;
            btStart.Enabled = false;
            btEnd.Enabled = true;
            btEnd1.Enabled = true;
            isStart = true;
        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("tick");
            timer2.Stop();
            AutoClosingMessageBox.Show(notifMessage, notifCaption, 1500);

        }

     

        


    }
}
