using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SQLiteFluent.Helpers;
using SQLiteFluent.Models;
using System;
using System.Reflection;
using System.Xml.Linq;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.ApplicationModel;
using SQLiteFluent.Services;

namespace SQLiteFluent.ViewModels
{
	public class SettingsViewModel : ObservableObject
	{
		public string Version
		{
			get
			{
				return "1.2.0.0";
			}
		}

		private bool _autoIncrement = SettingsHelper.AutoIncrement;

		public bool AutoIncrement
		{
			get { return _autoIncrement; }
			set
			{
				_autoIncrement = value;
				SettingsHelper.AutoIncrement = value;
				OnPropertyChanged(nameof(AutoIncrement));
			}
		}

		private string _exportPath = SettingsHelper.ExportLocation;

		public string ExportPath
		{
			get { return _exportPath; }
			set
			{
				_exportPath = value;
				SettingsHelper.ExportLocation = value;
				OnPropertyChanged(nameof(ExportPath));
			}
		}

		public IRelayCommand ChangeLocationCommand { get; private set; }
		public IRelayCommand ResetLocationCommand { get; private set; }
		public IRelayCommand OpenHelpCommand { get; private set; }

        public SettingsViewModel()
		{
			ChangeLocationCommand = new RelayCommand(ChangeLocation);
			ResetLocationCommand = new RelayCommand(ResetLocation);
			OpenHelpCommand = new RelayCommand(OpenHelp);
		}

        private async void OpenHelp()
        {
			await DialogService.OpenHelpAsync();
        }

        private async void ChangeLocation()
		{
			FolderPicker folderPicker = new()
			{
				ViewMode = PickerViewMode.Thumbnail,
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary
			};

			var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Window);
			WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hWnd);

			StorageFolder folder = await folderPicker.PickSingleFolderAsync();
			if (folder != null)
			{
				ExportPath = folder.Path;
			}
		}

		private void ResetLocation()
		{
			ExportPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}
	}
}
