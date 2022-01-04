using FolderSynchronizerApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace FolderSynchronizerApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}