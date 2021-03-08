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
    public partial class Dbconf : Form
    {
        sqlite sqlites = new sqlite();
        Dictionary<string, string> config = new Dictionary<string, string>(); 

        public Dbconf()
        {
            InitializeComponent();
        }

        private void Dbconf_Load(object sender, EventArgs e)
        {
            config = sqlites.getConfig("MGI");
            tbDbName.Text = config["dbname"];
            tbIP.Text = config["ipdb"];
            tbDbUser.Text = config["username_db"];
            tbDbPassword.Text = config["password_db"];
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            Dictionary<string, string> update = new Dictionary<string, string>();
            update.Add("dbname", tbDbName.Text);
            update.Add("ipdb", tbIP.Text);
            update.Add("username_db", tbDbUser.Text);
            update.Add("password_db", tbDbPassword.Text);
            if (sqlites.update(update, "config", "user='MGI'"))
            {
                MessageBox.Show("Data Successfully Saved");
                Application.Exit();
            }
            else {
                MessageBox.Show("Cannot Connect to Database!");

            }
    
            
        }
    }
}
