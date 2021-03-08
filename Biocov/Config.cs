using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.PointOfService;
using System.Windows.Forms;
using System.Management.Instrumentation;
using System.Management;
namespace Biocov
{
    public partial class Config : Form
    {

        sqlite sqlt = new sqlite();
        db db = new db();
        Dictionary<string, string> config = new Dictionary<string, string>();
        List<userList> _userList = new List<userList>();
        List<deviceList> _deviceList = new List<deviceList>();
        List<deviceList2> _deviceList2 = new List<deviceList2>();
        public string adminid;
        private class deviceList
        {
            public string devicename { get; set; }
            public string devicevalue { get; set; }

        }

        private class userList
        {
            public string user { get; set; }

        }
        private class deviceList2
        {
            public string devicename { get; set; }
            public string devicevalue { get; set; }



        }
        public Config()
        {

            InitializeComponent();
            cbUser.DisplayMember = "user";
            cbUser.ValueMember = "user";
            timer1.Interval = 1000;
            timer1.Start();

          
        }

        private void updateForm() {

            config = sqlt.getConfig(lblUserId.Text);
            tbIpPrinter.Text = config["ipprinter"];
            tbDbName.Text = config["dbname"];
            tbIpDB.Text = config["ipdb"];
            tbPortPrinter.Text = config["portprinter"];
            tbDbName.Text = config["dbname"];
            tbIpPrinter2.Text = config["ipprinter2"];
            tbPortPrinter2.Text = config["portprinter2"];
            tbIpPrinter3.Text = config["ipprinter3"];
            tbPortPrinter3.Text = config["portprinter3"];
        
        }



        private void Form_Load(object sender, EventArgs e)
        {
           
        }

        private void Config_Load(object sender, EventArgs e)
        {
            List<string> userlist = new List<string>();
            userlist = sqlt.getUser();
            Console.WriteLine(userlist.Count);
            if (userlist.Count > 0)
            {
                foreach (string items in userlist)
                {
                    Console.WriteLine(items);
                    _userList.Add(new userList { user = items });
                }
                cbUser.DataSource = _userList;
                cbUser.SelectedIndex = -1;
                cbUser.Text = "";
                cbUser.Focus();
                
            }
  }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var listOfStrings = new List<string>();
            string[] arrayOfStrings = listOfStrings.ToArray();
            Dashboard dbd = new Dashboard();
            dbd.lblRole.Text = lblRole.Text;
            dbd.lblUserId.Text = lblUserId.Text;
            dbd.adminid = adminid;
            dbd.Show();
            this.Hide();
            
        }

