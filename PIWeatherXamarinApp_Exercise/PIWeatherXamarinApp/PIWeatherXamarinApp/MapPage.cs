using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Threading.Tasks;
using PIWeatherXamarinApp.Models;

namespace PIWeatherXamarinApp
{
    public class MapPage : ContentPage
    {
        private City city;

        public MapPage(City selectedCity)
        {
            Title = "Map";
            this.city = selectedCity;
            //Exercise 2       
            //HINT: https://developer.xamarin.com/guides/xamarin-forms/user-interface/map/
            //HINT: Use the Latitude and Longitude properties from City input
        }
    }
}
