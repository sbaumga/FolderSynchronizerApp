using Amazon.SQS.Model;
using FolderSynchronizerApp.Business.AWS.Data;

namespace FolderSynchronizerApp.Business.AWS.Abstractions
{
    public interface ISQSAutomatedS3MessageDeserializationService
    {
        S3MessageData Deserialize(Message message);
    }
}