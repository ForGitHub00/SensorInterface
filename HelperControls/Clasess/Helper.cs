using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperControls {
    public static class Helper {
        public static void GetLaserDataFromTXT(string path, out double[] X, out double[] Z) {
            List<string> result = new List<string>();
            try {
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default)) {
                    string line;
                    while ((line = sr.ReadLine()) != null) {
                        result.Add(line);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            X = new double[result.Count];
            Z = new double[result.Count];

            for (int i = 0; i < result.Count; i++) {
                string[] temp = result[i].Split('|');
                X[i] = Convert.ToDouble(temp[0]);
                Z[i] = Convert.ToDouble(temp[1]);
            }


        }
        public static List<LPoint> GetLaserDataFromTXT(string path, bool zeroZ = false) {
            double[] x;
            double[] z;
            GetLaserDataFromTXT(path,out x, out z);
            return GetLaserData(x, z, zeroZ);
        }
        public static List<LPoint> GetLaserData(double[] X,double[] Z, bool _deleteZeroZ = false) {
            List<LPoint> result = new List<LPoint>();
            for (int i = 0; i < X.Length; i++) {
                if  (Z[i] == 0 ^ !_deleteZeroZ) {
                    continue;
                }
                result.Add(new LPoint() {
                    X = X[i],
                    Z = Z[i]
                });
            }
            return result;
        }
    }

    public struct LPoint {
        public double X;
        public double Z;
        public LPoint(double x = 0, double z = 0) {
            X = x;
            Z = z;
        }
    }
    public struct RPointCoordinates {
        public double X;
        public double Y;
        public double Z;
        public RPointCoordinates(double x = 0, double y = 0, double z = 0) {
            X = x;
            Y = y;
            Z = z;
        }
    }
    public struct RPointAngles {
        public double A;
        public double B;
        public double C;
    }
    public struct RPoint {
        public double X;
        public double Y;
        public double Z;
        public double A;
        public double B;
        public double C;
        public RPoint(double x = 0, double y = 0, double z = 0, double a = 0, double b = 0, double c = 0) {
            X = x;
            Y = y;
            Z = z;
            A = a;
            B = b;
            C = c;
        }
        public override string ToString() {
            return $"X = {X} | Y = {Y} | Z = {Z} | A = {A} | B = {B} | C = {C} | ";
        }
    }
}
