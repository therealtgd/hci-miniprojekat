using LiveCharts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private const string API_KEY = "LJUP3P0MBG9X3SBZ";
        
        private const string CPI = "Consumer Price Index";
        private const string INF = "Inflation";
        private const string CS = "Consumer Sentiment";

        private string selectedInterval = "monthly";
        private string activeView = "line";

        private LineChart lineChart;
        private BarChart barChart;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
            lineChart = new LineChart();
            barChart = new BarChart();
            DataContext = this;
        }

        public async Task getData()
        {
            lineChart.clear();
            barChart.clear();

            ComboBoxItem typeItem = (ComboBoxItem)dataType.SelectedItem;
            string selectedDataType = typeItem.Content.ToString();

            string QUERY_URL = "";
            if (selectedDataType == CPI)
                QUERY_URL = $"https://www.alphavantage.co/query?function=CPI&interval={selectedInterval}&apikey={API_KEY}";
            else if (selectedDataType == INF)
                QUERY_URL = $"https://www.alphavantage.co/query?function=INFLATION&apikey={API_KEY}";
            else if (selectedDataType == CS)
                QUERY_URL = $"https://www.alphavantage.co/query?function=CONSUMER_SENTIMENT&apikey={API_KEY}";
            else
                return;

            HttpClient client = new HttpClient();
            int retries = 0;
            while (retries < 5)
            {
                using (HttpResponseMessage response = await client.GetAsync(QUERY_URL))
                {
                    using (HttpContent content = response.Content)
                    {
                        string jsonResponse = await content.ReadAsStringAsync();
                        QueryResult? queryResult = JsonConvert.DeserializeObject<QueryResult>(jsonResponse);
                        if (queryResult.name == null)
                        {
                            retries += 1;
                            await Task.Delay(1000);
                            continue;
                        }

                        ChartValues<double> values = queryResult.getValues();
                        lineChart.fillDates(queryResult.data);
                        barChart.fillDates(queryResult.data);
                        lineChart.drawLine(values);
                        barChart.drawLine(values);

                        showTable.Visibility = Visibility.Visible;
                        return;
                    }
                }
            }
            // Show user that connection could not be established
            showTable.Visibility = Visibility.Hidden;
        }

        public void switchView()
        {
            if (activeView == "line")
            {
                barChartUI.Visibility = Visibility.Collapsed;
                lineChartUI.Visibility = Visibility.Visible;
            }
            else
            {
                lineChartUI.Visibility = Visibility.Collapsed;
                barChartUI.Visibility = Visibility.Visible;
            }
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            tgl1.Checked += Tgl1_Checked;
            tgl1.Unchecked += Tgl1_Unchecked;
            tgl2.Checked += Tgl2_Checked;
            tgl2.Unchecked += Tgl2_Unchecked;
            tgl1.IsChecked = true;

            intervalTgl1.Checked += intervalTgl1_Checked;
            intervalTgl1.Unchecked += intervalTgl1_Unchecked;
            intervalTgl2.Checked += intervalTgl2_Checked;
            intervalTgl2.Unchecked += intervalTgl2_Unchecked;
            intervalTgl1.IsChecked = true;

            dataType.SelectionChanged += dataType_SelectionChanged;
            await getData();
        }

        private void Tgl2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (tgl1_handle)
                tgl2.IsChecked = true;
        }

        private bool tgl2_handle = true;
        private void Tgl2_Checked(object sender, RoutedEventArgs e)
        {
            if (!tgl2_handle)
                return;
            tgl2_handle = false;
            tgl1.IsChecked = false;
            tgl2.IsChecked = true;
            activeView = "bar";
            switchView();
            tgl2_handle = true;
        }

        private void Tgl1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (tgl2_handle)
                tgl1.IsChecked = true;
        }

        private bool tgl1_handle = true;
        private void Tgl1_Checked(object sender, RoutedEventArgs e)
        {
            if (!tgl1_handle)
                return;
            tgl1_handle = false;
            tgl2.IsChecked = false;
            tgl1.IsChecked = true;
            activeView = "line";
            switchView();
            tgl1_handle = true;
        }

        private bool intervalTgl1_handle = true;
        private void intervalTgl1_Checked(object sender, RoutedEventArgs e)
        {
            if (!intervalTgl1_handle)
                return;
            intervalTgl1_handle = false;
            intervalTgl2.IsChecked = false;
            intervalTgl1.IsChecked = true;
            selectedInterval = "monthly";
            getData();
            intervalTgl1_handle = true;
        }

        private bool intervalTgl2_handle = true;
        private void intervalTgl2_Checked(object sender, RoutedEventArgs e)
        {
            if (!intervalTgl2_handle)
                return;
            intervalTgl2_handle = false;
            intervalTgl1.IsChecked = false;
            intervalTgl2.IsChecked = true;
            selectedInterval = "semiannual";
            getData();
            intervalTgl2_handle = true;
        }

        private void intervalTgl1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (intervalTgl2_handle)
                intervalTgl1.IsChecked = true;
        }

        private void intervalTgl2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (intervalTgl1_handle)
                intervalTgl2.IsChecked = true;
        }

        private void dataType_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)dataType.SelectedItem;
            string selectedDataType = typeItem.Content.ToString();
            if (selectedDataType == CPI)
                interval.Visibility = Visibility.Visible;
            else
                interval.Visibility = Visibility.Collapsed;
            getData();
        }

    }
}