using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperControls {
    public class Correction {
        public Correction() {
            Map = new ObservableCollection<RPoint>();
        }
        public Correction(double speed) {
            Map = new ObservableCollection<RPoint>();
            Speed = speed;
        }

        public ObservableCollection<RPoint> Map;
        public double Speed;

        public void Calculate(RPoint p, string rec, ref string send) {
            
            
        }
    }
}
