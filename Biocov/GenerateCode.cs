using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;


namespace Biocov
{
    public partial class GenerateCode : Form
    {
        db db = new db();
        sqlite sqlite = new sqlite();
        Dictionary<string, string> config = new Dictionary<string, string>();
        generate generate = new generate();
        private bool isSas;
        DataTable table = new DataTable();
        int qty;
        Logger log = new Logger();
        List<string> listisvial = new List<string>();
        tcp tcp = new tcp();
        Printer printer = new Printer();
        public int printed;
        public List<string> gs1idCartonList;
        public string adminid;
        string bnumber,productmodelid;
        public  bool isdbused = false;
        delegate void ProgressBarDelegate(int number);
        private class DataProduct
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        private class DataBatch
        {
            public string Name { get; set; }
        }

  
        
        public GenerateCode()
        {
            InitializeComponent();
            cbProduct.DisplayMember = "Name";
            cbProduct.ValueMember = "Value";
            bnumber = "";
           // button2.Enabled = false;
            pbClear.Enabled = false;
            timer1.Interval = 1000;
            timer1.Start();
            timer2.Interval = 5000;
            timer2.Start();
            getProductName();
            tbQty.Text = null;
        }



        private void GenerateCode_Load(object sender, EventArgs e)
        {
            config = sqlite.getConfig(lblUserId.Text);
            pbGenerate.Enabled = false;
            pbPrint.Enabled = false;
            tbBatchnumber.Enabled = false;
            tbQty.Enabled = false;
            progressBar1.Hide();
        }



        public void getProductName() {
            List<string> field = new List<string>();
            field.Add("product_model,model_name");
            List<string[]> dss = db.selectList(field,"[product_model]","");
            if (db.num_rows > 0) {

                int i = 0;
                 BindingList<DataProduct> _comboProduct = new BindingList<DataProduct>();
                 _comboProduct.Add(new DataProduct { Name = "-Select Product-", Value = null });
                foreach(string[] Row in dss){
                    i++;
                    _comboProduct.Add(new DataProduct { Name = Row[1], Value = Row[0] });
                  
                   
                }
                cbProduct.DataSource = _comboProduct;
               
            }
            
        }




      

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbBatchnumber.Enabled = true;
            tbBatchnumber.Focus();
            List<string> field = new List<string>();
            field.Add("isSas");
            try {
                List<string[]> sas = db.selectList(field, "product_model", "product_model='" + cbProduct.SelectedValue.ToString() + "'");
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



        private void button1_Click(object sender, EventArgs e){



            if(cbProduct.SelectedIndex > 0){
                if (bnumber != "")
                {

                    if (!String.IsNullOrEmpty(tbQty.Text))
                    {
                        cbProduct.Enabled = false;
                        tbQty.Enabled = false;
                        pbGenerate.Enabled = false;
                        tbBatchnumber.Enabled = false;
                        qty = int.Parse(tbQty.Text);
                        progressBar1.Show();
                        if (bnumber != null)
                        {
                            backgroundWorker2.RunWorkerAsync();
                            
                        }
                    }
                    else
                    {

                        MessageBox.Show("Silahkan isi Kolom Quantity");
                    }
                }
                else {
                    MessageBox.Show("Silahkan Pilih Batch Number");
                }
            }else{
                 MessageBox.Show("Silahkan Pilih Product");
            }

           

            
           
        }

        public void validateGS1() {

            if (generate.checkDbExist(this) == false)
            {
                insertDatagrid(gs1idCartonList);
            }
            else {
                gs1idCartonList = generate.generateCarton(this, int.Parse(tbQty.Text), bnumber, cbProduct.SelectedValue.ToString());
                validateGS1();
            }
        }

      

        public void insertDatagrid(List<string> gs1idcarton)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            table = new DataTable();
            table.Columns.Add("GS1 ID").SetOrdinal(0); ;
            foreach(string Row in gs1idcarton){

                DataRow row = table.NewRow();
                row[0] = Row;
                table.Rows.Add(row);
            }
            lblGs1id.Text = gs1idcarton.Count.ToString();
            dataGridView1.DataSource = table;
            this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            pbPrint.Enabled = true;
            pbClear.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            cbProduct.Enabled = true;
            pbGenerate.Enabled = false;
            pbPrint.Enabled = false;
            tbBatchnumber.Enabled = false;
            tbQty.Enabled = false;
            tbQty.Text = null;
            printed = 0;
            lblPrinted.Text = "";
            tbBatchnumber.Text = "";
            cbProduct.SelectedIndex = 0;
            table = new DataTable();
            lblGs1id.Text = "";
            backgroundWorker3.CancelAsync();
            generate.stopLoop = true;
        }

     
        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToShortDateString();
            //if (!isdbused) {
            //    backgroundWorker1.RunWorkerAsync();
            //}
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            pbPrint.Enabled = false;
            progressBar1.Show();
            backgroundWorker3.RunWorkerAsync();

        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_Click(object sender, EventArgs e)
        {
          
         using(var listbatch = new ListBatch("('"+cbProduct.SelectedValue.ToString()+"')"))
         {
            var result = listbatch.ShowDialog();
            if(result == DialogResult.OK)
            {
                bnumber = listbatch.batchNumber;
                tbBatchnumber.Text = listbatch.batchNumber;
                tbQty.Enabled = true;
                tbQty.Focus();
                productmodelid = cbProduct.SelectedValue.ToString();
                generate.getPO(bnumber);
            }
         }  
           
      

           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
                    
            Dashboard dbd = new Dashboard();
            dbd.lblRole.Text = lblRole.Text;
            dbd.lblUserId.Text = lblUserId.Text;
            dbd.adminid = adminid;
            this.Hide();
            dbd.Show();
        }

