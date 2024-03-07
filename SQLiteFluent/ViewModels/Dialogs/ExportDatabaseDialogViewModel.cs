using CommunityToolkit.Mvvm.ComponentModel;
using SQLiteFluent.Helpers;
using SQLiteFluent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteFluent.ViewModels.Dialogs
{
	public class ExportDatabaseDialogViewModel : ObservableObject
	{
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
			}
		}
	}
}
