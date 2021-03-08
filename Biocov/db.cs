using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;


namespace Biocov
{
    class db
    {
        public List<string> FIELD { get; set; }
        public Dictionary<string, string> PARAMETER { get; set; }
        public string WHERE { get; set; }
        public string TABLE { get; set; }
        public int num_rows { get; set; }
        public DataTable dataHeader;
        public string from, adminid, eventtype, eventname;
        public string ipAddress;
        bool isOpen;
        sqlite sqlt = new sqlite();
        Dictionary<string, string> config = new Dictionary<string, string>(); 
        SqlConnection Connection;
        SqlDataReader DataReader;
        SqlDataAdapter dataadapter;
        SqlCommand Command;
        Logger log = new Logger();

        public db()
        {
            num_rows = 0;
            config = sqlt.getConfig("MGI");

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


        public string[] cekLine(string where)
        {
            ipAddress = GetLocalIPAddress();
            List<string> field = new List<string>();
            string[] temp = new string[0];
            field.Add("lineName,linePackaging.linePackagingId");
            string from = "linePackagingDetail INNER JOIN linePackaging ON linePackagingDetail.linePackagingId = linePackaging.linePackagingId";
            //string where1 = where + "= '" + GetLocalIPAddress() + "'";
            // string where1 = where + "= '172.16.160.103'";
            string where1 = where + "= '192.168.11.100'";
            List<string[]> ds = selectList(field, from, where1);
            if (num_rows > 0)
                return ds[0];
            else
                return temp;
        }


        public string time()
        {
            try
            {
                string sql = "SELECT DATEDIFF(SECOND,'1970-01-01', GETUTCDATE()) AS 'Result'";

                if (OpenConnection())
                {
                    Command = new SqlCommand(sql, Connection);
                    Command.ExecuteNonQuery();
                    DataReader = Command.ExecuteReader();
                    DataReader.Read();
                    string Results = DataReader["Result"].ToString();
                    Connection.Close();
                    return Results;
                }
            }
            catch (SqlException sq)
            {
                MessageBox.Show(sq.ToString());
            }
            finally
            {
                try
                {
                    DataReader.Close();
                    Command.Dispose();
                }
                catch (SqlException sq)
                {
                    MessageBox.Show(sq.ToString());
                }
                catch (NullReferenceException nl)
                {
                    MessageBox.Show(nl.ToString());
                }

            }
            return "";
        }

        //tambahan
        internal List<string[]> getRole(string adminid)
        {
            List<string> field = new List<string>();
            field.Add("a.first_name, c.name, c.id as role ");
            string where = " a.id = '" + adminid + "'";
            string from = " users a INNER JOIN users_groups b ON a.id = b.user_id INNER JOIN groups c on b.group_id = c.id ";
            List<string[]> result = selectList(field, from, where);
            Connection.Close();
            if (result.Count > 0)
            {
                return result;
            }
            else
                return null;
        }

        internal bool cekPermision(string adminid, string modul_name, string permission)
        {
            List<string> field = new List<string>();
            field.Add("permissions_name");
            string where = "dbo.users.id = '" + adminid + "' AND dbo.permissions.permissions_name = '" + permission + "'";
            string from = "dbo.groups INNER JOIN dbo.permissions ON dbo.groups.id = dbo.permissions.role_id INNER JOIN dbo.users_groups ON dbo.users_groups.group_id = dbo.groups.id  INNER JOIN dbo.users ON dbo.users_groups.user_id = dbo.users.id";
            selectList(field, from, where);

            Connection.Close();
            if (num_rows > 0)
                return true;
            return false;
        }



        public List<string[]> validasi(string username, string password, string status)
        {
            try
            {

                List<string> field = new List<string>();
                field.Add("id");
                string where = "username = '" + username + "' AND password = '" + md5hash(password) + "' AND Active = " + status;
                List<string[]> result = selectList(field, "[users]", where);
                if (result.Count > 0)
                {

                    return result;
                }
                else
                    MessageBox.Show("Username or Password Incorect");
                    return null;


            }
            catch (NullReferenceException nl)
            {
                MessageBox.Show("Username or Password Incorect");

                return null;
            }

            catch (SqlException sqx)
            {

                MessageBox.Show(sqx.ToString());
                return null;
            }

            
        }



        public string md5hash(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, source);
                return hash;
            }
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
            

