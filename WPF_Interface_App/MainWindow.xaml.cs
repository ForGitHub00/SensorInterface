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
        #endregion

        public void Start2() {
            //Map
            _initMap();

            single = Singleton.GetInstance();
            _Cor = new Correction(0.01);

            _R = new Robot(6008);
            _R.StartListening();


            #region Laser Start
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
            void LaserThread2()
            {
                Random rnd = new Random();
                Laser.Init();
                while (true) {
                    Dispatcher.Invoke(() => {
                        Laser.GetProfile(out double[] X, out double[] Z);
                        List<LPoint> data = Helper.GetLaserData(X, Z, true);


                        data = Calculate.Laser.Filters.AveragingVerticalPro(data);


                        LV.SetData(data);
                        List<LPoint> points = Calculate.Laser.AngularSeam.FindMasPoint_ZX_Diff(data, 2);
                        RPoint findPoint = new RPoint(rnd.Next(1, 1000), rnd.Next(1, 1000), rnd.Next(1, 1000));
                        map.AddLaserPoint(findPoint);
                        single._MAP.Add(findPoint);
                        map.AddRobotPoint(single.Position);
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
    }
}
