using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HelperControls {
    public class MapViewModel : DependencyObject {

        public PlotModel Plot {
            get { return (PlotModel)GetValue(PlotProperty); }
            set { SetValue(PlotProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Plot.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlotProperty =
            DependencyProperty.Register("Plot", typeof(PlotModel), typeof(MapViewModel), new PropertyMetadata(null));



        public ICollectionView RobotMapXY {
            get { return (ICollectionView)GetValue(RobotMapProperty); }
            set { SetValue(RobotMapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RobotMap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RobotMapProperty =
            DependencyProperty.Register("RobotMapXY", typeof(ICollectionView), typeof(MapViewModel), new PropertyMetadata(null));

        public MapViewModel() {

            NewPlot();


            


            Console.WriteLine("SDfas"); 

        }

        public void NewPlot() {
            Plot = new PlotModel() { Title = "Test" };
            var series1 = new LineSeries { Title = "Profile", MarkerType = MarkerType.Diamond, MarkerStrokeThickness = 0.5 };
            foreach (var item in RobotMapXY) {
                series1.Points.Add(new DataPoint(((LPoint)item).X, ((LPoint)item).Z));
            }
            var xa = new LinearAxis() {
                Position = AxisPosition.Bottom,
                Title = "X",
                PositionAtZeroCrossing = false,
                MinorGridlineStyle = LineStyle.LongDash
            };
            xa.TickStyle = TickStyle.Outside;
            //xa.IsPanEnabled = false;
            //xa.IsZoomEnabled = false;

            var ya = new LinearAxis() {
                Position = AxisPosition.Left,
                Title = "Z",
                PositionAtZeroCrossing = false,
                MinorGridlineStyle = LineStyle.LongDash
            };

            Plot.Axes.Add(xa);
            Plot.Axes.Add(ya);

            Plot.Series.Add(series1);
            Plot.LegendItemOrder = LegendItemOrder.Reverse;
            Plot.LegendOrientation = LegendOrientation.Horizontal;
            Plot.LegendPosition = LegendPosition.RightBottom;
            Plot.LegendPlacement = LegendPlacement.Outside;
        }
       

    }
}
