using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MyTaskApp.ClassLibrary;
using MyTaskApp.Client.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyTaskApp.Client.Services
{
    public class AuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly AuthenticationStateProvider _autenticationStateProvide;
        private readonly ILocalStorageService _storageService;

        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration, AuthenticationStateProvider autenticationStateProvide, ILocalStorageService storageService)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _autenticationStateProvide = autenticationStateProvide;
            _storageService = storageService;
        }

        //string _baseURL = "https://localhost:44373/api/auth/"; 
        string _baseURL = "https://rafsmytaskappapi.azurewebsites.net/api/auth/"; 
        public async Task<string> LoginUserAsync(LoginModel model)
        {
            
            var client = _clientFactory.CreateClient();

            var response = await client.PostAsJsonAsync<LoginModel>($"{_baseURL}Login", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<UserResponse>();

                var userInfo = new UserModel()
                {
                    AccessToken = result.Message,
                };

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(result.Message);
                userInfo.UserName = token.Claims.First(c => c.Type == "UserName").Value;

                await _storageService.SetItemAsync("user", userInfo);

                await _autenticationStateProvide.GetAuthenticationStateAsync();
                return string.Empty;
            }
            else 
            {
                var result = await response.Content.ReadFromJsonAsync<UserResponse>();
                return result.Message;
            }


        }
        public async Task<UserResponse> RegisterUserAsync(RegisterModel model)
        {
            var client = _clientFactory.CreateClient();

            var response = await client.PostAsJsonAsync<RegisterModel>($"{_baseURL}Register", model);

            return await response.Content.ReadFromJsonAsync<UserResponse>();
        }
    }
}

