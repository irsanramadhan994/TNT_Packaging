using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using RawInput_dll;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management.Instrumentation;
using System.Management;
using System.Media;

namespace Biocov
{
    public partial class CartonVialValidation : Form
    {
        DataTable table = new DataTable();
        DataTable table2 = new DataTable();
        db db = new db();
        string bnumber, bnumber2, expired, created_time, hidscanner, hidscanner2, cap, cap2, infeedid, infeedid2;
        public string adminid, gs1carton1, gs1carton2, gs1idvial1, gs1idvial2;
        sqlite sqlite = new sqlite();
        Dictionary<string, string> config = new Dictionary<string, string>();
        private readonly RawInput _rawinput;
        const bool CaptureOnlyInForeground = true;
        public bool cartonvalidated, cartonvalidated2, isstart, isstart2;
        Logger log = new Logger();

        private class DataProduct
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Qty { get; set; }

        }

        private class DataProduct2
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Qty { get; set; }

        }



        private class DataBatch
        {
            public string Name { get; set; }
        }


        private class VialBatch
        {
            public string capId { get; set; }
            public string gsonevialid { get; set; }
            public string created_time { get; set; }
            public string linenumber { get; set; }
            public string batchnumber { get; set; }
            public string isreject { get; set; }
            public string productmodelid { get; set; }
            public string innerboxgsoneid { get; set; }
            public string expdate { get; set; }
            public string innerboxid { get; set; }


        }

        private class VialBatch2
        {
            public string capId { get; set; }
            public string gsonevialid { get; set; }
            public string created_time { get; set; }
            public string linenumber { get; set; }
            public string batchnumber { get; set; }
            public string isreject { get; set; }
            public string productmodelid { get; set; }
            public string innerboxgsoneid { get; set; }
            public string expdate { get; set; }
            public string innerboxid { get; set; }


        }
        List<string> agregation = new List<string>();
        List<string> vialvalidationLog = new List<string>();
        List<string> vialLog = new List<string>();
        List<string> vialLog2 = new List<string>();

        List<string> agregation2 = new List<string>();
        List<string> vialvalidationLog2 = new List<string>();
        BindingList<DataProduct> _comboProduct = new BindingList<DataProduct>();
        BindingList<DataProduct2> _comboProduct2 = new BindingList<DataProduct2>();




        public CartonVialValidation()
        {
            InitializeComponent();
            cartonvalidated = false;
            cartonvalidated2 = false;
            isstart = false;
            isstart2 = false;
            lblPass1.Text = "0";
            lblFail1.Text = "0";
            lblPass2.Text = "0";
            lblFail2.Text = "0";
            //_rawinput = new RawInput(Handle, CaptureOnlyInForeground);
            cbProduct1.DisplayMember = "Name";
            cbProduct1.ValueMember = "Value";
            cbProduct2.DisplayMember = "Name";
            cbProduct2.ValueMember = "Value";
            timer1.Interval = 1000;
            timer2.Interval = 1000;
            timer1.Start();
            table.Columns.Add("GSI 1D Vials");
            table.Columns.Add("Status");
            table.Columns.Add("Date");
            table.Columns.Add("Time");
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            table2.Columns.Add("GSI 1D Vials");
            table2.Columns.Add("Status");
            table2.Columns.Add("Date");
            table2.Columns.Add("Time");
            dataGridView2.DataSource = table2;
            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            getProductName();
        }

        private void splitContainer3_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OnKeyPressed(object sender, RawInputEventArg e)
        {
            if (e.KeyPressEvent.VKeyName != "ENTER" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.VKeyName != "LSHIFT" && e.KeyPressEvent.VKeyName != "CAPITAL" && e.KeyPressEvent.KeyPressState == "MAKE" && e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) && !cartonvalidated && isstart)
            {
                Console.WriteLine("IF1");
                if (e.KeyPressEvent.VKeyName.Length > 1 && e.KeyPressEvent.VKeyName[0].ToString() == "D")
                {
                    gs1carton1 += e.KeyPressEvent.VKeyName[1].ToString();
                }
                else
                {

                    gs1carton1 += e.KeyPressEvent.VKeyName;

                }



            }

            if (e.KeyPressEvent.VKeyName != "ENTER" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.VKeyName != "LSHIFT" && e.KeyPressEvent.VKeyName != "CAPITAL" && e.KeyPressEvent.KeyPressState == "MAKE" && e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) && cartonvalidated && isstart)
            {
                Console.WriteLine("IF2");
                if (e.KeyPressEvent.VKeyName.Length > 1 && e.KeyPressEvent.VKeyName[0].ToString() == "D")
                {
                    gs1idvial1 += e.KeyPressEvent.VKeyName[1].ToString();
                }
                else
                {

                    gs1idvial1 += e.KeyPressEvent.VKeyName;

                }



            }



            if (e.KeyPressEvent.VKeyName != "ENTER" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.VKeyName != "LSHIFT" && e.KeyPressEvent.VKeyName != "CAPITAL" && e.KeyPressEvent.KeyPressState == "MAKE" && e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower()) && !cartonvalidated2 && isstart2)
            {
                Console.WriteLine("IF3");

                if (e.KeyPressEvent.VKeyName.Length > 1 && e.KeyPressEvent.VKeyName[0].ToString() == "D")
                {
                    gs1carton2 += e.KeyPressEvent.VKeyName[1].ToString();
                }
                else
                {

                    gs1carton2 += e.KeyPressEvent.VKeyName;

                }

            }

            if (e.KeyPressEvent.VKeyName != "ENTER" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.VKeyName != "LSHIFT" && e.KeyPressEvent.VKeyName != "CAPITAL" && e.KeyPressEvent.KeyPressState == "MAKE" && e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower()) && cartonvalidated2 && isstart2)
            {
                Console.WriteLine("IF4");

                if (e.KeyPressEvent.VKeyName.Length > 1 && e.KeyPressEvent.VKeyName[0].ToString() == "D")
                {
                    gs1idvial2 += e.KeyPressEvent.VKeyName[1].ToString();
                }
                else
                {

                    gs1idvial2 += e.KeyPressEvent.VKeyName;

                }



            }


            if (e.KeyPressEvent.VKeyName == "ENTER" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.KeyPressState == "MAKE" && e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) && !cartonvalidated && isstart)
            {
                Console.WriteLine("IF5");

                tbCarton1.Text = gs1carton1;
                cartonValidation();
            }

            if (e.KeyPressEvent.VKeyName == "ENTER" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.KeyPressState == "MAKE" && e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) && cartonvalidated && gs1idvial1 != null & isstart)
            {
                Console.WriteLine("IF6");
                tbVial1.Text = gs1idvial1;
                vialValidation();
            }

            if (e.KeyPressEvent.VKeyName == "ENTER" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.KeyPressState == "MAKE" && e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower()) && !cartonvalidated2 && isstart2)
            {
                Console.WriteLine("IF7");
                tbCarton2.Text = gs1carton2;
                cartonValidation2();
            }

            if (e.KeyPressEvent.VKeyName == "ENTER" && e.KeyPressEvent.VKeyName != "F8" && e.KeyPressEvent.KeyPressState == "MAKE" && e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower()) && cartonvalidated2 && gs1idvial2 != null & isstart2)
            {
                Console.WriteLine("IF8");
                tbVial2.Text = gs1idvial2;
                vialValidation2();
            }

        }

        public void getProductName()
        {
            List<string> field = new List<string>();
            field.Add("product_model,model_name,outbox_qty");
            List<string[]> dss = db.selectList(field, "[product_model]", "");
            _comboProduct.Add(new DataProduct { Name = "-Select Product-", Value = null, Qty = "" });
            _comboProduct2.Add(new DataProduct2 { Name = "-Select Product-", Value = null, Qty = "" });
            if (db.num_rows > 0)
            {

                int i = 0;

                foreach (string[] Row in dss)
                {
                    i++;
                    _comboProduct.Add(new DataProduct { Name = Row[1], Value = Row[0], Qty = Row[2] });
                    _comboProduct2.Add(new DataProduct2 { Name = Row[1], Value = Row[0], Qty = Row[2] });


                }
                cbProduct1.DataSource = _comboProduct;
                cbProduct2.DataSource = _comboProduct2;
            }

        }

        private void splitContainer3_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CartonVialValidation_Load(object sender, EventArgs e)
        {
            //_rawinput.KeyPressed += OnKeyPressed;
            //_rawinput.AddMessageFilter();
            dataGridView1.Enabled = false;
            dataGridView2.Enabled = false;
            config = sqlite.getConfig(lblUserId.Text);
            hidscanner = config["hidscanner"];
            hidscanner2 = config["hidscanner2"];
            lblQty1.Text = table.Rows.Count.ToString();
            lblQty2.Text = table2.Rows.Count.ToString();
            //tbCarton1.Enabled = false;
            tbBatchnumber1.Enabled = false;
            //tbVial1.Enabled = false;
            btStart1.Enabled = false;
            btEnd1.Enabled = false;
            tbBatchnumber2.Enabled = false;
            tbCarton2.Enabled = false;
            tbVial2.Enabled = false;
            btStart2.Enabled = false;
            btEnd2.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
            

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

        private void CartonVialValidation_FormClosed(object sender, FormClosedEventArgs e)
        {
            _rawinput.KeyPressed -= OnKeyPressed;
            Application.Exit();

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

        private void comboBox2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            using (var listbatch = new ListBatch("('" + cbProduct1.SelectedValue.ToString() + "')"))
            {
                var result = listbatch.ShowDialog();
                if (result == DialogResult.OK)
                {
                    bnumber = listbatch.batchNumber;
                    tbBatchnumber1.Text = listbatch.batchNumber;
                    btStart1.Enabled = true;
                    btEnd1.Enabled = true;
                    lblQtyBatch1.Text = db.selectCountPass(bnumber).ToString();
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbBatchnumber1.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            isstart = true;
            cbProduct1.Enabled = false;
            tbBatchnumber1.Enabled = false;
            btStart1.Enabled = false;
            btnLog1.Enabled = false;
            gs1carton1 = "";
        }

        public void getPO(string batchnumber)
        {

            List<string> field = new List<string>();
            field.Add("a.manufacturingdate,a.expired");
            List<string[]> dss = db.selectList(field, "[packaging_order]  a inner join [product_model] b ON a.productModelId = b.product_model ", "batchNumber ='" + batchnumber + "'");

            if (db.num_rows > 0)
            {
                foreach (string[] Row in dss)
                {

                    expired = Row[1];
                    created_time = Row[0];
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cartonValidation()
        {
            List<string> field = new List<string>();
            field.Add("infeedinnerboxid");
            List<string[]> dsinnerbox = db.selectList(field, "[innerbox]", "gsoneinnerboxid='" + tbCarton1.Text + "' AND batchnumber='" + bnumber + "' AND isreject='1' AND flag='2'");
            if (db.num_rows > 0)
            {

                log.LogWriter("Scan GS1 ID Carton " + tbCarton1.Text + " Batchnumber " + tbBatchnumber1.Text + " Status Success");
                //tbCarton1.Enabled = false;
                cartonvalidated = true;
                foreach (string[] Row in dsinnerbox)
                {

                    infeedid = Row[0];
                }

            }
            else
            {
                log.LogWriter("Scan GS1 ID Carton " + tbCarton1.Text + " Batchnumber " + tbBatchnumber1.Text + " Status Error");
                DataRow row = table.NewRow();
                row[0] = "Carton Not Found";
                row[1] = "FAIL";
                row[2] = DateTime.Now.ToLongDateString();
                row[3] = DateTime.Now.ToLongTimeString();
                table.Rows.Add(row);
                vialLog.Add("Carton Not Found" + "&" + tbVial1.Text + "&" + "FAIL" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                vialvalidationLog.Add(tbCarton1.Text + "&" + tbVial1.Text + "&" + "FAIL" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                dataGridView1.DataSource = table;
                lblQty1.Text = table.Rows.Count.ToString();
                lblFail1.Text = vialvalidationLog.Count.ToString();
                gs1carton1 = null;
                tbCarton1.Text = "";
                //tbCarton1.Enabled = false;
            }

        }

        private void vialValidation()
        {
            bool isvialvalid = true;
            for (int i = 0; i < agregation.Count; i++)
            {

                if (agregation[i].Contains(tbVial1.Text))
                {

                    isvialvalid = false;
                    break;
                }

            }

            foreach (string items in agregation2)
            {
                System.Console.WriteLine(items);
                if (items.Contains( tbVial1.Text))
                {
                    vialvalidationLog.Add(tbCarton1.Text + "&" + tbVial1.Text + "&" + "FAIL" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                    vialLog.Add(tbCarton1.Text + "&" + tbVial1.Text + "&" + "FAIL" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                    DataRow row = table.NewRow();
                    row[0] = tbVial1.Text;
                    row[1] = "FAIL";
                    row[2] = DateTime.Now.ToLongDateString();
                    row[3] = DateTime.Now.ToLongTimeString();
                    table.Rows.Add(row);
                    dataGridView1.DataSource = table;
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    lblQty1.Text = table.Rows.Count.ToString();
                    lblFail1.Text = vialvalidationLog.Count.ToString();
                    gs1idvial1 = null;
                    tbVial1.Text = "";
                    //tbVial1.Enabled = false;
                    isvialvalid = false;
                    break;


                }
            }

            if (isvialvalid)
            {


                List<string> field = new List<string>();
                field.Add("gsonevialid");
                List<string> field2 = new List<string>();
                field2.Add("gsonevialid");
                string guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6).ToUpper();

                if (cap2 == null)
                {
                    cap2 = bnumber + guid.PadLeft(3, '0');
                }

                if (db.selectList(field, "[vaccine_packaging]", "gsonevialid='" + tbVial1.Text + "' AND batchnumber='" + bnumber + "'AND isreject='1' AND flag='2'") != null)
                {
                    cap = cbProduct1.SelectedValue.ToString() + bnumber + guid.PadLeft(3, '0');
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    agregation.Add("'" + tbVial1.Text + "'");
                    DataRow row = table.NewRow();
                    row[0] = tbVial1.Text;
                    row[1] = "PASS";
                    row[2] = DateTime.Now.ToLongDateString();
                    row[3] = DateTime.Now.ToLongTimeString();
                    table.Rows.Add(row);
                    dataGridView1.DataSource = table;
                    dataGridView1.Sort(dataGridView1.Columns[3], ListSortDirection.Descending);
                    lblQty1.Text = table.Rows.Count.ToString();
                    lblPass1.Text = agregation.Count.ToString();
                    tbVial1.Text = "";
                    //tbVial1.Enabled = false;
                    gs1idvial1 = null;
                    vialLog.Add(tbCarton1.Text + "&" + tbVial1.Text + "&" + "Success" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());


                    if (agregation.Count == int.Parse(_comboProduct[cbProduct1.SelectedIndex].Qty))
                    {
                        //tbVial1.Enabled = false;
                        int i = 0;
                        string aggregationWhere = String.Join(",", agregation.ToArray());
                        if (db.aggregationVial(aggregationWhere, tbCarton1.Text, infeedid))
                        {
                            agregation.Clear();
                            vialvalidationLog.Clear();
                            lblQty1.Text = "";
                            table.Rows.Clear();
                            Dictionary<string, string> syslog = new Dictionary<string, string>();
                            syslog.Add("eventType", "12");
                            syslog.Add("eventName", "Aggregation GS1 ID Carton " + tbCarton1.Text + " Batchnumber " + tbBatchnumber1.Text);
                            syslog.Add("[from]", "Packaging");
                            syslog.Add("uom", "Carton");
                            syslog.Add("userid", adminid);
                            db.insert(syslog, "[system_log]");
                            log.LogWriter("Aggregation GS1 ID Carton " + tbCarton1.Text + " Batchnumber " + tbBatchnumber1.Text + " Status Success");
                            lblQtyBatch1.Text = db.selectCountPass(bnumber).ToString();
                            lblPass1.Text = "0";
                            lblFail1.Text = "0";
                            lblQty1.Text = "0";
                            tbCarton1.Text = "";
                            //tbCarton1.Enabled = false;
                            gs1carton1 = null;
                            gs1idvial1 = null;
                            cartonvalidated = false;

                        }
                        else
                        {
                            tbCarton1.Text = "";
                            //tbCarton1.Enabled = false;
                            gs1carton1 = null;
                            gs1idvial1 = null;
                            cartonvalidated = false;
                        }
                    }
                }
                else
                {

                    vialvalidationLog.Add(tbCarton1.Text + "&" + tbVial1.Text + "&" + "FAIL" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                    vialLog.Add(tbCarton1.Text + "&" + tbVial1.Text + "&" + "FAIL" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                    DataRow row = table.NewRow();
                    row[0] = tbVial1.Text;
                    row[1] = "FAIL";
                    row[2] = DateTime.Now.ToLongDateString();
                    row[3] = DateTime.Now.ToLongTimeString();
                    table.Rows.Add(row);
                    dataGridView1.DataSource = table;
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    lblQty1.Text = table.Rows.Count.ToString();
                    lblFail1.Text = vialvalidationLog.Count.ToString();
                    gs1idvial1 = null;
                    tbVial1.Text = "";
                    //tbVial1.Enabled = false;

                }

            }
            else
            {

                tbVial1.Text = "";
                //tbVial1.Enabled = false;
                gs1idvial1 = null;

            }

        }







        public bool checkScanner(string hid)
        {

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

        private void button3_Click(object sender, EventArgs e)
        {
            Log log = new Log("Log Vial & Carton Validation", vialvalidationLog);
            log.Show();
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToShortDateString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            agregation.Clear();
            vialvalidationLog.Clear();
            cartonvalidated = false;
            tbCarton1.Text = "";
            tbVial1.Text = "";
            lblPass1.Text = "0";
            lblFail1.Text = "0";
            btnLog1.Enabled = true;
            //tbCarton1.Enabled = false;
            //tbVial1.Enabled = false;
            cbProduct1.Enabled = true;
            cbProduct1.SelectedIndex = 0;
            tbBatchnumber1.Text = "";
            tbBatchnumber1.Enabled = false;
            lblQtyBatch1.Text = "";
            btEnd1.Enabled = false;
            btStart1.Enabled = false;
            table.Rows.Clear();
            dataGridView1.DataSource = table;
            lblQty1.Text = "0";
            isstart = false;
        }





        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbBatchnumber2.Enabled = true;
        }

        private void textBox6_Click(object sender, EventArgs e)
        {
            using (var listbatch = new ListBatch("('" + cbProduct2.SelectedValue.ToString() + "')"))
            {
                var result = listbatch.ShowDialog();
                if (result == DialogResult.OK)
                {
                    bnumber2 = listbatch.batchNumber;
                    tbBatchnumber2.Text = listbatch.batchNumber;
                    btStart2.Enabled = true;
                    lblQtyBatch2.Text = db.selectCountPass(bnumber2).ToString();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            isstart2 = true;
            cbProduct2.Enabled = false;
            btEnd2.Enabled = true;
            btStart2.Enabled = false;
            tbBatchnumber2.Enabled = false;
            btnLog2.Enabled = false;
        }

        private void cartonValidation2()
        {

            List<string> field = new List<string>();
            field.Add("infeedinnerboxid");
            List<string[]> dsinnerbox = db.selectList(field, "[innerbox]", "gsoneinnerboxid='" + tbCarton2.Text + "' AND batchnumber='" + bnumber2 + "' AND isreject='1' AND flag='2'");
            if (db.num_rows > 0)
            {
                log.LogWriter("Scan GS1 ID Carton " + tbCarton2.Text + " Batchnumber " + bnumber2 + " Status Success");
                tbCarton2.Enabled = false;
                cartonvalidated2 = true;
                foreach (string[] Row in dsinnerbox)
                {

                    infeedid2 = Row[0];
                }

            }
            else
            {
                log.LogWriter("Scan GS1 ID Carton " + tbCarton2.Text + " Batchnumber " + tbBatchnumber2.Text + " Status Error");
                tbCarton2.Text = "";
                tbCarton2.Enabled = false;
                DataRow row = table2.NewRow();
                row[0] = "Carton Not Found";
                row[1] = "FAIL";
                row[2] = DateTime.Now.ToLongDateString();
                row[3] = DateTime.Now.ToLongTimeString();
                table2.Rows.Add(row);
                vialLog2.Add("Carton Not Found" + "&" + tbVial2.Text + "&" + "FAIL" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                vialvalidationLog2.Add(tbCarton2.Text + "&" + tbVial2.Text + "&" + "FAIL" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                lblQty2.Text = table2.Rows.Count.ToString();
                lblFail2.Text = vialvalidationLog2.Count.ToString();
                gs1carton2 = null;
            }
        }


        private void vialValidation2()
        {
            bool isvialvalid2 = true;

            for (int i = 0; i < agregation2.Count; i++)
            {

                if (agregation2[i].Contains(tbVial2.Text))
                {

                    isvialvalid2 = false;
                    break;
                }

            }

            foreach (string items in agregation)
            {
                System.Console.WriteLine(items);
                if (items.Contains(tbVial2.Text))
                {
                    DataRow row = table2.NewRow();
                    row[0] = tbVial2.Text;
                    row[1] = "FAIL";
                    row[2] = DateTime.Now.ToLongDateString();
                    row[3] = DateTime.Now.ToLongTimeString();
                    table2.Rows.Add(row);
                    dataGridView2.DataSource = table2;
                    lblQty2.Text = table2.Rows.Count.ToString();
                    tbVial2.Text = "";
                    tbVial2.Enabled = false;
                    vialLog2.Add(tbCarton2.Text + "&" + tbVial2.Text + "&" + "FAIL" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                    vialvalidationLog2.Add(tbCarton2.Text + "&" + tbVial2.Text + "&" + "FAIL" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                    tbVial2.Text = "";
                    lblFail2.Text = vialvalidationLog2.Count.ToString();
                    gs1idvial2 = null;
                    isvialvalid2 = false;
                    break;


                }
            }

            if (isvialvalid2)
            {


                List<string> field = new List<string>();
                field.Add("gsonevialid");
                List<string> field2 = new List<string>();
                field2.Add("gsonevialid");
                string guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6).ToUpper();

                if (cap2 == null)
                {
                    cap2 = bnumber2 + guid.PadLeft(3, '0');
                }
                if (db.selectList(field, "[vaccine_packaging]", "gsonevialid='" + tbVial2.Text + "' AND batchnumber='" + bnumber2 + "'AND isreject='1' AND flag='2'") != null)
                {
                    cap = cbProduct2.SelectedValue.ToString() + bnumber2 + guid.PadLeft(3, '0');
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    agregation2.Add("'" + tbVial2.Text + "'");
                    DataRow row = table2.NewRow();
                    row[0] = tbVial2.Text;
                    row[1] = "PASS";
                    row[2] = DateTime.Now.ToLongDateString();
                    row[3] = DateTime.Now.ToLongTimeString();
                    table2.Rows.Add(row);
                    dataGridView2.DataSource = table2;
                    dataGridView2.Sort(dataGridView2.Columns[3], ListSortDirection.Descending);
                    vialLog2.Add(tbCarton2.Text + "&" + tbVial2.Text + "&" + "Pass" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                    lblQty2.Text = table2.Rows.Count.ToString();
                    lblPass2.Text = agregation2.Count.ToString();
                    tbVial2.Text = "";
                    tbVial2.Enabled = false;
                    gs1idvial2 = null;

                    if (agregation2.Count == int.Parse(_comboProduct2[cbProduct2.SelectedIndex].Qty))
                    {
                        tbVial2.Enabled = false;
                        int i = 0;
                        string aggregationWhere = String.Join(",", agregation2.ToArray());

                        if (db.aggregationVial(aggregationWhere, tbCarton2.Text, infeedid2))
                        {
                            agregation2.Clear();
                            vialvalidationLog2.Clear();
                            lblQty2.Text = "";
                            table2.Rows.Clear();
                            Dictionary<string, string> syslog = new Dictionary<string, string>();
                            syslog.Add("eventType", "12");
                            syslog.Add("eventName", "Aggregation GS1 ID Carton " + tbCarton1.Text+ " Batchnumber " + tbBatchnumber1.Text);
                            syslog.Add("[from]", "Packaging");
                            syslog.Add("uom", "Carton");
                            syslog.Add("userid", adminid);
                            db.insert(syslog, "[system_log]");
                            log.LogWriter("Aggregation GS1 ID Carton " + tbCarton2.Text + " Batchnumber " + tbBatchnumber2.Text + " Status Success");
                            lblQtyBatch2.Text = db.selectCountPass(bnumber2).ToString();
                            tbCarton2.Text = "";
                            lblPass2.Text = "0";
                            lblFail2.Text = "0";
                            lblQty2.Text = "0"; 
                            cartonvalidated2 = false;
                            gs1carton2 = null;
                            gs1idvial2 = null;
                            tbCarton2.Enabled = false;

                        }
                        else
                        {

                            AutoClosingMessageBox.Show("Cannot Connect To Database Server!", "Error", 3000);
                        }
                    }
                }
                else
                {

                    DataRow row = table2.NewRow();
                    row[0] = tbVial2.Text;
                    row[1] = "FAIL";
                    row[2] = DateTime.Now.ToLongDateString();
                    row[3] = DateTime.Now.ToLongTimeString();
                    table2.Rows.Add(row);
                    dataGridView2.DataSource = table2;
                    lblQty2.Text = table2.Rows.Count.ToString();
                    tbVial2.Text = "";
                    tbVial2.Enabled = false;
                    vialLog2.Add(tbCarton2.Text + "&" + tbVial2.Text + "&" + "FAIL" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                    vialvalidationLog2.Add(tbCarton2.Text + "&" + tbVial2.Text + "&" + "FAIL" + "&" + DateTime.Now.ToLongDateString() + "&" + DateTime.Now.ToLongTimeString());
                    tbVial2.Text = "";
                    lblFail2.Text = vialvalidationLog2.Count.ToString();
                    gs1idvial2 = null;
                }

            }
            else
            {
                tbVial2.Text = "";
                tbVial2.Enabled = false;
                gs1idvial2 = null;

            }





        }

        private void button5_Click(object sender, EventArgs e)
        {
            Log log = new Log("Log Vial Validation", vialvalidationLog);
            log.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Log log = new Log("Log Vial Validation", vialvalidationLog2);
            log.Show();
        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            tbCarton2.Text = "";
            agregation2.Clear();
            vialvalidationLog2.Clear();
            cartonvalidated2 = false;
            tbVial2.Text = "";
            gs1carton2 = null;
            gs1idvial2 = null;
            lblPass2.Text = "0";
            lblFail2.Text = "0";
            btnLog2.Enabled = true;
            cbProduct2.Enabled = true;
            cbProduct2.SelectedIndex = 0;
            tbVial2.Enabled = false;
            tbCarton2.Enabled = false;
            tbBatchnumber2.Text = "";
            tbBatchnumber2.Enabled = false;
            lblQty2.Text = "0";
            btEnd2.Enabled = false;
            btStart2.Enabled = false;
            table2.Rows.Clear();
            dataGridView2.DataSource = table2;
            isstart2 = false;

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pbOnOff1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Log logs = new Log("Log Vial Validation", vialLog);
            logs.Show();
        }

        private void btnLog2_Click(object sender, EventArgs e)
        {
            Log logs = new Log("Log Vial Validation", vialLog2);
            logs.Show();
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

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {


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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tbCarton1_KeyDown(object sender, KeyEventArgs e)
        {
            System.Console.WriteLine(e.KeyValue);
            if (e.KeyValue == 13)
            {
                cartonValidation();
            }
        }

        private void tbVial1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                vialValidation();
            }
        }


    }
}
    

