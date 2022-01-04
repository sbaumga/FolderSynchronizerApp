using System;

namespace FolderSynchronizerApp.Business.Exceptions
{
    public class FolderSynchronizerAppException : Exception
    {
        public FolderSynchronizerAppException(string message) : base(message)
        {
        }

        public FolderSynchronizerAppException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}