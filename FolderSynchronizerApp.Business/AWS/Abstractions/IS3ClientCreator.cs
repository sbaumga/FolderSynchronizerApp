using Amazon.S3;

namespace FolderSynchronizerApp.Business.AWS.Abstractions
{
    public interface IS3ClientCreator
    {
        IAmazonS3 GetS3Client();
    }
}