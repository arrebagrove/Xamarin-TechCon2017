using PIWeatherXamarinApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PIWeatherXamarinApp
{
    public partial class PITrendsPage : ContentPage
    {
        private WeatherData weatherData;
        public PITrendsPage(WeatherData weatherData)
        {
            //Exercise 4          
            Title = string.Format("PI Coresight Trend - {0} - {1}", weatherData.CityName, weatherData.Property);
            this.weatherData = weatherData;
            InitializeComponent();

            //HINT: Call WatherData.GetCoresightUrl() to get the URL to be used on PI Coresight.
            //HINT: Call Device.OnPlatform to execute different actions for differnts platforms.
        }
    }
}
