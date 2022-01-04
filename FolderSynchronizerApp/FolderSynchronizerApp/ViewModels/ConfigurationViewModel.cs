using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.AWS.Data;
using FolderSynchronizerApp.Utilities.Helpers;
using System;
using Xamarin.Forms;

namespace FolderSynchronizerApp.ViewModels
{
    public class ConfigurationViewModel : BaseViewModel
    {
        private string accessKey;

        public string AccessKey
        {
            get => accessKey;
            set => SetProperty(ref accessKey, value);
        }

        private string secretKey;

        public string SecretKey
        {
            get => secretKey;
            set => SetProperty(ref secretKey, value);
        }

        private string queueUrl;

        public string QueueUrl
        {
            get => queueUrl;
            set => SetProperty(ref queueUrl, value);
        }

        private string bucketName;

        public string BucketName
        {
            get => bucketName;
            set => SetProperty(ref bucketName, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public IConfigManager ConfigManager { get; }

        public ConfigurationViewModel(IConfigManager configManager)
        {
            ConfigManager = configManager ?? throw new ArgumentNullException(nameof(configManager));

            SaveCommand = new Command(OnSave, ValidateSave);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            CancelCommand = new Command(Reload);

            Reload();
        }

        public void SetUpPopups(Action<string, string, string> popupAction)
        {
            PopupAction = popupAction;
        }

        private Action<string, string, string> PopupAction { get; set; }

        private void SendPopup(string title, string message, string cancel)
        {
            PopupAction(title, message, cancel);
        }

        private void OnSave()
        {
            var configData = CreateAndPopulateConfigData();
            ConfigManager.Save(configData);

            SendPopup("Configuration Saved", null, "OK");

            Reload();
        }

        private AWSConfigData CreateAndPopulateConfigData()
        {
            var configData = new AWSConfigData
            {
                KeyConfigData = new AWSKeyConfigData
                {
                    AccessKey = AccessKey,
                    SecretKey = SecretKey
                },
                SQSConfigData = new AWSSQSConfigData
                {
                    Url = QueueUrl,
                },
                S3ConfigData = new AWSS3ConfigData
                {
                    BucketName = BucketName,
                }
            };

            return configData;
        }

        private bool ValidateSave()
        {
            var result = !StringHelper.AnyAreNullOrEmpty(accessKey, secretKey, queueUrl);
            return result;
        }

        private void Reload()
        {
            var data = ConfigManager.Load();

            AccessKey = data.KeyConfigData.AccessKey;
            SecretKey = data.KeyConfigData.SecretKey;

            QueueUrl = data.SQSConfigData.Url;

            BucketName = data.S3ConfigData.BucketName;
        }
    }
}