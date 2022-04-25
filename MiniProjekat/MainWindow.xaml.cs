﻿using Newtonsoft.Json;
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
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        public async Task getData(string data_type)
        {
            string QUERY_URL = "";
            if (data_type == CPI)
                QUERY_URL = $"https://www.alphavantage.co/query?function=CPI&interval={selectedInterval}&apikey={API_KEY}";
            else if (data_type == INF)
                QUERY_URL = $"https://www.alphavantage.co/query?function=INFLATION&apikey={API_KEY}";
            else if (data_type == CS)
                QUERY_URL = $"https://www.alphavantage.co/query?function=CONSUMER_SENTIMENT&apikey={API_KEY}"; 


            HttpClient client = new HttpClient();
            int retries = 0;
            while (retries < 5)
            {
                using (HttpResponseMessage response = await client.GetAsync(QUERY_URL))
                {
                    using (HttpContent content = response.Content)
                    {
                        QueryResult? queryResult;

                        string json = await content.ReadAsStringAsync();
                        queryResult = JsonConvert.DeserializeObject<QueryResult>(json);
                        if (queryResult.name != null)
                        {
                            retries += 1;
                            await Task.Delay(1000);
                            continue;
                        }
                        Console.WriteLine(queryResult);
                        return;
                        // if linechart
                        // forward data to linechart
                        // else
                        // forward data to candlechart
                    }
                }
            }
            // Show user that connection could not be established
            
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            tgl1.Checked += Tgl1_Checked;
            //tgl1.Unchecked += Tgl1_Unchecked;
            tgl2.Checked += Tgl2_Checked;
            //tgl2.Unchecked += Tgl2_Unchecked;
            tgl1.IsChecked = true;

            intervalTgl1.Checked += intervalTgl1_Checked;
            //intervalTgl1.Unchecked += intervalTgl1_Unchecked;
            intervalTgl2.Checked += intervalTgl2_Checked;
           // intervalTgl2.Unchecked += intervalTgl2_Unchecked;
            intervalTgl1.IsChecked = true;

            dataType.SelectionChanged += dataType_SelectionChanged;
            await getData(CPI);
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

        private void intervalTgl1_Checked(object sender, RoutedEventArgs e)
        {
            intervalTgl1.IsChecked = true;
        }

        private void intervalTgl2_Checked(object sender, RoutedEventArgs e)
        {
            intervalTgl1.IsChecked = false;
        }

        private void intervalTgl1_Unchecked(object sender, RoutedEventArgs e)
        {
            intervalTgl2.IsChecked = true;
        }

        private void intervalTgl2_Unchecked(object sender, RoutedEventArgs e)
        {
             intervalTgl2.IsChecked = false;
        }

        private void dataType_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)dataType.SelectedItem;
            string selectedDataType = typeItem.Content.ToString();
            getData(selectedDataType);
            if (selectedDataType == CPI)
                interval.Visibility = Visibility.Visible;
            else
                interval.Visibility = Visibility.Hidden;
        }

    }
}