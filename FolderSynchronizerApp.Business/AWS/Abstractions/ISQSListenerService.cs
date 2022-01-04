using System.Collections.Generic;

namespace FolderSynchronizerApp.Business.AWS.Abstractions
{
    public interface ISQSListenerService
    {
        IEnumerable<string> GetMessages();
    }
}