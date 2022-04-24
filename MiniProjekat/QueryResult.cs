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
    }
}