        private void Config_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbIpDB.Text != "")
            {
                if (tbDbName.Text != "")
                {
                    if (tbUserDb.Text != "")
                    {
                        if (tbPasswordDb.Text != "")
                        {
                            if (tbIpPrinter.Text != "")
                            {
                                if (tbPortPrinter.Text != "")
                                {
                                    if (tbIpPrinter2.Text != "")
                                    {
                                        if (tbPortPrinter2.Text != "")
                                        {
                                            if (tbIpPrinter3.Text != "")
                                            {
                                                if (tbPortPrinter3.Text != "")
                                                {

                                                            if (cbUser.Text != "")
                                                            {
                                                                if (cbUser.SelectedIndex == -1)
                                                                {
                                                                    Dictionary<string, string> insertconf = new Dictionary<string, string>();
                                                                    insertconf.Add("user", cbUser.Text);
                                                                    insertconf.Add("ipprinter", tbIpPrinter.Text);
                                                                    insertconf.Add("portprinter", tbPortPrinter.Text);
                                                                    insertconf.Add("ipdb", tbIpDB.Text);
                                                                    insertconf.Add("dbname", tbDbName.Text);
                                                                    insertconf.Add("username_db", tbUserDb.Text);
                                                                    insertconf.Add("password_db", tbPasswordDb.Text);
                                                                    insertconf.Add("hidscanner", "0");
                                                                    insertconf.Add("hidscanner2", "0");
                                                                    insertconf.Add("ipprinter2", tbIpPrinter2.Text);
                                                                    insertconf.Add("portprinter2", tbPortPrinter2.Text);
                                                                    insertconf.Add("ipprinter3", tbIpPrinter3.Text);
                                                                    insertconf.Add("portprinter3", tbPortPrinter3.Text);


                                                                    if (sqlt.insert(insertconf, "config"))
                                                                    {
                                                                        MessageBox.Show("Configuration successfully created");
                                                                        Dictionary<string, string> syslog = new Dictionary<string, string>();
                                                                        syslog.Add("eventType", "12");
                                                                        syslog.Add("eventName", "Configuration Insert User=" + lblUserId.Text);
                                                                        syslog.Add("[from]", "Packaging");
                                                                        syslog.Add("uom", "Vial");
                                                                        syslog.Add("userid", adminid);
                                                                        db.insert(syslog, "[system_log]");
                                                                    }
                                                                    else
                                                                    {

                                                                        MessageBox.Show("Configuration Error");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Dictionary<string, string> updateconf = new Dictionary<string, string>();
                                                                    updateconf.Add("ipprinter", tbIpPrinter.Text);
                                                                    updateconf.Add("portprinter", tbPortPrinter.Text);
                                                                    updateconf.Add("ipdb", tbIpDB.Text);
                                                                    updateconf.Add("dbname", tbDbName.Text);
                                                                    updateconf.Add("username_db", tbUserDb.Text);
                                                                    updateconf.Add("password_db", tbPasswordDb.Text);
                                                                    updateconf.Add("hidscanner", "0");
                                                                    updateconf.Add("hidscanner2", "0");
                                                                    updateconf.Add("ipprinter2", tbIpPrinter2.Text);
                                                                    updateconf.Add("portprinter2", tbPortPrinter2.Text);
                                                                    updateconf.Add("ipprinter3", tbIpPrinter3.Text);
                                                                    updateconf.Add("portprinter3", tbPortPrinter3.Text);


                                                                    if (sqlt.update(updateconf, "config", "user='" + cbUser.Text + "'"))
                                                                    {
                                                                        MessageBox.Show("Configuration successfully Changed");
                                                                        Dictionary<string, string> syslog = new Dictionary<string, string>();
                                                                        syslog.Add("eventType", "12");
                                                                        syslog.Add("eventName", "Configuration Update User=" + lblUserId.Text);
                                                                        syslog.Add("[from]", "Packaging");
                                                                        syslog.Add("uom", "Vial");
                                                                        syslog.Add("userid", adminid);
                                                                        db.insert(syslog, "[system_log]");

                                                                    }
                                                                    else
                                                                    {
                                                                        MessageBox.Show("Configuration Error");


                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("Please Select User ");
                                                            }
              
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Please Fill PORT PRINTER 3");
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Please Fill IP PRINTER 3");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please Fill PORT PRINTER 2");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Please Fill IP PRINTER 2");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Please Fill PORT PRINTER 1");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please Fill IP PRINTER 1");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Fill Database Password");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Fill Database User");
                    }
                }
                else
                {
                    MessageBox.Show("Please Fill Database Name");
                }
            }
            else {
                MessageBox.Show("Please Fill Database IP");
            }


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToShortDateString();

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbUser.SelectedIndex != -1)
            {
                config = sqlt.getConfig(cbUser.SelectedValue.ToString());
                if (config.ContainsKey("ipprinter"))
                {
                    tbIpPrinter.Text = config["ipprinter"];
                    tbDbName.Text = config["dbname"];
                    tbIpDB.Text = config["ipdb"];
                    tbPortPrinter.Text = config["portprinter"];
                    tbDbName.Text = config["dbname"];
                    tbIpPrinter2.Text = config["ipprinter2"];
                    tbPortPrinter2.Text = config["portprinter2"];
                    tbIpPrinter3.Text = config["ipprinter3"];
                    tbPortPrinter3.Text = config["portprinter3"];
                    tbPasswordDb.Text = config["password_db"];
                    tbUserDb.Text = config["username_db"];
                }

            }
            else {

                tbIpPrinter.Text = "";
                tbDbName.Text = "";
                tbIpDB.Text = "";
                tbPortPrinter.Text = "";
                tbDbName.Text = "";
                tbIpPrinter2.Text = "";
                tbPortPrinter2.Text = "";
                tbIpPrinter3.Text = "";
                tbPortPrinter3.Text = "";
                tbPasswordDb.Text = "";
                tbUserDb.Text = "";
            }
            
        }

        private void tbPortPrinter_TextChanged(object sender, EventArgs e)
        {
    
        }

        private void tbPortPrinter2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbPortPrinter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
    (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void tbPortPrinter2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
    (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void tbPortPrinter3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
    (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

     
    }
}
