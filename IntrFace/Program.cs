using LaserDLL;
using RSI_DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntrFace {
    class Program {
        static void Main(string[] args) {
            Start();
        }
        private static void Start() {
            Robot R = new Robot(6008);
            R.StartListening();
            Laser.Init();
            //Console.WriteLine("start/////////");
            //Thread.Sleep(2500);
            //R.Stoplistening();
            //Console.WriteLine("stop////////////");
            //R.Correction = cor2;
            //Thread.Sleep(1200);
            //Console.WriteLine("start now");
            //R.StartListening();
            //R.StartListening();
        }
        private static void cor(string strR, ref string strS) {
            //ParserXML.SetValue(ref strS, new string[] { "Sen", "RKorr", "X" }, 6);
            //ParserXML.SetValue(ref strS, "Sen\\RKorr\\X\\", 6);
            strS = "SD";
        }
        private static void cor2(string strR, ref string strS) {
            //ParserXML.SetValue(ref strS, new string[] { "Sen", "RKorr", "X" }, 6);
            //ParserXML.SetValue(ref strS, "Sen\\RKorr\\X\\", 6);
            strS = "SDsadsd";
        }
    }
}
