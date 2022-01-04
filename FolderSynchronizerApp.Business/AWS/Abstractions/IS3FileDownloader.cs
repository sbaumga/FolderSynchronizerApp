namespace FolderSynchronizerApp.Business.AWS.Abstractions
{
    public interface IS3FileDownloader
    {
        void DownloadFile(string fileKey);
    }
}