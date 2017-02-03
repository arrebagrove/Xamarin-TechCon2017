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
                //Browser.Source set the url of the WebView
                //The credentials won't be added to the PI Coresight URL
                Browser.Source = weatherData.GetCoresightUrl(false);
            };
            Action actionAndroid = () =>
            {
                //The credentials will be added to the PI Coresight URL
                Browser.Source = weatherData.GetCoresightUrl(true);
            };
            //Device.OnPlatform allow you to run different Actions for different platforms. For Android and Windows, 
            // the input for weatherData.GetCoresightUrl method would be different
            Device.OnPlatform(null, actionAndroid, actionDefault, actionDefault);
        }
    }
}
