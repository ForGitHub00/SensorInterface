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

namespace HelperControls {
    /// <summary>
    /// Логика взаимодействия для MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window {
        public MapWindow() {
            InitializeComponent();
            Loaded += MapWindow_Loaded;
        }

        private void MapWindow_Loaded(object sender, RoutedEventArgs e) {
           // DataContext = new MapViewModel();
           // _vm.DataContext = new MapViewModel();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            //Console.WriteLine($"{_vm.Model.Series.Count}");
        }
    }
}
