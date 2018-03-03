using ElectronNet.ApiSettings;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;

namespace ElectronNet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddMvc(options =>
            {
                options.Filters.Add(new ExceptionFilter());
                //options.Filters.Add(new ActionFilter());
            });

            IConfigurationBuilder configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("apisettings.json");
            IConfigurationRoot configRoot = configuration.Build();
            services.Configure<FirstVersion>(configRoot);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/ErrorView");
            }

            app.UseStaticFiles();
            app.UseExceptionless("3zFPfqmQz23px7CLdgmQfCh3VqG2aX9eN6NPgdCk");
            app.UseMiddleware(typeof(HttpMiddleware));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            Task.Run(async () =>
            {
                var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
                {
                    Show = true,
                    Center = true,
                    Title = "租客"
                });
                browserWindow.OnReadyToShow += () => browserWindow.Show();
                browserWindow.SetTitle("租客");
                Electron.App.SetName("租客");
            });
        }
    }
}
