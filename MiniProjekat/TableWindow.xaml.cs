﻿using System;
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
using System.Threading;

namespace MiniProjekat
{
    /// <summary>
    /// Interaction logic for TableWindow.xaml
    /// </summary>
    public partial class TableWindow : Window
    {

        List<DataPoint>? dataPoints = new List<DataPoint>();

        public TableWindow()
        {
            InitializeComponent();
        }

        public void setDataPoints(List<DataPoint> dataPoints)
        {
            this.dataPoints = dataPoints;
            updateTable();
        }

        public void updateTitle(string newTitle, bool newSemiannual)
        {
            string newTableTitle = newTitle;

            if (newTitle == "Consumer Price Index")
            {
                newTableTitle += newSemiannual ? " - Semi-annual" : " - Monthly";
            }
            TitleLabel.Content = newTableTitle;
        }

        private void updateTable()
        {
            
            if (dataPoints?.Count > 0)
            {
                if (TableXAML.Items.Count > 0)
                {
                    TableXAML.Items.Clear();
                }

                foreach (DataPoint dataPoint in dataPoints)
                {
                    TableXAML.Items.Add(dataPoint);
                }
            }
            
        }
    }
}