        public bool OpenConnection()
        {
            try
            {
                string connec = @"Data Source=" + config["ipdb"] + ";" +
                                "Initial Catalog=" + config["dbname"] + ";" +
                                "Connection Timeout='2'" + ";" +
                "User id=" + config["username_db"] + ";" +
                "Password=" + config["password_db"] + ";";

                //string connec = "server=anorangga\\sqlexpress;" +
                //       "trusted_connection=yes;" +
                //       "database=master; " +
                //       "connection timeout=30";
                Connection = new SqlConnection(connec);
                Connection.Open();
                isOpen = true;
                return true;

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Can't open connection to Database Server.");
                log.LogWriter(ex.ToString());
                Connection.Close();

                return false;
            }

            catch (Exception ex)
            {
                isOpen = false;
                MessageBox.Show("Can't open connection to Database Server.");
                log.LogWriter(ex.ToString());
                // new Modals("Can't open connection to Database Server.", "Information", MessageBoxButtons.OK);
                Connection.Close();
                return false;

            }
               
            
   
 
           
        }

        public bool CloseConnection()
        {
            try
            {
                isOpen = false;
                Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                log.LogWriter(ex.ToString());
                return false;
            }
        }

        public bool aggregationVial(string aggregationWhere,string gsoneinnerboxid,string infeed)
        {
            OpenConnection();
            Command = Connection.CreateCommand();
            SqlTransaction transaction;
            transaction = Connection.BeginTransaction("aggregationVial");

            Command.Connection = Connection;
            Command.Transaction = transaction;

            try
            {
                Console.WriteLine("UPDATE [vaccine_packaging] set innerboxgsoneid='" + gsoneinnerboxid + "', innerboxid='" + infeed + "' , isReject = 0,  flag = 0 where gsOneVialId in (" + aggregationWhere + ")");
                Command.CommandText =
                    "UPDATE [vaccine_packaging] set innerboxgsoneid='" + gsoneinnerboxid + "', innerboxid='" + infeed + "' , isReject = 0,  flag = 0 where gsOneVialId in (" + aggregationWhere + ")";
                Command.ExecuteNonQuery();
                Command.CommandText =
                    "  update innerBox set isReject = 0, flag = 0 where gsOneInnerBoxId = '"+gsoneinnerboxid+"'";
                Command.ExecuteNonQuery();
             
                transaction.Commit();

                return true;
            }




            catch (Exception ex)
            {
                log.LogWriter("Error SQLSERVER : Commit Exception Type: {0}" + ex.GetType());
                log.LogWriter("Error SQLSERVER : Message: {0}" + ex.Message);
                // Attempt to roll back the transaction.
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    log.LogWriter("Error SQLSERVER : Rollback Exception Type: {0}" + ex2.GetType());
                    log.LogWriter("Error SQLSERVER : Message: {0}" + ex2.Message);
                }
                
            }
            finally
            {
                try
                {
                    //DataReader.Close();
                    Command.Dispose();
                    Connection.Close();
                }
                catch (SqlException sq)
                {
                    log.LogWriter(sq.ToString());

                }
                catch (NullReferenceException nl)
                {
                    log.LogWriter(nl.ToString());

                }

            }
            return false;
        }

