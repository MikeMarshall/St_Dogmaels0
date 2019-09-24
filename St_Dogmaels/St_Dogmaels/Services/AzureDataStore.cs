using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using St_Dogmaels.Models;
using Xamarin.Forms;

namespace St_Dogmaels.Services
{
    class AzureDataStore : IDataStore<Place>
    {
        private static AzureDataStore singleton = null;
        public static AzureDataStore Store { get { if (singleton == null) singleton = new AzureDataStore(); return singleton; } }
        private AzureDataStore() { }

        private Places cache;
        private HttpClient client = new HttpClient();

        public async Task<bool> AddItemAsync(Place item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        private async Task SetCache()
        {
            cache = new Places();
            client.Timeout = TimeSpan.FromSeconds(30);
            var response = await client.GetAsync("https://moylgrove-history.azurewebsites.net/api/places");
            if (response.IsSuccessStatusCode)
            {
                var js = await response.Content.ReadAsStringAsync();
                cache.AddJson(js);
                DependencyService.Get<ILocalFile>().WriteAll("houseCache.js", js);
            }
            else
            {
                var fileString = DependencyService.Get<ILocalFile>().ReadAll("houseCache.js");
                if (String.IsNullOrWhiteSpace(fileString))
                {
                    cache.AddJson(fileString);
                }
            }
        }

        public async Task<Place> GetItemAsync(string id)
        {
            if (cache == null) await SetCache();

            if (!cache.KeyDict.ContainsKey(id))
            {
                var js = await client.GetStringAsync("https://moylgrove-history.azurewebsites.net/api/place?id=" + Place.Row(id));
                cache.Add1Json(js);
            }
            return cache.KeyDict[id];
        }

        public async Task<IEnumerable<Place>> GetItemsAsync(bool forceRefresh = false)
        {
            if (cache == null || forceRefresh)
            {
                await SetCache();
            }
            return new List<Place>(cache);
        }

        public Task<bool> UpdateItemAsync(Place item)
        {
            throw new NotImplementedException();
        }
    }
    class Places : IEnumerable<Place>
    {
        public Dictionary<String, Place> KeyDict { get; set; }
        public Places()
        {
            KeyDict = new Dictionary<string, Place>();
        }
        public Places AddJson(string js)
        {
            var placeList = JsonConvert.DeserializeObject<List<Place>>(js);
            foreach (var place in placeList)
            {
                KeyDict[place.Id] = place;
            }
            return this;
        }
        public Place Add1Json(string js)
        {
            var place = JsonConvert.DeserializeObject<Place>(js);
            KeyDict[place.Id] = place;
            return place;
        }
        public string Json()
        {
            return JsonConvert.SerializeObject(new List<Place>(KeyDict.Values));
        }

        IEnumerator<Place> IEnumerable<Place>.GetEnumerator()
        {
            return KeyDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return KeyDict.Values.GetEnumerator();
        }

    }

}
