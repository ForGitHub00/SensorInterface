
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HelperControls {
    public class Correction {
        public Correction() {
            Map = new List<RPoint>();
            Speed = 0.05;
            _oneCor = Speed * 0.012;
            single = Singleton.GetInstance();
        }
        public Correction(double speed) {
            Map = new List<RPoint>();
            Speed = speed;
            _oneCor = Speed * 0.012;
            CalibX = 112.29;
            CalibY = -3.7;
            CalibZ = 365;
            single = Singleton.GetInstance();
        }
        public Correction(double speed, double x, double y, double z) {
            Map = new List<RPoint>();
            Speed = speed;
            _oneCor = Speed * 0.012;
            CalibX = x;
            CalibY = y;
            CalibZ = z;
            single = Singleton.GetInstance();
        }

        public List<RPoint> Map;
        public double Speed;
        private double _oneCor;
        public double CalibX;
        public double CalibY;
        public double CalibZ;
        private Singleton single;

        public void AddPoint(RPoint p) {
            Map.Add(p);
        }
        public RPoint Trans(RPoint robP, LPoint lasP) {
            RPoint res = Transform.Trans(robP.X, robP.Y, robP.Z, 0, 0, 0, CalibX, -CalibY + lasP.X, -(CalibZ - lasP.Z)); //todo CalibY + X ???
            Map.Add(res);
            return res;
        }

        private int prevIndex = 0;
        public RPoint Calculate(string rec, string send) {
            double RX = ParserXML.GetValues(rec, new string[] { "Rob", "RIst", "X" });
            double RY = ParserXML.GetValues(rec, new string[] { "Rob", "RIst", "Y" });
            double RZ = ParserXML.GetValues(rec, new string[] { "Rob", "RIst", "Z" });
            double RA = ParserXML.GetValues(rec, new string[] { "Rob", "RIst", "A" });
            double RB = ParserXML.GetValues(rec, new string[] { "Rob", "RIst", "B" });
            double RC = ParserXML.GetValues(rec, new string[] { "Rob", "RIst", "C" });

            if (Map.Count != 0) {
                int index = prevIndex;
                while (index < Map.Count && RX > Map[index].X) {
                    index++;
                }
                prevIndex = index;
                if (index < Map.Count) {
                    double sum = Math.Abs(Map[index].X - RX) + Math.Abs(Map[index].Y - RY) + Math.Abs(Map[index].Z - RZ);
                    double xProc = (Map[index].X - RX) / sum;
                    double yProc = (Map[index].Y - RY) / sum;
                    double zProc = (Map[index].Z - RZ) / sum;

                    //ParserXML.SetValue(ref send, "Sen\\RKorr\\X", xProc * _oneCor);
                    // ParserXML.SetValue(ref send, "Sen\\RKorr\\Y", yProc * _oneCor);
                    // ParserXML.SetValue(ref send, "Sen\\RKorr\\Z", zProc * _oneCor);

                } else {
                    // ParserXML.SetValue(ref send, "Sen\\RKorr\\X", _oneCor);
                }
            }
            return new RPoint() { X = RX, Y = RY, Z = RZ, A = RA, B = RB, C = RC };
        }
    }

    public static class Transform {
        public static RPoint Trans(double x, double y, double z, double a, double b, double c, double lx, double ly, double lz) {
            double[][] mat = matrix(x, y, z, a, b, c);
            double[] point = { lx, ly, lz };
            double[] point_transformed = mul(mat, point);

            return new RPoint() {
                X = point_transformed[0],
                Y = point_transformed[1],
                Z = point_transformed[2]
            };
        }
        public static double[][] matrix(double x, double y, double z, double aDeg, double bDeg, double cDeg) {
            //ABC: Euler angles, A: round z-axis     B: round y-axis        C: round y-axis
            double a = -aDeg * Math.PI / 180;
            double b = -bDeg * Math.PI / 180;
            double c = -cDeg * Math.PI / 180;
            double ca = Math.Cos(a);
            double sa = Math.Sin(a);
            double cb = Math.Cos(b);
            double sb = Math.Sin(b);
            double cc = Math.Cos(c);
            double sc = Math.Sin(c);
            double[][] tt = new double[3][];
            tt[0] = new double[] { ca * cb, sa * cc + ca * sb * sc, sa * sc - ca * sb * cc, x };
            tt[1] = new double[] { -sa * cb, ca * cc - sa * sb * sc, ca * sc + sa * sb * cc, y };
            tt[2] = new double[] { sb, -cb * sc, cb * cc, z };
            return tt;
        }
        public static double[] mul(double[][] a, double[] b) { // multiple a 3*4 (or 3*3) matrix with a vector
            double[] re = new double[3];
            int len = a[0].Length;
            if (len == 4) {
                for (int i = 0; i < 3; i++)
                    re[i] = a[i][0] * b[0] + a[i][1] * b[1] + a[i][2] * b[2] + a[i][3];
            } else if (len == 3) {
                for (int i = 0; i < 3; i++)
                    re[i] = a[i][0] * b[0] + a[i][1] * b[1] + a[i][2] * b[2];
            }
            return re;

        }
    }
}
