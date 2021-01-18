using System;
using System.Windows;
using AudioMonitor.Services;
using AudioMonitor.ViewModels;
using AudioMonitor.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TinyLittleMvvm;

namespace AudioMonitor {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        private readonly IHost host;

        public App() {
            host = new HostBuilder()
                .ConfigureAppConfiguration((context, configurationBuilder) => {
                    configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
                    configurationBuilder.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureServices((context, services) => {
                    ConfigureServices(services);
                })
                .ConfigureLogging(logging => {
                    logging.AddDebug();
                })
                .Build();
        }

        private async void Application_Startup(object sender, StartupEventArgs e) {
            await host.StartAsync();

            host.Services
                .GetRequiredService<IWindowManager>()
                .ShowWindow<MainViewModel>();
        }

        private async void Application_Exit(object sender, ExitEventArgs e) {
            await host.StopAsync(TimeSpan.FromSeconds(5));
            host.Dispose();
        }

        private void ConfigureServices(IServiceCollection services) {
            services.AddTinyLittleMvvm();

            services.AddSingleton<MainView>();
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<IRenderer, Renderer>();
        }
    }
}
