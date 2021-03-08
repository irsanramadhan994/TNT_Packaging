using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


namespace Biocov
{
    class Printer
    {

        tcp tcp = new tcp();
        sqlite sqlite = new sqlite();
        Dictionary<string,string> config = new Dictionary<string,string>();
        public Printer()
        {

        }

        public void Print(String Gtin, String SerialNumber, String BatchNumber, String ExpiryDate,String CreatedDate ,String gs1idcarton,string ipprinter,string portprinter,String nie)
        {
     
            tcp.Connect(ipprinter, portprinter, "500");
            tcp.send(ZplString(Gtin, ExpiryDate, SerialNumber, BatchNumber, gs1idcarton,CreatedDate,nie));
            System.Console.WriteLine(ZplString(Gtin, ExpiryDate, SerialNumber, BatchNumber, gs1idcarton, CreatedDate, nie));
            tcp.dc();
           
        }

                public void PrintSas(String Gtin, String SerialNumber, String BatchNumber,String label, String ExpiryDate, String gs1idcarton,string ipprinter,string portprinter)
        {
     
            tcp.Connect(ipprinter, portprinter, "500");
            System.Console.WriteLine(ZplSas(Gtin, SerialNumber, BatchNumber, label, ExpiryDate, gs1idcarton));
            tcp.send(ZplSas(Gtin, SerialNumber, BatchNumber, label,ExpiryDate, gs1idcarton));
            tcp.dc();
           
        }

        private string ZplString(String gtin, String exp, String serialnumber, String batchno, String gs1idcarton,String mfg,String nie)
        {
            DateTime dateExpiry = DateTime.ParseExact(exp, "yyMMdd", CultureInfo.InvariantCulture);
            string expiryDate = dateExpiry.ToString("dd MMM yy").ToUpper();
            DateTime dateMfg = DateTime.ParseExact(mfg, "yyMMdd", CultureInfo.InvariantCulture);
            string mfgDate = dateMfg.ToString("dd MMM yy").ToUpper();


            string zpl = 
"^XA"
+ "^FO25,25"
+ "^BXN,5,200,,,"
+ "^FH\\^FD\\7E1"+"01"+ gtin + "\\7E1" +"10"+ batchno+"\\7E1"+"17"+exp+"21"+serialnumber+ "^FS"
//+ "^FD" + gs1idcarton + "^FS"
+ "^FT170,37^A018,18^FDBatch No^FS"
+ "^FT255,37^A018,18^FD: " + batchno + "^FS"
+ "^FT170,69^A018,18^FDSerial No^FS"
+ "^FT255,69^A018,18^FD: " + serialnumber + "^FS"
+ "^FT170,101^A018,18^FDMfg Date^FS"
+ "^FT255,101^A018,18^FD: " + mfgDate + "^FS"
+ "^FT170,133^A018,18^FDExp Date^FS"
+ "^FT255,133^A018,18^FD: " + expiryDate +"^FS"
//+ "^FT170,140^A018,18^FD"+nie+"^FS"
+ "^XZ";

            return zpl;
        }


        private string ZplSas(String sas, String serialnumber, String batchno, String exp, String label, String gs1idcarton)
        {
            DateTime dateExpiry = DateTime.ParseExact(exp, "yyMMdd", CultureInfo.InvariantCulture);
            string expiryDate = dateExpiry.ToString("dd MMM yy").ToUpper();


            string zpl = 
"^XA"
+ "^FO20,10"
+ "^BXN,5,200,,,"
+ "^FH\\^FD\\7E1" + "01" + sas + "\\7E1" + "10" + batchno +"\\7E1"+ "17" + exp + "21"+ serialnumber + "^FS"
//+ "^FD" + gs1idcarton + "^FS"
//+ "^FT20,145^A014,14^FDMILIK KEMENKES RI^FS"
+ "^FT150,40^A017,17^FDNO^FS"
+ "^FT220,40^A017,17^FD: " + label + sas + "^FS"
+ "^FT150,65^A017,17^FDNO.BATCH^FS"
+ "^FT220,65^A017,17^FD: " + batchno + "^FS"
+ "^FT150,90^A017,17^FDEXP.DATE^FS"
+ "^FT220,90^A017,17^FD: " + expiryDate + "^FS"
+ "^FT150,115^A017,17^FDS.N.^FS"
+ "^FT220,115^A017,17^FD: " + serialnumber + "^FS"
//+ "^FT150,140^A018,18^FDEUA2102907543A1^FS"
+ "^XZ";

            return zpl;
        }
    }
    



}
