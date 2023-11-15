using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.Helpers;
using SQLiteFluent.Models;
using SQLiteFluent.ViewModels.Dialogs;
using SQLiteFluent.Views.Dialogs;
using System;
using System.Threading.Tasks;

namespace SQLiteFluent.Services
{
	public static class DialogService
	{
		public static async Task<ContentDialogResult> AddDatabaseAsync()
		{
			ContentDialog contentDialog = new()
			{
				XamlRoot = App.Root.XamlRoot,
				Title = "AddDatabaseDialogTitle".GetLocalized(),
				Content = new AddDatabaseDialog(),
				PrimaryButtonText = "AddDatabaseDialogPrimaryBtn".GetLocalized(),
				CloseButtonText = "AddDatabaseDialogCloseBtn".GetLocalized(),
				DefaultButton = ContentDialogButton.Primary,
			};
			ContentDialogResult result = await contentDialog.ShowAsync();
			
			if (result != ContentDialogResult.Primary)
			{
				return result;
			}

			AddDatabaseDialogViewModel vm = (AddDatabaseDialogViewModel)((AddDatabaseDialog)contentDialog.Content).DataContext;
			DataAccess.CreateDatabase(vm.Name);

			return result;
		}
	}
}
