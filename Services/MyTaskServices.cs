using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using MyTaskApp.ClassLibrary;
using System.Net;
using Blazored.LocalStorage;
using System.Net.Http.Headers;
using MyTaskApp.Client.Models;
using Newtonsoft.Json;
using System.Text;

namespace MyTaskApp.Client.Services
{
    public class MyTaskServices : IMyTaskServices
    {
        private readonly IHttpClientFactory _client;
        //private readonly string _baseURL = "https://localhost:44373/api/task";
        private readonly string _baseURL = "https://rafsmytaskappapi.azurewebsites.net/api/task";
        private readonly ILocalStorageService _local;
 
 
        
        public MyTaskServices(IHttpClientFactory client, ILocalStorageService local)
        {
            _client = client;
            _local = local;
        }


        public async Task<List<SingleTaskModel>> GetTasks()
        {
            var userInfo = await _local.GetItemAsync<UserModel>("user");
            var _token = userInfo.AccessToken;

            var request = new HttpRequestMessage(HttpMethod.Get, _baseURL);
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token);
            var client = _client.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<SingleTaskModel>>();
            }
            else { return new List<SingleTaskModel>(); }
        }

        public async Task<string> PostMyTask(SingleTaskModel model)
        {
            var userInfo = await _local.GetItemAsync<UserModel>("user");
            var _token = userInfo.AccessToken;

           
            var request = new HttpRequestMessage(HttpMethod.Post, _baseURL);
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token);
            var content = JsonConvert.SerializeObject(model);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var client = _client.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            return response.ReasonPhrase;

        }

        public async Task<string> UpdateItem(SingleTaskModel model)
        {
            var userInfo = await _local.GetItemAsync<UserModel>("user");
            var _token = userInfo.AccessToken;

            var request = new HttpRequestMessage(HttpMethod.Put, _baseURL);
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token);
            var content = JsonConvert.SerializeObject(model);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var client = _client.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            return response.ReasonPhrase;
        }

        public async Task<List<SingleTaskModel>> DeleteTasks (List<SingleTaskModel> models)
        { 
            var userInfo = await _local.GetItemAsync<UserModel>("user");
            var _token = userInfo.AccessToken;
            var result = new List<SingleTaskModel>();
            foreach (var item in models)
            {             
                if (item.IsDone)
                {
                    var request = new HttpRequestMessage(HttpMethod.Delete, _baseURL);
                    request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token);

                    var content = JsonConvert.SerializeObject(item);
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                    var client = _client.CreateClient();
                    HttpResponseMessage response = await client.SendAsync(request);
                }
                else
                {
                    result.Add(item);
                }
            }
            return result;
        }
            
    }

}
