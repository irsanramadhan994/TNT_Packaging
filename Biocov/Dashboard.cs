using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Management.Instrumentation;
using System.Management;

namespace Biocov
{
    public partial class Dashboard : Form
    {

        db db;
        ListBatch lbf;
        public string adminid;
        sqlite sqlites = new sqlite();
        Dictionary<string, string> config = new Dictionary<string, string>();
        public Dashboard()
        {
            InitializeComponent();
            db = new db();

            

        }

        public List<string[]> GetOpenBatch(string product)
        {
            List<string> field = new List<string>();
            field.Add("batchNumber,productModelId");
            string where = "status = '1' and productModelId IN " + product + "";
            return db.selectList(field, "[packaging_order]", where);
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            if (lblRole.Text != "admin") {
                pbConfiguration.Hide();
            }
            List<string> field = new List<string>();
            field.Add("a.batchNumber 'Batch Number', b.model_name 'Product', a.packagingQty 'Qty', a.startdate 'Start Date', a.enddate 'End Date'");
            List<string[]> dss = db.selectList(field, "[packaging_order] a inner join [product_model] b on a.productModelId = b.product_model ", "a.status = 1  group by a.batchNumber, b.model_name, a.packagingQty, a.startdate, a.enddate, a.packagingOrderNumber order by a.packagingOrderNumber desc");
            lblCountBiocov.Text = db.num_rows.ToString();
            timer1.Start();
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToShortDateString();
            config = sqlites.getConfig(lblUserId.Text);
            if (config.ContainsKey("ipprinter") == false)
            {

                MessageBox.Show("User " + lblUserId.Text + " Dont Have Configuration, Please Contact Administrator!", "Error");
                if (lblRole.Text == "admin")
                {
                    Config cfg = new Config();
                    cfg.lblRole.Text = lblRole.Text;
                    cfg.lblUserId.Text = lblUserId.Text;
                    cfg.Show();
                    this.Hide();
                }
                else
                { Application.Exit();
                }


            }
            using (var calibration = new Calibration())
            {
                var result = calibration.ShowDialog();
                if (result == DialogResult.OK)
                {   Dictionary<string,string> fieldUpdate = new Dictionary<string,string>();
                var hidscannersplit1 = calibration.hidscanner1.Split('&');
                var hidscannersplit2 = calibration.hidscanner2.Split('&');

                fieldUpdate.Add("hidscanner", hidscannersplit1[3].ToUpper());
                fieldUpdate.Add("hidscanner2", hidscannersplit2[3].ToUpper());
                sqlites.update(fieldUpdate, "config", "user='" + lblUserId.Text + "'");
                }
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
                Console.WriteLine(array[3]);
                Console.WriteLine(hid);
        
                if (array[3] == hid.ToUpper())
                {
                    return true;
                }
            }
            return false;

        }

       

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void pnlBiocov_Paint(object sender, PaintEventArgs e)
        {

        }
        private void lblCountBiocov_Click(object sender, EventArgs e)
        {

            lbf = new ListBatch("");
            lbf.Show();
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Decomision dcm = new Decomision();
            dcm.lblRole.Text = lblRole.Text;
            dcm.lblUserId.Text = lblUserId.Text;
            dcm.adminid = adminid;
            dcm.Show();
            this.Hide();
        }

        private void pnlDashboard_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pbGenerateCode_Click(object sender, EventArgs e)
        {

            GenerateCode gnc = new GenerateCode();
            gnc.lblRole.Text = lblRole.Text;
            gnc.lblUserId.Text = lblUserId.Text;
            gnc.adminid = adminid;
            gnc.Show();
            this.Hide();
                
        }

        private void pbValidasi_Click(object sender, EventArgs e)
        {
            CartonValidation cv = new CartonValidation();
            cv.lblRole.Text = lblRole.Text;
            cv.lblUserId.Text = lblUserId.Text;
            cv.adminid = adminid;
            cv.Show();
            this.Hide();
  
        }

        private void pbConfiguration_Click(object sender, EventArgs e)
        {
            Config cfg = new Config();
            cfg.lblRole.Text = lblRole.Text;
            cfg.lblUserId.Text = lblUserId.Text;
            cfg.adminid = adminid;
            cfg.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CartonVialValidation cvv = new CartonVialValidation();
            cvv.lblRole.Text = lblRole.Text;
            cvv.lblUserId.Text = lblUserId.Text;
            cvv.adminid = adminid;
            cvv.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Comissioning cmn = new Comissioning();
            cmn.lblRole.Text = lblRole.Text;
            cmn.lblUserId.Text = lblUserId.Text;
            cmn.adminid = adminid;
            cmn.Show();
            this.Hide();
        }

        private void Receh_Click(object sender, EventArgs e)
        {
            Cartoning crtn = new Cartoning();
            crtn.lblRole.Text = lblRole.Text;
            crtn.lblUserId.Text = lblUserId.Text;
            crtn.adminid = adminid;
            crtn.Show();
            this.Hide();

        }


  
    }
}
