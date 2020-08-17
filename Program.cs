using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MyTaskApp.Client.Services;
using System.IdentityModel.Tokens.Jwt;

namespace MyTaskApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddHttpClient(); 
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddOptions(); 
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<AuthenticationStateProvider, LocalAutenticationStateProvider>();
            builder.Services.AddTransient<IMyTaskServices, MyTaskServices>();

            await builder.Build().RunAsync();
            var host = builder.Build();
            _ = new JwtPayload();
        }
    }
}
