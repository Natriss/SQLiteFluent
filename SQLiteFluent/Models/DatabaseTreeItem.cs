using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SQLiteFluent.Enums;
using SQLiteFluent.Helpers;
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
		public IRelayCommand DeleteTableCommand { get; set; }

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
			DeleteDatabaseCommand = new RelayCommand<object>(DeleteDatabase);
			DeleteTableCommand = new RelayCommand<object>(DeleteTable);
		}

		private void DeleteDatabase(object sender)
		{
			DatabaseTreeItem selectedItem = sender as DatabaseTreeItem;
			IEnumerable<Database> db = AppHelpers.Databases.Where(x => x.Name == selectedItem.Name);
			DataAccess.DeleteDatabase(selectedItem.Name);
		}

		private void DeleteTable(object sender)
		{
			DatabaseTreeItem selectedItem = sender as DatabaseTreeItem;
			int indexDatabase = AppHelpers.DataSource.IndexOf(selectedItem.DataBase);
			DataAccess.DeleteTable(AppHelpers.Databases.Where(db => db.Name == AppHelpers.DataSource[indexDatabase].Name).First(), selectedItem.Name);
			AppHelpers.DataSource[indexDatabase].Children[0].Children.Remove(selectedItem);
		}

		private void RefreshTables(object sender)
		{
			DatabaseTreeItem selectedItem = sender as DatabaseTreeItem;
			DataAccess.RefreshTables(selectedItem);
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
