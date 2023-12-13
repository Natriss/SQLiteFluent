using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.ApplicationModel.DataTransfer;

namespace SQLiteFluent.ViewModels.Controls
{
	public class RecipeControlViewModel : ObservableObject
    {
		private bool _infoToggled;

		public bool InfoToggled
		{
			get { return _infoToggled; }
			set { _infoToggled = value; OnPropertyChanged(nameof(InfoToggled)); }
		}

		public IRelayCommand CopyCommand { get; private set; }

		public RecipeControlViewModel()
		{
			CopyCommand = new RelayCommand<string>(Copy);
		}

		private void Copy(string text)
		{
			DataPackage package = new()
			{
				RequestedOperation = DataPackageOperation.Copy
			};
			package.SetText(text);
			Clipboard.SetContent(package);
		}
	}
}
