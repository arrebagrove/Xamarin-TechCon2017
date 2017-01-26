using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIWeatherXamarinApp.Models
{
    public class PIBatchResponse
    {
        public Object Status { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public JObject Content { get; set; }
    }
}
