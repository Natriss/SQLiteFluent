﻿using Microsoft.UI.Xaml;
using SQLiteFluent.Models;
using System.Collections.ObjectModel;

namespace SQLiteFluent.Helpers
{
	public static class AppHelpers
	{

		public static FrameworkElement Root {  get; set; }
		public static ObservableCollection<DatabaseTreeItem> DataSource { get; set; } = DataAccess.GetAllData();
		public static ObservableCollection<Database> Databases { get; set; } = DataAccess.GetAvailableDatabases();
	}
}