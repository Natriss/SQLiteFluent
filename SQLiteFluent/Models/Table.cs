using System.Collections.ObjectModel;

namespace SQLiteFluent.Models
{
	public class Table
	{
		public ObservableCollection<string> Columns { get; set; }
		public ObservableCollection<ObservableCollection<string>> Rows { get; set; }
	}
}
