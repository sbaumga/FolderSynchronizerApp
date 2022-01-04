using Amazon.SQS.Model;
using FolderSynchronizerApp.Business.AWS.Data;
using System.Collections.Generic;

namespace FolderSynchronizerApp.Business.AWS.Abstractions
{
    public interface ISQSMessageConsumerService
    {
        IEnumerable<S3MessageData> ConsumeMessages(IList<Message> messages);
    }
}