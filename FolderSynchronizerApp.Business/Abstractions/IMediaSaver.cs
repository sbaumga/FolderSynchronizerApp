namespace FolderSynchronizerApp.Business.Abstractions
{
    public interface IMediaSaver
    {
        void Save(string fileName, byte[] fileContents, string mimeType);
    }
}