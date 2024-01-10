using Microsoft.UI.Xaml.Data;
using System;

namespace SQLiteFluent.Converters
{
	public class PrimaryKeyToGlyphConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return (bool)value ? "\uE006" : "\uE004";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return (bool)value ? "\uE006" : "\uE004";
		}
	}
}
