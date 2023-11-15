using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SQLiteFluent.ViewModels.Dialogs
{
	public class AddDatabaseDialogViewModel : ObservableObject
	{
		private string _name;

		[Required]
		public string Name
		{
			get { return _name; }
			set { _name = value; OnPropertyChanged(nameof(Name)); }
		}

	}
}
