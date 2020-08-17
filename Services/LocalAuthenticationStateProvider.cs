using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MyTaskApp.ClassLibrary;
using MyTaskApp.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyTaskApp.Client.Services
{
   
    public class LocalAutenticationStateProvider : AuthenticationStateProvider
    {        
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILocalStorageService _storageService; 

    public LocalAutenticationStateProvider(ILocalStorageService storageService, IHttpClientFactory clientFactory)    
        { 
            _storageService = storageService;
            _clientFactory = clientFactory;
        }
        
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (await _storageService.ContainKeyAsync("user"))
            {
                var userInfo = await _storageService.GetItemAsync<UserModel>("user");
                var claims = new[]
                {                    
                    new Claim ("UserName", userInfo.UserName),
                    new Claim ("AccessToken", userInfo.AccessToken)
                };
                var identity = new ClaimsIdentity(claims, "BearerToken"); 
                var user = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(user); 
                NotifyAuthenticationStateChanged(Task.FromResult(state)); 
                return state;
            }
            return new AuthenticationState(new ClaimsPrincipal());
        }
        public async Task LogoutAsync()
        {
            await _storageService.RemoveItemAsync("user");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }



    }



}

