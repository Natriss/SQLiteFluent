using Microsoft.Data.Sqlite;
using SQLiteFluent.Enums;
using SQLiteFluent.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace SQLiteFluent.Models
{
	public static class DataAccess
	{
		private static SqliteConnection GetConnection(string databaseName, bool isAlreadyPath = false)
		{
			string db = isAlreadyPath ? databaseName : GetDatabasePath(databaseName);
			return new SqliteConnection($"Filename={db}");
		}

		private static string GetDatabasePath(string databaseName)
		{
			return Path.Combine(ApplicationData.Current.LocalFolder.Path, $"{databaseName}.db");
		}

		public static bool DoesFileExistAsync(string name)
		{
			return File.Exists(Path.Combine(ApplicationData.Current.LocalFolder.Path, name));
		}

		public static async void CreateDatabase(string name)
		{
			await ApplicationData.Current.LocalFolder.CreateFileAsync($"{name}.db", CreationCollisionOption.OpenIfExists);
		}

		public static void ImportDatabase(string path)
		{
			string name = path.Trim(new char[] { '\"' }).Split("\\").ToList().Last();
			File.Copy(path.Trim(new char[] { '\"' }), Path.Combine(ApplicationData.Current.LocalFolder.Path, name), true);
			TestConnectionAfterImport(Path.Combine(ApplicationData.Current.LocalFolder.Path, name));
		}

		public static void DeleteDatabase(string name)
		{
			SqliteConnection.ClearPool(GetConnection(name));
			File.Delete(GetDatabasePath(name));
			RefreshDatabases();
		}

		public static void RenameDatabase(string oldName, string newName)
		{
			SqliteConnection.ClearPool(GetConnection(oldName));
			File.Move(GetDatabasePath(oldName), GetDatabasePath(newName));
			RefreshDatabases();
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
				DataBase = rootItem,
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
				using SqliteConnection db = GetConnection(database, true);
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
			using SqliteConnection db = GetConnection(dbpath, true);
			try
			{
				db.Open();
				SqliteCommand command = new(query, db)
				{
					Transaction = db.BeginTransaction()
				};
				SqliteDataReader sqliteDataReader = command.ExecuteReader();
				Table table = GetTable(sqliteDataReader);
				db.Dispose();
				return table;
			}
			catch (Exception e)
			{
				db.Close();
				throw new Exception(e.Message);
			}
			finally
			{
				db.Dispose();
			}
		}

		private static Table GetTable(SqliteDataReader sqliteDataReader)
		{
			ObservableCollection<string> tableNames = new();
			for (int i = 0; i < sqliteDataReader.FieldCount; i++)
			{
				tableNames.Add(sqliteDataReader.GetName(i));
			}

			ObservableCollection<ObservableCollection<string>> rows = new();
			while (sqliteDataReader.Read())
			{
				ObservableCollection<string> row = new();
				for (int i = 0; i < sqliteDataReader.FieldCount; i++)
				{
					row.Add(sqliteDataReader.IsDBNull(i) == true ? null : sqliteDataReader.GetValue(i).ToString());
				}
				rows.Add(row);
			}
			return new Table() { Columns = tableNames, Rows = rows };
		}

		public static void DeleteTable(Database db, string tableName)
		{
			using SqliteConnection sqliteConnection = GetConnection(db.Path, true);
			sqliteConnection.Open();
			SqliteCommand cmd = new($"drop TABLE {tableName};", sqliteConnection);
			cmd.ExecuteNonQuery();
			sqliteConnection.Dispose();
		}

		public static Table GetTop1000TableItems(Database db, string tableName)
		{
			using SqliteConnection sqliteConnection = GetConnection(db.Path, true);
			sqliteConnection.Open();
			SqliteCommand cmd = new($"SELECT * FROM {tableName} LIMIT 1000;", sqliteConnection);
			SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
			Table table = GetTable(sqliteDataReader);
			sqliteConnection.Dispose();
			return table;
		}

		public static void InsertDataIntoTable(Database db, string tableName, Dictionary<string, string> data)
		{
			string[] columns = data.Keys.ToArray();
			string[] values = data.Values.ToArray();
			using SqliteConnection sqliteConnection = GetConnection(db.Path, true);
			sqliteConnection.Open();
			SqliteCommand cmd = new($"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES (\"{string.Join("\", \"", values)}\");", sqliteConnection);
			cmd.ExecuteNonQuery();
			sqliteConnection.Dispose();
		}

		public static ObservableCollection<DatabaseTreeItem> RefreshTables(DatabaseTreeItem selectedItem)
		{
			SqliteConnection sqliteConnection = GetConnection(selectedItem.DataBase.Name);
			sqliteConnection.Open();
			ObservableCollection<DatabaseTreeItem> newChilds = GetDatabaseTableChilds(sqliteConnection, selectedItem.DataBase).Children;
			sqliteConnection.Dispose();
			return newChilds;
		}

		public static void RefreshDatabases()
		{
			RefreshDatabasesList();
			RefreshDatabaseTreeView();
		}

		private static void RefreshDatabasesList()
		{
			ObservableCollection<Database> newDatabasesList = GetAvailableDatabases();
			List<Database> oldDatabasesList = AppHelpers.Databases.ToList();
			foreach (Database db in oldDatabasesList)
			{
				if (newDatabasesList.Contains(db))
				{
					continue;
				}
				AppHelpers.Databases.Remove(db);
			}
			foreach (Database db in newDatabasesList)
			{
				if (oldDatabasesList.Contains(db))
				{
					continue;
				}
				AppHelpers.Databases.Add(db);
			}
		}

		private static void RefreshDatabaseTreeView()
		{
			ObservableCollection<DatabaseTreeItem> newList = GetAllData();
			List<DatabaseTreeItem> oldList = AppHelpers.DataSource.ToList();
			foreach (DatabaseTreeItem db in oldList)
			{
				if (newList.Contains(db))
				{
					continue;
				}
				AppHelpers.DataSource.Remove(db);
			}
			foreach (DatabaseTreeItem db in newList)
			{
				if (oldList.Contains(db))
				{
					continue;
				}
				AppHelpers.DataSource.Add(db);
			}
		}

		public static void RenameTable(Database db, string oldName, string newName)
		{
			using SqliteConnection sqliteConnection = GetConnection(db.Path, true);
			sqliteConnection.Open();
			SqliteCommand cmd = new($"ALTER TABLE {oldName} RENAME TO {newName};", sqliteConnection);
			cmd.ExecuteNonQuery();
			sqliteConnection.Dispose();
		}

		public static void ClearTable(Database db, string tableName)
		{
			using SqliteConnection sqliteConnection = GetConnection(db.Path, true);
			sqliteConnection.Open();
			SqliteCommand cmd = new($"DELETE FROM {tableName};", sqliteConnection);
			cmd.ExecuteNonQuery();
			sqliteConnection.Dispose();
		}

		public static void AddColumnIntoTable(Database db, string tableName, string columnName, ColumnDeclaredType type, bool isDefaultChecked, string defaultValue)
		{
			using SqliteConnection sqliteConnection = GetConnection(db.Path, true);
			sqliteConnection.Open();
			string queryDefaultValue = (type == ColumnDeclaredType.TEXT ? $"'{defaultValue}'" : defaultValue);
			string queryDefault = isDefaultChecked ? $" NOT NULL DEFAULT {queryDefaultValue}" : null;
			string query = $"ALTER TABLE {tableName} ADD COLUMN {columnName} {Enum.GetName<ColumnDeclaredType>(type)}{queryDefault};";
			SqliteCommand cmd = new(query, sqliteConnection);
			cmd.ExecuteNonQuery();
			sqliteConnection.Dispose();
		}

		public static void RefreshTableFields(Database db, DatabaseTreeItem selectedItem)
		{
			using SqliteConnection sqliteConnection = GetConnection(db.Path, true);
			sqliteConnection.Open();
			selectedItem.Children.Clear();
			foreach (DatabaseTreeItem item in GetDatabaseTableFields(selectedItem.Name, sqliteConnection))
			{
				selectedItem.Children.Add(item);
			}
			sqliteConnection.Dispose();
		}

		private static void TestConnectionAfterImport(string path)
		{
			using SqliteConnection sqliteConnection = GetConnection(path, true);
			try
			{
				sqliteConnection.Open();
				SqliteCommand cmd = new($"pragma quick_check;", sqliteConnection);
				cmd.ExecuteNonQuery();

			}
			catch (Exception e)
			{
				sqliteConnection.Dispose();
				SqliteConnection.ClearPool(sqliteConnection);
				File.Delete(path);
				throw new Exception(e.Message);
			}
			finally
			{
				sqliteConnection.Dispose();
			}
		}
	}
}
