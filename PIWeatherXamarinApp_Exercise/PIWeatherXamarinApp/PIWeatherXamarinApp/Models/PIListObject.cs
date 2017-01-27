using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PIWeatherXamarinApp.Models
{


    public class PIListListObject
    {
        public List<PIListObject> Items { get; set; }
    }


    public class PIListObject
    {
        public List<PIObject> Items { get; set; }

        public List<string> GetItemsWebIds()
        {
            return Items.Select(m => m.WebId).ToList();
        }
    }


    public class PIObject
    {
        public string WebId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Links Links { get; set; }

        public PIValue Value { get; set; }
    }

    public class Links
    {
        public string Attributes { get; set; }
        public string Element { get; set; }
    }
}