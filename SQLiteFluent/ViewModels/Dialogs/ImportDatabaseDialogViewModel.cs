using CommunityToolkit.Mvvm.ComponentModel;

namespace SQLiteFluent.ViewModels.Dialogs
{
	public class ImportDatabaseDialogViewModel : ObservableObject
	{
		private string _path;

		public string Path
		{
			get { return _path; }
			set { _path = value; OnPropertyChanged(Path); }
		}
	}
}
