using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MiniProjekat
{
    public class BarChart
    {
        public SeriesCollection seriesCollection;
        public List<string> dates { get; set; }

        public BarChart()
        {
            seriesCollection = new SeriesCollection();
            dates = new List<string>();
        }

        public void drawBars(ChartValues<double> values)
        {
            seriesCollection.Add(new ColumnSeries()
             {
                 Values = values,
                 Configuration = new CartesianMapper<double>()
                                    .Y(value => value)
                                    .Stroke(value => (value == values.Max()) ? Brushes.Red : (value == values.Min()) ? Brushes.Yellow : Brushes.BlueViolet)
                                    .Fill(value => (value == values.Max()) ? Brushes.Red : (value == values.Min()) ? Brushes.Yellow : Brushes.LightBlue),
             });
        }

        public void fillDates(List<DataPoint> datapoints)
        {
            foreach (DataPoint point in datapoints)
            {
                dates.Add(point.date);
            }
        }

        public void clear()
        {
            if (seriesCollection != null && seriesCollection.Chart != null)
                seriesCollection.Clear();
            dates.Clear();
        }
    }
}
