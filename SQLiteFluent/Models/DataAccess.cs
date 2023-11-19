using Microsoft.Data.Sqlite;
using SQLiteFluent.Core.Interfaces;
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

		public static void ImportDatabase(string path)
		{
			string name = path.Trim(new char[] { '\"' }).Split("\\").ToList().Last();
			File.Copy(path.Trim(new char[] { '\"' }), Path.Combine(ApplicationData.Current.LocalFolder.Path, name), true);
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

		private static DatabaseTreeItem GetDatabaseTableChilds(SqliteConnection db, DatabaseTreeItem rootItem)
		{
			SqliteCommand getAllTablesCommand = new("SELECT * FROM sqlite_master where type='table';", db);
			SqliteDataReader getAllTablesDataReader = getAllTablesCommand.ExecuteReader();

			DatabaseTreeItem tableGroup = new()
			{
				Type = TreeType.TablesGroup,
			};

			ObservableCollection<DatabaseTreeItem> tableItems = new();
			while (getAllTablesDataReader.Read())
			{
				DatabaseTreeItem databaseTreeItem = new()
				{
					Name = getAllTablesDataReader.GetString(1),
					Type = TreeType.Table,
					DataBase = rootItem,
				};
				databaseTreeItem.Children = GetDatabaseTableFields(databaseTreeItem.Name, db);
				tableItems.Add(databaseTreeItem);
			}
			tableGroup.Children = tableItems;
			return tableGroup;
		}

		private static ObservableCollection<DatabaseTreeItem> GetDatabaseTableFields(string tableName, SqliteConnection db)
		{
			SqliteCommand getAllFieldsCommand = new()
			{
				Connection = db,
				CommandText = $"PRAGMA table_info({tableName});"
			};
			getAllFieldsCommand.Prepare();
			SqliteDataReader getAllFieldsDataReader = getAllFieldsCommand.ExecuteReader();

			ObservableCollection<DatabaseTreeItem> fieldItems = new();
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
				DatabaseTreeItem tableGroup = GetDatabaseTableChilds(db, rootItem);
				rootItem.Children = new ObservableCollection<DatabaseTreeItem> { tableGroup };
				list.Add(rootItem);

				db.Dispose();
			}
			return list;
		}

		public static ObservableCollection<Database> GetAvailableDatabases()
		{
			ObservableCollection<Database> list = new();
			List<string> dbPaths = Directory.EnumerateFiles(ApplicationData.Current.LocalFolder.Path, "*", SearchOption.TopDirectoryOnly).Where(search => search.EndsWith(".db")).ToList();

			foreach (string dbPath in dbPaths)
			{
				list.Add(new Database() { Name = dbPath.Split("\\").ToList().Last().Split('.').ToList()[0], Path = dbPath });
			}
			return list;
		}

		public static Table ExecuteAnyQuery(string dbpath, string query)
		{
			using SqliteConnection db = new($"Filename={dbpath}");
			db.Open();
			SqliteCommand command = new(query, db);
			SqliteDataReader sqliteDataReader = command.ExecuteReader();

			List<string> tableNames = new();
			Dictionary<int, List<string>> rows = null;

			for (int i = 0; i < sqliteDataReader.FieldCount; i++)
			{
				tableNames.Add(sqliteDataReader.GetName(i));
			}

			int row = 1;
			while (sqliteDataReader.Read())
			{
				rows ??= new Dictionary<int, List<string>>();

				List<string> values = new();
				for (int i = 0; i < sqliteDataReader.FieldCount; i++)
				{
					values.Add(sqliteDataReader.IsDBNull(i) == true ? "" : sqliteDataReader.GetString(i));
				}

				rows.Add(row, values);
				row++;
			}

			db.Dispose();
			return new Table() { Tables = tableNames, Rows = rows };
		}

		internal static void DeleteTable(Database db, string tableName)
		{
			using SqliteConnection sqliteConnection = new($"Filename={db.Path}");
			sqliteConnection.Open();
			SqliteCommand cmd = new($"drop TABLE {tableName};", sqliteConnection);
			cmd.ExecuteNonQuery();
			sqliteConnection.Dispose();
		}
	}
}
