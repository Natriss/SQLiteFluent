using SQLiteFluent.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SQLiteFluent.Models
{
	public class Table : ITable
	{
		public List<string> Tables { get; set; }
		public Dictionary<int, List<string>> Rows { get; set; }

		public string TablesToString()
		{
			if (this.Tables == null || this.Tables.Count == 0)
			{
				return string.Empty;
			}
			return string.Join(" | ", Tables.ToArray());
		}

		public string RowsToString()
		{
			if (this.Rows == null || this.Rows.Count == 0)
			{
				return string.Empty;
			}
			string rowsFormatted = "";
			foreach (var row in this.Rows)
			{
				List<string> list = row.Value;
				rowsFormatted += string.Join(" | ", list.ToArray());
				rowsFormatted += '\n';
			}
			return rowsFormatted;
		}
	}
}
