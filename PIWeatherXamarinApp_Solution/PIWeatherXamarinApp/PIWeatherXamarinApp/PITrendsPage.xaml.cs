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
            Action actionDefault = () =>
            {
                Browser.Source = weatherData.GetCoresightUrl(false);
            };
            Action actionAndroid = () =>
            {
                Browser.Source = weatherData.GetCoresightUrl(true);
            };
            Device.OnPlatform(null, actionAndroid, actionDefault, actionDefault);
        }
    }
}
