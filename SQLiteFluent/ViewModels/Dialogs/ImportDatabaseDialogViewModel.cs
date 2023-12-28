using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SQLiteFluent.ViewModels.Dialogs
{
	public class ImportDatabaseDialogViewModel : ObservableObject
	{
		private string _path;

		public string Path
		{
			get { return _path; }
			set { _path = value; OnPropertyChanged(Path); }
			//TODO not updaing the UI
		}

		public IRelayCommand PickAFileCommand { get; private set; }

		public ImportDatabaseDialogViewModel()
		{
			PickAFileCommand = new RelayCommand(PickAFile);
		}

		private async void PickAFile()
		{
			FileOpenPicker openPicker = new()
			{
				ViewMode = PickerViewMode.Thumbnail,
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary
			};
			openPicker.FileTypeFilter.Add(".db");

			var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Window);
			WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

			StorageFile file = await openPicker.PickSingleFileAsync();
			if (file != null)
			{
				// Application now has read/write access to the picked file
				Path = file.Name;
			}
			else
			{
				Path = "Operation cancelled.";
			}
		}
	}
}
