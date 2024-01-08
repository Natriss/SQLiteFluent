using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SQLiteFluent.Models;
using System;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SQLiteFluent.ViewModels.Dialogs
{
	public class ImportDatabaseDialogViewModel : ObservableObject
	{
		private string _name = null;

		public string Name
		{
			get { return _name; }
			set { _name = value; OnPropertyChanged(nameof(Name)); }
		}

		private string _path = null;

		public string Path
		{
			get { return _path; }
			set { _path = value; OnPropertyChanged(nameof(Path)); }
		}

		private bool _showError;

		public bool ShowError
		{
			get { return _showError; }
			set { _showError = value; OnPropertyChanged(nameof(ShowError)); }
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
				Name = file.Name;
				Path = file.Path;
				ShowError = DataAccess.DoesFileExistAsync(Name);
			}
			else
			{
				Name = null;
				Path = null;
			}
		}

	}
}
