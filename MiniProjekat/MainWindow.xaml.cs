using LiveCharts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        private bool firstRun = true;
        private const string CPI = "Consumer Price Index";
        private const string INF = "Inflation";
        private const string CS = "Consumer Sentiment";

        private string selectedInterval = "semiannual";
        private string activeView = "line";

        private List<DataPoint> dataPoints;
        private TableWindow tableWindow;

        public LineChart lineChart { get; set; }
        public BarChart barChart { get; set; }

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
            disableButtons();
            if (firstRun)
            {
                firstRun = false;
                return;
            }
            lineChart.clear();
            barChart.clear();

            string selectedDataType = getSelectedDataTypeString();

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
                    if (!response.IsSuccessStatusCode)
                    {
                        retries += 1;
                        await Task.Delay(1000);
                        continue;
                    }
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
                        barChart.drawBars(values);
                        dataPoints = queryResult.data;

                        if(tableWindow != null)
                        {
                            tableWindow.setDataPoints(dataPoints);
                            tableWindow.updateTitle(getSelectedDataTypeString(), (selectedInterval == "semiannual" ? true : false));
                        }

                        enableButtons();
                        return;
                    }
                }
            }
            // Show user that connection could not be established
            enableButtons();
            MessageBox.Show(
                "Unable to fetch data from Alphavantage. " +
                "Check your connection and try again.",
                "Connection Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
                );
            /*ErrorWindow errorWindow = new ErrorWindow("Error: API not available.");
            errorWindow.ShowDialog();*/
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
            intervalTgl2.IsChecked = true;

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

        private string getSelectedDataTypeString()
        {
            ComboBoxItem typeItem = (ComboBoxItem)dataType.SelectedItem;
            if (typeItem.Content.ToString() is not null)
            {
                return typeItem.Content.ToString();
            }

            return "";
            
        }

        private void enableButtons()
        {
            dataType.IsEnabled = true;
            intervalTgl1.IsEnabled = true;
            intervalTgl2.IsEnabled = true;
            tgl1.IsEnabled = true;
            tgl2.IsEnabled = true;
            showTable.Visibility = Visibility.Visible;
            loading.Visibility = Visibility.Collapsed;
            switchView();
        }

        private void disableButtons()
        {
            dataType.IsEnabled = false;
            intervalTgl1.IsEnabled = false;
            intervalTgl2.IsEnabled = false;
            tgl1.IsEnabled = false;
            tgl2.IsEnabled = false;
            showTable.Visibility = Visibility.Hidden;
            loading.Visibility = Visibility.Visible;
            barChartUI.Visibility = Visibility.Collapsed;
            lineChartUI.Visibility = Visibility.Collapsed;
        }

        private void showTable_Click(object sender, RoutedEventArgs e)
        {
            if (tableWindow != null)
            {
                tableWindow.Close();
            }
            tableWindow = new TableWindow();
            tableWindow.updateTitle(getSelectedDataTypeString(), (selectedInterval == "semiannual" ? true : false));
            tableWindow.setDataPoints(dataPoints);
            tableWindow.Visibility = Visibility.Visible;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tableWindow != null)
                tableWindow.Close();
        }
    }
}