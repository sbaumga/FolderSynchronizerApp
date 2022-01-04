using FolderSynchronizerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FolderSynchronizerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigurationPage : ContentPage
    {
        private ConfigurationViewModel ViewModel { get; }

        public ConfigurationPage()
        {
            InitializeComponent();

            ViewModel = Startup.ServiceProvider.GetService<ConfigurationViewModel>();
            BindingContext = ViewModel;

            ViewModel.SetUpPopups((x, y, z) => DisplayAlert(x, y, z));
        }
    }
}