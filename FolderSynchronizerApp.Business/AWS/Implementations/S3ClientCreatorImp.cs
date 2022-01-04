using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.AWS.Abstractions;
using System;

namespace FolderSynchronizerApp.Business.AWS.Implementations
{
    public class S3ClientCreatorImp : IS3ClientCreator
    {
        private AWSCredentials AWSCredentials { get; }

        public S3ClientCreatorImp(IConfigManager configManager)
        {
            if (configManager == null)
            {
                throw new ArgumentNullException(nameof(configManager));
            }

            var configData = configManager.Load();

            AWSCredentials = new BasicAWSCredentials(configData.KeyConfigData.AccessKey, configData.KeyConfigData.SecretKey);
        }

        public IAmazonS3 GetS3Client()
        {
            try
            {
                var client = new AmazonS3Client(AWSCredentials, RegionEndpoint.CACentral1);
                return client;
            } catch (Exception ex)
            {
                throw;
            }
        }
    }
}