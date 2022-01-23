using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.AWS.Abstractions;
using FolderSynchronizerApp.Business.AWS.Implementations;
using FolderSynchronizerApp.Business.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace FolderSynchronizerApp.Business
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ISerializationService, JSONSerializationServiceImp>();
            services.AddTransient<IJsonSerializationService, JSONSerializationServiceImp>();
            services.AddTransient<IConfigManager, ConfigManagerImp>();

            ConfigureAWS(services);
        }

        private static void ConfigureAWS(IServiceCollection services)
        {
            ConfigureSQS(services);
            ConfigureS3(services);
        }

        private static void ConfigureSQS(IServiceCollection services)
        {
            services.AddTransient<ISQSPollingService, SQSPollingServiceImp>();
            services.AddTransient<IRepeatedSQSPollingService, RepeatedSQSPollingServiceImp>();
            services.AddTransient<ISQSClientCreator, SQSClientCreatorImp>();

            services.AddTransient<ISQSMessageConsumerService, SQSMessageConsumerServiceImp>();
            services.AddTransient<ISQSAutomatedS3MessageDeserializationService, SQSAutomatedS3MessageDeserializationServiceImp>();
            services.AddTransient<ISQSMessageDeleter, SQSMessageDeleterImp>();
        }

        private static void ConfigureS3(IServiceCollection services)
        {
            services.AddTransient<IS3ClientCreator, S3ClientCreatorImp>();
            services.AddTransient<IS3FileDownloader, S3FileDownloaderImp>();
        }
    }
}