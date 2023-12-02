using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls.Primitives;
using SQLiteFluent.Enums;
using SQLiteFluent.Helpers;
using SQLiteFluent.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SQLiteFluent.Models
{
	public class DatabaseTreeItem : ObservableObject
	{
		public TreeType Type { get; set; }
		public DatabaseTreeItem DataBase { get; set; }
		public string Name { get; set; }
		public string FieldType { get; set; }
		public IRelayCommand DeleteDatabaseCommand { get; set; }
		public IRelayCommand RenameDatabaseCommand { get; set; }
		public IRelayCommand DeleteTableCommand { get; set; }
		public IRelayCommand RenameTableCommand { get; set; }
		public IRelayCommand RefreshTablesCommand { get; set; }
		public IRelayCommand Top1000TableItemsCommand { get; set; }

		private ObservableCollection<DatabaseTreeItem> _children;
		public ObservableCollection<DatabaseTreeItem> Children
		{
			get
			{
				if (_children == null)
				{
					_children = new ObservableCollection<DatabaseTreeItem>();
				}
				return _children;
			}
			set
			{
				_children = value;
			}
		}

		private bool _isExpanded = false;
		public bool IsExpanded
		{
			get
			{
				return _isExpanded;
			}
			set
			{
				if (_isExpanded != value)
				{
					_isExpanded = value;
					OnPropertyChanged(nameof(IsExpanded));
				}
			}
		}

		public DatabaseTreeItem()
		{
			DeleteDatabaseCommand = new RelayCommand<object>(DeleteDatabaseAsync);
			RenameDatabaseCommand = new RelayCommand<object>(RenameDatabaseAsync);
			DeleteTableCommand = new RelayCommand<object>(DeleteTableAsync);
			RefreshTablesCommand = new RelayCommand<object>(RefreshTables);
			RenameTableCommand = new RelayCommand<object>(RenameTable);
			Top1000TableItemsCommand = new RelayCommand<object>(Top1000TableItems);
		}

		private async void DeleteDatabaseAsync(object sender)
		{
			if (await DialogService.AskBeforeDeletionAsync("Are you sure you want to delete the database?") == Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
			{
				DatabaseTreeItem selectedItem = sender as DatabaseTreeItem;
				IEnumerable<Database> db = AppHelpers.Databases.Where(x => x.Name == selectedItem.Name);
				DataAccess.DeleteDatabase(selectedItem.Name);
			}
		}

		private async void RenameDatabaseAsync(object sender)
		{
			DatabaseTreeItem selectedItem = sender as DatabaseTreeItem;
			if (await DialogService.RenameDatabaseAsync(selectedItem.Name) == Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
			{				
				DataAccess.RefreshDatabases();
			}
		}

		private async void DeleteTableAsync(object sender)
		{
			if (await DialogService.AskBeforeDeletionAsync("Are you sure you want to delete the table?") == Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
			{
				DatabaseTreeItem selectedItem = sender as DatabaseTreeItem;
				int indexDatabase = AppHelpers.DataSource.IndexOf(selectedItem.DataBase);
				DataAccess.DeleteTable(AppHelpers.Databases.Where(db => db.Name == AppHelpers.DataSource[indexDatabase].Name).First(), selectedItem.Name);
				AppHelpers.DataSource[indexDatabase].Children[0].Children.Remove(selectedItem);
			}
		}

		private async void RenameTable(object sender)
		{
			DatabaseTreeItem databaseTreeItem = sender as DatabaseTreeItem;
			int indexDatabase = AppHelpers.DataSource.IndexOf(databaseTreeItem.DataBase);
			if (await DialogService.RenameTableAsync(AppHelpers.Databases.Where(db => db.Name == AppHelpers.DataSource[indexDatabase].Name).First(), databaseTreeItem.Name) == Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
			{
				ObservableCollection<DatabaseTreeItem> newChilds = DataAccess.RefreshTables(databaseTreeItem);
				AppHelpers.DataSource[indexDatabase].Children[0].Children.Clear();
				foreach (DatabaseTreeItem item in newChilds)
				{
					AppHelpers.DataSource[indexDatabase].Children[0].Children.Add(item);
				}
			}
		}

		private void RefreshTables(object sender)
		{
			DatabaseTreeItem selectedItem = sender as DatabaseTreeItem;
			ObservableCollection<DatabaseTreeItem> newChilds = DataAccess.RefreshTables(selectedItem);
			int indexDatabase = AppHelpers.DataSource.IndexOf(selectedItem.DataBase);
			AppHelpers.DataSource[indexDatabase].Children[0].Children.Clear();
			foreach (DatabaseTreeItem item in newChilds)
			{
				AppHelpers.DataSource[indexDatabase].Children[0].Children.Add(item);
			}
		}

		private void Top1000TableItems(object sender)
		{
			DatabaseTreeItem selectedItem = sender as DatabaseTreeItem;
			int indexDatabase = AppHelpers.DataSource.IndexOf(selectedItem.DataBase);
			Table table = DataAccess.GetTop1000TableItems(AppHelpers.Databases.Where(db => db.Name == AppHelpers.DataSource[indexDatabase].Name).First(), selectedItem.Name);
			AppHelpers.FillTable(table.Columns, table.Rows);
		}

		public override bool Equals(object obj)
		{
			if ((DatabaseTreeItem)obj == null)
			{
				return false;
			}
			return this.Name == ((DatabaseTreeItem)obj).Name;
		}

		public override int GetHashCode()
		{
			return (Name + Type.ToString() + FieldType).GetHashCode();
		}
	}
}
