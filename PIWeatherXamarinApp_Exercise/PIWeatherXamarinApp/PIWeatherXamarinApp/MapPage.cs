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
            
            this.city = selectedCity;
            //Exercise 2       
        }
    }
}
