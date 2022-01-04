using Amazon.SQS.Model;
using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.AWS.Abstractions;
using System;

namespace FolderSynchronizerApp.Business.AWS.Implementations
{
    public class SQSMessageDeleterImp : ISQSMessageDeleter
    {
        private string SQSUrl { get; }

        private ISQSClientCreator SQSClientCreator { get; }

        public SQSMessageDeleterImp(IConfigManager configManager, ISQSClientCreator sqsClientCreator)
        {
            if (configManager == null)
            {
                throw new ArgumentNullException(nameof(configManager));
            }

            if (sqsClientCreator == null)
            {
                throw new ArgumentNullException(nameof(sqsClientCreator));
            }

            var configData = configManager.Load();
            SQSUrl = configData.SQSConfigData.Url;

            SQSClientCreator = sqsClientCreator;
        }

        public void Delete(Message message)
        {
            var client = SQSClientCreator.CreateClient();

            var request = CreateDeleteMessageRequest(message);

            var result = client.DeleteMessageAsync(request).Result;
        }

        private DeleteMessageRequest CreateDeleteMessageRequest(Message message)
        {
            var request = new DeleteMessageRequest
            {
                QueueUrl = SQSUrl,
                ReceiptHandle = message.ReceiptHandle
            };

            return request;
        }
    }
}