using FolderSynchronizerApp.Business.AWS.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FolderSynchronizerApp.ViewModels
{
    public class QueueMessagesListViewModel : BaseViewModel
    {
        private ISQSListenerService QueueListener { get; }

        public QueueMessagesListViewModel(ISQSListenerService awsQueueListenerService)
        {
            QueueListener = awsQueueListenerService ?? throw new ArgumentNullException(nameof(awsQueueListenerService));

            Messages = new ObservableCollection<string>();            

            PollCommand = new Command(async () => await Task.Run(async () => LoadMessages()));
        }

        public ObservableCollection<string> Messages { get; private set; }

        public void LoadMessages()
        {
            try
            {
                Messages.Clear();

                var messagesFromQueue = QueueListener.GetMessages();

                foreach (var message in messagesFromQueue)
                {
                    Messages.Add(message);
                }
            } catch (Exception ex)
            {
                throw;
            }
        }

        public Command PollCommand { get; }
    }
}