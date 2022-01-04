using FolderSynchronizerApp.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FolderSynchronizerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QueueMessagesListPage : ContentPage
    {
        public QueueMessagesListViewModel ViewModel { get; }

        public QueueMessagesListPage()
        {
            InitializeComponent();

            ViewModel = Startup.ServiceProvider.GetService<QueueMessagesListViewModel>();
            BindingContext = ViewModel;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
