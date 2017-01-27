using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PIWeatherXamarinApp.Models
{
    class PIBatchRequest
    {
       
        [DataMember(Name = "Method", EmitDefaultValue = false)]
        public string Method { get; set; }

        [DataMember(Name = "Resource", EmitDefaultValue = false)]
        public string Resource { get; set; }
       
        [DataMember(Name = "RequestTemplate", EmitDefaultValue = false)]
        public PIRequestTemplate RequestTemplate { get; set; }
    
        [DataMember(Name = "Parameters", EmitDefaultValue = false)]
        public List<string> Parameters { get; set; }
       
        [DataMember(Name = "Headers", EmitDefaultValue = false)]
        public Dictionary<string, string> Headers { get; set; }


        [DataMember(Name = "Content", EmitDefaultValue = false)]
        public string Content { get; set; }

        [DataMember(Name = "ParentIds", EmitDefaultValue = false)]
        public List<string> ParentIds { get; set; }
     
    }

    public class PIRequestTemplate
    {
        [DataMember(Name = "Resource", EmitDefaultValue = false)]
        public string Resource { get; set; }
    }
}

