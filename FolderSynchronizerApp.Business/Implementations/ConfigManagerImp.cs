using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.AWS.Data;
using System;
using System.IO;

namespace FolderSynchronizerApp.Business.Implementations
{
    public class ConfigManagerImp : IConfigManager
    {
        private ISerializationService SerializationService { get; }

        public ConfigManagerImp(ISerializationService serializationService)
        {
            SerializationService = serializationService ?? throw new ArgumentNullException(nameof(serializationService));
        }

        public void Save(AWSConfigData data)
        {
            var serializedData = SerializationService.Serialize(data);

            File.WriteAllText(ConfigFilePath, serializedData);
        }

        private string ConfigFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FolderSynchronizerConfig.json");

        public AWSConfigData Load()
        {
            if (File.Exists(ConfigFilePath))
            {
                return LoadFromFile();
            } else
            {
                return NewConfigData();
            }
        }

        private AWSConfigData LoadFromFile()
        {
            var fileText = File.ReadAllText(ConfigFilePath);

            var data = SerializationService.Deserialize<AWSConfigData>(fileText);
            ConstructNewObjectsForNullReferences(data);

            return data;
        }

        private void ConstructNewObjectsForNullReferences(AWSConfigData configData)
        {
            configData.KeyConfigData = configData.KeyConfigData ?? new AWSKeyConfigData();
            configData.SQSConfigData = configData.SQSConfigData ?? new AWSSQSConfigData();
            configData.S3ConfigData = configData.S3ConfigData ?? new AWSS3ConfigData();
        }

        private AWSConfigData NewConfigData()
        {
            return new AWSConfigData()
            {
                KeyConfigData = new AWSKeyConfigData(),
                SQSConfigData = new AWSSQSConfigData(),
                S3ConfigData = new AWSS3ConfigData(),
            };
        }
    }
}