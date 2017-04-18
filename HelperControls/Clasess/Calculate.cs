using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static alglib;

namespace HelperControls {
    public static class Calculate {
        #region LaserData Algorithms
        public static class Laser {
            // Угловой шов
            public static class AngularSeam {
                public static List<LPoint> FindAngularSeamByRegions(List<LPoint> data, int countOfRegions = 10) {
                    LPoint result = new LPoint() { X = 0, Z = 0 };
                    List<LPoint> tempResult = new List<LPoint>();

                    int pointsInRegion = data.Count / countOfRegions;
                    for (int t = 0; t < countOfRegions; t++) {
                        double min = Double.MaxValue;
                        LPoint tempPoint = new LPoint();
                        for (int i = t * pointsInRegion; i < (t + 1) * pointsInRegion; i++) {
                            if (data[i].Z < min) {
                                min = data[i].Z;
                                tempPoint = data[i];
                            }
                        }
                        tempResult.Add(tempPoint);
                    }
                    result = tempResult[5];

                    return tempResult;
                }

                public static LPoint FindAngularSeamSimple(List<LPoint> data) {
                    LPoint result = new LPoint() { X = 0, Z = 0 };
                    if (data.Count != 0) {
                        result = data.OrderBy(item => item.Z).ToList()[0];
                    }
                    return result;
                }
                public static List<LPoint> FindMasPoint_ZX_Diff(List<LPoint> data, int count) {
                    //поиск по каждой соседней точке, по отношению dz к dx
                    Dictionary<int, double> diffs = new Dictionary<int, double>();
                    for (int i = 0; i < data.Count - 1; i++) {
                        diffs.Add(i, calculate_ratio(data[i], data[i + 1]));
                    }

                    //int count = (int)(data.Count * percent);
                    var tempResult = diffs.OrderByDescending(pair => pair.Value).Take(count).ToDictionary(pair => pair.Key, pair => pair.Value);

                    List<LPoint> result = new List<LPoint>();
                    foreach (var item in tempResult) {
                        if (data[item.Key + 1].Z > data[item.Key].Z) {
                            result.Add(data[item.Key + 1]);
                        } else {
                            result.Add(data[item.Key]);
                        }
                    }
                    return result;

                    double calculate_ratio(LPoint p1, LPoint p2)
                    {
                        double xDiff = Math.Abs(p1.X - p2.X);
                        double zDiff = Math.Abs(p1.Z - p2.Z);
                        return zDiff / xDiff;
                    }
                }
                public static (LPoint left, LPoint right) FindTwoPoints(List<LPoint> data) {
                    LPoint Lres = new LPoint() { X = 0, Z = 0 };
                    LPoint Rres = new LPoint() { X = 0, Z = 0 };
                    //TODO
                    return (Lres, Rres);
                }
                public static List<LPoint> FindMasPoint_Z_Diff(List<LPoint> data, int count) {
                    Dictionary<int, double> diffs = new Dictionary<int, double>();
                    for (int i = 0; i < data.Count - 1; i++) {
                        diffs.Add(i, Math.Abs(data[i].Z - data[i + 1].Z));
                    }

                    //int count = (int)(data.Count * percent);
                    var tempResult = diffs.OrderByDescending(pair => pair.Value).Take(count).ToDictionary(pair => pair.Key, pair => pair.Value);

                    List<LPoint> result = new List<LPoint>();
                    foreach (var item in tempResult) {
                        if (data[item.Key + 1].Z > data[item.Key].Z) {
                            result.Add(data[item.Key + 1]);
                        } else {
                            result.Add(data[item.Key]);
                        }
                    }
                    return result;

                    double calculate_ratio(LPoint p1, LPoint p2)
                    {
                        double xDiff = Math.Abs(p1.X - p2.X);
                        double zDiff = Math.Abs(p1.Z - p2.Z);
                        return zDiff / xDiff;
                    }
                }
            }
            // Стыковое соединение
            public static class SpliceSeam {
                public static LPoint FindOnePoint(List<LPoint> data) {
                    LPoint result = new LPoint() { X = 0, Z = 0 };
                    //TODO
                    return result;
                }
                public static (LPoint left, LPoint right) FindTwoPoints(List<LPoint> data) {
                    LPoint Lres = new LPoint() { X = 0, Z = 0 };
                    LPoint Rres = new LPoint() { X = 0, Z = 0 };
                    //TODO
                    return (Lres, Rres);
                }
            }
            //Фильтры
            public static class Filters {
                public static List<LPoint> SortByX(List<LPoint> data, bool _rightToLeft = false) {
                    List<LPoint> res = new List<LPoint>();
                    if (_rightToLeft) {
                        res = data.OrderBy(item => -item.X).ToList();
                    } else {
                        res = data.OrderBy(item => item.X).ToList();
                    }
                    return res;
                }
                public static List<LPoint> SortByZ(List<LPoint> data, bool _topToBot = false) {
                    List<LPoint> res = new List<LPoint>();
                    if (_topToBot) {
                        res = data.OrderBy(item => item.Z).ToList();
                    } else {
                        res = data.OrderBy(item => -item.Z).ToList();
                    }
                    return res;
                }
                public static List<LPoint> AveragingVertical(List<LPoint> data, double _verticalStep = 1, double _horizontalStep = 100, int _pointsInStep = 2) {
                    List<LPoint> res = data;
                    int current = 0;
                    int dataCount = data.Count;
                    while (current + _pointsInStep < dataCount) {
                        if (Math.Abs(data[current].Z - data[current + _pointsInStep].Z) <= _verticalStep && Math.Abs(data[current].X - data[current + _pointsInStep].X) <= _horizontalStep) {
                            double tempZ = (data[current].Z + data[current + _pointsInStep].Z) / 2;
                            for (int i = current + 1; i < current + _pointsInStep; i++) {
                                data[i] = new LPoint() {
                                    Z = tempZ,
                                    X = data[i].X
                                };
                            }
                        }
                        current += _pointsInStep - 1;
                        //current++;
                    }
                    return res;
                }
                public static List<LPoint> AveragingVerticalPro(List<LPoint> data, double _verticalStep = 1, double _horizontalStep = 100,
                    int _pointsInStep = 2, bool _everyPoint = false) {
                    List<LPoint> res = data;
                    int current = 0;
                    int dataCount = data.Count;
                    while (current + _pointsInStep < dataCount) {
                        if (Math.Abs(data[current].Z - data[current + _pointsInStep].Z) <= _verticalStep && Math.Abs(data[current].X - data[current + _pointsInStep].X) <= _horizontalStep) {
                            for (int i = current + 1; i < current + _pointsInStep; i++) {
                                data[i] = new LPoint() {
                                    Z = _calcTempZ(data[current], data[current + _pointsInStep], data[i].X),
                                    X = data[i].X
                                };
                            }
                        }
                        if (_everyPoint) {
                            current++;
                        } else {
                            current += _pointsInStep - 1;
                        }
                    }
                    return res;

                    double _calcTempZ(LPoint p1, LPoint p2, double difX)
                    {
                        double x1 = Math.Min(p1.X, p2.X);
                        double x2 = Math.Max(p1.X, p2.X);
                        double x = difX;


                        double z1 = p1.X == x1 ? p1.Z : p2.Z;
                        double z2 = p1.X == x2 ? p1.Z : p2.Z;

                        double zRes = (((x - x1) * (z2 - z1)) / (x2 - x1)) + z1;


                        return zRes;
                    }

                }
            }
            public static class Voronej {
                public static LPoint Type1_1point(List<LPoint> data, double lifting = 0) {
                    LPoint result = new LPoint() { X = 0, Z = 0 };
                    if (data.Count != 0) {
                        result = data.OrderBy(item => item.Z).ToList()[0];
                    }
                    return new LPoint(result.X, result.Z + lifting);
                }

