using Microsoft.UI.Xaml.Controls;
using System;

namespace SQLiteFluent.Helpers
{
	public static class NavigationHelper
	{
		private static Frame _rootFrame;

		public static void SetRootFrame(Frame rootFrame)
		{
			_rootFrame = rootFrame;
		}

		public static bool NavigateTo(Type pageType)
		{
			return _rootFrame.Navigate(pageType);
		}

		public static void GoBack()
		{
			if (_rootFrame == null) { return; }
			if (_rootFrame.CanGoBack)
			{
				_rootFrame.GoBack();
			}
		}

		public static bool CanGoBack { get { return _rootFrame.CanGoBack; } }

	}
}
