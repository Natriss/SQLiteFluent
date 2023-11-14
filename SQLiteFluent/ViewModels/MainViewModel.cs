using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace SQLiteFluent.ViewModels
{
	public class MainViewModel
	{
		public ObservableCollection<TreeViewNode> Nodes { get; private set; }

		public MainViewModel()
		{
			Nodes = new ObservableCollection<TreeViewNode>
			{
				new TreeViewNode() { Content = "Databases" }
			};
		}
	}
}
