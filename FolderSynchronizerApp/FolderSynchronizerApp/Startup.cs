using FolderSynchronizerApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FolderSynchronizerApp
{
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            Business.Startup.ConfigureServices(services);

            ConfigureViewModels(services);
        }

        private static void ConfigureViewModels(IServiceCollection services)
        {
            services.AddTransient<ConfigurationViewModel>();
            services.AddTransient<QueueMessagesListViewModel>();
        }
    }
}