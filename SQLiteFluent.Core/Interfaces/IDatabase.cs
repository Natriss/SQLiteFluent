namespace SQLiteFluent.Core.Interfaces
{
	public interface IDatabase
	{
		string Name { get; set; }
		string Path { get; set; }
	}
}
