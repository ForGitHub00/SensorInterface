using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace HelperControls {
    /// <summary>
    /// Логика взаимодействия для LaserViewer.xaml
    /// </summary>
    public partial class LaserViewer : UserControl{
        public LaserViewer() {
            InitializeComponent();
        }
        public LaserViewer(List<LPoint> data, string title, string subT) {
            InitializeComponent();
            _Title = title;
            _SubTitle = subT;
            Data = data;
            DrawProfile();
        }
        public LaserViewer(List<LPoint> data, string title, string subT, LPoint find) {
            InitializeComponent();
            _Title = title;
            _SubTitle = subT;
            Data = data;
            DrawProfile();
            FindPoint = find;
            DrawPoint();
        }

        public string _Title;
        public string _SubTitle;
        public List<LPoint> Data { get; set; }
        public int c = 651;
        public LPoint FindPoint;

        public void DrawProfile() {
            var tmp = new PlotModel { Title = _Title, Subtitle = _SubTitle };
            if (Data.Count == 0) {
                return;
            }
            var series1 = new LineSeries { Title = "Profile", MarkerType = MarkerType.Circle, MarkerStrokeThickness = 0.5 };
            for (int i = 0; i < Data.Count; i++) {
                series1.Points.Add(new DataPoint(Data[i].X, Data[i].Z));
                series1.Points.Add(new DataPoint(Data[i].X, Data[i].Z));
            }
            var xa = new LinearAxis() {
                Position = AxisPosition.Bottom,
                Minimum = -50,
                Maximum = 50,
                Title = "X",
                PositionAtZeroCrossing = false,
                MinorGridlineStyle = LineStyle.LongDash
            };
            xa.TickStyle = TickStyle.Outside;
            xa.IsPanEnabled = false;
            xa.IsZoomEnabled = false;

            var ya = new LinearAxis() {
                Position = AxisPosition.Left,
                Minimum = Data.Min(item => item.Z) - 2,
                Maximum = Data.Max(item => item.Z) + 2,
                Title = "Z",
                PositionAtZeroCrossing = false,
                MinorGridlineStyle = LineStyle.LongDash
            };

            tmp.Axes.Add(xa);
            tmp.Axes.Add(ya);

            tmp.Series.Add(series1);
            tmp.LegendItemOrder = LegendItemOrder.Reverse;
            tmp.LegendOrientation = LegendOrientation.Horizontal;
            tmp.LegendPosition = LegendPosition.RightBottom;
            tmp.LegendPlacement = LegendPlacement.Outside;

            v_laser.Model = tmp;
        }
        public void DrawPoint() {
            var series2 = new LineSeries { Title = "Point", MarkerType = MarkerType.Circle };
            series2.Points.Add(new DataPoint(FindPoint.X, FindPoint.Z));
            series2.Points.Add(new DataPoint(FindPoint.X, FindPoint.Z));
            //series2.Points.Add(new DataPoint(5, 287));
            v_laser.Model.Series.Add(series2);
        }
        public void DrawPoint(LPoint point) {
            var series2 = new LineSeries { Title = "Point", MarkerType = MarkerType.Circle };
            series2.Points.Add(new DataPoint(point.X, point.Z));
            series2.Points.Add(new DataPoint(point.X, point.Z));
            v_laser.Model.Series.Add(series2);
        }
        public void DrawTwoPoints(LPoint point, LPoint point2 ) {
            var series2 = new LineSeries { Title = "Point", MarkerType = MarkerType.Circle };
            series2.Points.Add(new DataPoint(point.X, point.Z));
            series2.Points.Add(new DataPoint(point2.X, point2.Z));
            v_laser.Model.Series.Add(series2);
        }

        public void Init() {
            
            Random rnd = new Random();
            var tmp = new PlotModel { Title = "Seam Tracking", Subtitle = _Title };


           // Helper.GetLaserDataFromTXT($"Laser\\prof23\\prof{c}.txt", out double[] X, out double[] Z);
            //Array.Sort(X);
            //Data = Helper.GetLaserData(X, Z, true);
            //Data = Calculate.Laser.Filters.SortByX(Data, false);
            //Data = Calculate.Laser.Filters.AveragingVertical(Data, 1, 100, 2);
            


            if (Data.Count == 0) {
                return;
            }

            var series1 = new LineSeries { Title = "Series 1", MarkerType = MarkerType.Diamond, MarkerStrokeThickness = 0.5};
            for (int i = 0; i < Data.Count; i++) {
                series1.Points.Add(new DataPoint(Data[i].X, Data[i].Z));

            }
            //var series2 = new LineSeries { Title = "Series 2", MarkerType = MarkerType.Circle };
            //series2.Points.Add(new DataPoint(5, 287));
            //series2.Points.Add(new DataPoint(5, 287));
            //tmp.Series.Add(series2);

            var xa = new LinearAxis() {
                Position = AxisPosition.Bottom,
                Minimum = -50,
                Maximum = 50,
                Title = "X",
                PositionAtZeroCrossing = false,
                MinorGridlineStyle = LineStyle.LongDash
            };
            xa.TickStyle = TickStyle.Outside;
            xa.IsPanEnabled = false;
            xa.IsZoomEnabled = false;


            var ya = new LinearAxis() {
                Position = AxisPosition.Left,
                Minimum = Data.Min(item => item.Z) - 2,
                Maximum = Data.Max(item => item.Z) + 2,
                Title = "Z",
                PositionAtZeroCrossing = false,
                MinorGridlineStyle = LineStyle.LongDash
            };

            tmp.Axes.Add(xa);
            tmp.Axes.Add(ya);

            tmp.Series.Add(series1);
            tmp.LegendItemOrder = LegendItemOrder.Reverse;

            v_laser.Model = tmp;

            
        }
        public void redraw() {
            Random rnd = new Random();
            var tmp = new PlotModel { Title = "Seam Tracking", Subtitle = "Find Seam Alg" };
            
            
            Helper.GetLaserDataFromTXT($"Laser\\prof23\\prof{c}.txt", out double[] X, out double[] Z);
            //Array.Sort(X);
            List<LPoint> data = Helper.GetLaserData(X, Z, true);
            data = Calculate.Laser.Filters.AveragingVertical(data, 1, 100, 2);

            
            
            if (data.Count == 0) {
                return;
            }

            var series1 = new LineSeries { Title = "Series 1", MarkerType = MarkerType.Cross, MarkerStrokeThickness = 0.1 };
            for (int i = 0; i < data.Count; i++) {
                    series1.Points.Add(new DataPoint(data[i].X, data[i].Z));
                
            }
            var series2 = new LineSeries { Title = "Series 2", MarkerType = MarkerType.Circle };
            series2.Points.Add(new DataPoint(5, 287));
            series2.Points.Add(new DataPoint(5, 287));
            tmp.Series.Add(series2);

            var xa = new LinearAxis() {
                Position = AxisPosition.Bottom,
                Minimum = -50,
                Maximum = 50,
                Title = "X",
                PositionAtZeroCrossing = false,
                MinorGridlineStyle = LineStyle.LongDash
            };
            xa.TickStyle = TickStyle.Outside;
            xa.IsPanEnabled = false;
            xa.IsZoomEnabled = false;


            var ya = new LinearAxis() {
                Position = AxisPosition.Left,
                Minimum = data.Min(item => item.Z) - 2,
                Maximum = data.Max(item => item.Z) + 2,
                Title = "Z",
                PositionAtZeroCrossing = false,
                MinorGridlineStyle = LineStyle.LongDash
            };

            tmp.Axes.Add(xa);
            tmp.Axes.Add(ya);

            tmp.Series.Add(series1);
            tmp.LegendItemOrder = LegendItemOrder.Reverse;

            v_laser.Model = tmp;

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e) {

        }
    }
}
