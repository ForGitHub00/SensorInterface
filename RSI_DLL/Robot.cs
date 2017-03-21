using HelperControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RSI_DLL {
    public class Robot {
        private int _port;
        private UdpClient server;
        public delegate string CorrectionDelegate(string strRecive, string strSend);
        public CorrectionDelegate Correction;
        private bool work;
        public Singleton single;
        private double speed;
        private double _oneCor;

        int prevIndex = 0;
        private string _defaultDelegate(string strRecive, string strSend) {
            double RX = ParserXML.GetValues(strRecive, new string[] { "Rob", "RIst", "X" });
            double RY = ParserXML.GetValues(strRecive, new string[] { "Rob", "RIst", "Y" });
            double RZ = ParserXML.GetValues(strRecive, new string[] { "Rob", "RIst", "Z" });
            double RA = ParserXML.GetValues(strRecive, new string[] { "Rob", "RIst", "A" });
            double RB = ParserXML.GetValues(strRecive, new string[] { "Rob", "RIst", "B" });
            double RC = ParserXML.GetValues(strRecive, new string[] { "Rob", "RIst", "C" });

            single.Position = new RPoint(RX, RY, RZ, RA, RB, RC);

            if (single._MAP.Count != 0) {
                int index = prevIndex;
                while (index < single._MAP.Count && RX > single._MAP[index].X) {
                    index++;
                }
                prevIndex = index;
                if (index < single._MAP.Count) {
                    double sum = Math.Abs(single._MAP[index].X - RX) + Math.Abs(single._MAP[index].Y - RY) + Math.Abs(single._MAP[index].Z - RZ);
                    double xProc = (single._MAP[index].X - RX) / sum;
                    double yProc = (single._MAP[index].Y - RY) / sum;
                    double zProc = (single._MAP[index].Z - RZ) / sum;


                    Console.WriteLine(xProc);


                    ParserXML.SetValue(ref strSend, "Sen\\RKorr\\X", xProc * _oneCor);
                    ParserXML.SetValue(ref strSend, "Sen\\RKorr\\Y", yProc * _oneCor);
                    ParserXML.SetValue(ref strSend, "Sen\\RKorr\\Z", zProc * _oneCor);

                } else {
                    ParserXML.SetValue(ref strSend, "Sen\\RKorr\\X", _oneCor);
                }
            }
           
            return strSend;
        }
        public Robot(int port) {
            _port = port;
            Correction = _defaultDelegate;
            speed = 0.01;
            _oneCor = speed * 0.012;
        }
        public Robot(int port, CorrectionDelegate dlgt) {
            _port = port;
            Correction = dlgt;
        }

   

        public void StartListening() {
            work = true;
            Thread thrd_listen;
            single = Singleton.GetInstance();
            thrd_listen = new System.Threading.Thread(new System.Threading.ThreadStart(start));
            thrd_listen.Start();


        }
        private void start() {
            System.Xml.XmlDocument SendXML = new System.Xml.XmlDocument();  // XmlDocument pattern
            SendXML.PreserveWhitespace = true;
            SendXML.Load("ExternalData.xml");

            server = new UdpClient(_port);

            Singleton s = Singleton.GetInstance();
            //Console.WriteLine(s.Name);

            try {
                while (work) {
                    IPEndPoint client = null;
                    byte[] data = server.Receive(ref client);
                    //Console.WriteLine("Donnees recues en provenance de {0}:{1}.", client.Address, client.Port);
                    string message = Encoding.ASCII.GetString(data);
                    string strReceive = message;
                    //Recive_data = message;

                    // if ((strReceive.LastIndexOf("</Rob>")) == -1) {
                    if ((strReceive.LastIndexOf("</Rob>")) == -1) {
                        continue;
                    } else {

                        string strSend;
                        strSend = SendXML.InnerXml;

                        // strSend = mirrorIPOC(strReceive, strSend);
                        strSend = Correction(strReceive, strSend);

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(strSend);
                        // server.Send(msg, msg.Length, client);
                    }

                    strReceive = null;


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
