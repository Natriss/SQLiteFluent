using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.Models;
using SQLiteFluent.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SQLiteFluent.ViewModels
{
	public class MainViewModel : ObservableObject
	{
		public ObservableCollection<DatabaseTreeItem> DataSource { get; private set; }

		public ICommand AddDatabaseFlyoutCommand { get; private set; }
		public ICommand ImportDatabaseFlyoutCommand { get; private set; }
		public ICommand RefreshDatabaseListCommand { get; private set; }

		public MainViewModel()
		{
			RefreshDatabaseList();
			AddDatabaseFlyoutCommand = new RelayCommand(AddDatabaseAsync);
			ImportDatabaseFlyoutCommand = new RelayCommand(ImportDatabaseAsync);
			RefreshDatabaseListCommand = new RelayCommand(RefreshDatabaseList);
		}
		private async void AddDatabaseAsync()
		{
			if (await DialogService.AddDatabaseAsync() == ContentDialogResult.Primary)
			{
				RefreshDatabaseList();
			}
		}

		private async void ImportDatabaseAsync()
		{
			if (await DialogService.ImportDatabaseAsync() == ContentDialogResult.Primary)
			{
				RefreshDatabaseList();
			}
		}

		private void RefreshDatabaseList()
		{
			DataSource = DataAccess.GetAllData();
			OnPropertyChanged(nameof(DataSource));
		}
	}
}