                public static (LPoint left, LPoint right) Type3_2point(List<LPoint> data) {
                    LPoint left = new LPoint(0, 0), right = new LPoint(0, 0);
                    Dictionary<int, double> diffs = new Dictionary<int, double>();
                    for (int i = 0; i < data.Count - 1; i++) {
                        diffs.Add(i, calculate_ratio(data[i], data[i + 1]));
                    }

                    //int count = (int)(data.Count * percent);
                    var tempResult = diffs.OrderByDescending(pair => pair.Value).Take(2).ToDictionary(pair => pair.Key, pair => pair.Value);

                    List<LPoint> result = new List<LPoint>();
                    foreach (var item in tempResult) {
                        if (data[item.Key + 1].Z > data[item.Key].Z) {
                            result.Add(data[item.Key + 1]);
                        } else {
                            result.Add(data[item.Key]);
                        }
                    }

                    if (tempResult.Count > 1) {
                        if (result[0].X < result[1].X) {
                            left = result[0];
                            right = result[1];
                        } else {
                            left = result[1];
                            right = result[0];
                        }
                    }



                    return (left, right);

                    double calculate_ratio(LPoint p1, LPoint p2)
                    {
                        double xDiff = Math.Abs(p1.X - p2.X);
                        double zDiff = Math.Abs(p1.Z - p2.Z);
                        return zDiff / xDiff;
                    }


                }

