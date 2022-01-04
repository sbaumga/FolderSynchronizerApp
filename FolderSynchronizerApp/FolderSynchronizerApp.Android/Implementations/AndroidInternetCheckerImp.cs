using FolderSynchronizerApp.Business.Abstractions;
using System.Linq;
using Xamarin.Essentials;

namespace FolderSynchronizerApp.Droid.Implementations
{
    public class AndroidInternetCheckerImp : IInternetChecker
    {
        public bool HasWifiConnection()
        {
            var current = Connectivity.NetworkAccess;

            if (!IsConnectedToInternet())
            {
                return false;
            }

            return IsConnectedToWifi();
        }

        private bool IsConnectedToInternet()
        {
            var current = Connectivity.NetworkAccess;

            return current == NetworkAccess.Internet;
        }

        private bool IsConnectedToWifi()
        {
            var profiles = Connectivity.ConnectionProfiles;
            return profiles.Contains(ConnectionProfile.WiFi);
        }
    }
}