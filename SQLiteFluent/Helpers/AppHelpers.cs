using Microsoft.UI.Xaml;
using SQLiteFluent.Models;
using System.Collections.ObjectModel;

namespace SQLiteFluent.Helpers
{
	public static class AppHelpers
	{
		public static FrameworkElement Root {  get; set; }
		public static ObservableCollection<DatabaseTreeItem> DataSource { get; set; } = DataAccess.GetAllData();
		public static ObservableCollection<Database> Databases { get; set; } = DataAccess.GetAvailableDatabases();
		public static ObservableCollection<string> Columns { get; set; } = new();
		public static ObservableCollection<ObservableCollection<string>> Rows { get; set; } = new();

		public static void FillTable(ObservableCollection<string> columns, ObservableCollection<ObservableCollection<string>> rows)
		{
			Columns.Clear();
			foreach (var column in columns)
			{
				Columns.Add(column);
			}

			Rows.Clear();
			foreach (var row in rows)
			{
				Rows.Add(row);
			}
		}
	}
}