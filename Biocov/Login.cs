using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Biocov
{
    public partial class Login : Form
    {
        db db = new db();
        Dbconf dbconf = new Dbconf();
        Logger log = new Logger();
        public string adminid;
        public Login()
        {
            InitializeComponent();

        }

     

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            do_login();
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtUsername.Text.Length > 0)
                {
                    this.ActiveControl = txtPassword;
                }
                else
                {
                    MessageBox.Show("Username cannot empty");
                    txtUsername.Focus();
                }
            }
        }


        private void do_login()
        {
            //this.Hide();
            //dbd.Show();

            //db = new db();
            try
            {
                //cek useraname
                if (txtUsername.Text.Length != 0)
                {
                    //cek Password
                    if (txtPassword.Text.Length != 0)
                    {
                        List<string[]> res = db.validasi(txtUsername.Text, txtPassword.Text, "0");
                        if (res != null)
                        {
                            adminid = res[0][0];
                            if (db.cekPermision(adminid, "Serialization", "read"))
                            {
                                //string[] lineBottle = db.cekLine("biocov");

                                List<string[]> resu = db.getRole(adminid);
                                Dashboard dbd = new Dashboard();
                                dbd.lblUserId.Text = resu[0][0].ToString();
                                dbd.lblRole.Text = resu[0][1].ToString();
                                dbd.adminid = adminid;
                                Dictionary<string, string> systemLog = new Dictionary<string, string>();
                                systemLog.Add("eventType", "1");
                                systemLog.Add("eventName", "Login");
                                systemLog.Add("[from]", "Packaging");
                                systemLog.Add("uom", "");
                                systemLog.Add("userid", adminid);
                                db.insert(systemLog, "[system_log]");
                                dbd.Show();
                                clearText();
                                this.Hide();
                                //if (lineBottle.Length > 0)
                                //{

                                //}
                                //else
                                //{
                                //    string ip = db.ipAddress;
                                //    MessageBox.Show("IP Address " + ip + " is not recognize,\n Contact your administrator", "Information", MessageBoxButtons.OK);
                                //    clearText();
                                //}
                            }
                            else
                            {
                                MessageBox.Show("User tidak memiliki permission");
                                clearText();
                            }
                        }
                        else
                        {
                            clearText();
                            dbconf.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Silahkan isi Password");
                        txtPassword.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Silahkan Isi Username");
                    txtUsername.Focus();
                }
            }
            catch (ArgumentException ae)
            {
                log.LogWriter(ae.ToString());
                clearText();
            }
        }

   


        private void clearText()
        {  
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtUsername.Focus();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            var regex = new Regex(@"[^a-zA-Z0-9\s\b\@\-_\.]");
            if (regex.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }


        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtUsername.Text.Length > 0)
                {
                    if (txtPassword.Text.Length > 0)
                    {
                        do_login();
                    }
                    else
                    {
                        MessageBox.Show("Password cannot empty");
                        txtPassword.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Password cannot empty");
                    txtUsername.Focus();
                }
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            var gs1 = "010500045606612921A00000000187891721053110CTMAV504";
            MessageBox.Show(gs1.Contains("CTMAV504").ToString());
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
         
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
