using System.Collections.Generic;

namespace SQLiteFluent.Core.Interfaces
{
	public interface ITable
	{
		List<string> Tables { get; set; }
		Dictionary<int, List<string>> Rows { get; set; }
		string TablesToString();
		string RowsToString();
	}
}
