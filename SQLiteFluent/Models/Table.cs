using System.Collections.Generic;

namespace SQLiteFluent.Models
{
	public class Table
	{
		public List<string> Columns { get; set; }
		public List<List<object>> Rows { get; set; }
	}
}
