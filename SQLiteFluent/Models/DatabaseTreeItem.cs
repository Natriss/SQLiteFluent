using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SQLiteFluent.Enums;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SQLiteFluent.Models
{
	public class DatabaseTreeItem : ObservableObject
	{
		public TreeType Type { get; set; }
		public string Name { get; set; }
		public string FieldType { get; set; }
		public IRelayCommand ClearChildsCommand { get; set; }

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
			ClearChildsCommand = new RelayCommand(ClearChilds);
		}

		private void ClearChilds()
		{
			_children.Clear();
		}
	}
}
