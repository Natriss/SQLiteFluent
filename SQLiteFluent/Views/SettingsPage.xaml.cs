using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.ViewModels;
using System;
using System.Diagnostics;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SQLiteFluent.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SettingsPage : Page
    {
		public SettingsViewModel ViewModel { get { return this.DataContext as SettingsViewModel; } }
        public SettingsPage()
        {
            this.InitializeComponent();
            this.DataContext = new SettingsViewModel();
        }

		private async void HyperlinkButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
		{
			StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Colophon/Colophon.rtf"));
			await Windows.System.Launcher.LaunchFileAsync(file);
		}
    }
}