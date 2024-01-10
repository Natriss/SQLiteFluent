using System.Reflection;

namespace SQLiteFluent.ViewModels
{
	public class SettingsViewModel
    {
		public string Version { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
    }
}
