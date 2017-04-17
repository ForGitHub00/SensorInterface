using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        public class Robot {

        }


        #endregion
    }
}