        public bool insertSelect(String expdate,String innerboxgsoneid,String productmodelid) {

            try {

                string sql = "INSERT INTO vaccine_packaging (batchnumber,innerboxgsoneid,innerboxid,createdtime,isreject,flag,expdate,productmodelid) SELECT batchnumber,gsoneinnerboxid,infeedinnerboxid,createdtime,isreject,flag,'" + expdate + "','" + productmodelid + "' FROM innerbox where gsoneinnerboxid='" + innerboxgsoneid + "'";

                if (OpenConnection())
                {
                    Console.WriteLine(sql);
                    Command = new SqlCommand(sql, Connection);
                    Command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException see)
            {
                log.LogWriter(see.ToString());

            }
            catch (Exception sq)
            {
                log.LogWriter(sq.ToString());

                try
                {


                    CloseConnection();
                }
                catch (Exception ex)
                {
                    log.LogWriter(ex.ToString());
                }
            }
            finally
            {
                try
                {
                    //DataReader.Close();
                    Command.Dispose();
                    Connection.Close();
                }
                catch (SqlException sq)
                {
                    log.LogWriter(sq.ToString());

                }
                catch (NullReferenceException nl)
                {
                    log.LogWriter(nl.ToString());

                }

            }

            return false;
        } 


        public List<String[]> selectList(List<string> field, string table, string where)
        {
            if (OpenConnection())
            {
                try
                {
                    string sql = "SELECT ";
                    for (int j = 0; j < field.Count; j++)
                    {
                        sql += field[j];
                        if (j + 1 < field.Count)
                        {
                            sql += ",";
                            j++;
                        }
                    }
                    sql += " From " + table;

                    if (where.Length > 0)
                        sql += " WHERE " + where;

                    List<String[]> Results = new List<String[]>();

                    if (isOpen)
                    {
                        Console.WriteLine(sql);
                        Command = new SqlCommand(sql, Connection);
                        Command.CommandTimeout = 30;
                        DataReader = Command.ExecuteReader();
                        int num = 0;
                        while (DataReader.Read())
                        {
                            string[] data = new string[DataReader.FieldCount];
                            for (int u = 0; u < DataReader.FieldCount; u++)
                            {
                                data[u] = DataReader.GetValue(u).ToString();
                            }

                            Results.Add(data);
                            num++;
                        }
                        num_rows = num;
                        dataHeader = new DataTable();
                        for (int i = 0; i < DataReader.FieldCount; i++)
                        {
                            dataHeader.Columns.Add(DataReader.GetName(i));
                        }
                        Connection.Close();
                        if (num_rows > 0)
                        {
                            return Results;

                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (SqlException sq)
                {
                    try
                    {
                        var st = new StackTrace(sq, true);
                        var frame = st.GetFrame(0);
                        string line = frame.ToString();
                        Connection.Close();
                    }
                    catch (Exception ex)
                    {
                        log.LogWriter(ex.ToString());
                        Connection.Close();
                    }
                }
                catch (NullReferenceException nl)
                {

                    var st = new StackTrace(nl, true);
                    var frame = st.GetFrame(0);
                    string line = frame.ToString();
                    Connection.Close();
                }
                catch (InvalidOperationException ide)
                {
                    log.LogWriter(ide.ToString());

                    Application.Exit();
                }
                finally
                {
                    try
                    {
                        DataReader.Close();
                        Command.Dispose();
                        Connection.Close();
                    }
                    catch (SqlException sq)
                    {
                        log.LogWriter(sq.ToString());

                    }
                    catch (NullReferenceException nl)
                    {
                        log.LogWriter(nl.ToString());

                    }

                }
                return null;
            }
            return null;

        }

        public int selectCountPass(string batchNumber)
        {
            try
            {
                string sql = "SELECT Count(gsoneinnerboxid) FROM [Innerbox] WHERE batchnumber='"+batchNumber+"' AND isreject='0' AND flag='0'";

                OpenConnection();
                Command = new SqlCommand(sql, Connection);
                DataReader = Command.ExecuteReader();
                int Results = 0;
                while (DataReader.Read())
                {
                    string[] data = new string[DataReader.FieldCount];
                    for (int u = 0; u < DataReader.FieldCount; u++)
                    {
                        data[u] = DataReader.GetValue(u).ToString();
                    }

                    Results = Int32.Parse(data[0]);
                }

                return Results;
            }
            catch (SqlException sq)
            {
                MessageBox.Show("Error SQLSERVER : " + sq.ToString());
            }
            finally
            {
                try
                {
                    DataReader.Close();
                    Command.Dispose();
                    Connection.Close();
                }
                catch (SqlException sq)
                {
                    MessageBox.Show("Error SQLSERVER : " + sq.ToString());
                }
                catch (NullReferenceException nl)
                {
                    MessageBox.Show("Error SQLSERVER : " + nl.ToString());
                }

            }
            return 0;
        }




        public bool insert(Dictionary<string, string> field, string table)
        {
            try
            {
                string sql = "INSERT INTO " + table + " (";
                int i = 0;
                foreach (string key in field.Keys)
                {
                    sql += "" + key + "";
                    if (i + 1 < field.Count)
                    {
                        sql += ",";
                        i++;
                    }
                }

                sql += ") values (";

                i = 0;
                foreach (string key in field.Values)
                {
                    sql += "'" + key + "'";
                    if (i + 1 < field.Count)
                    {
                        sql += ",";
                        i++;
                    }
                }
                sql += ")";
                Console.WriteLine(sql);
                if (OpenConnection())
                {
                    Command = new SqlCommand(sql, Connection);
                    Command.ExecuteNonQuery();
                    CloseConnection();
                    return true;
                }
            }
            catch (SqlException see)
            {
                log.LogWriter(see.ToString());

            }
            catch (Exception sq)
            {
                log.LogWriter(sq.ToString());

                try
                {
                   
   
                    CloseConnection();
                }
                catch (Exception ex)
                {
                    log.LogWriter(ex.ToString());
                }
            }
            finally
            {
                try
                {
                    //DataReader.Close();
                    Command.Dispose();
                    Connection.Close();
                }
                catch (SqlException sq)
                {
                    log.LogWriter(sq.ToString());

                }
                catch (NullReferenceException nl)
                {
                    log.LogWriter(nl.ToString());

                }

            }

            return false;
        }



        public bool insertBatch(List<string> field, string table,string batchnumber)
        {
            int i = 0;
            try
            {
                string sql = "INSERT INTO " + table + " (isreject,batchnumber,flag,gsoneinnerboxid";
   

                sql += ") values ";

                i = 0;
                foreach (string key in field)
                {
                    sql += "('1','"+batchnumber+"','2','" + key + "')";
                    if (i + 1 < field.Count)
                    {
                        sql += ",";
                        i++;
                    }
                }
                
                Console.WriteLine(sql);
                if (OpenConnection())
                {
                    Command = new SqlCommand(sql, Connection);
                    Command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException see)
            {
                log.LogWriter(see.ToString());

            }
            catch (Exception sq)
            {
                log.LogWriter(sq.ToString());

                try
                {


                    CloseConnection();
                }
                catch (Exception ex)
                {
                    log.LogWriter(ex.ToString());
                }
            }
            finally
            {
                try
                {
                    //DataReader.Close();
                    Command.Dispose();
                    Connection.Close();
                }
                catch (SqlException sq)
                {
                    log.LogWriter(sq.ToString());

                }
                catch (NullReferenceException nl)
                {
                    log.LogWriter(nl.ToString());

                }

            }

            return false;
        }

        public bool delete(string table, string where)
        {
            try
            {
                string sql = "DELETE FROM" + table + "WHERE " + where;
                if (OpenConnection())
                {

                    Command = new SqlCommand(sql, Connection);
                    Command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException see)
            {
                log.LogWriter(see.ToString());
            }
            catch (Exception sq)
            {
                log.LogWriter(sq.ToString());

                try
                {

                    CloseConnection();
                }
                catch (Exception ex)
                {
                    log.LogWriter(ex.ToString());
                }
            }
            finally
            {
                try
                {
                    //DataReader.Close();
                    Command.Dispose();
                    Connection.Close();
                }
                catch (SqlException sq)
                {
                    log.LogWriter(sq.ToString());

                }
                catch (NullReferenceException nl)
                {
                    log.LogWriter(nl.ToString());

                }

            }

            return false;
        }






       public int update(Dictionary<string, string> field, string table, string where)
        {
            try
            {
                string sql = "UPDATE " + table + " SET ";
                int i = 0;
                int row = 0;
                foreach (KeyValuePair<string, string> key in field)
                {
                    if (key.Value == "NULL")
                    {
                        sql += key.Key + " = "+ key.Value;

                    }
                    else {
                        sql += key.Key + " = " + "'" + key.Value + "'";

                    }
                    if (i + 1 < field.Count)
                    {
                        sql += ",";
                        i++;
                    }
                }

                if (where.Length > 0)
                    sql += " WHERE " + where;

                if (OpenConnection())
                {
                    Console.WriteLine(sql);
                    Command = new SqlCommand(sql, Connection);
                    row = Command.ExecuteNonQuery();
                    Connection.Close();
   
                }
                return row;
            }
            catch (SqlException sq)
            {
                log.LogWriter(sq.ToString());

                try
                {
                    Connection.Close();
                    return 0;
                }
                catch (Exception ex)
                {
                    log.LogWriter(ex.ToString());

                    return 0;
                }
            }
        }
    }
}
