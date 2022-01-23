using FolderSynchronizerApp.Business.Data;

namespace FolderSynchronizerApp.Business.Abstractions
{
    public interface IMediaQueryer
    {
        // TODO: use generic query result data
        AndroidFileQueryResultData Query(string filename);
    }
}