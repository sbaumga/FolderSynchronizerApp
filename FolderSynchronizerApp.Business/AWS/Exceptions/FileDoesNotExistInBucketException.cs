using FolderSynchronizerApp.Business.Exceptions;

namespace FolderSynchronizerApp.Business.AWS.Exceptions
{
    public class FileDoesNotExistInBucketException : FolderSynchronizerAppException
    {
        public FileDoesNotExistInBucketException(string fileKey) : base($"\"{fileKey}\" does not exist in the specified s3 bucket.")
        {
        }
    }
}