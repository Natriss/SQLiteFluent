using Windows.Storage;

namespace SQLiteFluent.Helpers
{
	public static class SettingsHelper
	{
		private static ApplicationDataContainer _localSettings;

		public static bool AutoIncrement
		{
			get
			{
				return (bool)_localSettings.Values["AutoIncrement"];
			}
			set
			{
				_localSettings.Values["AutoIncrement"] = value;
			}
		}

		public static void Initialize()
		{
			_localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

			if (_localSettings.Values.Count == 0)
			{
				_localSettings.Values["AutoIncrement"] = true;
			}
		}
	}
}
