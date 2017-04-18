using HelperControls;
using LaserDLL;
using RSI_DLL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Interface_App {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Start2();


            d3 = new d3_win();
            d3.Show();


            //for (int i = 0; i < 50; i++) {
            //    d3.d3.AddPoint(i, i, i);
            //}


            // StartWork();
            #region

            //string[] tempMas = Directory.GetDirectories(@"Laser\");
            //sld_path.Maximum = tempMas.Length - 1;

            //_initMap();
            //поток
            //Thread thrd = new Thread(new ThreadStart(mmm));
            //thrd.Start();
            //void mmm()
            //{
            //    Random rnd = new Random();
            //    int cou = 0;
            //    double pos = 0;
            //    double ypos = 0;
            //    while (true) {
            //       Dispatcher.Invoke(() => {
            //            map.AddRobotPoint(new RPoint() {
            //                X = cou++,
            //                Y = ypos,
            //                Z = pos,
            //                A = 5,
            //                B = 10,
            //                C = 40
            //            });
            //            map.AddLaserPoint(new RPointCoordinates() {
            //                X = cou++ + 50,
            //                Y = ypos,
            //                Z = pos
            //            });
            //        });
            //        pos += rnd.Next(-1, 2);
            //        ypos += rnd.Next(-1, 2);
            //        Thread.Sleep(100);
            //    }


            //    //for (int i = 0; i < 50; i++) {
            //    //    Dispatcher.Invoke(() => map.AddPoint(i, rnd.Next(0, 20)));
            //    //    Thread.Sleep(500);
            //    //}
            //}
            //

            #endregion

            //Viewers = new List<LaserViewer>();
        }

        #region Data
        public List<LaserViewer> Viewers;
        private int fileCounter = 0;
        MapModel map;
        Correction _Cor;
        Robot _R;
        Singleton single;
        d3_win d3;
        #endregion

        public void Start2() {
            //Map
            _initMap();

            single = Singleton.GetInstance();
            _Cor = new Correction(0.01);

            _R = new Robot(6008);
            _R.StartListening();


            #region Laser Start
            /*
            Thread laser_thrd = new Thread(new ThreadStart(LaserThread2));
            laser_thrd.Start();
            void LaserThread()
            {
                Random rnd = new Random();
                Laser.Init();
                while (true) {
                    Dispatcher.Invoke(() => {
                        Laser.GetProfile(out double[] X, out double[] Z);
                        List<LPoint> data = Helper.GetLaserData(X, Z, true);


                        //data = Calculate.Laser.Filters.AveragingVerticalPro(data, _everyPoint:true, _pointsInStep: 6);
                        

                        LV.SetData(data);
                        List<LPoint> points = Calculate.Laser.AngularSeam.FindMasPoint_Z_Diff(data, 2);
                        if (points.Count >= 1) {
                            LPoint resultP = new LPoint() {
                                //X = (points[0].X + points[1].X) / 2,
                                //Z = (points[0].Z + points[1].Z) / 2,
                                X = points[0].X ,
                                Z = points[0].Z,
                            };
                            LV.SetPoint(resultP);                       
                            if (single.Position.X != Double.MinValue) {
                                RPoint findPoint = _Cor.Trans(single.Position, resultP);
                                if (single._MAP.Count != 0) {
                                    if ((Math.Abs(findPoint.Y - single._MAP[single._MAP.Count - 1].Y) < 3) && (Math.Abs(findPoint.Z - single._MAP[single._MAP.Count - 1].Z) < 3)) {
                                        map.AddLaserPoint(findPoint);
                                        single._MAP.Add(findPoint);                                       
                                    }
                                } else {
                                    map.AddLaserPoint(findPoint);
                                    single._MAP.Add(findPoint);
                                }
                                map.AddRobotPoint(single.Position);
                                map.UsredLaser();
                            }
                        }
                    });
                    Thread.Sleep(36);
                }
            }
            void LaserThread2()
            {
                Random rnd = new Random();
                Laser.Init();

                double tx = 5;
                double ty = 5;
                double tz = 5;

                while (true) {
                    Dispatcher.Invoke(() => {
                        Laser.GetProfile(out double[] X, out double[] Z);
                        List<LPoint> data = Helper.GetLaserData(X, Z, true);


                        data = Calculate.Laser.Filters.AveragingVerticalPro(data);


                        LV.SetData(data);

                        //List<LPoint> points = Calculate.Laser.AngularSeam.FindAngularSeamByRegions(data);
                        //LV.SetPoints(points);

                        LPoint point = Calculate.Laser.AngularSeam.FindAngularSeamSimple(data);
                        LV.SetPoint(point);

                        //RPoint findPoint = new RPoint(rnd.Next(1, 10), rnd.Next(1, 10), rnd.Next(1, 10));
                        RPoint findPoint = new RPoint(tx = single.Position.X + 100, ty += rnd.NextDouble() - 0.5 , tz += rnd.NextDouble() - 0.5);
                        map.AddLaserPoint(findPoint);
                        single._MAP.Add(findPoint);
                        map.AddRobotPoint(single.Position);
                        map.UsredLaser();

                    });
                    Thread.Sleep(36);
                }
            } */
            #endregion

            #region Laser Start Testing Types
            Thread laser_thrd = new Thread(new ThreadStart(LaserThread));
            laser_thrd.Start();
            void LaserThread()
            {
                Random rnd = new Random();
                Laser.Init();
                int tx = 0;
                List<RPoint> line1 = new List<RPoint>();
                List<RPoint> line2 = new List<RPoint>();

                Dispatcher.Invoke(() => {



                    for (int i = 100; i <= 1000; i++) {
                        line1.Add(new RPoint(i + (rnd.NextDouble() - 0.5) * 10, rnd.Next(50, 300) + i / 3, rnd.Next(50, 100)));
                        //line1.Add(new RPoint(rnd.Next(50, 1000), rnd.Next(50, 1000), rnd.Next(50, 1000)));
                        //line1.Add(new RPoint(i, 50 + (rnd.NextDouble() - 0.5) * 10, 50 + (rnd.NextDouble() - 0.5) * 10));
                        // line1.Add(new RPoint(i, 50 , 50));
                    }
                    //line1.Add(new RPoint(0, 0, 0));
                    //line1.Add(new RPoint(0, 100, 0));
                    //line1.Add(new RPoint(0, 200, 40));
                    //line1.Add(new RPoint(0, 300, 0));

                    foreach (var item in line1) {
                        d3.AddPoint(item);
                    }

                    int typ = Calculate.Robot.LineType(line1);
                    var line1t = Calculate.Robot.LineApprox(line1, typ);

                    foreach (var item in line1t) {
                        d3.AddPoint(item, 1);
                    }




                    line2 = new List<RPoint>();

                    for (int i = 200; i < 1000; i++) {
                        line2.Add(new RPoint(rnd.Next(50, 100), i + (rnd.NextDouble() - 0.5) * 10, rnd.Next(50, 150)));
                        //line2.Add(new RPoint(rnd.Next(50, 1000), rnd.Next(50, 1000), rnd.Next(50, 1000)));
                        //line2.Add(new RPoint(50 + (rnd.NextDouble() - 0.3) * 10, i + rnd.Next(0,5), 50 + (rnd.NextDouble() - 0.5) * 10));
                        //line2.Add(new RPoint(50 , i , 50 ));
                    }
                    foreach (var item in line2) {
                        d3.AddPoint(item);
                    }

                    int typ2 = Calculate.Robot.LineType(line2);
                    var line2t = Calculate.Robot.LineApprox(line2, typ2);

                    foreach (var item in line2t) {
                        d3.AddPoint(item, 1);
                    }

                    d3.AddPoint(Calculate.Robot.CalculatePoint_2line(line1, line2), 2);



                });

                








                while (true) {
                    Dispatcher.Invoke(() => {
                        Laser.GetProfile(out double[] X, out double[] Z);
                        List<LPoint> data = Helper.GetLaserData(X, Z, true);


                        int vstep = 2;
                        int hstep = 100;
                        int pointsCount = 6;
                        if (cb_filter.IsChecked == true) {
                            if (!tb_hstep.IsFocused) {
                                hstep = Convert.ToInt32(tb_hstep.Text);
                            }
                            if (!tb_vstep.IsFocused) {
                                vstep = Convert.ToInt32(tb_vstep.Text);
                            }
                            if (!tb_points.IsFocused) {
                                pointsCount = Convert.ToInt32(tb_points.Text);
                            }
                            bool evrP = (bool)cb_everyPoint.IsChecked;

                            data = Calculate.Laser.Filters.AveragingVerticalPro(data, vstep, hstep, pointsCount, evrP);
                        }




                       



                        //for (int i = 0; i < 10; i++) {
                        //   d3.AddPoint(new RPoint(tx, rnd.Next(0,1000), rnd.Next(0, 1000)));
                        //   d3.AddPoint(new RPoint(tx, rnd.Next(0, 1000) + 100, rnd.Next(0, 1000)), index: 1);
                        //   d3.AddPoint(new RPoint(tx, rnd.Next(0, 1000), rnd.Next(0, 1000)), index: 2);
                        //}
                        //tx++;




                        LV.SetData(data);
                        Stopwatch s1 = new Stopwatch();
                        s1.Start();

                        if (rb_t1.IsChecked == true) {
                            LPoint res = Calculate.Laser.Voronej.Type1_1point(data);
                            LV.SetPoint(res);
                            //d3.AddPoint(HelperControls.Transform.Trans(tx,0,0,0,0,0, 0, res.X,res.Z));
                            if (res.X != 0) {
                                RPoint findPoint = _Cor.Trans(new RPoint(tx, 0, 0, 0, 0, 0), res);
                                map.AddLaserPoint(findPoint);
                                single._MAP.Add(findPoint);

                                //if (tx < 500) {
                                //    d3.AddPoint(HelperControls.Transform.Trans(tx + 100, 0, 0, 0, 0, 0, 0, res.X, res.Z));
                                //    line1.Add(HelperControls.Transform.Trans(tx + 100, 0, 0, 0, 0, 0, 0, res.X, res.Z));
                                //} else if (tx < 1000) {
                                //    d3.AddPoint(HelperControls.Transform.Trans(50, 0, tx, 0, 0, 0, 0, res.X, res.Z));
                                //    line2.Add(HelperControls.Transform.Trans(50, 0, tx, 0, 0, 0, 0, res.X, res.Z));
                                //} else if (tx == 1000){

                                //    int typ = Calculate.Robot.LineType(line1);
                                //    var line1t = Calculate.Robot.LineApprox(line1, typ);
                                //    foreach (var item in line1t) {
                                //        d3.AddPoint(item, 1);
                                //    }
                                //    int typ2 = Calculate.Robot.LineType(line2);
                                //    var line2t = Calculate.Robot.LineApprox(line2, typ2);

                                //    foreach (var item in line2t) {
                                //        d3.AddPoint(item, 1);
                                //    }

                                //    d3.AddPoint(Calculate.Robot.CalculatePoint_2line(line1, line2), 2);

                                //}
                                //Console.WriteLine($"TX = {tx}");
                                //tx+=10;
                            }
                            map.UsredLaser();

                        } else if (rb_t2.IsChecked == true) {
                            (LPoint left, LPoint right) = Calculate.Laser.Voronej.Type3_2point(data);
                            LV.SetPoints(new List<LPoint>() {
                                left, right
                            });
                        } else {
                            LPoint res = Calculate.Laser.Voronej.Type3_1point(data);
                            LV.SetPoint(res);
                        }

                        s1.Stop();
                        //Console.WriteLine(s1.ElapsedMilliseconds);
                    });
                    Thread.Sleep(36);
                }
            }
            void LaserThread2()
            {
                Random rnd = new Random();
                Laser.Init();

                double tx = 5;
                double ty = 5;
                double tz = 5;

                while (true) {
                    Dispatcher.Invoke(() => {
                        Laser.GetProfile(out double[] X, out double[] Z);
                        List<LPoint> data = Helper.GetLaserData(X, Z, true);


                        data = Calculate.Laser.Filters.AveragingVerticalPro(data);


                        LV.SetData(data);

                        //List<LPoint> points = Calculate.Laser.AngularSeam.FindAngularSeamByRegions(data);
                        //LV.SetPoints(points);

                        LPoint point = Calculate.Laser.AngularSeam.FindAngularSeamSimple(data);
                        LV.SetPoint(point);

                        //RPoint findPoint = new RPoint(rnd.Next(1, 10), rnd.Next(1, 10), rnd.Next(1, 10));
                        RPoint findPoint = new RPoint(tx = single.Position.X + 100, ty += rnd.NextDouble() - 0.5, tz += rnd.NextDouble() - 0.5);
                        map.AddLaserPoint(findPoint);
                        single._MAP.Add(findPoint);
                        map.AddRobotPoint(single.Position);
                        map.UsredLaser();

                    });
                    Thread.Sleep(36);
                }
            }
            #endregion

        }


        private void _initMap() {
            MapWindow mapW = new MapWindow();
            mapW.Show();
            map = new MapModel();
            mapW.DataContext = map;
        }
        public void StartWork() {
            _initMap();
            _Cor = new Correction(0.01);

            _R = new Robot(6008);
            _R.StartListening();
            // Laser.Init();
            _R.Correction = cor;


            #region laserThrd
            Thread laser_thrd = new Thread(new ThreadStart(LaserThrd));
            laser_thrd.Start();
            void LaserThrd()
            {
                Laser.Init();
                while (true) {
                    Dispatcher.Invoke(() => {
                        Laser.GetProfile(out double[] X, out double[] Z);
                        List<LPoint> data = Helper.GetLaserData(X, Z, true);


                        data = Calculate.Laser.Filters.AveragingVerticalPro(data);


                        LV.SetData(data);
                        List<LPoint> points = Calculate.Laser.AngularSeam.FindMasPoint_ZX_Diff(data, 2);
                        if (points.Count > 1) {
                            LPoint resultP = new LPoint() {
                                X = (points[0].X + points[1].X) / 2,
                                Z = (points[0].Z + points[1].Z) / 2,
                            };
                            LV.SetPoint(resultP);
                            if (single.Position.X != Double.MinValue) {
                                RPoint findPoint = _Cor.Trans(single.Position, resultP);
                                map.AddLaserPoint(findPoint);
                                single._MAP.Add(findPoint);
                            }
                        }
                    });
                    Thread.Sleep(36);
                }
            }
            #endregion
        }


        private string cor(string strR, string strS) {
            Stopwatch sw = new Stopwatch();
            sw.Start();



            //Console.WriteLine(strR);

            //strREC = strR;
            // strSEND = strS;

            //RPoint r = _Cor.Calculate(strR, strS);

            //Dispatcher.Invoke(() => {
            //    //    RPoint r = _Cor.Calculate(strR, strS);
            //    //    map.AddRobotPoint(r);
            //    //_R.foo();
            //});
            //ParserXML.SetValue(ref strS, "Sen\\RKorr\\X", 0.01);

            sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds);
            return strS;
        }

        #region
        /*
        
        public void addNew(int fold, int file, int par) {
            lb.Items.Clear();
            Helper.GetLaserDataFromTXT($"Laser\\prof{fold}\\prof{file}.txt", out double[] X, out double[] Z);
            List<LPoint> data = Helper.GetLaserData(X, Z, true);
            LaserViewer tempLV = new LaserViewer(data, "No Filtres", "No Filtres");
            var points = Calculate.Laser.AngularSeam.FindMasPoint_ZX_Diff(data, 3);
            for (int i = 0; i < points.Count; i++) {
                tempLV.DrawPoint(points[i]);
            }
            lb.Items.Add(tempLV);


            data = Calculate.Laser.Filters.AveragingVerticalPro(data, _pointsInStep: par);
            tempLV = new LaserViewer(data, "AveragingVerticalPro", "Standart With Points");
            points = Calculate.Laser.AngularSeam.FindMasPoint_ZX_Diff(data, 3);
            for (int i = 0; i < points.Count; i++) {
                tempLV.DrawPoint(points[i]);
            }
            lb.Items.Add(tempLV);


            data = Calculate.Laser.Filters.AveragingVerticalPro(data, _pointsInStep: par, _everyPoint: true);
            lb.Items.Add(new LaserViewer(data, "AveragingVerticalPro", "Every Point"));
        }
        private void sld_file_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (sld.IsLoaded) {
                string[] fileEntries = Directory.GetFiles($"Laser\\prof{(int)sld_path.Value}");
                fileCounter = fileEntries.Length;
                sld_file.Minimum = 0;
                sld_file.Maximum = fileEntries.Length - 1;
                addNew((int)sld_path.Value, (int)sld_file.Value, (int)sld.Value);
            }
        }
        */
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e) {
            _R.exit = true;
            Thread.Sleep(36);
            _R.Stoplistening();
        }
    }
}
