using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Biocov
{
    class generate
    {
        db db = new db();
        Printer printer = new Printer();
        string bnumber, expired, gtinoutbox, createddate,productmodelid,labelSas,nie;
        List<string> where = new List<string>();
        List<string> gs1IdCartonPrinted = new List<string>();
        Logger log = new Logger();
        Dictionary<string, string> config = new Dictionary<string, string>();
        sqlite sqlite = new sqlite();
        public bool stopLoop = false;
            

        public void getPO(string batchnumber)
        {

            List<string> field = new List<string>();
            field.Add("a.batchNumber,a.expired,a.manufacturingdate,b.gtinoutbox,a.productmodelid,b.lblsas,b.nie");
            List<string[]> dss = db.selectList(field, "[packaging_order]  a inner join [product_model] b ON a.productModelId = b.product_model ", "batchNumber ='" + batchnumber + "'");

            if (db.num_rows > 0)
            {
                foreach (string[] Row in dss)
                {

                    bnumber = Row[0];
                    expired = Row[1];
                    gtinoutbox = Row[3];
                    createddate = Row[2];
                    productmodelid = Row[4];
                    labelSas = Row[5];
                    nie = Row[6];
                }
            }
            else {

                log.LogWriter("SQL Error Cannot Get Packaging Order data");
            }
        }



        public bool insertVaccine(string innerboxgsoneid,string productModelId)
        {
                    
                    DateTime dateExpiry = DateTime.ParseExact(expired, "yyMMdd", CultureInfo.InvariantCulture);
                    string expiryDate = dateExpiry.ToString("DD MMM yy").ToUpper();
                    Dictionary<string, string> field = new Dictionary<string, string>();
                    string guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6).ToUpper();
                    string cap = productModelId + bnumber + guid.PadLeft(3, '0');
                    string sn = guid.PadLeft(6, '0');
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    field.Add("capId", cap);
                    field.Add("gsonevialid",cap);
                    field.Add("createdTime", unixTimestamp.ToString());
                    field.Add("linenumber", "12");
                    field.Add("batchnumber", bnumber);
                    field.Add("isreject", "0");
                    field.Add("productmodelid", productModelId);
                    field.Add("innerboxgsoneid", innerboxgsoneid);
                    field.Add("expdate", expired);
                    if (db.insert(field, "Vaccine"))
                    {
                        return true;
                    }else{
                    
                        return false;
                    }
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

        public void insertDB(List<string> gs1dCartonList, string productModelId,GenerateCode gcd,string userid,bool isSas)
        {

                config = sqlite.getConfig(userid);
                int printed = 0;
                int jumlah = gs1dCartonList.Count;
                int j = 0;
               for(int i=0;i<gs1dCartonList.Count;i++)
                {
                    if (j == 3)
                    {
                        j = 0;
                    }
                        DateTime dateExpiry = DateTime.ParseExact(expired, "yyMMdd", CultureInfo.InvariantCulture);
                        string expiryDate = dateExpiry.ToString("dd MMM yy").ToUpper();
                        Dictionary<string, string> field = new Dictionary<string, string>();
                        string guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6).ToUpper();
                        var now = DateTime.UtcNow;
                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        string sn = unixTimestamp + now.ToString("ffff");
                        field.Add("batchnumber", bnumber);
                        field.Add("flag", "2");
                        field.Add("gsoneinnerboxid", gs1dCartonList[i]);
                        field.Add("isreject", "1");

                        if (PingHost(config["ipprinter"]) || PingHost(config["ipprinter2"]) || PingHost(config["ipprinter3"]))
                   {
                       if (isSas)
                       {
                           if (db.insert(field, "[innerbox]"))
                           {
                               if (db.insertSelect(expired, gs1dCartonList[i], productmodelid))
                               {
                                   for (int k = 0; k < 2; k++)
                                   {
                                       if (k == 1)
                                       {

                                           k = 0;
                                       }
                                       switch (j)
                                       {
                                           case 0:
                                               if (PingHost(config["ipprinter"]))
                                               {
                                                   printer.PrintSas(gtinoutbox, gs1dCartonList[i].Substring(Math.Max(0, gs1dCartonList[i].Length - 14)), bnumber, expired, labelSas, gs1dCartonList[i], config["ipprinter"], config["portprinter"]);
                                                   j = 0;
                                                   printed++;
                                                   k++;
                                               }
                                               else
                                               {
                                                   log.LogWriter("Printer 1 Offline");
                                                   k = 0;
                                                   j++;
                                               }
                                               break;
                                           case 1:
                                               if (PingHost(config["ipprinter2"]))
                                               {
                                                   printer.PrintSas(gtinoutbox, gs1dCartonList[i].Substring(Math.Max(0, gs1dCartonList[i].Length - 14)), bnumber, expired, labelSas, gs1dCartonList[i], config["ipprinter2"], config["portprinter2"]);
                                                   j = 1;
                                                   printed++;
                                                   k++;
                                               }
                                               else
                                               {
                                                   log.LogWriter("Printer 2 Offline");
                                                   k = 0;
                                                   j++;
                                               }
                                               break;
                                           case 2:
                                               if (PingHost(config["ipprinter3"]))
                                               {
                                                   printer.PrintSas(gtinoutbox, gs1dCartonList[i].Substring(Math.Max(0, gs1dCartonList[i].Length - 14)), bnumber, expired, labelSas, gs1dCartonList[i], config["ipprinter3"], config["portprinter3"]);
                                                   j = 2;
                                                   printed++;
                                                   k++;
                                               }
                                               else
                                               {
                                                   log.LogWriter("Printer 3 Offline");
                                                   k = 0;
                                                   j = 0;
                                               }

                                               break;
                                       }

                                      
                                   }

                                   j++;
                                   Thread.Sleep(100);

                               }
                               else
                               { MessageBox.Show("Cant Connect To Database Server"); }

                           }
                           else
                           {

                               MessageBox.Show("Cant Connect To Database Server");
                           }

                       }
                       else {
                           if (db.insert(field, "[innerbox]"))
                           {
              
                                   for (int k = 0; k < 2; k++)
                                   {
                                       if (k == 1)
                                       {

                                           k = 0;
                                       }
                                       switch (j)
                                       {
                                           case 0:
                                               if (PingHost(config["ipprinter"]))
                                               {
                                                   printer.Print(gtinoutbox, gs1dCartonList[i].Substring(Math.Max(0, gs1dCartonList[i].Length - 14)), bnumber, expired, createddate,gs1dCartonList[i], config["ipprinter"], config["portprinter"],nie);
                                                   j = 0;
                                                   k++;
                                                   printed++;

                                               }
                                               else
                                               {
                                                   log.LogWriter("Printer 1 Offline");
                                                   k = 0;
                                                   j++;
                                               }
                                               break;
                                           case 1:
                                               if (PingHost(config["ipprinter2"]))
                                               {
                                                   printer.Print(gtinoutbox, gs1dCartonList[i].Substring(Math.Max(0, gs1dCartonList[i].Length - 14)), bnumber, expired,createddate, gs1dCartonList[i], config["ipprinter2"], config["portprinter2"],nie);
                                                   j = 1;
                                                   k++;
                                                   printed++;

                                               }
                                               else
                                               {
                                                   log.LogWriter("Printer 2 Offline");
                                                   k = 0;
                                                   j++;
                                               }
                                               break;
                                           case 2:
                                               if (PingHost(config["ipprinter3"]))
                                               {
                                                   printer.Print(gtinoutbox, gs1dCartonList[i].Substring(Math.Max(0, gs1dCartonList[i].Length - 14)), bnumber, expired,createddate, gs1dCartonList[i], config["ipprinter3"], config["portprinter3"],nie);
                                                   j = 2;
                                                   k++;
                                                   printed++;

                                               }
                                               else
                                               {
                                                   log.LogWriter("Printer 3 Offline");
                                                   k = 0;
                                                   j = 0;
                                               }

                                               break;
                                       }

                                    
                                   }

                                   j++;
                                   Thread.Sleep(100);
                               }
                               else
                           { MessageBox.Show("Cant Connect To Database Server"); }

                           
                     
                           
                       }
               
                   }else
                   {
                    MessageBox.Show("No Online Printer Connected. Please connect the printer!", "Error");
                          break;

                    }
                   }
               gcd.printed = printed;

            


        }

        public List<string> generateCarton(GenerateCode gc, int quantity, string batchnumber, string productModelId)
        {

            string gs1idcarton = "";
            where = new List<string>();
            List<string> gs1dCartonList = new List<string>();

               

                for (int i = 0; i < quantity; i++)
                {
                    var now = DateTime.UtcNow;
                    var unixTimestamp = (Int32)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    gs1idcarton = "01" + gtinoutbox + "10" + bnumber + "17" + expired + "21" + unixTimestamp +now.ToString("ffff");
                    gs1dCartonList.Add(gs1idcarton);
                    Thread.Sleep(1);
                }

          
            return gs1dCartonList;
        }

        public bool checkDbExist(GenerateCode gc)
        {

            
                List<string> field = new List<string>();
                field.Add("innerBoxGsOneId");
                string result="";
                for (int i = 0; i < where.Count(); i++)
                {
                    if (i != where.Count() - 1)
                    {
                        result += where[i] + ",";

                    }
                    else {
                        result += where[i];

                    }
                }
                List<string[]> dss = db.selectList(field, "[temp_carton]", "innerBoxGsOneId IN (" + result + ")");
                if (db.num_rows > 0)
                {
                    where = new List<string>();
                    return true;
                }
                else
                {

                    List<string[]> dss2 = db.selectList(field, "[Vaccine]", "innerBoxGsOneId IN (" + result + ")") ;
                    if (db.num_rows > 0)
                    {
                       where = new List<string>();
                        return true;
                    }
                    else {
                       where = new List<string>();
                        return false;
                    }
                }
            


        }
    }
}
