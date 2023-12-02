using CommunityToolkit.Mvvm.ComponentModel;

namespace SQLiteFluent.ViewModels.Dialogs
{
	public class RenameTableDialogViewModel : ObservableObject
	{
		private string _oldName;

		public string OldName
		{
			get { return _oldName; }
			set { _oldName = value; OnPropertyChanged(nameof(OldName)); }
		}

		private string _newName;

		public string NewName
		{
			get { return _newName; }
			set { _newName = value; OnPropertyChanged(nameof(NewName)); }
		}
	}
}
