using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SQLiteFluent.Enums;
using SQLiteFluent.Helpers;
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
			DeleteTableCommand = new RelayCommand<object>(DeleteTable);
		}

		private void DeleteTable(object sender)
		{
			DatabaseTreeItem selectedItem = sender as DatabaseTreeItem;
			int indexDatabase = AppHelpers.DataSource.IndexOf(selectedItem.DataBase);
			DataAccess.DeleteTable(AppHelpers.Databases.Where(db => db.Name == AppHelpers.DataSource[indexDatabase].Name).First(), selectedItem.Name);
			AppHelpers.DataSource[indexDatabase].Children[0].Children.Remove(selectedItem);

		}
	}
}
