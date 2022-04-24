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

namespace MiniProjekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            tgl1.Checked += Tgl1_Checked;
            tgl1.Unchecked += Tgl1_Unchecked;
            tgl2.Checked += Tgl2_Checked;
            tgl2.Unchecked += Tgl2_Unchecked;
            tgl1.IsChecked = true;
        }

        private void Tgl2_Unchecked(object sender, RoutedEventArgs e)
        {
            tgl1.IsChecked = true;
        }

        private void Tgl2_Checked(object sender, RoutedEventArgs e)
        {
            tgl1.IsChecked = false;
        }

        private void Tgl1_Unchecked(object sender, RoutedEventArgs e)
        {
            tgl2.IsChecked = true;
        }

        private void Tgl1_Checked(object sender, RoutedEventArgs e)
        {
            tgl2.IsChecked = false;
        }
    
    }
}
