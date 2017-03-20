using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RSI_DLL
{
    public class Robot {
        private int _port;
        private UdpClient server;
        public delegate string CorrectionDelegate(string strRecive,string strSend);
        public CorrectionDelegate Correction;
        private bool work;
        private string _defaultDelegate(string strRecive, string strSend) {
            return String.Empty;
        }
        public Robot(int port) {
            _port = port;          
            Correction = _defaultDelegate;
        }
        public Robot(int port, CorrectionDelegate dlgt) {
            _port = port;
            Correction = dlgt;
        }
        public void StartListening() {
            work = true;
            Thread thrd_listen;
            thrd_listen = new System.Threading.Thread(new System.Threading.ThreadStart(start));
            thrd_listen.Start();
        }
        private void start() {
            System.Xml.XmlDocument SendXML = new System.Xml.XmlDocument();  // XmlDocument pattern
            SendXML.PreserveWhitespace = true;
            SendXML.Load("ExternalData.xml");

            server = new UdpClient(_port);

            try {
                while (work) {
                    IPEndPoint client = null;
                    byte[] data = server.Receive(ref client);
                    //Console.WriteLine("Donnees recues en provenance de {0}:{1}.", client.Address, client.Port);
                    string message = Encoding.ASCII.GetString(data);
                    string strReceive = message;
                    //Recive_data = message;

                    if ((strReceive.LastIndexOf("</Rob>")) == -1) {
                        continue;
                    } else {

                        string strSend;
                        strSend = SendXML.InnerXml;
                        
                        strSend = mirrorIPOC(strReceive, strSend);
                        strSend = Correction(strReceive, strSend);

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(strSend);
                        server.Send(msg, msg.Length, client);
                    }

                    strReceive = null;


                    
                    //st = Correction("ReciveData", st);
                    //Console.WriteLine(st);
                    //Thread.Sleep(1000);
                    
                }

            } catch (Exception ex) {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
        }

        private string mirrorIPOC(string receive, string send) {
            // separate IPO counter as string
            int startdummy = receive.IndexOf("<IPOC>") + 6;
            int stopdummy = receive.IndexOf("</IPOC>");
            string Ipocount = receive.Substring(startdummy, stopdummy - startdummy);

            // find the insert position		
            startdummy = send.IndexOf("<IPOC>") + 6;
            stopdummy = send.IndexOf("</IPOC>");

            // remove the old value an insert the actualy value
            send = send.Remove(startdummy, stopdummy - startdummy);
            send = send.Insert(startdummy, Ipocount);

            return send;
        }

        public void Stoplistening() {
            work = false;
            server.Client.Shutdown(SocketShutdown.Receive);
            server.Client.Close();
            server.Close();
        }
    }
}