                public static LPoint Type3_1point(List<LPoint> data, double decrease = 0) {
                    LPoint left = new LPoint(0, 0), right = new LPoint(0, 0);
                    (left, right) = Type3_2point(data);

                    return new LPoint((left.X + right.X) / 2, (left.Z + right.Z) / 2 - decrease);
                }

            }


        }
        #endregion
        #region Robot Algorithms
        public static class Robot {
            public static RPoint CalculatePoint_2line(List<RPoint> line1, List<RPoint> line2) {
                RPoint result = new RPoint(0, 0, 0);

                int type_line1 = LineType(line1);
                int type_line2 = LineType(line2);

                line1 = LineApprox(line1, type_line1);
                line2 = LineApprox(line2, type_line2);

                var A = line1[0];
                var B = line1[line1.Count - 1];
                var C = line2[0];
                var D = line2[line2.Count - 1];

                double xo = A.X;
                double yo = A.Y;
                double zo = A.Z;
                double p = B.X - A.X;
                double q = B.Y - A.Y;
                double r = B.Z - A.Z;

                double x1 = C.X;
                double y1 = C.Y;
                double z1 = C.Z;
                double p1 = D.X - C.X;
                double q1 = D.Y - C.Y;
                double r1 = D.Z - C.Z;

                double x = (xo * q * p1 - x1 * q1 * p - yo * p * p1 + y1 * p * p1) /
                    (q * p1 - q1 * p);
                double y = (yo * p * q1 - y1 * p1 * q - xo * q * q1 + x1 * q * q1) /
                    (p * q1 - p1 * q);
                double z = (zo * q * r1 - z1 * q1 * r - yo * r * r1 + y1 * r * r1) /
                    (q * r1 - q1 * r);

                if (true) {

                }

                result = new RPoint(x, y, z);



                #region
                //if (type_line1 == 1) {
                //    if (type_line2 == 2) {

                //    } else if (type_line2 == 2) {
                //    }
                //}
                //else if(type_line1 == 2) {
                //    if (type_line2 == 1) {

                //    } else if (type_line2 == 3) {

                //    }
                //} else if (type_line1 == 3) {
                //    if (type_line2 == 1) {

                //    } else if (type_line2 == 2) {

                //    }
                //}
                #endregion





                return result;
            }
            public static RPoint CalculatePoint_3line(List<RPoint> line1, List<RPoint> line2, List<RPoint> line3) {
                RPoint result = new RPoint(0, 0, 0);
                List<RPoint> tempResult = new List<RPoint>();

                RPoint tempPoint = CalculatePoint_2line(line1, line2);
                tempResult.Add(tempPoint);

                tempPoint = CalculatePoint_2line(line1, line3);
                tempResult.Add(tempPoint);

                tempPoint = CalculatePoint_2line(line3, line2);
                tempResult.Add(tempPoint);

                double x = tempResult.Sum(it => it.X);
                double y = tempResult.Sum(it => it.Y);
                double z = tempResult.Sum(it => it.Z);

                result = new RPoint(x, y, z);
                return result;
            }

            public static int LineType(List<RPoint> line) {
                double x_diff = Math.Abs(line[0].X - line[line.Count - 1].X);
                double y_diff = Math.Abs(line[0].Y - line[line.Count - 1].Y);
                double z_diff = Math.Abs(line[0].Z - line[line.Count - 1].Z);
                if (x_diff > y_diff) {
                    if (x_diff > z_diff) {
                        return 1; // 1 - x, 2 - y, 3 - z;
                    } else {
                        return 3;
                    }
                } else {
                    if (y_diff > z_diff) {
                        return 2;
                    } else {
                        return 3;
                    }
                }
            }