        private void GenerateCode_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress,100);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {



            if (PingHost(config["ipprinter"]))
            {
                pbOnOffPrinter.Image = Properties.Resources.on1;

            }
            else
            {
                pbOnOffPrinter.Image = Properties.Resources.off1;
            }

            if (PingHost(config["ipprinter2"]))
            {
                pbOnOffPrinter2.Image = Properties.Resources.on1;

            }
            else
            {
                pbOnOffPrinter2.Image = Properties.Resources.off1;
            }

            if (PingHost(config["ipprinter3"]))
            {
                pbOnOffPrinter3.Image = Properties.Resources.on1;

            }
            else
            {
                pbOnOffPrinter3.Image = Properties.Resources.off1;
            }


        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(tbQty.Text, out value))
            {
                if (value > 5000)
                    tbQty.Text = "5000";
                else if (value < 0)
                    tbQty.Text = "0";
            }

            if (tbQty.Text != "")
            {
                pbGenerate.Enabled = true;
            }
            else {
                pbGenerate.Enabled = false;
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            timer2.Stop();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer2.Start();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            gs1idCartonList = generate.generateCarton(this, qty, bnumber, productmodelid);

        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (gs1idCartonList != null)
            {

                insertDatagrid(gs1idCartonList);
                Dictionary<string, string> syslog = new Dictionary<string, string>();
                syslog.Add("eventType", "12");
                syslog.Add("eventName", "Generate GS1 ID Carton " + tbQty.Text + " Qty");
                syslog.Add("[from]", "Packaging");
                syslog.Add("uom", "Carton");
                syslog.Add("userid", adminid);
                db.insert(syslog,"[system_log]");
                progressBar1.Hide();

            }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            generate.insertDB(gs1idCartonList, productmodelid, this,lblUserId.Text,isSas);

        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           lblGs1id.Text = gs1idCartonList.Count.ToString();
           lblGs1id.Refresh();
           progressBar1.Hide();
           dataGridView1.DataSource = null;
           dataGridView1.Refresh();
           Dictionary<string, string> syslog = new Dictionary<string, string>();
           syslog.Add("eventType", "12");
           syslog.Add("eventName", "Print GS1 ID Carton " + tbQty.Text + " Qty");
           syslog.Add("[from]", "Packaging");
           syslog.Add("uom", "Carton");
           syslog.Add("userid", adminid);
           db.insert(syslog, "[system_log]");
           log.LogWriter("Print GS1 ID Carton Quantity " + tbQty.Text + " Batchnumber " + tbBatchnumber.Text);
           lblPrinted.Text = printed.ToString();

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }



    }

}
