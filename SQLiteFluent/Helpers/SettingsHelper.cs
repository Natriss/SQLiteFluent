using System;
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

		public static string ExportLocation
		{
			get
			{
				return (string)_localSettings.Values["ExportLocation"];
			}
			set
			{
				_localSettings.Values["ExportLocation"] = value;
			}
		}

		public static void Initialize()
		{
			_localSettings = ApplicationData.Current.LocalSettings;

			if (_localSettings.Values["AutoIncrement"] == null)
			{
				_localSettings.Values["AutoIncrement"] = true;
			}
			if (_localSettings.Values["ExportLocation"] == null)
			{
				_localSettings.Values["ExportLocation"] = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			}
		}
	}
}
