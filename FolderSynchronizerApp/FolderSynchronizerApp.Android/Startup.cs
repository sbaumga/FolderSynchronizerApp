using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Droid.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using Xamarin.Essentials;

namespace FolderSynchronizerApp.Droid
{
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void Init()
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(c =>
                {
                    // Tell the host configuration where to file the file (this is required for Xamarin apps)
                    c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });

                })
                .ConfigureServices((c, x) =>
                {
                    // Configure our local services and access the host configuration
                    ConfigureServices(c, x);
                })
                .ConfigureLogging(l => l.AddConsole(o =>
                {
                    //setup a console logger and disable colors since they don't have any colors in VS
                    o.DisableColors = true;
                }))
                .Build();

            //Save our service provider so we can use it later.
            ServiceProvider = host.Services;
            FolderSynchronizerApp.Startup.SetServiceProvider(ServiceProvider);
        }

        public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            FolderSynchronizerApp.Startup.ConfigureServices(services);

            services.AddTransient<IInternetChecker, AndroidInternetCheckerImp>();
            services.AddTransient<IMediaSaver, MediaSaverImp>();
            services.AddTransient<IMediaQueryer, MediaQueryerImp>();
            services.AddTransient<IMediaDeleter, MediaDeleterImp>();
        }
    }
}