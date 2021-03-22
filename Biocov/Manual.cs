using System.Windows.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RawInput_dll;
using System.Net.NetworkInformation;
using System.Management.Instrumentation;
using System.Management;
using System.Threading;
using System.Net;

namespace Biocov
{
    public partial class Manual : Form
    {
       DataTable table = new DataTable();
        DataTable table2 = new DataTable();
        db db = new db();
        generate gnc = new generate();
        Logger logs = new Logger();
        string bnumber,bnumber2,hidscanner,hidscanner2,expired,productmodelid;
        public string adminid,gs1carton1,gs1carton2;
        sqlite sqlite = new sqlite();
        Dictionary<string, string> config = new Dictionary<string, string>();
        private readonly RawInput _rawinput;
        const bool CaptureOnlyInForeground = false;
        private bool isstart, istart2,isSas,isSas2;
        private class DataProduct
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private class DataProduct2
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private class DataBatch
        {
            public string Name { get; set; }
        }

        List<string> validationLog = new List<string>();
        List<string> validationLog2 = new List<string>();

        public Manual()
        {
            InitializeComponent();
            isstart = false;
            istart2 = false;
            isSas = false;
            isSas2 = false;
            _rawinput = new RawInput(Handle, CaptureOnlyInForeground);
            lblQty1.Text = dataGridView1.RowCount.ToString();
            timer1.Interval = 1000; 
            timer1.Start();
            timer2.Interval = 5000;
            timer2.Start();
            cbProduct1.DisplayMember = "Name";
            cbProduct1.ValueMember = "Value";
            cbProduct2.DisplayMember = "Name";
            cbProduct2.ValueMember = "Value";
            table.Columns.Add("GSI 1D");
            table.Columns.Add("Status");
            table.Columns.Add("Date");
            table.Columns.Add("Time");
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            table2.Columns.Add("GSI 1D");
            table2.Columns.Add("Status");
            table2.Columns.Add("Date");
            table2.Columns.Add("Time");
            dataGridView2.DataSource = table2;
            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            getProductName();
        }



        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void OnKeyPressed(object sender, RawInputEventArg e)
        
