using Microsoft.Windows.ApplicationModel.Resources;

namespace SQLiteFluent.Helpers
{
	public static class ResourceHelper
	{
		private static readonly ResourceLoader _resourceLoader = new();

		public static string GetLocalized(this string resourceName)
		{
			return _resourceLoader.GetString(resourceName);
		}
	}
}
