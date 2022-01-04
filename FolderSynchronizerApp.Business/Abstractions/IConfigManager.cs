using FolderSynchronizerApp.Business.AWS.Data;

namespace FolderSynchronizerApp.Business.Abstractions
{
    public interface IConfigManager
    {
        AWSConfigData Load();
        void Save(AWSConfigData data);
    }
}