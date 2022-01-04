using Amazon.SQS.Model;

namespace FolderSynchronizerApp.Business.AWS.Abstractions
{
    public interface ISQSMessageDeleter
    {
        void Delete(Message message);
    }
}