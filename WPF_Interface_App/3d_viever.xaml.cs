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
    /// Логика взаимодействия для _3d_viever.xaml
    /// </summary>
    public partial class _3d_viever : Window {
        public _3d_viever() {
            InitializeComponent();
            Loaded += _3d_viever_Loaded;
        }

        private void _3d_viever_Loaded(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }
        public void AddPoint(RPoint point) {

        }
    }
}
