﻿using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HelperControls {
    public class MapModel : DependencyObject {
        public MapModel(bool robot = false, bool laser = false, bool angle = false) {
            RobotDraw = robot;
            LaserDraw = laser;
            AngleDraw = angle;
            RobotMap = new ObservableCollection<RPoint>();
            LaserMap = new ObservableCollection<RPoint>();

            single = Singleton.GetInstance();
        }

        public bool RobotDraw;
        public bool LaserDraw;
        public bool AngleDraw;

        public ObservableCollection<RPoint> RobotMap;
        public ObservableCollection<RPoint> LaserMap;

        Singleton single;

        public void AddRobotPoint(RPoint p) {
            RDataXZ.Add(new DataPoint(p.X, p.Z));
            RDataXY.Add(new DataPoint(p.X, p.Y));
            RDataXA.Add(new DataPoint(p.X, p.A));
            RDataXB.Add(new DataPoint(p.X, p.B));
            RDataXC.Add(new DataPoint(p.X, p.C));
        }
        public void AddLaserPoint(RPoint p) {
            LDataXZ.Add(new DataPoint(p.X, p.Z));
            LDataXY.Add(new DataPoint(p.X, p.Y));
        }

        public void UsredLaser() {
            List<LPoint> tempData = new List<LPoint>();
            foreach (var item in LDataXY) {
                tempData.Add(new LPoint() { X = item.X, Z = item.Y });
            }
            tempData = Calculate.Laser.Filters.SortByX(tempData);
            tempData = Calculate.Laser.Filters.AveragingVerticalPro(tempData);

           


            List<LPoint> tempData2 = new List<LPoint>();
            foreach (var item in LDataXZ) {
                tempData2.Add(new LPoint() { X = item.X, Z = item.Y });
            }
            tempData2 = Calculate.Laser.Filters.SortByX(tempData2);
            tempData2 = Calculate.Laser.Filters.AveragingVerticalPro(tempData2);


            LDataXY = new ObservableCollection<DataPoint>();
            LDataXZ = new ObservableCollection<DataPoint>();


            int i = 0;
            List<RPoint> tempmas = new List<RPoint>();
            foreach (var item in tempData) {
                LDataXY.Add(new DataPoint(item.X, item.Z));
                LDataXZ.Add(new DataPoint(tempData2[i].X, tempData2[i].Z));
                RPoint t = new RPoint(item.X, item.Z, tempData2[i].Z, single._MAP[i].A, single._MAP[i].B, single._MAP[i].C);
                tempmas.Add(t);
                i++;
            }
            single._MAP = tempmas;

        }

        public ObservableCollection<DataPoint> RDataXZ {
            get { return (ObservableCollection<DataPoint>)GetValue(RDataXZProperty); }
            set { SetValue(RDataXZProperty, value); }
        }
        public static readonly DependencyProperty RDataXZProperty =
            DependencyProperty.Register("RDataXZ", typeof(ObservableCollection<DataPoint>), typeof(MapModel), new PropertyMetadata(new ObservableCollection<DataPoint>()));
        public ObservableCollection<DataPoint> LDataXZ {
            get { return (ObservableCollection<DataPoint>)GetValue(LDataXZProperty); }
            set { SetValue(LDataXZProperty, value); }
        }
        public static readonly DependencyProperty LDataXZProperty =
            DependencyProperty.Register("LDataXZ", typeof(ObservableCollection<DataPoint>), typeof(MapModel), new PropertyMetadata(new ObservableCollection<DataPoint>()));

        public ObservableCollection<DataPoint> RDataXY {
            get { return (ObservableCollection<DataPoint>)GetValue(RDataXYProperty); }
            set { SetValue(RDataXYProperty, value); }
        }
        public static readonly DependencyProperty RDataXYProperty =
            DependencyProperty.Register("RDataXY", typeof(ObservableCollection<DataPoint>), typeof(MapModel), new PropertyMetadata(new ObservableCollection<DataPoint>()));
        public ObservableCollection<DataPoint> LDataXY {
            get { return (ObservableCollection<DataPoint>)GetValue(LDataXYProperty); }
            set { SetValue(LDataXYProperty, value); }
        }
        public static readonly DependencyProperty LDataXYProperty =
            DependencyProperty.Register("LDataXY", typeof(ObservableCollection<DataPoint>), typeof(MapModel), new PropertyMetadata(new ObservableCollection<DataPoint>()));

        public ObservableCollection<DataPoint> RDataXA {
            get { return (ObservableCollection<DataPoint>)GetValue(RDataXAProperty); }
            set { SetValue(RDataXAProperty, value); }
        }
        public static readonly DependencyProperty RDataXAProperty =
            DependencyProperty.Register("RDataXA", typeof(ObservableCollection<DataPoint>), typeof(MapModel), new PropertyMetadata(new ObservableCollection<DataPoint>()));
        public ObservableCollection<DataPoint> RDataXB {
            get { return (ObservableCollection<DataPoint>)GetValue(RDataXBProperty); }
            set { SetValue(RDataXBProperty, value); }
        }
        public static readonly DependencyProperty RDataXBProperty =
            DependencyProperty.Register("RDataXB", typeof(ObservableCollection<DataPoint>), typeof(MapModel), new PropertyMetadata(new ObservableCollection<DataPoint>()));
        public ObservableCollection<DataPoint> RDataXC {
            get { return (ObservableCollection<DataPoint>)GetValue(RDataXCProperty); }
            set { SetValue(RDataXCProperty, value); }
        }
        public static readonly DependencyProperty RDataXCProperty =
            DependencyProperty.Register("RDataXC", typeof(ObservableCollection<DataPoint>), typeof(MapModel), new PropertyMetadata(new ObservableCollection<DataPoint>()));


    }
}
