using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjekat
{
    // Used https://json2csharp.com/ to generate class structure
    internal class QueryResult
    {
        public string name { get; set; }
        public string interval { get; set; }
        public string unit { get; set; }
        public List<DataPoint> data { get; set; }

        public QueryResult() { }

        public ChartValues<double> getValues()
        {
            ChartValues<double> values = new ChartValues<double>();
            foreach (DataPoint point in data)
            {
                // if it fails to parse, parsed_value is 0.
                double parsed_value = 0.0;
                Double.TryParse(point.value, out parsed_value);
                values.Add(parsed_value);
            }
            return values;
        }
    }
}
