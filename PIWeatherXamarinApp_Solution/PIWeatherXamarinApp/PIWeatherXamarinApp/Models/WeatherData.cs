﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PIWeatherXamarinApp.Models
{
    public class WeatherData
    {
        public string Property { get; set; }
        public string Value { get; set; }

        public string CityName { get; set; }

        public WeatherData(string property, string value, string cityName)
        {
            this.Property = property;
            this.Value = value;
            this.CityName = cityName;
        }

        internal string GetCoresightUrl(bool useCredentialsOnUrl)
        {
            string credentials = useCredentialsOnUrl == true ? @"student01:student@" : string.Empty;
            string url = "https://" + credentials + @"pisrv01.pischool.int/Coresight/#/Displays/AdHoc?DataItems=\\PISRV01\Weather\" + System.Net.WebUtility.UrlEncode(CityName) + " | " + System.Net.WebUtility.UrlEncode(Property) + " &mode=kiosk";
            return url;
        }
    }
}
