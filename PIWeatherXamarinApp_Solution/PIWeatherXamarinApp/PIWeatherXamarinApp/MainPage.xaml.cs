using PIWeatherXamarinApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PIWeatherXamarinApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Cities cities = null;
            Task t = Task.Run(async () =>
            {
                CustomWebClient webClient = new CustomWebClient();
                cities = await webClient.GetCitiesData();
            });
            t.Wait();
            listView.ItemsSource = cities;
            ImageCircle.Forms.Plugin.Abstractions.CircleImage myImage = null;
        }

        public async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            City city = ((ListView)sender).SelectedItem as City;
            await Navigation.PushAsync(new CityDetailsPage(city));
        }
    }
}
