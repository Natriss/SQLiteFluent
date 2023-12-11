using CommunityToolkit.Mvvm.ComponentModel;
using SQLiteFluent.Core.Interfaces;
using System;

namespace SQLiteFluent.Models
{
	public class PaneNavLink : ObservableObject, IPaneNavLink
	{
		private string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		private string _codeGlyph;
		public string CodeGlyph
		{
			get { return _codeGlyph; }
			set
			{
				_codeGlyph = value;
				OnPropertyChanged(nameof(CodeGlyph));
			}
		}

		private Type _page;
		public Type Page
		{
			get { return _page; }
			set
			{
				_page = value;
				OnPropertyChanged(nameof(Page));
			}
		}
	}
}
