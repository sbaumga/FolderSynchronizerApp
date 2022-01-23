using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.AWS.Abstractions;
using FolderSynchronizerApp.Business.AWS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FolderSynchronizerApp.Business.AWS.Implementations
{
    public class RepeatedSQSPollingServiceImp : IRepeatedSQSPollingService
    {
        private ISQSPollingService PollingService { get; }

        private IInternetChecker InternetChecker { get; }

        public RepeatedSQSPollingServiceImp(ISQSPollingService pollingService, IInternetChecker internetChecker)
        {
            PollingService = pollingService ?? throw new ArgumentNullException(nameof(pollingService));
            InternetChecker = internetChecker ?? throw new ArgumentNullException(nameof(internetChecker));
        }

        public async Task<RepeatedPollResultData> PollUntilEmptyAsync()
        {
            var result = new RepeatedPollResultData { PollMessages = Enumerable.Empty<string>() };

            IEnumerable<string> messages;
            do
            {
                if (!InternetChecker.HasWifiConnection())
                {
                    result.ErrorMessage = "Wifi not detected. Polling has stopped";
                    return result;
                }

                messages = await PollingService.GetMessagesAsync();

                result.PollMessages = result.PollMessages.Concat(messages);
            } while (messages.Any());

            return result;
        }
    }
}