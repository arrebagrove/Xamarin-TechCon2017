using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PIWeatherXamarinApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace PIWeatherXamarinApp
{
    public class CustomWebClient
    {
        private string baseUrl = "https://pisrv01.pischool.int/piwebapi/";
        private async Task<string> DownloadWebData(string url)
        {
            var handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential("student01", "student", "pischool");
            HttpClient httpClient = new HttpClient(handler);
            HttpResponseMessage httpMessage = await httpClient.GetAsync(url);
            if (httpMessage.IsSuccessStatusCode == true)
            {
                using (var stream = await httpMessage.Content.ReadAsStreamAsync())
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        return await streamReader.ReadToEndAsync();
                    }
                }
            }
            return string.Empty;
        }

        private async Task<string> PostWebData(string url, HttpContent httpContent)
        {
            var handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential("student01", "student", "pischool");
            HttpClient httpClient = new HttpClient(handler);
            HttpResponseMessage httpMessage = await httpClient.PostAsync(url, httpContent);
            if (httpMessage.IsSuccessStatusCode == true)
            {
                using (var stream = await httpMessage.Content.ReadAsStreamAsync())
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        return await streamReader.ReadToEndAsync();
                    }
                }
            }
            return string.Empty;
        }



        public async Task<City> GetCityData(string elementWebId)
        {
            string url = baseUrl + "streamsets/" + elementWebId + "/value";
            string response = await DownloadWebData(url);
            PIListObject elementData = JsonConvert.DeserializeObject<PIListObject>(response);
            return new City(elementData);
        }

        private async Task<Cities> GetCitiesDataWithBatch()
        {
            //Exercise 5
            //Please refer to the following page for more information about Batch: https://pisrv01.pischool.int/piwebapi/help/controllers/batch/actions/execute
            Dictionary<string, PIBatchRequest> globalBatch = new Dictionary<string, PIBatchRequest>();

            //First internal request gets the WebId of the AF database Weather
            PIBatchRequest batchGetDbWebId = new PIBatchRequest()
            {
                Method = "GET",
                Resource = baseUrl + @"assetdatabases?path=\\pisrv01\Weather"
            };

            //Second internal request gets the WebId from all attributes "Wikipedia Thumbnail Url" (one per each city)
            //using the WebId returned from the first internal request
            PIBatchRequest batchGetElementsWebIds = new PIBatchRequest()
            {
                Method = "GET",
                Resource = baseUrl + "assetdatabases/{0}/elementattributes?attributeNameFilter=*Wikipedia%20Thumbnail%20Url*",
                Parameters = new List<string>() { "$.1.Content.WebId" },
                ParentIds = new List<string>() { "1" }
            };

            //Third internal request gets the current values from all attributes "Wikipedia Thumbnail Url" 
            //using the WebIds returned from the second internal request
            PIBatchRequest batchGetAttributesValues = new PIBatchRequest()
            {
                Method = "GET",
                RequestTemplate = new PIRequestTemplate()
                {
                    Resource = baseUrl + "streams/{0}/value"
                },
                Parameters = new List<string>() { "$.2.Content.Items[*].WebId" },
                ParentIds = new List<string>() { "2" }
            };


            //Add all internal requests to the dictionary and create a JSON response for the body of the HTTP request
            globalBatch.Add("1", batchGetDbWebId);
            globalBatch.Add("2", batchGetElementsWebIds);
            globalBatch.Add("3", batchGetAttributesValues);
            string json = JsonConvert.SerializeObject(globalBatch);

            //It is important to define that the format of the body message is JSON
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            string url = baseUrl + "/batch";

            //Start to make the HTTP request and wait for the response
            string response = await PostWebData(url, httpContent);

            //Deserialize the response to have access to the responses of each internal request
            Dictionary<string, PIBatchResponse> batchData = JsonConvert.DeserializeObject<Dictionary<string, PIBatchResponse>>(response);

            //Convert the string responses to PIListObject and PIValue[]
            PIListObject imageAttributes = JsonConvert.DeserializeObject<PIListObject>(batchData["2"].Content.ToString());
            JArray items = (JArray)(batchData["3"].Content["Items"]);
            PIValue[] piValues = new PIValue[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                piValues[i] = JsonConvert.DeserializeObject<PIValue>(items[i]["Content"].ToString());
            }

            //Using both PIListObjects as inputs, the Cities object is created.
            Cities cities = new Cities(imageAttributes, piValues);
            return cities;
        }


        private async Task<Cities> GetCitiesDataNoBatch()
        {
            //Exercise 1

            //First HTTP request against PI Web API to get the WebId of the AF database Weather
            //Generate the url for making the first HTTP request
            string url = baseUrl + @"assetdatabases?path=\\pisrv01\Weather";

            //Retrieve the string response from URL
            string response = await DownloadWebData(url);
            
            //JsonConvert.DeserializeObject from JSON.NET converts the JSON string into a PIObject.
            PIObject dbData = JsonConvert.DeserializeObject<PIObject>(response);

            //Second HTTP request against PI Web API to get the WebId from all attributes "Wikipedia Thumbnail Url" (one per each city).
            url = baseUrl + "assetdatabases/" + dbData.WebId + "/elementattributes?attributeNameFilter=*Wikipedia%20Thumbnail%20Url*";
            response = await DownloadWebData(url);

            //Third HTTP request against PI Web API to get the current values from all attributes "Wikipedia Thumbnail Url" using the WebId retrieved from the previous call.
            PIListObject imageAttributes = JsonConvert.DeserializeObject<PIListObject>(response);

            //Get the WebIds from the PIListObject
            List<string> webIds = imageAttributes.GetItemsWebIds();

            //Using the GetValuesAdHoc method from StreamSet Controller
            //For more information: https://pisrv01.pischool.int/piwebapi/help/controllers/streamset/actions/getvaluesadhoc
            url = baseUrl + "streamsets/value?";
            foreach (string webId in webIds)
            {
                url = url + string.Format("webId={0}&", webId);
            }
            url = url.Substring(0, url.Length - 1);


            response = await DownloadWebData(url);
            PIListObject imagesValues = JsonConvert.DeserializeObject<PIListObject>(response);

            //Using both PIListObjects as inputs, the Cities object is created.
            Cities cities = new Cities(imageAttributes, imagesValues);
            return cities;
        }

        public async Task<Cities> GetCitiesData(bool useBatch = false)
        {
            if (useBatch == false)
            {
                //For exercise 1
                return await GetCitiesDataNoBatch();
            }
            else
            {
                //For exercise 5
                return await GetCitiesDataWithBatch();
            }
        }


    }
}
