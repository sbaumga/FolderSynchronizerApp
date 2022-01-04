using Amazon.Runtime;
using Amazon.SQS;
using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.AWS.Abstractions;
using System;

namespace FolderSynchronizerApp.Business.AWS.Implementations
{
    public class SQSClientCreatorImp : ISQSClientCreator
    {
        private string AccessKey { get; }
        private string SecretKey { get; }

        public SQSClientCreatorImp(IConfigManager configManager)
        {
            if (configManager == null)
            {
                throw new ArgumentNullException(nameof(configManager));
            }

            var configData = configManager.Load();

            AccessKey = configData.KeyConfigData.AccessKey;
            SecretKey = configData.KeyConfigData.SecretKey;
        }

        public IAmazonSQS CreateClient()
        {
            var credentials = new BasicAWSCredentials(AccessKey, SecretKey);
            var sqsConfig = new AmazonSQSConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.CACentral1
            };

            var client = new AmazonSQSClient(credentials, sqsConfig);
            return client;
        }
    }
}