using CommunityToolkit.Mvvm.ComponentModel;
using SQLiteFluent.Helpers;
using System.Reflection;

namespace SQLiteFluent.ViewModels
{
	public class SettingsViewModel : ObservableObject
	{
		public string Version { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

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
	}
}
