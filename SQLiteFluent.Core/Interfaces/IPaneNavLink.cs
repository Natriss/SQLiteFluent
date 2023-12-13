using System;

namespace SQLiteFluent.Core.Interfaces
{
	public interface IPaneNavLink
	{
		string Name { get; set; }
		string CodeGlyph { get; set; }
		Type Page { get; set; }
	}
}
