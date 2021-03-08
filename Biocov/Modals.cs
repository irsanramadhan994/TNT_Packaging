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
    public partial class Modals : Form
    {

    public Modals(string textConfirm, string labelatas)
        {
            InitializeComponent();
            lblMessage.Text = textConfirm;
            this.Text = labelatas;
        }

        public bool conf()
        {
            btnYes.DialogResult = DialogResult.Yes;
            btnNo.DialogResult = DialogResult.No;
            this.ShowDialog();
            if (this.DialogResult == DialogResult.Yes)
            {
                //if (txt.Contains("Database"))
                //{
                //   fcon = true;
                //}
                return true;
            }
            else
                return false;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {

        }
        public Modals(string textConfirm, string labelatas, MessageBoxButtons oke)
        {
            InitializeComponent();
            //txt = textConfirm;
            lblMessage.Text = textConfirm;
            this.Text = labelatas;
            btnOk.Visible = true;
            btnYes.Visible = false;
            btnNo.Visible = false;
            this.ShowDialog();
        }

        public Modals(string textConfirm)
        {
            InitializeComponent();
            lblMessage.Text = textConfirm;
            this.Text = "Information";
            btnOk.Visible = true;
            btnYes.Visible = false;
            btnNo.Visible = false;
            this.ShowDialog();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


    }
}
