using Amazon.SQS;

namespace FolderSynchronizerApp.Business.AWS.Abstractions
{
    public interface ISQSClientCreator
    {
        IAmazonSQS CreateClient();
    }
}