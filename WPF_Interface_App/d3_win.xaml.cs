using HelperControls;
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
using System.Windows.Shapes;

namespace WPF_Interface_App {
    /// <summary>
    /// Логика взаимодействия для d3_win.xaml
    /// </summary>
    public partial class d3_win : Window {
        public d3_win() {
            InitializeComponent();
        }
        public void AddPoint(RPoint point, int index = 0) {
            d3.AddPoint(point.X, point.Y, point.Z, index);
        }
    }
}
