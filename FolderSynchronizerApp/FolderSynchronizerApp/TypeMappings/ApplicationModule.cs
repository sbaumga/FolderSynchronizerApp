using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.Implementations;
using FolderSynchronizerApp.Services;
using Xamarin.Forms;

namespace FolderSynchronizerApp.TypeMappings
{
    public static class ApplicationModule
    {
        public static void Register()
        {
            // TODO: remove
            DependencyService.Register<MockDataStore>();
        }
    }
}