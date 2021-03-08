using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;

namespace Biocov
{
    class sqlite
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private SQLiteDataReader datareader;
        bool isConnect = false;
        public Dictionary<string, string> config = new Dictionary<string, string>();
        public Dictionary<string, string> userconfig = new Dictionary<string, string>();



        List<string> _userList = new List<string>();

        Logger log = new Logger();

        public void SetConnection()
        {
            if (!isConnect)
            {
                try
                {
                    sql_con = new SQLiteConnection
                    ("Data Source=config.db;Version=3;New=False;Compress=True;");
                    sql_con.Open();
                    isConnect = true;
                }
                catch (SQLiteException sqlitex)
                {
                    log.LogWriter(sqlitex.ToString());
                }

            }
        }

        public Dictionary<string,string> getConfig(string userid)
        {
            try
            {
                Dictionary<string, string> config = new Dictionary<string, string>();
                string sql = "Select ipprinter," +
                                "portprinter,"   +
                                "ipprinter2," +
                                "portprinter2,"   +
                                "ipprinter3," +
                                "portprinter3,"   +
                                "ipdb,"          +
                                "username_db,"        +
                                "password_db," +
                                "dbname,"        +
                                "hidscanner,"     +
                                "hidscanner2,"+
                                 "user"    +
                                " From config WHERE user='"+
                                userid+"'";
                SetConnection();
                Console.WriteLine(sql);

                sql_cmd = new SQLiteCommand(sql, sql_con);
                datareader = sql_cmd.ExecuteReader();
                while (datareader.Read())
                {
                    config.Add("ipprinter", datareader.GetValue(0).ToString());
                    config.Add("portprinter", datareader.GetValue(1).ToString());
                    config.Add("ipprinter2", datareader.GetValue(2).ToString());
                    config.Add("portprinter2", datareader.GetValue(3).ToString());
                    config.Add("ipprinter3", datareader.GetValue(4).ToString());
                    config.Add("portprinter3", datareader.GetValue(5).ToString());
                    config.Add("ipdb", datareader.GetValue(6).ToString());
                    config.Add("username_db", datareader.GetValue(7).ToString());
                    config.Add("password_db", datareader.GetValue(8).ToString());
                    config.Add("dbname", datareader.GetValue(9).ToString());
                    config.Add("hidscanner", datareader.GetValue(10).ToString());
                    config.Add("hidscanner2", datareader.GetValue(11).ToString());
                    config.Add("user", datareader.GetValue(12).ToString());

                }
                return config;
            }
            catch (SQLiteException sqlitex)
            {
                sql_con.Close();
                isConnect = false;
                SetConnection();
                log.LogWriter(sqlitex.ToString());
                return null;
            }
        }

         public List<string> getUser()
        {
            try
            {
                Dictionary<string, string> config = new Dictionary<string, string>();
                string sql = "Select user from config";
                 SetConnection();
                sql_cmd = new SQLiteCommand(sql, sql_con);
                datareader = sql_cmd.ExecuteReader();
                while (datareader.Read())
                {
                    _userList.Add(datareader.GetValue(0).ToString());
                }
                return  _userList;


             }
                catch (SQLiteException sqlitex)
            {
                sql_con.Close();
                isConnect = false;
                SetConnection();
                log.LogWriter(sqlitex.ToString());
                return null;
            }
            }

        public void ExecuteQuery(string txtQuery)
        {
            try
            {
                SetConnection();
                sql_cmd = sql_con.CreateCommand();
                sql_cmd.CommandText = txtQuery;
                sql_cmd.ExecuteNonQuery();
            }
            catch (Exception sq)
            {
                log.LogWriter(sq.ToString());

                try
                {
                    isConnect = false;
                    sql_con.Close();
                    SetConnection();
                }
                catch (Exception ex)
                {
                    log.LogWriter(ex.ToString());


                }
            }
        }

        public bool insert(Dictionary<string, string> field, string table)
        {
            try
            {
                string sql = "INSERT INTO " + table + " (";
                int i = 0;
                foreach (string key in field.Keys)
                {
                    sql += "'" + key + "'";
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
                ExecuteQuery(sql);
                return true;
            }
            catch (Exception e)
            {
                log.LogWriter(e.ToString());
                return false;
            }
        }

        public bool update(Dictionary<string, string> field, string table, string where)
        {

            try
            {
                string sql = "UPDATE " + table + " SET ";
                int i = 0;
                foreach (KeyValuePair<string, string> key in field)
                {
                    sql += "" + key.Key + " = " + "'" + key.Value + "'";
                    if (i + 1 < field.Count)
                    {
                        sql += ",";
                        i++;
                    }
                }
                if (where.Length > 0)
                    sql += " WHERE " + where + "";
                SQLiteCommand command = new SQLiteCommand(sql_con);
                Console.WriteLine(sql);
                command.CommandText = sql;
                command.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                log.LogWriter(e.ToString());
                return false;
            }

           
        }
    }
}
