namespace FolderSynchronizerApp.Business.Exceptions
{
    public class FileSaveFailedException : FolderSynchronizerAppException
    {
        public FileSaveFailedException(string fileName) : base($"Unable to save file \"{fileName}\"")
        {
        }
    }
}