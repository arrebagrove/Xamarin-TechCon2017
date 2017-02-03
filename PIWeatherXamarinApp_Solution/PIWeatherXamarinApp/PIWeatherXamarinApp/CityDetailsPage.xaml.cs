using PIWeatherXamarinApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PIWeatherXamarinApp
{
    public partial class CityDetailsPage : ContentPage
    {
        public CityDetailsPage()
        {
            InitializeComponent();
        }


        public City SelectedCity { get; set; }
        public CityDetailsPage(City city)
        {

            List<WeatherData> data = new List<WeatherData>();
            string name = city.Name;
            CustomWebClient customWebClient = new CustomWebClient();
            Task t = Task.Run(async () =>
            {
                SelectedCity = await customWebClient.GetCityData(city.ElementWebId);
                SelectedCity.Name = name;
                Title = name;
                data.Add(new WeatherData("Visibility", SelectedCity.Visibility.ToString(), name));
                data.Add(new WeatherData("Cloud Cover", SelectedCity.CloudCover.ToString(), name));
                data.Add(new WeatherData("Temperature", SelectedCity.Temperature.ToString(), name));
                data.Add(new WeatherData("Wind Speed", SelectedCity.WindSpeed.ToString(), name));
                data.Add(new WeatherData("Humidity", SelectedCity.Humidity.ToString(), name));

            });
            t.Wait();
            BindingContext = SelectedCity;
            InitializeComponent();

            //Exercise3

            //The ItemsSource of the ListView allows you to define the source of the ListView data.
            WeatherListView.ItemsSource = data;

        }

        public async void OnViewMapBtnClicked(object sender, EventArgs e)
        {
            //Exercise 2   
            //Navigation.PushAsync is used to change the page shown on the screen
            await Navigation.PushAsync(new MapPage(SelectedCity));
        }

        public async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            //Exercise 4
            WeatherData weatherData = ((ListView)sender).SelectedItem as WeatherData;
            await Navigation.PushAsync(new PITrendsPage(weatherData));
        }
    }
}
