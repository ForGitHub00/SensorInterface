using HelperControls;
using LaserDLL;
using RSI_DLL;
using System;
using System.Collections.Generic;
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

            StartWork();
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
            Viewers = new List<LaserViewer>();
        }

        public List<LaserViewer> Viewers;
        private int fileCounter = 0;
        MapModel map;
        Correction _Cor;

        private void _initMap() {
            MapWindow mapW = new MapWindow();
            mapW.Show();
            map = new MapModel();
            mapW.DataContext = map;
        }
        public void StartWork() {
            _initMap();
            _Cor = new Correction(0.01);


            Robot R = new Robot(6008);
            R.StartListening();
            // Laser.Init();
            R.Correction = cor;

            strREC = "";
            strSEND = "";



            #region
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

                            if (strREC != "") {
                                map.AddLaserPoint(_Cor.AddPoint(strREC, resultP.X, resultP.Z));
                                RPoint r = _Cor.Calculate(strREC, strSEND);
                                map.AddRobotPoint(r);
                            }

                        }


                    });
                    Thread.Sleep(12);
                }
            }
            #endregion
        }

        public string strREC;
        public string strSEND;

        private string cor(string strR, string strS) {
            strREC = strR;
            strSEND = strS;
            //RPoint r = _Cor.Calculate(strR, strS);

            //Dispatcher.Invoke(() => {
            //    //RPoint r = _Cor.Calculate(strR, strS);
            //   // map.AddRobotPoint(r);

            //});
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
