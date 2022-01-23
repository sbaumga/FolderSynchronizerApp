using FolderSynchronizerApp.Business.AWS.Data;
using System.Threading.Tasks;

namespace FolderSynchronizerApp.Business.AWS.Abstractions
{
    public interface IRepeatedSQSPollingService
    {
        Task<RepeatedPollResultData> PollUntilEmptyAsync();
    }
}