        {
            if (e.KeyPressEvent.VKeyName != "ENTER" && e.KeyPressEvent.VKeyName != "LSHIFT" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.VKeyName != "CAPITAL" && e.KeyPressEvent.KeyPressState == "MAKE" && e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) && isstart)
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
            if (e.KeyPressEvent.VKeyName != "ENTER" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.VKeyName != "LSHIFT" && e.KeyPressEvent.VKeyName != "CAPITAL" && e.KeyPressEvent.KeyPressState == "MAKE" && e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower()) && istart2)
            {
                if (e.KeyPressEvent.VKeyName.Length > 1 && e.KeyPressEvent.VKeyName[0].ToString() == "D")
                {
                    gs1carton2 += e.KeyPressEvent.VKeyName[1].ToString();
                }
                else
                {

                    gs1carton2 += e.KeyPressEvent.VKeyName;

                }



            }
            if (e.KeyPressEvent.VKeyName == "ENTER" && e.KeyPressEvent.KeyPressState == "MAKE" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) && isstart)
            {
                tbCarton1.Text = gs1carton1;
                cartonValidation();
            }
            if (e.KeyPressEvent.VKeyName == "ENTER" && e.KeyPressEvent.KeyPressState == "MAKE" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower()) && istart2)
            {
                tbCarton2.Text = gs1carton2;
                cartonValidation2();
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void cbProduct1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbBatchnumber1.Enabled = true;
            tbBatchnumber1.Focus();
            List<string> field = new List<string>();
            field.Add("isSas");
            try
            {
                List<string[]> sas = db.selectList(field, "product_model", "product_model='" + cbProduct1.SelectedValue.ToString() + "'");
                if (db.num_rows > 0)
                {
                    foreach (string[] Row in sas)
                    {
                        Console.WriteLine(Row[0]);
                        if (Row[0] == "True")
                        {
                           
                            isSas = true;
                        }
                        else
                        {
                            isSas = false;
                        }
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex);
            }

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }



        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer3_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void Manual_Load(object sender, EventArgs e)
        {
            _rawinput.KeyPressed += OnKeyPressed;
            _rawinput.AddMessageFilter();
            config = sqlite.getConfig(lblUserId.Text);
            hidscanner = config["hidscanner"];
            hidscanner2 = config["hidscanner2"];
            dataGridView1.Enabled = false;
            dataGridView2.Enabled = false;
            tbCarton1.Enabled = false;
            tbBatchnumber1.Enabled = false;
            tbBatchnumber2.Enabled = false;
            tbCarton2.Enabled = false;
            btEnd2.Enabled = false;
            btStart2.Enabled = false;
            btStart1.Enabled = false;
            btEnd1.Enabled = false;

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
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
                BindingList<DataProduct2> _comboProduct2 = new BindingList<DataProduct2>();

                _comboProduct.Add(new DataProduct { Name = "-Select Product-", Value = null });
                _comboProduct2.Add(new DataProduct2 { Name = "-Select Product-", Value = null });

                foreach (string[] Row in dss)
                {
                    i++;
                    _comboProduct.Add(new DataProduct { Name = Row[1], Value = Row[0] });
                    _comboProduct2.Add(new DataProduct2 { Name = Row[1], Value = Row[0] });


                }
                cbProduct1.DataSource = _comboProduct;
                cbProduct2.DataSource = _comboProduct2;
            }

        }





        private void cartonValidation() {


            List<string> field = new List<string>();
            field.Add("gsoneinnerboxid");
            if (db.selectList(field, "[innerbox]", "gsoneinnerboxid='" + tbCarton1.Text + "' AND batchNumber='" + tbBatchnumber1.Text + "' AND isreject='0' AND flag='0'") == null && tbCarton1.Text.Contains(tbBatchnumber1.Text))
            {
                Dictionary<string, string> innerbox = new Dictionary<string, string>();
                Dictionary<string, string> fieldvaccine = new Dictionary<string, string>();
                innerbox.Add("isreject", "0");
                innerbox.Add("flag", "0");
                fieldvaccine.Add("batchnumber", bnumber);
                fieldvaccine.Add("flag", "0");
                fieldvaccine.Add("gsoneinnerboxid", tbCarton1.Text);
                fieldvaccine.Add("isreject", "0");

                if (isSas)
                {

                        if (db.insertManual(tbBatchnumber1.Text,tbCarton1.Text,expired,productmodelid))
                        {

                            DataRow row = table.NewRow();
                            row[0] = tbCarton1.Text;
                            row[1] = "PASS";
                            row[2] = DateTime.Now.ToLongDateString();
                            row[3] = DateTime.Now.ToLongTimeString();
                            table.Rows.Add(row);
                            validationLog.Add(tbCarton1.Text + "&PASS" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());


                            Dictionary<string, string> syslog = new Dictionary<string, string>();
                            syslog.Add("eventType", "12");
                            syslog.Add("eventName", "Validation GS1 ID Carton " + tbCarton1.Text);
                            syslog.Add("[from]", "Packaging");
                            syslog.Add("uom", "Carton");
                            syslog.Add("userid", adminid);
                            db.insert(syslog, "[system_log]");

                            logs.LogWriter("Scan GS1 ID Carton " + tbCarton1.Text + " Batchnumber " + tbBatchnumber1.Text + " Status Success");

                        }
                        else
                        {

                            MessageBox.Show("Cant Connect to Database Server");

                        }
  



                }
                else
                {
                    if (db.update(innerbox, "[innerbox]", "batchnumber='" + bnumber + "' AND gsoneinnerboxid='" + tbCarton1.Text + "'") > 0)
                    {
                        DataRow row = table.NewRow();
                        row[0] = tbCarton1.Text;
                        row[1] = "PASS";
                        row[2] = DateTime.Now.ToLongDateString();
                        row[3] = DateTime.Now.ToLongTimeString();
                        table.Rows.Add(row);
                        validationLog.Add(tbCarton1.Text + "&PASS" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());


                            Dictionary<string, string> syslog = new Dictionary<string, string>();
                            syslog.Add("eventType", "12");
                            syslog.Add("eventName", "Validation GS1 ID Carton " + tbCarton1.Text);
                            syslog.Add("[from]", "Packaging");
                            syslog.Add("uom", "Carton");
                            syslog.Add("userid", adminid);
                            db.insert(syslog, "[system_log]");

                            logs.LogWriter("Scan GS1 ID Carton " + tbCarton1.Text + " Batchnumber " + tbBatchnumber1.Text + " Status Success");

                        }
                        else
                        {

                            MessageBox.Show("Cant Connect to Database Server");

                        }
                    
                  
                
                }
            }
            else
            {
                DataRow row = table.NewRow();
                row[0] = tbCarton1.Text;
                row[1] = "FAIL";
                row[2] = DateTime.Now.ToLongDateString();
                row[3] = DateTime.Now.ToLongTimeString();
                table.Rows.Add(row);
                validationLog.Add(tbCarton1.Text + "&FAILED" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                logs.LogWriter("Scan GS1 ID Carton " + tbCarton1.Text + " Batchnumber " + tbBatchnumber1.Text + " Status Fail");

            }
            dataGridView1.DataSource = table;
            dataGridView1.Sort(dataGridView1.Columns[3], ListSortDirection.Descending);
            tbCarton1.Text = "";
            tbCarton1.Focus();
            int rowcount = dataGridView1.RowCount - 1;
            lblQty1.Text = rowcount.ToString();
            lblQtyBatch1.Text = db.selectCountPass(bnumber).ToString();
            lblPass1.Text = table.Select("Status= 'PASS'").Length.ToString();
            lblFail1.Text = table.Select("Status= 'FAIL'").Length.ToString();
            gs1carton1 = "";

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToShortDateString();

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }




        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            


        }

        private void button3_Click(object sender, EventArgs e)
        {
            Log log = new Log("Log Carton Validation",validationLog);
            log.Show();
        }

        private void Manual_FormClosed(object sender, FormClosedEventArgs e)
        {
            _rawinput.KeyPressed -= OnKeyPressed;
            Application.Exit();
        }

        public bool checkScanner(string hid) {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_PnPEntity where DeviceID Like ""HID%""");
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject any in coll)
            {
                string dvid = any.GetPropertyValue("DeviceID").ToString();
                string[] array = dvid.Split('&');
                if (array[3] == hid)
                {
                    return true;
                }
            }
            return false;
        
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
          
   
        }

        private void button1_Click(object sender, EventArgs e)
        {
 

        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            timer2.Stop();
        }

        private void splitContainer3_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (cbProduct2.SelectedIndex > 0)
            {

                if (tbBatchnumber2.Text.Length > 0)
                {
                    cbProduct2.Enabled = false;
                    tbBatchnumber2.Enabled = false;
                    btStart2.Enabled = false;
                    btEnd2.Enabled = true;
                    gnc.getPO(bnumber2);
                    istart2 = true;
                    btnLog2.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Silahkan Pilih Kolom Batch");
                }
            }
            else
            {
                MessageBox.Show("Silahkan Pilih Kolom Produk");
            }
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            

        }

        private void cartonValidation2() 
        {

            List<string> field = new List<string>();
            field.Add("gsoneinnerboxid");
            if (db.selectList(field, "[innerbox]", "gsoneinnerboxid='" + tbCarton2.Text + "' AND batchNumber='" + tbBatchnumber2.Text + "' AND isreject='0' AND flag='0'") == null && tbCarton2.Text.Contains(tbBatchnumber2.Text))
            {
                Dictionary<string, string> innerbox = new Dictionary<string, string>();
                Dictionary<string, string> fieldvaccine = new Dictionary<string, string>();
                innerbox.Add("isreject", "0");
                innerbox.Add("flag", "0");
                fieldvaccine.Add("batchnumber", bnumber2);
                fieldvaccine.Add("flag", "0");
                fieldvaccine.Add("gsoneinnerboxid", tbCarton2.Text);
                fieldvaccine.Add("isreject", "0");

                if (isSas2)
                {
                    if (db.insert(fieldvaccine, "[innerbox]"))
                    {
                        if (db.insertSelect(expired, tbCarton2.Text, productmodelid))
                        {

                            DataRow row = table2.NewRow();
                            row[0] = tbCarton2.Text;
                            row[1] = "PASS";
                            row[2] = DateTime.Now.ToLongDateString();
                            row[3] = DateTime.Now.ToLongTimeString();
                            table2.Rows.Add(row);
                            validationLog.Add(tbCarton2.Text + "&PASS" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());


                            Dictionary<string, string> syslog = new Dictionary<string, string>();
                            syslog.Add("eventType", "12");
                            syslog.Add("eventName", "Validation GS1 ID Carton " + tbCarton2.Text);
                            syslog.Add("[from]", "Packaging");
                            syslog.Add("uom", "Carton");
                            syslog.Add("userid", adminid);
                            db.insert(syslog, "[system_log]");

                            logs.LogWriter("Scan GS1 ID Carton " + tbCarton2.Text + " Batchnumber " + tbBatchnumber2.Text + " Status Success");

                        }
                        else
                        {

                            MessageBox.Show("Cant Connect to Database Server");

                        }
                    }
                    else
                    {
                        MessageBox.Show("Cant Connect to Database Server");
                    }



                }
                else {

                    if (db.update(innerbox, "[innerbox]", "batchnumber='" + bnumber2 + "' AND gsoneinnerboxid='" + tbCarton2.Text + "'") > 0)
                    {

                            logs.LogWriter("Scan GS1 ID Carton " + tbCarton2.Text + " Batchnumber " + tbBatchnumber2.Text + " Status Success");
                            Dictionary<string, string> syslog = new Dictionary<string, string>();
                            syslog.Add("eventType", "12");
                            syslog.Add("eventName", "Validation GS1 ID Carton " + tbCarton2.Text);
                            syslog.Add("[from]", "Packaging");
                            syslog.Add("uom", "Carton");
                            syslog.Add("userid", adminid);
                            db.insert(syslog, "[system_log]");
                        }
                        else
                        {

                            MessageBox.Show("Can't Connect to Database");

                        }

                    
           
                }
            }
            else
            {
                DataRow row = table2.NewRow();
                row[0] = tbCarton2.Text;
                row[1] = "FAIL";
                row[2] = DateTime.Now.ToLongDateString();
                row[3] = DateTime.Now.ToLongTimeString();
                table2.Rows.Add(row);
                validationLog2.Add(tbCarton1.Text + "&FAILED" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                logs.LogWriter("Scan GS1 ID Carton " + tbCarton2.Text + " Batchnumber " + tbBatchnumber2.Text + " Status Fail");

            }
            dataGridView2.DataSource = table2;
            dataGridView2.Sort(dataGridView2.Columns[3], ListSortDirection.Descending);
            tbCarton2.Text = "";
            tbCarton2.Focus();
            int rowcount = dataGridView2.RowCount - 1;
            lblQty2.Text = rowcount.ToString();
            lblQtyBatch2.Text = db.selectCountPass(bnumber2).ToString();
            lblPass2.Text = table2.Select("Status= 'PASS'").Length.ToString();
            lblFail2.Text = table2.Select("Status= 'FAIL'").Length.ToString();
            gs1carton2 = "";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {


        }

        private void Manual_FormClosing(object sender, FormClosingEventArgs e)
        {
            _rawinput.KeyPressed -= OnKeyPressed;   

        }

        private void btnLog_Click(object sender, EventArgs e)
        {
        }

        private void btnLog2_Click(object sender, EventArgs e)
        {
        }

        private void splitContainer3_Panel2_Paint(object sender, PaintEventArgs e)
        {

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

        private void dataGridView2_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (dataGridView2.Rows.Count > 1)
            {
                try
                {
                    if (dataGridView2[1, e.RowIndex].Value.ToString() == "FAIL")
                    {
                        dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;

                    }
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }


        



        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void tbBatchnumber2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Manual_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            _rawinput.KeyPressed -= OnKeyPressed;   
            Application.Exit();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToShortDateString();
        }

        public void getPO(string batchnumber)
        {

            List<string> field = new List<string>();
            field.Add("a.expired,a.productmodelid");
            List<string[]> dss = db.selectList(field, "[packaging_order]  a inner join [product_model] b ON a.productModelId = b.product_model ", "batchNumber ='" + batchnumber + "'");

            if (db.num_rows > 0)
            {
                foreach (string[] Row in dss)
                {
                    expired = Row[0];
                    productmodelid = Row[1];
           }
            }
            else
            {

                logs.LogWriter("SQL Error Cannot Get Packaging Order data");
            }
        }

        private void tbBatchnumber1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbBatchnumber1_Click(object sender, EventArgs e)
        {
            using (var listbatch = new ListBatch("('" + cbProduct1.SelectedValue.ToString() + "')"))
            {
                var result = listbatch.ShowDialog();
                if (result == DialogResult.OK)
                {
                    bnumber = listbatch.batchNumber;
                    tbBatchnumber1.Text = listbatch.batchNumber;
                    btStart1.Enabled = true;
                    lblQtyBatch1.Text = db.selectCountPass(bnumber).ToString();
                    getPO(bnumber);


                }
            }
        }

        private void btStart1_Click(object sender, EventArgs e)
        {
            if (cbProduct1.SelectedIndex > 0)
            {

                if (tbBatchnumber1.Text.Length > 0)
                {
                    cbProduct1.Enabled = false;
                    tbBatchnumber1.Enabled = false;
                    btStart1.Enabled = false;
                    btEnd1.Enabled = true;
                    isstart = true;
                    btnLog.Enabled = false;

                }
                else
                {
                    MessageBox.Show("Silahkan Pilih Kolom Batch");
                }
            }
            else
            {
                MessageBox.Show("Silahkan Pilih Kolom Produk");
            }
        }

        private void btEnd1_Click(object sender, EventArgs e)
        {
            tbCarton1.Text = "";
            tbCarton1.Enabled = false;
            btStart1.Enabled = false;
            cbProduct1.Enabled = true;
            cbProduct1.SelectedIndex = 0;
            tbBatchnumber1.Text = "";
            tbBatchnumber1.Enabled = false;
            table.Rows.Clear();
            dataGridView1.DataSource = table;
            isstart = false;
            btnLog.Enabled = true;
            lblQty1.Text = "";
            lblQtyBatch1.Text = "";
            lblPass1.Text = "";
            lblFail1.Text = "";
        }

        private void dataGridView1_RowPrePaint_1(object sender, DataGridViewRowPrePaintEventArgs e)
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

        private void cbProduct2_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbBatchnumber2.Enabled = true;
            List<string> field = new List<string>();
            field.Add("isSas");
            try
            {
                List<string[]> sas = db.selectList(field, "product_model", "product_model='" + cbProduct2.SelectedValue.ToString() + "'");
                if (db.num_rows > 0)
                {
                    foreach (string[] Row in sas)
                    {
                        Console.WriteLine(Row[0]);
                        if (Row[0] == "True")
                        {
                            isSas2 = true;
                        }
                        else
                        {
                            isSas2 = false;
                        }
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void tbBatchnumber2_Click(object sender, EventArgs e)
        {
            using (var listbatch = new ListBatch("('" + cbProduct2.SelectedValue.ToString() + "')"))
            {
                var result = listbatch.ShowDialog();
                if (result == DialogResult.OK)
                {
                    bnumber2 = listbatch.batchNumber;
                    tbBatchnumber2.Text = listbatch.batchNumber;
                    btStart2.Enabled = true;
                    getPO(bnumber2);
                    lblQtyBatch2.Text = db.selectCountPass(bnumber2).ToString();

                }
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
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

        private void btEnd2_Click(object sender, EventArgs e)
        {
            tbCarton2.Text = "";
            tbCarton2.Enabled = false;
            cbProduct2.SelectedIndex = 0;
            cbProduct2.Enabled = true;
            tbBatchnumber2.Text = "";
            istart2 = false;
            tbBatchnumber2.Enabled = false;
            table2.Rows.Clear();
            lblQtyBatch2.Text = "";
            dataGridView2.DataSource = table2;
            lblQty2.Text = "";
            lblPass2.Text = "";
            lblFail2.Text = "";
            btnLog2.Enabled = true;
        }

        private void btnLog_Click_1(object sender, EventArgs e)
        {
            Log log = new Log("Log Carton Validation", validationLog);
            log.Show();

        }

        private void btnLog2_Click_1(object sender, EventArgs e)
        {
            Log log = new Log("Log Carton Validation", validationLog2);
            log.Show();

        }

        private void btStart2_Click(object sender, EventArgs e)
        {
            if (cbProduct2.SelectedIndex > 0)
            {

                if (tbBatchnumber2.Text.Length > 0)
                {
                    cbProduct2.Enabled = false;
                    tbBatchnumber2.Enabled = false;
                    btStart2.Enabled = false;
                    btEnd2.Enabled = true;
                    istart2 = true;
                    btnLog2.Enabled = false;

                }
                else
                {
                    MessageBox.Show("Silahkan Pilih Kolom Batch");
                }
            }
            else
            {
                MessageBox.Show("Silahkan Pilih Kolom Produk");
            }
        }

        private void dataGridView2_RowPrePaint_1(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (dataGridView2.Rows.Count > 1)
            {
                try
                {
                    if (dataGridView2[1, e.RowIndex].Value.ToString() == "FAIL")
                    {
                        dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;

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
