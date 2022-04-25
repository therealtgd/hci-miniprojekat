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
    internal class LineChart
    {
        public SeriesCollection seriesCollection { get; set; }
        public List<string> dates { get; set; }

        public LineChart()
        {
            seriesCollection = new SeriesCollection();
            dates = new List<string>();
        }

        public void drawLine(ChartValues<double> values)
        {
            seriesCollection.Add(new LineSeries()
            {
                Values = values,
                Configuration = new CartesianMapper<double>()
                                    .Y(value => value)
                                    .Stroke(value => (value == values.Max()) ? Brushes.Red : (value == values.Min()) ? Brushes.Yellow : Brushes.Teal)
                                    .Fill(value => (value == values.Max()) ? Brushes.Red : (value == values.Min()) ? Brushes.Yellow : Brushes.Teal),
                PointGeometry = DefaultGeometries.Square,
                PointGeometrySize = 10,
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
            seriesCollection.Clear();
            dates.Clear();
        }

    }
}
