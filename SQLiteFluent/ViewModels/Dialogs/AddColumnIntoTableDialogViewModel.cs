using CommunityToolkit.Mvvm.ComponentModel;
using SQLiteFluent.Enums;
using System;

namespace SQLiteFluent.ViewModels.Dialogs
{
	public class AddColumnIntoTableDialogViewModel : ObservableObject
	{
		private string _name;

		public string Name
		{
			get { return _name; }
			set { _name = value; OnPropertyChanged(nameof(Name)); }
		}

		public ColumnDeclaredType[] ItemSource { get { return Enum.GetValues<ColumnDeclaredType>(); } }

		private ColumnDeclaredType _selectedType;

		public ColumnDeclaredType SelectedType
		{
			get { return _selectedType; }
			set
			{
				_selectedType = value;
				if (value != _selectedType)
				{
					OnPropertyChanged(nameof(SelectedType));
				}
			}
		}

		private bool _isDefaultChecked;

		public bool IsDefaultChecked
		{
			get { return _isDefaultChecked; }
			set { _isDefaultChecked = value; OnPropertyChanged(nameof(IsDefaultChecked)); }
		}

		private string _defaultValue;

		public string DefaultValue
		{
			get { return _defaultValue; }
			set { _defaultValue = value; OnPropertyChanged(nameof(DefaultValue)); }
		}
	}
}
