using System;
using System.Collections.Generic;

namespace PIWeatherXamarinApp.Models
{

    public class PIDataJsonObject
    {
        public PIValues Items { get; set; }
    }

    public class PIValues : List<PIValue>
    {
    }

    public class PIValue
    {
        public object Value { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Good { get; set; }

        public string UnitsAbbreviation { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(UnitsAbbreviation) == false)
            {
                return Value.ToString() + " " + UnitsAbbreviation;
            }
            else
            {
                return Value.ToString();
            }
        }

    }
}