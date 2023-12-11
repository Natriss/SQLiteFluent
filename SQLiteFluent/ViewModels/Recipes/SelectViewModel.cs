using CommunityToolkit.Mvvm.ComponentModel;

namespace SQLiteFluent.ViewModels.Recipes
{
	public class SelectViewModel : ObservableObject
	{
		private bool _infoToggled;

		public bool InfoToggled
		{
			get { return _infoToggled; }
			set { _infoToggled = value; OnPropertyChanged(nameof(InfoToggled)); }
		}
	}
}
