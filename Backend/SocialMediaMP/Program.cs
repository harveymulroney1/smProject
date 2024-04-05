using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace SocialMediaMP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<ProtectedSessionStorage>();
            builder.Services.AddScoped<AuthenticationStateProvider,SessionAuthenticationProvider>();
            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddScoped<SessionAuthenticationProvider>();


        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
