using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace SQLiteFluent.Converters
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if ((bool)value == false)
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if ((bool)value == false)
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}
	}
}
