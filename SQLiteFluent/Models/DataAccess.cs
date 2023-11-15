using Microsoft.Data.Sqlite;
using SQLiteFluent.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace SQLiteFluent.Models
{
	public static class DataAccess
	{
		public static async void CreateDatabase(string name)
		{
			await ApplicationData.Current.LocalFolder.CreateFileAsync($"{name}.db", CreationCollisionOption.OpenIfExists);
		}

		public static void DeleteDatabase(string name)
		{
			string pathDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, $"{name}.db");
			File.Delete(pathDB);
		}

		private static DatabaseTreeItem GetDatabaseTreeItem(string dbPath)
		{
			List<string> splittedPath = dbPath.Split("\\").ToList();
			string databaseFileName = splittedPath.Last().Split('.').ToList()[0];
			DatabaseTreeItem databaseTreeItem = new()
			{
				Name = databaseFileName,
				Type = TreeType.Database,
				Children = null
			};

			return databaseTreeItem;
		}

		private static DatabaseTreeItem GetDatabaseTableChilds(SqliteConnection db)
		{
			SqliteCommand getAllTablesCommand = new("SELECT * FROM sqlite_master where type='table';", db);
			SqliteDataReader getAllTablesDataReader = getAllTablesCommand.ExecuteReader();

			DatabaseTreeItem tableGroup = new()
			{
				Type = TreeType.TablesGroup,
			};

			ObservableCollection<DatabaseTreeItem> tableItems = new ObservableCollection<DatabaseTreeItem>();
			while (getAllTablesDataReader.Read())
			{
				DatabaseTreeItem databaseTreeItem = new()
				{
					Name = getAllTablesDataReader.GetString(1),
					Type = TreeType.Table,
				};
				databaseTreeItem.Children = GetDatabaseTableFields(databaseTreeItem.Name, db);
				tableItems.Add(databaseTreeItem);
			}
			tableGroup.Children = tableItems;
			return tableGroup;
		}

		private static ObservableCollection<DatabaseTreeItem> GetDatabaseTableFields(string tableName, SqliteConnection db)
		{
			SqliteCommand getAllFieldsCommand = new();
			getAllFieldsCommand.Connection = db;
			getAllFieldsCommand.CommandText = $"PRAGMA table_info({tableName});";
			getAllFieldsCommand.Prepare();
			SqliteDataReader getAllFieldsDataReader = getAllFieldsCommand.ExecuteReader();

			ObservableCollection<DatabaseTreeItem> fieldItems = new ObservableCollection<DatabaseTreeItem>();
			while (getAllFieldsDataReader.Read())
			{
				DatabaseTreeItem databaseTreeItem = new()
				{
					Name = getAllFieldsDataReader.GetString(1),
					FieldType = getAllFieldsDataReader.GetString(2),
					Type = TreeType.Field,
				};
				fieldItems.Add(databaseTreeItem);
			}
			return fieldItems;
		}

		public static ObservableCollection<DatabaseTreeItem> GetAllData()
		{
			ObservableCollection<DatabaseTreeItem> list = new();
			List<string> databases = Directory.EnumerateFiles(ApplicationData.Current.LocalFolder.Path, "*", SearchOption.TopDirectoryOnly).Where(search => search.EndsWith(".db")).ToList();
			foreach (string database in databases)
			{
				using SqliteConnection db = new($"Filename={database}");
				db.Open();

				DatabaseTreeItem rootItem = GetDatabaseTreeItem(database);
				DatabaseTreeItem tableGroup = GetDatabaseTableChilds(db);
				rootItem.Children = new ObservableCollection<DatabaseTreeItem> { tableGroup };
				list.Add(rootItem);
			}
			return list;
		}
	}
}
