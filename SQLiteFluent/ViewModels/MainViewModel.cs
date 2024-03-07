using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.Helpers;
using SQLiteFluent.Models;
using SQLiteFluent.Services;
using SQLiteFluent.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SQLiteFluent.ViewModels
{
	public class MainViewModel : ObservableObject
	{
		public ObservableCollection<DatabaseTreeItem> DataSource { get; private set; } = AppHelpers.DataSource;
		public ObservableCollection<Database> ComboboxItemSource { get; private set; } = AppHelpers.Databases;
		private Database _selectedComboboxItem;

		public Database SelectedComboboxItem
		{
			get { return _selectedComboboxItem; }
			set
			{
				if (_selectedComboboxItem != value)
				{
					_selectedComboboxItem = value;
				}
				ExecuteQueryCommand.NotifyCanExecuteChanged();
			}
		}
		private string _query;

		public string Query
		{
			get { return _query; }
			set { _query = value; OnPropertyChanged(nameof(Query)); ExecuteQueryCommand.NotifyCanExecuteChanged(); }
		}
		public ObservableCollection<string> Columns { get; set; } = AppHelpers.Columns;
		public ObservableCollection<ObservableCollection<string>> Rows { get; set; } = AppHelpers.Rows;

		public ICommand AddDatabaseFlyoutCommand { get; private set; }
		public ICommand ImportDatabaseFlyoutCommand { get; private set; }
		public ICommand ExportDatabaseFlyoutCommand { get; private set; }
		public IRelayCommand ExecuteQueryCommand { get; private set; }
		public IRelayCommand OpenSettingsCommand { get; private set; }
		public IRelayCommand OpenHelpRecipeCommand { get; private set; }

		public MainViewModel()
		{
			AddDatabaseFlyoutCommand = new RelayCommand(AddDatabaseAsync);
			ImportDatabaseFlyoutCommand = new RelayCommand(ImportDatabaseAsync);
			ExportDatabaseFlyoutCommand = new RelayCommand(ExportDatabaseAsync);
			ExecuteQueryCommand = new RelayCommand(ExecuteQuery, () => { return (SelectedComboboxItem != null) && !string.IsNullOrWhiteSpace(Query); });
			OpenSettingsCommand = new RelayCommand(NavigateToSettings);
			OpenHelpRecipeCommand = new RelayCommand(NavigateToRecipe);
		}

		private async void ExportDatabaseAsync()
		{
			try
			{
				if (await DialogService.ExportDatabaseAsync() == ContentDialogResult.Primary)
				{
					InfoBarService.ShowSuccess("DatabaseHasBeenExported".GetLocalized());
				}
			}
			catch (Exception e)
			{
				InfoBarService.ShowError(e.Message);
			}
		}

		private async void AddDatabaseAsync()
		{
			try
			{
				if (await DialogService.AddDatabaseAsync() == ContentDialogResult.Primary)
				{
					DataAccess.RefreshDatabases();
					InfoBarService.ShowSuccess("DatabaseHasBeenAdded".GetLocalized());
				}

			}
			catch (Exception e)
			{
				InfoBarService.ShowError(e.Message);
			}
		}

		private async void ImportDatabaseAsync()
		{
			try
			{
				if (await DialogService.ImportDatabaseAsync() == ContentDialogResult.Primary)
				{
					DataAccess.RefreshDatabases();
					InfoBarService.ShowSuccess("DatabaseHasBeenImported".GetLocalized());
				}
			}
			catch (Exception e)
			{
				InfoBarService.ShowError(e.Message);
			}
		}

		private void ExecuteQuery()
		{
			try
			{
				Table table = DataAccess.ExecuteAnyQuery(SelectedComboboxItem.Path, Query);
				AppHelpers.FillTable(table.Columns, table.Rows);
				InfoBarService.ShowSuccess("QueryWasExecuted".GetLocalized());
			}
			catch (Exception e)
			{
				InfoBarService.ShowError(e.Message);
			}
		}

		private void NavigateToSettings()
		{
			NavigationHelper.NavigateTo(typeof(SettingsPage));
		}

		private void NavigateToRecipe()
		{
			NavigationHelper.NavigateTo(typeof(RecipePage));
		}
	}
}
