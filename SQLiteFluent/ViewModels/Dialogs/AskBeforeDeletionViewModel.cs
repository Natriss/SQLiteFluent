using CommunityToolkit.Mvvm.ComponentModel;

namespace SQLiteFluent.ViewModels.Dialogs
{
	public class AskBeforeDeletionViewModel : ObservableObject
	{
		private string _message;

		public string Message
		{
			get { return _message; }
			set { _message = value; OnPropertyChanged(nameof(Message)); }
		}

		public AskBeforeDeletionViewModel() { }
	}
}
