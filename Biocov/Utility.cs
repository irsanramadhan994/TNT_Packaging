using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Biocov
{
    class Utility
    {

        public static bool CHECK_EMPTY(TextBox data)
        {
            if (data.Text.Length > 0)
            {
                return false;
            }
            return true;
        }

        public static string REMOVE_UNUSED_CHAR(string data)
        {

            data = data.Replace("\r", "");
            data = data.Replace("\n", "");
            data = data.Replace("]d2", "");
            data = data.Replace("$", "");
            data = data.Replace(" ", "");
            data = data.Replace("<GS>", "");
            char da = (char)29;
            data = data.Replace("" + da, "");

            return data;
        }

     

        public static string SetWithDate(string line)
        {
            string dates = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            line = dates + "\t" + line;
            return line;
        }
    }
}
