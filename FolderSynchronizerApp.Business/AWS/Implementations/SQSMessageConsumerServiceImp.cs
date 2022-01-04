using Amazon.SQS.Model;
using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.AWS.Abstractions;
using FolderSynchronizerApp.Business.AWS.Data;
using FolderSynchronizerApp.Business.AWS.Enums;
using FolderSynchronizerApp.Business.AWS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSynchronizerApp.Business.AWS.Implementations
{
    public class SQSMessageConsumerServiceImp : ISQSMessageConsumerService
    {
        private ISQSAutomatedS3MessageDeserializationService MessageDeserializationService { get; }
        private ISQSMessageDeleter MessageDeleter { get; }
        private IS3FileDownloader FileDownloader { get; }

        public SQSMessageConsumerServiceImp(ISQSAutomatedS3MessageDeserializationService messageDeserializationService, ISQSMessageDeleter messageDeleter, IS3FileDownloader fileDownloader)
        {
            MessageDeserializationService = messageDeserializationService ?? throw new ArgumentNullException(nameof(messageDeserializationService));
            MessageDeleter = messageDeleter ?? throw new ArgumentNullException(nameof(messageDeleter));
            FileDownloader = fileDownloader ?? throw new ArgumentNullException(nameof(fileDownloader));
        }

        public IEnumerable<S3MessageData> ConsumeMessages(IList<Message> messages)
        {
            var messageData = new List<S3MessageData>();

            foreach (var message in messages)
            {
                var data = ConsumeMessage(message);
                if (data != null)
                {
                    messageData.Add(data);
                }
            }

            return messageData;
        }

        private S3MessageData ConsumeMessage(Message message)
        {
            S3MessageData messageData = null;
            try
            {
                messageData = MessageDeserializationService.Deserialize(message);
                TakeActionFromMessage(messageData);
            } catch (NotAutomatedMessageException)
            {
                // If not an automated message, can't do anything with it,
                // but we'll delete it so we don't have to deal with it again later.
            }

            MessageDeleter.Delete(message);

            return messageData;
        }

        private void TakeActionFromMessage(S3MessageData data)
        {
            switch (data.Action)
            {
                case S3Action.Upload:
                    DownloadFile(data.Key);
                    break;
                case S3Action.Deletion:
                    // TODO:
                    break;
                default:
                    throw new NotImplementedException($"Unsupported {nameof(S3Action)} of {data.Action}");
            }
        }

        private void DownloadFile(string fileKey)
        {
            try
            {
                FileDownloader.DownloadFile(fileKey);
            }
            catch (FileDoesNotExistInBucketException ex)
            {
                // File doesn't exist in bucket, can't download so just continue
            }
        }
    }
}
