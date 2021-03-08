using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Biocov
{
    class tcp
    {

        
        private static TcpClient Client;
        private static Byte[] Data = new Byte[256];
        private static Stream ClientStream;
        public static bool IsConnected = false;
        Logger log = new Logger();

        public bool Connect(string ip, string port, string timeout)
        {
            try
            {
                Client = new TcpClient();
                IAsyncResult result = Client.BeginConnect(ip, Int32.Parse(port), null, null);
                bool success = result.AsyncWaitHandle.WaitOne(Int32.Parse(timeout), true);
                if (success)
                {
                    IsConnected = true;
                    return true;
                }
                else
                {
                    IsConnected = false;
                    return false;
                }
            }
            catch (SocketException ex)
            {
                // new Confirm("error +" + ex.ToString());
                if (!ex.NativeErrorCode.Equals(10056))
                    IsConnected = false;
            }
            catch (InvalidOperationException te)
            {

                System.Console.WriteLine(te);
            }
            return false;
        }




        public void send(string dataTerima)
        {
            try
            {
                if (IsConnected)
                {
                    ClientStream = Client.GetStream();
                    StreamWriter sw = new StreamWriter(ClientStream);
                    sw.WriteLine(dataTerima);
                    sw.Flush();
                }
            }
            catch (InvalidOperationException ex)
            {
                log.LogWriter(ex.ToString());

            }
        }

        public void dc()
        {
            Client.Close();
        }
    }


    }

