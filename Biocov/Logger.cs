﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Biocov
{
    class Logger
    {

   
            private string m_exePath = string.Empty;
            public  void LogWriter(string logMessage)
            {
                LogWrite(logMessage);
            }
            public void LogWrite(string logMessage)
            {
                m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                try
                {
                    using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                    {
                        Log(logMessage, w);
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.ToString());

                }
            }

            public void Log(string logMessage, TextWriter txtWriter)
            {
                try
                {
                    txtWriter.Write("\r\nLog Entry : ");
                    txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                        DateTime.Now.ToLongDateString());
                    txtWriter.WriteLine("  :");
                    txtWriter.WriteLine("  :{0}", logMessage);
                    txtWriter.WriteLine("-------------------------------");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.ToString());

                }
            }
        }
}
