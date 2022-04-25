using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjekat
{
    // Used https://json2csharp.com/ to generate class structure
    public class QueryResult
    {
        private const int MAX_NUM_DATAPOINTS = 200;
        public string name { get; set; }
        public string interval { get; set; }
        public string unit { get; set; }
        public List<DataPoint> data { get; set; }

        public QueryResult() { }

        public ChartValues<double> getValues()
        {
            ChartValues<double> values = new ChartValues<double>();
            if (data.Count > MAX_NUM_DATAPOINTS)
            {
                data = data.Take(MAX_NUM_DATAPOINTS).ToList();
            }
            data.Reverse();
            for (int i = 0; i < data.Count; i++) 
            {
                double parsed_value = 0.0;
                Double.TryParse(data[i].value, out parsed_value);
                values.Add(parsed_value);
            } 
            return values;
        }
    }
}
