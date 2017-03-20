using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для LViewer.xaml
    /// </summary>
    public partial class LViewer : UserControl {
        public LViewer() {
            InitializeComponent();
            Data = new LasViewer();
            DataContext = Data;
        }
        public LasViewer Data;
        public void SetData(double[] X, double[] Z) {
            List<LPoint> data = Helper.GetLaserData(X, Z, true);
            Data.LasData = new ObservableCollection<DataPoint>();
            for (int i = 0; i < data.Count; i++) {
                Data.LasData.Add(new DataPoint(data[i].X, data[i].Z));
            }
            
        }
        public void SetData(List<LPoint> data) {
            Data.LasData = new ObservableCollection<DataPoint>();           
            for (int i = 0; i < data.Count; i++) {
                Data.LasData.Add(new DataPoint(data[i].X, data[i].Z));
            }
        }
        public void SetPoints(List<LPoint> points) {
            Data.Points = new ObservableCollection<DataPoint>();
            if (points.Count == 1) {
                Data.Points.Add(new DataPoint(points[0].X, points[0].Z));
                Data.Points.Add(new DataPoint(points[0].X, points[0].Z));
            } else {
                for (int i = 0; i < points.Count; i++) {
                    Data.Points.Add(new DataPoint(points[i].X, points[i].Z));
                }
            }
        }
        public void SetPoint(LPoint point) {
            Data.Points = new ObservableCollection<DataPoint>();
                Data.Points.Add(new DataPoint(point.X, point.Z));
                Data.Points.Add(new DataPoint(point.X, point.Z));        
        }
    }
    public class LasViewer : DependencyObject{



        public ObservableCollection<DataPoint> Points {
            get { return (ObservableCollection<DataPoint>)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Points.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(ObservableCollection<DataPoint>), typeof(LasViewer), new PropertyMetadata(new ObservableCollection<DataPoint>()));



        public ObservableCollection<DataPoint> LasData {
            get { return (ObservableCollection<DataPoint>)GetValue(LasDataProperty); }
            set { SetValue(LasDataProperty, value); }
        }
        // Using a DependencyProperty as the backing store for LasData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LasDataProperty =
            DependencyProperty.Register("LasData", typeof(ObservableCollection<DataPoint>), typeof(LasViewer), new PropertyMetadata(new ObservableCollection<DataPoint>()));
    }
}
