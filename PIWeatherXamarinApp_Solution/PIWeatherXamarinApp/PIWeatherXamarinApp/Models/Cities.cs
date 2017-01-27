using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PIWeatherXamarinApp.Models
{
    public class Cities : List<City>
    {
        private PIListObject imageAttributes;
        private PIValue[] piValues;

        public Cities(PIListObject imageAttributes, PIValue[] piValues)
        {
            for (int i = 0; i < imageAttributes.Items.Count; i++)
            {
                PIObject attribute = imageAttributes.Items[i];
                this.Add(new City(attribute, piValues[i]));

            }
            this.imageAttributes = imageAttributes;
            this.piValues = piValues;
        }

        public Cities(PIListObject imageAttributes, PIListObject imagesValues)
        {
            foreach (var imageAttributeItem in imageAttributes.Items)
            {
                var imageValueItem = imagesValues.Items.Where(iv => iv.WebId == imageAttributeItem.WebId).First();
                this.Add(new City(imageAttributeItem, imageValueItem));
            }


        }
    }


    public class City
    {

        public City(PIObject imageAttributeItem, PIValue piValue)
        {
            ImageUrl = piValue.Value.ToString();
            ElementWebId = imageAttributeItem.Links.Element.Substring(64);
            Name = imageAttributeItem.Path.Substring(25, imageAttributeItem.Path.Length - 49);
        }

        public City(PIObject imageAttributeItem, PIObject imageValueItem)
        {
            ImageUrl = imageValueItem.Value.Value.ToString();
            ElementWebId = imageAttributeItem.Links.Element.Substring(64);
            Name = imageAttributeItem.Path.Substring(25, imageAttributeItem.Path.Length - 49);
        }

        private PIListObject elementData = null;

        public string GetAttributeWebId(string attributeName)
        {

            var attribute = elementData.Items.Where(m => m.Name == attributeName).FirstOrDefault();
            if (attribute != null)
            {
                return attribute.WebId;
            }
            else
            {
                return string.Empty;
            }
        }

        public City(PIListObject elementData)
        {
            this.elementData = elementData;
            if (elementData != null)
            {
                Latitude = System.Convert.ToDouble(elementData.Items.Where(m => m.Name == "Latitude").First().Value.Value.ToString());
                Longitude = System.Convert.ToDouble(elementData.Items.Where(m => m.Name == "Longitude").First().Value.Value.ToString());
                WikipediaDescription = elementData.Items.Where(m => m.Name == "Wikipedia Description").First().Value.Value.ToString();
                WikipediaUrl = "https://en.wikipedia.org/wiki/" + elementData.Items.Where(m => m.Name == "Wikipedia Title").First().Value.Value.ToString().Replace(" ", "_");
                ImageUrl = elementData.Items.Where(m => m.Name == "Wikipedia Thumbnail Url").First().Value.Value.ToString();

                WindDirection = elementData.Items.Where(m => m.Name == "Wind Direction").First().Value.ToString();
                WeatherDescription = elementData.Items.Where(m => m.Name == "Weather Description").First().Value.ToString();
                Humidity = elementData.Items.Where(m => m.Name == "Humidity").First().Value.ToString();
                CloudCover = elementData.Items.Where(m => m.Name == "Cloud Cover").First().Value.ToString();
                Temperature = elementData.Items.Where(m => m.Name == "Temperature").First().Value.ToString();
                Visibility = elementData.Items.Where(m => m.Name == "Visibility").First().Value.ToString();
                WindSpeed = elementData.Items.Where(m => m.Name == "Wind Speed").First().Value.ToString();
                WeatherIconUrl = elementData.Items.Where(m => m.Name == "Weather Icon Url").First().Value.Value.ToString();
            }

        }



        public string Name { get; set; }
        public string WeatherIconUrl { get; set; }
        public string WindSpeed { get; set; }
        public string Visibility { get; set; }
        public string Temperature { get; set; }
        public string CloudCover { get; set; }
        public string Humidity { get; set; }
        public string WeatherDescription { get; set; }
        public string WindDirection { get; set; }

        private string imageUrl = string.Empty;
        private PIObject attribute;
        private PIValue pIValue;

        public string ImageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(imageUrl) == false)
                {
                    return imageUrl;
                }
                else
                {
                    return "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e0/City_icon_(Noun_Project).svg/2000px-City_icon_(Noun_Project).svg.png";
                }
            }
            set
            {
                imageUrl = value;
            }
        }

        public string ElementWebId { get; set; }
        public string WikipediaUrl { get; set; }

        public string WikipediaDescription { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }

}
