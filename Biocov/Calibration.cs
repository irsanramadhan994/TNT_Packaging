using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RawInput_dll;


namespace Biocov
{
    public partial class Calibration : Form
    {
        private readonly RawInput _rawinput;
        const bool CaptureOnlyInForeground = true;
        public string hidscanner1, hidscanner2;


        public Calibration()
        {
            InitializeComponent();
            _rawinput = new RawInput(Handle, CaptureOnlyInForeground);

        }

        private void Calibration_Load(object sender, EventArgs e)
        {
            _rawinput.KeyPressed += OnKeyPressed;

        }

        private void OnKeyPressed(object sender, RawInputEventArg e)
        {

            if (e.KeyPressEvent.VKey.ToString(CultureInfo.InvariantCulture) == "13" && hidscanner1 == null && e.KeyPressEvent.KeyPressState == "BREAK")
            {

                hidscanner1 = e.KeyPressEvent.DeviceName;
                label1.Text = "Please Scan For Operator 2";



            }
            else if (e.KeyPressEvent.VKey.ToString(CultureInfo.InvariantCulture) == "13" && hidscanner1 != null && e.KeyPressEvent.KeyPressState == "BREAK")
            {
                hidscanner2 = e.KeyPressEvent.DeviceName;
                this.DialogResult = DialogResult.OK;
                this.Close();
            
            }


        }

        private void Calibration_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Calibration_FormClosing(object sender, FormClosingEventArgs e)
        {
            _rawinput.KeyPressed -= OnKeyPressed;

        }
    }
}
