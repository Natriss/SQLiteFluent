using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.Helpers;
using SQLiteFluent.Models;
using SQLiteFluent.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SQLiteFluent.ViewModels
{
	public class MainViewModel : ObservableObject
	{
		public ObservableCollection<DatabaseTreeItem> DataSource { get; private set; }
		public ObservableCollection<Database> ComboboxItemSource { get; private set; }
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
		public string Tables { get; set; }
		public string Rows { get; set; }

		public ICommand AddDatabaseFlyoutCommand { get; private set; }
		public ICommand ImportDatabaseFlyoutCommand { get; private set; }
		public ICommand RefreshDatabaseListCommand { get; private set; }
		public IRelayCommand ExecuteQueryCommand { get; private set; }

		public MainViewModel()
		{
			AppHelpers.DataSource = DataAccess.GetAllData();
			AppHelpers.Databases = DataAccess.GetAvailableDatabases();
			RefreshDatabaseList();
			AddDatabaseFlyoutCommand = new RelayCommand(AddDatabaseAsync);
			ImportDatabaseFlyoutCommand = new RelayCommand(ImportDatabaseAsync);
			RefreshDatabaseListCommand = new RelayCommand(RefreshDatabaseList);
			ExecuteQueryCommand = new RelayCommand(ExecuteQuery, () => { return (SelectedComboboxItem != null) && !string.IsNullOrWhiteSpace(Query); });
		}
		private async void AddDatabaseAsync()
		{
			if (await DialogService.AddDatabaseAsync() == ContentDialogResult.Primary)
			{
				RefreshDatabaseList();
			}
		}

		private async void ImportDatabaseAsync()
		{
			if (await DialogService.ImportDatabaseAsync() == ContentDialogResult.Primary)
			{
				RefreshDatabaseList();
			}
		}

		private void RefreshDatabaseList()
		{
			DataSource = AppHelpers.DataSource;
			OnPropertyChanged(nameof(DataSource));
			ComboboxItemSource = AppHelpers.Databases;
			OnPropertyChanged(nameof(ComboboxItemSource));
		}

		private void ExecuteQuery()
		{
			Table table = DataAccess.ExecuteAnyQuery(SelectedComboboxItem.Path, Query);
			Tables = table.TablesToString();
			OnPropertyChanged(nameof(Tables));
			Rows = table.RowsToString();
			OnPropertyChanged(nameof(Rows));

		}
	}
}
