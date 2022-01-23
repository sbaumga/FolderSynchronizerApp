using System.Collections.Generic;
using System.Threading.Tasks;

namespace FolderSynchronizerApp.Business.AWS.Abstractions
{
    public interface ISQSPollingService
    {
        Task<IEnumerable<string>> GetMessagesAsync();
    }
}