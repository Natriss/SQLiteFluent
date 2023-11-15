using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SQLiteFluent.Models
{
	public class DatabaseTreeItemTemplateSelector : DataTemplateSelector
	{
		public DataTemplate DatabaseTemplate { get; set; }
		public DataTemplate TablesGroupTemplate { get; set; }
		public DataTemplate TableTemplate { get; set; }
		public DataTemplate ViewsGroupTemplate { get; set; }
		public DataTemplate ViewTemplate { get; set; }
		public DataTemplate FieldTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item)
		{
			DatabaseTreeItem databaseTreeItem = (DatabaseTreeItem)item;
			return databaseTreeItem.Type switch
			{
				Enums.TreeType.Database => DatabaseTemplate,
				Enums.TreeType.TablesGroup => TablesGroupTemplate,
				Enums.TreeType.Table => TableTemplate,
				Enums.TreeType.ViewsGroup => ViewsGroupTemplate,
				Enums.TreeType.View => ViewTemplate,
				_ => FieldTemplate,
			};
		}
	}
}
