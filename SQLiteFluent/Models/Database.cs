using CommunityToolkit.Mvvm.ComponentModel;
using SQLiteFluent.Core.Interfaces;

namespace SQLiteFluent.Models
{
	public class Database : ObservableObject, IDatabase
	{
		private string _name;

		public string Name
		{
			get { return _name; }
			set { _name = value; OnPropertyChanged(nameof(Name)); }
		}

		private string _path;
		public string Path
		{
			get { return _path; }
			set { _path = value; OnPropertyChanged(nameof(Path)); }
		}

		public override bool Equals(object obj)
		{
			if ((Database)obj == null)
			{
				return false;
			}
			return this.Name == ((Database)obj).Name;
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