            public static List<RPoint> LineApprox(List<RPoint> line, int type) {
                List<RPoint> result = new List<RPoint>();

                #region временное усреднение соседних точек

                //if (type == 1) {
                //    for (int i = 0; i < line.Count - 1; i++) {
                //        result.Add(new RPoint(line[i].X, (line[i].Y + line[i + 1].Y) / 2, (line[i].Z + line[i + 1].Z) / 2));
                //    }
                //} else if (type == 2) {
                //    for (int i = 0; i < line.Count - 1; i++) {
                //        result.Add(new RPoint((line[i].X + line[i + 1].X) / 2, line[i].Y, (line[i].Z + line[i + 1].Z) / 2));
                //    }
                //} else if (type == 3) {
                //    for (int i = 0; i < line.Count - 1; i++) {
                //        result.Add(new RPoint((line[i].X + line[i + 1].X) / 2, (line[i].Y + line[i + 1].Y) / 2, line[i].Z));
                //    }
                //}
                //result.Add(new RPoint(line[line.Count - 1].X, line[line.Count - 1].Y, line[line.Count - 1].Z));
                #endregion

                #region

                //int count = line.Count;
                //if (type == 1) {
                //    double y_sred = 0;
                //    double z_sred = 0;
                //    for (int i = 0; i < count; i++) {
                //        y_sred += line[i].Y;
                //        z_sred += line[i].Z;
                //    }
                //    y_sred /= count;
                //    z_sred /= count;
                //    for (int i = 0; i < count; i++) {
                //        result.Add(new RPoint(line[i].X, y_sred, z_sred));
                //    }
                //} else if (type == 2) {
                //    double x_sred = 0;
                //    double z_sred = 0;
                //    for (int i = 0; i < count; i++) {
                //        x_sred += line[i].X;
                //        z_sred += line[i].Z;
                //    }
                //    x_sred /= count;
                //    z_sred /= count;
                //    for (int i = 0; i < count; i++) {
                //        result.Add(new RPoint(x_sred, line[i].Y, z_sred));
                //    }
                //} else if (type == 3) {
                //    double y_sred = 0;
                //    double x_sred = 0;
                //    for (int i = 0; i < count; i++) {
                //        y_sred += line[i].Y;
                //        x_sred += line[i].X;
                //    }
                //    y_sred /= count;
                //    x_sred /= count;
                //    for (int i = 0; i < count; i++) {
                //        result.Add(new RPoint(x_sred, y_sred, line[i].X));
                //    }
                //}

                #endregion

                int count = line.Count;
                double a, b, c, d;

                if (type == 1) {
                    (a, b) = GetApprox(line.Select(t => t.X).ToArray(), line.Select(t => t.Y).ToArray());
                    (c, d) = GetApprox(line.Select(t => t.X).ToArray(), line.Select(t => t.Z).ToArray());
                    for (int i = 0; i < count; i++) {
                        result.Add(new RPoint(line[i].X, a * line[i].X + b, c * line[i].X + d));
                    }
                } else if (type == 2) {
                    (a, b) = GetApprox(line.Select(t => t.Y).ToArray(), line.Select(t => t.X).ToArray());
                    (c, d) = GetApprox(line.Select(t => t.Y).ToArray(), line.Select(t => t.Z).ToArray());
                    for (int i = 0; i < count; i++) {
                        result.Add(new RPoint(a * line[i].Y + b, line[i].Y, c * line[i].Y + d));
                    }
                } else if (type == 3) {
                    (a, b) = GetApprox(line.Select(t => t.Z).ToArray(), line.Select(t => t.X).ToArray());
                    (c, d) = GetApprox(line.Select(t => t.Z).ToArray(), line.Select(t => t.Y).ToArray());
                    for (int i = 0; i < count; i++) {
                        result.Add(new RPoint(a * line[i].Z + b, c * line[i].Z + d, line[i].Z));
                    }
                }

                return result;


                (double a1, double b1) GetApprox(double[] x, double[] y)
                {
                    double sumx = 0;
                    double sumy = 0;
                    double sumx2 = 0;
                    double sumxy = 0;
                    int n = x.Length;
                    for (int i = 0; i < n; i++) {
                        sumx += x[i];
                        sumy += y[i];
                        sumx2 += x[i] * x[i];
                        sumxy += x[i] * y[i];
                    }
                    double a1 = (n * sumxy - (sumx * sumy)) / (n * sumx2 - sumx * sumx);
                    double b1 = (sumy - a1 * sumx) / n;
                    return (a1, b1);
                }
            }

        }


        #endregion
    }
}
