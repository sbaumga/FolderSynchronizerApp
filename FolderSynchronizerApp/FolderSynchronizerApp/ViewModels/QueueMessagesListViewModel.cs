using FolderSynchronizerApp.Business.AWS.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FolderSynchronizerApp.ViewModels
{
    public class QueueMessagesListViewModel : BaseViewModel
    {
        private ISQSPollingService QueueListener { get; }

        private IRepeatedSQSPollingService RepeatedPollingService { get; }

        public QueueMessagesListViewModel(ISQSPollingService awsQueueListenerService, IRepeatedSQSPollingService repeatedSQSPollingService)
        {
            QueueListener = awsQueueListenerService ?? throw new ArgumentNullException(nameof(awsQueueListenerService));
            RepeatedPollingService = repeatedSQSPollingService ?? throw new ArgumentNullException(nameof(repeatedSQSPollingService));

            Messages = new ObservableCollection<string>();

            PollCommand = new Command(async () => LoadMessages());
            PollAllCommand = new Command(async () => LoadAllMessages());
        }

        public ObservableCollection<string> Messages { get; private set; }

        private async Task LoadMessages()
        {
            try
            {
                Messages.Clear();

                var messagesFromQueue = await QueueListener.GetMessagesAsync();

                foreach (var message in messagesFromQueue)
                {
                    Messages.Add(message);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Command PollCommand { get; }

        private async Task LoadAllMessages()
        {
            try
            {
                Messages.Clear();

                var pollResult = await RepeatedPollingService.PollUntilEmptyAsync();
                var messagesFromQueue = pollResult.PollMessages;

                foreach (var message in messagesFromQueue)
                {
                    Messages.Add(message);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Command PollAllCommand { get; }
    }
}