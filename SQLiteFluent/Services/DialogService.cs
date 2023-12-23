using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.Helpers;
using SQLiteFluent.Models;
using SQLiteFluent.ViewModels.Dialogs;
using SQLiteFluent.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SQLiteFluent.Services
{
	public static class DialogService
	{
		public static async Task<ContentDialogResult> AddDatabaseAsync()
		{
			ContentDialog contentDialog = new()
			{
				XamlRoot = AppHelpers.Root.XamlRoot,
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

		public static async Task<ContentDialogResult> ImportDatabaseAsync()
		{
			ContentDialog contentDialog = new()
			{
				XamlRoot = AppHelpers.Root.XamlRoot,
				Title = "ImportDatabaseDialogTitle".GetLocalized(),
				Content = new ImportDatabaseDialog(),
				PrimaryButtonText = "ImportDatabaseDialogPrimaryBtn".GetLocalized(),
				CloseButtonText = "ImportDatabaseDialogCloseBtn".GetLocalized(),
				DefaultButton = ContentDialogButton.Primary,
			};
			ContentDialogResult result = await contentDialog.ShowAsync();

			if (result != ContentDialogResult.Primary)
			{
				return result;
			}

			ImportDatabaseDialogViewModel vm = (ImportDatabaseDialogViewModel)((ImportDatabaseDialog)contentDialog.Content).DataContext;
			DataAccess.ImportDatabase(vm.Path);

			return result;
		}

		public static async Task<ContentDialogResult> RenameDatabaseAsync(string oldDatabaseName)
		{
			ContentDialog contentDialog = new()
			{
				XamlRoot = AppHelpers.Root.XamlRoot,
				Title = "RenameDatabaseDialogTitle".GetLocalized(),
				Content = new RenameDatabaseDialog(oldDatabaseName),
				PrimaryButtonText = "RenameDatabaseDialogPrimaryBtn".GetLocalized(),
				CloseButtonText = "RenameDatabaseDialogCloseBtn".GetLocalized(),
				DefaultButton = ContentDialogButton.Primary,
			};
			ContentDialogResult result = await contentDialog.ShowAsync();

			if (result != ContentDialogResult.Primary)
			{
				return result;
			}

			RenameDatabaseDialogViewModel vm = ((RenameDatabaseDialog)contentDialog.Content).DataContext as RenameDatabaseDialogViewModel;
			DataAccess.RenameDatabase(vm.OldName, vm.NewName);

			return result;
		}

		public static async Task<ContentDialogResult> AskBeforeDeletionAsync(string message)
		{
			ContentDialog contentDialog = new()
			{
				XamlRoot = AppHelpers.Root.XamlRoot,
				Title = "AskBeforeDeletionDialogTitle".GetLocalized(),
				Content = new AskBeforeDeletionDialog(message),
				PrimaryButtonText = "AskBeforeDeletionDialogTitlePrimaryBtn".GetLocalized(),
				CloseButtonText = "AskBeforeDeletionDialogTitleCloseBtn".GetLocalized(),
				DefaultButton = ContentDialogButton.Primary,
			};
			ContentDialogResult result = await contentDialog.ShowAsync();

			return result;
		}

		public static async Task<ContentDialogResult> RenameTableAsync(Database db, string oldTableName)
		{
			ContentDialog contentDialog = new()
			{
				XamlRoot = AppHelpers.Root.XamlRoot,
				Title = "RenameTableDialogTitle".GetLocalized(),
				Content = new RenameTableDialog(oldTableName),
				PrimaryButtonText = "RenameTableDialogPrimaryBtn".GetLocalized(),
				CloseButtonText = "RenameTableDialogCloseBtn".GetLocalized(),
				DefaultButton = ContentDialogButton.Primary,
			};
			ContentDialogResult result = await contentDialog.ShowAsync();

			if (result != ContentDialogResult.Primary)
			{
				return result;
			}

			RenameTableDialogViewModel vm = ((RenameTableDialog)contentDialog.Content).DataContext as RenameTableDialogViewModel;
			DataAccess.RenameTable(db, vm.OldName, vm.NewName);

			return result;
		}

		public static async Task<ContentDialogResult> InsertDataIntoTableAsync(Database db, DatabaseTreeItem table)
		{
			ContentDialog contentDialog = new()
			{
				XamlRoot = AppHelpers.Root.XamlRoot,
				Title = "InsertDataIntoTableDialogTitle".GetLocalized(),
				Content = new InsertDataIntoTableDialog(table.Children),
				PrimaryButtonText = "InsertDataIntoTableDialogPrimaryBtn".GetLocalized(),
				CloseButtonText = "InsertDataIntoTableDialogCloseBtn".GetLocalized(),
				DefaultButton = ContentDialogButton.Primary,
			};
			ContentDialogResult result = await contentDialog.ShowAsync();

			if (result != ContentDialogResult.Primary)
			{
				return result;
			}

			InsertDataIntoTableDialogViewModel vm = ((InsertDataIntoTableDialog)contentDialog.Content).DataContext as InsertDataIntoTableDialogViewModel;
			Dictionary<string, string> values = vm.GetTableValues();
			DataAccess.InsertDataIntoTable(db, table.Name, values);

			return result;
		}

		public static async Task<ContentDialogResult> AddColumnIntoTableAsync(Database db, DatabaseTreeItem table)
		{
			ContentDialog contentDialog = new()
			{
				XamlRoot = AppHelpers.Root.XamlRoot,
				Title = "AddColumnIntoTableDialogTitle".GetLocalized(),
				Content = new AddColumnIntoTableDialog(),
				PrimaryButtonText = "AddColumnIntoTableDialogPrimaryBtn".GetLocalized(),
				CloseButtonText = "AddColumnIntoTableDialogCloseBtn".GetLocalized(),
				DefaultButton = ContentDialogButton.Primary,
			};
			ContentDialogResult result = await contentDialog.ShowAsync();

			if (result != ContentDialogResult.Primary)
			{
				return result;
			}

			AddColumnIntoTableDialogViewModel vm = ((AddColumnIntoTableDialog)contentDialog.Content).DataContext as AddColumnIntoTableDialogViewModel;
			DataAccess.AddColumnIntoTable(db, table.Name, vm.Name, vm.SelectedType, vm.IsDefaultChecked, vm.DefaultValue);

			return result;
		}
	}
}
