using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SQLiteFluent.Helpers;

namespace SQLiteFluent.Services
{
	public static class InfoBarService
	{
		public static void Show(string title, string message)
		{
			InfoBar infoBar = new()
			{
				XamlRoot = AppHelpers.Root.XamlRoot,
				Title = title,
				Message = message,
				IsOpen = true,
				Severity = InfoBarSeverity.Informational,
				Background = (Brush)Application.Current.Resources["AcrylicBackgroundFillColorDefaultBrush"]
			};
			ShowInfoBar(infoBar);
		}

		public static void Show(string title, string message, InfoBarSeverity infoBarSeverity)
		{
			InfoBar infoBar = new()
			{
				XamlRoot = AppHelpers.Root.XamlRoot,
				Title = title,
				Message = message,
				IsOpen = true,
				Severity = infoBarSeverity,
				Background = (Brush)Application.Current.Resources["AcrylicBackgroundFillColorDefaultBrush"]
			};
			ShowInfoBar(infoBar);
		}

		public static void ShowInfo(string message)
		{
			Show("Info".GetLocalized(), message, InfoBarSeverity.Informational);
		}

		public static void ShowSuccess(string message)
		{
			Show("Success".GetLocalized(), message, InfoBarSeverity.Success);
		}

		public static void ShowError(string message)
		{
			Show("Error".GetLocalized(), message, InfoBarSeverity.Error);
		}

		public static void ShowWarning(string message)
		{
			Show("Warning".GetLocalized(), message, InfoBarSeverity.Warning);
		}

		private static void ShowInfoBar(InfoBar infoBar)
		{
			AppHelpers.InfoBarGrid.Children.Clear();
			AppHelpers.InfoBarGrid.Children.Add(infoBar);
		}
	}
}
