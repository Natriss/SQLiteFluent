﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.Helpers;
using SQLiteFluent.Models;
using SQLiteFluent.Services;
using System.Collections.Generic;
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
		public List<string> Columns { get; set; }
		public List<List<string>> Rows { get; set; }

		public ICommand AddDatabaseFlyoutCommand { get; private set; }
		public ICommand ImportDatabaseFlyoutCommand { get; private set; }
		public IRelayCommand ExecuteQueryCommand { get; private set; }

		public MainViewModel()
		{
			AddDatabaseFlyoutCommand = new RelayCommand(AddDatabaseAsync);
			ImportDatabaseFlyoutCommand = new RelayCommand(ImportDatabaseAsync);
			ExecuteQueryCommand = new RelayCommand(ExecuteQuery, () => { return (SelectedComboboxItem != null) && !string.IsNullOrWhiteSpace(Query); });
		}
		private async void AddDatabaseAsync()
		{
			if (await DialogService.AddDatabaseAsync() == ContentDialogResult.Primary)
			{
				DataAccess.RefreshDatabases();
			}
		}

		private async void ImportDatabaseAsync()
		{
			if (await DialogService.ImportDatabaseAsync() == ContentDialogResult.Primary)
			{
				DataAccess.RefreshDatabases();
			}
		}

		private void ExecuteQuery()
		{
			Table table = DataAccess.ExecuteAnyQuery(SelectedComboboxItem.Path, Query);
			Columns = table.Columns;
			OnPropertyChanged(nameof(Columns));
			Rows = table.Rows;
			OnPropertyChanged(nameof(Rows));
		}
	}
}
