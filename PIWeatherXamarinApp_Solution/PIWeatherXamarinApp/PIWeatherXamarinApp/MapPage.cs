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

            //For more infomration, please refer to https://developer.xamarin.com/guides/xamarin-forms/user-interface/map/

            //Get the object from the constructor's input
            this.city = selectedCity;

            //Create a position object using the properties of the City object.
            Position position = new Position(city.Latitude, city.Longitude);

            //The center of the map is the position of the city
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromMiles(40));
            var map = new Map(mapSpan);
            var pin = new Pin();
            pin.Label = city.Name;
            pin.Position = position;

            //Add a pin where the city is location with the name of the city
            map.Pins.Add(pin);
            Title = "Map";

            //It is important that  map.IsShowingUser = false; otherwise the map will try to get your current geolocation
            map.IsShowingUser = false;

            //The size of the map could be defined
            map.HeightRequest = 100;
            map.WidthRequest = 960;
            map.VerticalOptions = LayoutOptions.FillAndExpand;
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);

            //In Xamarin.Forms, the Content needs to be set on the Content property of the ContentPage
            Content = stack;
        }
    }
}


