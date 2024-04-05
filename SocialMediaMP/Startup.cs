using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialMediaMP.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;


namespace SocialMediaMP
{
    public class Startup
    {
        //private readonly IHostApplicationLifetime _applicationLifetime; // stuff to do when program ended , IHostApplicationLifetime applicationLifetime (goes in constructor->)_applicationLifetime = applicationLifetime;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
             
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthenticationCore();
            services.AddRazorPages();
            services.AddScoped<SocialNetwork>();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider,SessionAuthenticationProvider>();
            services.AddSingleton<WeatherForecastService>();
            services.AddTransient<UserService>();
            services.AddTransient<PostService>();
            services.AddBlazoredSessionStorage();
            services.AddScoped<SessionAuthenticationProvider>();
            
            //services.AddSingleton<IHostApplicationLifetime>(); // new _> used to SaveUpon closing application
            // services.AddSingleton<IHostApplicationLifetime, HostApplicationLifetime>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // enables authentication
            }

            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //_applicationLifetime.ApplicationStopping.Register(() =>
            //{
            //    This method will be called when the application is shutting down
            //    SocialNetwork sn = new SocialNetwork();
            //    sn.SaveConnections();
            //});
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
