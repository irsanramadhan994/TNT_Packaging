using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RawInput_dll;

namespace Biocov
{
    public partial class Comissioning : Form
    {

        db db = new db();
        string bnumber, infeed,hidscanner,hidscanner2,notifMessage,notifCaption;
        public bool cartonvalidated = false;
        public string adminid,gs1carton1,gs1vial1;
        DataTable table = new DataTable();
        Logger log = new Logger();
        private readonly RawInput _rawinput;
        private bool isStart;
        
        Dictionary<string, string> config = new Dictionary<string, string>();
        const bool CaptureOnlyInForeground = true;
        sqlite sqlite = new sqlite();

        private class DataProduct
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Qty { get; set; }
        }

        BindingList<DataProduct> _comboProduct = new BindingList<DataProduct>();

        private void OnKeyPressed(object sender, RawInputEventArg e)
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
                    gs1vial1 += e.KeyPressEvent.VKeyName[1].ToString();
                }
                else
                {

                    gs1vial1 += e.KeyPressEvent.VKeyName;

                }



            }

            if (e.KeyPressEvent.VKeyName == "ENTER" && e.KeyPressEvent.KeyPressState == "MAKE" && (e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) || e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower())) && !cartonvalidated && isStart)
            {
                tbCarton.Text = gs1carton1;
                cartonValidation();
            }

            if (e.KeyPressEvent.VKeyName == "ENTER" && e.KeyPressEvent.KeyPressState == "MAKE" && (e.KeyPressEvent.DeviceName.Contains(hidscanner.ToLower()) || e.KeyPressEvent.DeviceName.Contains(hidscanner2.ToLower())) && cartonvalidated && isStart && gs1vial1!= null)
            {
                
                tbVial.Text = gs1vial1;
                vialCommision();
            }
            
            
        }

        public Comissioning()
        {
            InitializeComponent();
            timer1.Interval = 1000;
            timer1.Start();
            _rawinput = new RawInput(Handle, CaptureOnlyInForeground);
            isStart = false;
            cbProduct.DisplayMember = "Name";
            cbProduct.ValueMember = "Value";
            table.Columns.Add("GS1 ID");
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            getProductName();

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


        private void Comissioning_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = false;
            tbVial.Enabled = false;
            textBox2.Enabled = false;
            tbCarton.Enabled = false;
            tbBatchnumber.Enabled = false;
            dataGridView1.Enabled = false;
            btClear.Enabled = false;
            _rawinput.KeyPressed += OnKeyPressed;
            config = sqlite.getConfig(lblUserId.Text);
            hidscanner = config["hidscanner"];
            hidscanner2 = config["hidscanner2"];

        }
        public void getProductName()
        {
            List<string> field = new List<string>();
            field.Add("product_model,model_name,outbox_qty");
            List<string[]> dss = db.selectList(field, "[product_model]", "");
            if (db.num_rows > 0)
            {

                int i = 0;
                _comboProduct.Add(new DataProduct { Name = "-Select Product-", Value = null ,Qty = ""});
                foreach (string[] Row in dss)
                {
                    i++;
                    _comboProduct.Add(new DataProduct { Name = Row[1], Value = Row[0] , Qty = Row[2] });


                }
                cbProduct.DataSource = _comboProduct;

            }

        }

        private void Comissioning_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Comissioning_FormClosed(object sender, FormClosedEventArgs e)
        {
            _rawinput.KeyPressed -= OnKeyPressed;
            Application.Exit();
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

                }
            }
        }

        private void cartonValidation()
        {

            List<string> field = new List<string>();
            field.Add("gsonevialid 'GS1 Vial ID'");
            List<string[]> dss = db.selectList(field, "[vaccine]", "innerboxgsoneid='" + tbCarton.Text + "'AND batchnumber='" + bnumber + "'");
            if (db.num_rows > 0)
            {
                table = db.dataHeader;
                foreach (string[] Row in dss)
                {
                    DataRow row = table.NewRow();
                    row[0] = Row[0];
                    table.Rows.Add(row);


                }

                List<string> field2 = new List<string>();
                field2.Add("infeedinnerboxid");
                List<string[]> dsinnerbox = db.selectList(field2, "[innerbox]", "gsoneinnerboxid='" + tbCarton.Text + "' AND batchnumber='" + bnumber + "'");
                if (db.num_rows > 0)
                {

                    foreach (string[] Row in dsinnerbox)
                    {

                        infeed = Row[0];
                    }

                    this.dataGridView1.DataSource = table;
                    this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    lblVial.Text = (dataGridView1.Rows.Count - 1).ToString();
                    btClear.Enabled = true;
                    tbCarton.Enabled = false;
                    cartonvalidated = true;


                }
                else
                {
                    notifMessage = "Carton Not Found";
                    notifCaption = "Notification";
                    timer2.Start();
                    gs1carton1 = "";
                    tbCarton.Text = "";
                    tbCarton.Enabled = false;
                }



            }
            else
            {
                notifMessage = "Carton Not Found";
                notifCaption = "Notification";
                timer2.Start();
                gs1carton1 = "";
                tbCarton.Text = "";
                tbCarton.Enabled = false;
            }

        }


        private void vialCommision()
        {

            if (dataGridView1.Rows.Count > int.Parse(_comboProduct[cbProduct.SelectedIndex].Qty))
            {
                notifMessage = "Carton Box Full";
                notifCaption = "Notification";
                timer2.Start();
                tbVial.Text = "";
                tbVial.Focus();
            }
            else
            {

                List<string> field = new List<string>();
                field.Add("gsonevialid");
                if (db.selectList(field, "[vaccine]", "gsonevialid='" + tbVial.Text + "' AND batchnumber='" + bnumber + "' AND isreject='1' AND flag='2'") != null)
                {
                    Dictionary<string, string> updatelist = new Dictionary<string, string>();
                    updatelist.Add("innerboxgsoneid", tbCarton.Text);
                    updatelist.Add("innerboxid", infeed);
                    updatelist.Add("isreject", "0");
                    updatelist.Add("flag", "0");
                    if (db.update(updatelist, "[vaccine]", "gsonevialid='" + tbVial.Text + "' AND batchnumber='" + bnumber + "'") > 0)
                    {
                        table.Rows.Clear();

                        List<string[]> dss = db.selectList(field, "[vaccine]", "innerboxgsoneid='" + tbCarton.Text + "'AND batchnumber='" + bnumber + "'");
                        if (db.num_rows > 0)
                        {

                            foreach (string[] Row in dss)
                            {
                                DataRow row = table.NewRow();
                                row[0] = Row[0];
                                table.Rows.Add(row);


                            }
                            dataGridView1.DataSource = table;
                            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            notifMessage = "Commisioning Success!";
                            notifCaption = "Notification";
                            timer2.Start(); lblVial.Text = (dataGridView1.Rows.Count - 1).ToString();
                            Dictionary<string, string> syslog = new Dictionary<string, string>();
                            syslog.Add("eventType", "12");
                            syslog.Add("eventName", "Commisioning " + tbVial.Text + " Batchnumber " + tbBatchnumber.Text);
                            syslog.Add("[from]", "Packaging");
                            syslog.Add("uom", "Vial");
                            syslog.Add("userid", adminid);
                            db.insert(syslog, "[system_log]");
                            log.LogWriter("Commisioning " + tbVial.Text + " Batchnumber " + tbBatchnumber.Text + " Status Success");
                            tbVial.Text = "";
                            gs1vial1 = "";


                        }

                    }
                    else
                    {
                        notifMessage = "Vial Not Found";
                        notifCaption = "Notification";
                        timer2.Start();
                        log.LogWriter("Vial " + tbVial.Text + " Batchnumber " + textBox2.Text + " Not Found");
                        tbVial.Text = "";
                        gs1vial1 = "";
                        tbVial.Enabled = false;
                    }


                }
                else
                {
                    notifMessage = "Vial Not Found";
                    notifCaption = "Notification";
                    timer2.Start();
                    log.LogWriter("Vial " + tbVial.Text + " Batchnumber " + textBox2.Text + " Not Found");
                    tbVial.Text = "";
                    gs1vial1 = "";
                    tbVial.Enabled = false;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard dbd = new Dashboard();
            dbd.lblRole.Text = lblRole.Text;
            dbd.lblUserId.Text = lblUserId.Text;
            dbd.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > int.Parse(_comboProduct[cbProduct.SelectedIndex].Qty))
            {

                MessageBox.Show("Carton Box Full");
                tbVial.Text = "";
                tbVial.Focus();
            }
            else
            {

                List<string> field = new List<string>();
                field.Add("gsonevialid");
                if (db.selectList(field, "[Vaccine]", "gsonevialid='" + tbVial.Text + "' AND batchnumber='" + bnumber + "'") != null)
                {
                    Dictionary<string, string> updatelist = new Dictionary<string, string>();
                    updatelist.Add("innerboxgsoneid", tbCarton.Text);
                    updatelist.Add("innerboxid", infeed);
                    updatelist.Add("isreject", "0");
                    updatelist.Add("flag", "0");
                    if (db.update(updatelist, "[vaccine]", "gsonevialid='" + tbVial.Text + "' AND batchnumber='" + bnumber + "'") > 0)
                    {
                        table.Rows.Clear();

                        List<string[]> dss = db.selectList(field, "[vaccine]", "innerboxgsoneid='" + tbCarton.Text + "'AND batchnumber='" + bnumber + "'");
                        if (db.num_rows > 0)
                        {
                            table = db.dataHeader;
                            foreach (string[] Row in dss)
                            {
                                DataRow row = table.NewRow();
                                row[0] = Row[0];
                                table.Rows.Add(row);


                            }
                            dataGridView1.DataSource = table;
                            AutoClosingMessageBox.Show("Commisioning Success", "Notification", 3000);
                            Dictionary<string, string> syslog = new Dictionary<string, string>();
                            syslog.Add("eventType", "12");
                            syslog.Add("eventName", "Commisioning " + tbVial.Text + " Batchnumber " + tbBatchnumber.Text);
                            syslog.Add("[from]", "Packaging");
                            syslog.Add("uom", "Vial");
                            syslog.Add("userid", adminid);
                            db.insert(syslog, "[system_log]");
                            log.LogWriter("Commisioning " + tbVial.Text + " Batchnumber " + tbBatchnumber.Text + " Status Success");
                            tbCarton.Text = "";



                        }

                    }
                    else {
                        AutoClosingMessageBox.Show("Vial Not Found", "Error", 3000);
                        log.LogWriter("Vial " + tbVial.Text + " Batchnumber " + textBox2.Text + " Not Found");
                        tbCarton.Text = "";
                        tbCarton.Enabled = false;
                    }


                }
                else
                {
                    AutoClosingMessageBox.Show("Vial Not Found", "Error", 3000);
                    log.LogWriter("Vial " + tbVial.Text + " Batchnumber " + textBox2.Text + " Not Found");
                    tbCarton.Text = "";
                    tbCarton.Enabled = false;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToShortDateString();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbBatchnumber.Enabled = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("cek");
            isStart = false;
            cartonvalidated = false;
            cbProduct.SelectedIndex = 0;
            btnStart.Enabled = true;
            cbProduct.Enabled = true;
            gs1carton1 = "";
            gs1vial1 = "";
            table.Clear();
            dataGridView1.DataSource = table;
            tbCarton.Text = "";
            lblVial.Text = "";
            bnumber = "";
            tbBatchnumber.Text = "";
            tbVial.Text = "";
            tbVial.Enabled = false;
            textBox2.Enabled = false;
            tbCarton.Enabled = false;
            tbBatchnumber.Enabled = false;
            btClear.Enabled = false;
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            Dashboard dbd = new Dashboard();
            dbd.lblRole.Text = lblRole.Text;
            _rawinput.KeyPressed -= OnKeyPressed;
            _rawinput.RemoveMessageFilter();
            dbd.lblUserId.Text = lblUserId.Text;
            dbd.adminid = adminid;
            dbd.Show();
            this.Hide();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            AutoClosingMessageBox.Show(notifMessage, notifCaption, 1500);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            btClear.Enabled = true;
            tbBatchnumber.Enabled = false;
            dataGridView1.Focus();
            cbProduct.Enabled = false;
            isStart = true;
            btnStart.Enabled = false;
            this.ActiveControl = null;
        }

    }
}
    