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
            //Exercise 2
            this.city = selectedCity;
            Position position = new Position(city.Latitude, city.Longitude);

            MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromMiles(40));
            var map = new Map(mapSpan);
            var pin = new Pin();
            pin.Label = city.Name;
            pin.Position = position;
            map.Pins.Add(pin);
            Title = "Map";
            map.IsShowingUser = false;
            map.HeightRequest = 100;
            map.WidthRequest = 960;
            map.VerticalOptions = LayoutOptions.FillAndExpand;
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;
        }
    }
}


