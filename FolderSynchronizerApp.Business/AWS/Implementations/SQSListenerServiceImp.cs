using Amazon.SQS.Model;
using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.AWS.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FolderSynchronizerApp.Business.AWS.Implementations
{
    public class SQSListenerServiceImp : ISQSListenerService
    {
        private string SQSUrl { get; }

        private ISQSClientCreator SQSClientCreator { get; }

        private ISQSMessageConsumerService MessageConsumer { get; }

        public SQSListenerServiceImp(IConfigManager configManager, ISQSClientCreator sqsClientCreator, ISQSMessageConsumerService messageConsumer)
        {
            if (configManager == null)
            {
                throw new ArgumentNullException(nameof(configManager));
            }

            if (sqsClientCreator == null)
            {
                throw new ArgumentNullException(nameof(sqsClientCreator));
            }

            if (messageConsumer == null)
            {
                throw new ArgumentNullException(nameof(messageConsumer));
            }

            var configData = configManager.Load();
            SQSUrl = configData.SQSConfigData.Url;

            SQSClientCreator = sqsClientCreator;

            MessageConsumer = messageConsumer;
        }

        public IEnumerable<string> GetMessages()
        {
            var client = SQSClientCreator.CreateClient();
            var request = CreateRequest();

            // await doesn't work for some reason...
            var response = client.ReceiveMessageAsync(request).Result;

            var messageData = MessageConsumer.ConsumeMessages(response.Messages);

            var result = messageData.Select(d => $"{d.Key} {(d.Action == Enums.S3Action.Upload ? "Downloaded" : "Deleted")}");
            return result;
        }

        private ReceiveMessageRequest CreateRequest()
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = SQSUrl,
                MaxNumberOfMessages = 10, 
                WaitTimeSeconds = 5,
            };
            return request;
        }
    }
}