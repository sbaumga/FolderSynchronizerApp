using Amazon.SQS.Model;
using FolderSynchronizerApp.Business.Exceptions;
using System;

namespace FolderSynchronizerApp.Business.AWS.Exceptions
{
    public class NotAutomatedMessageException : FolderSynchronizerAppException
    {
        public NotAutomatedMessageException(Message message, Exception innerException) :
            base($"The following message is not an automated message: {message.Body}", innerException)
        { }
    }
}