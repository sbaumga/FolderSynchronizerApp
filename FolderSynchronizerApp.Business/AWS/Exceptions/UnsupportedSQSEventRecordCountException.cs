using FolderSynchronizerApp.Business.Exceptions;

namespace FolderSynchronizerApp.Business.AWS.Exceptions
{
    public class UnsupportedSQSEventRecordCountException : FolderSynchronizerAppException
    {
        public UnsupportedSQSEventRecordCountException(int recordCount) : base($"Message from SQS contained {recordCount} records. Only one record per message is supported.")
        {
        }
    }
}