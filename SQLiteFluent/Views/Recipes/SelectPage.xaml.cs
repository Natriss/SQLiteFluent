using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.ViewModels.Recipes;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SQLiteFluent.Views.Recipes
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SelectPage : Page
	{
		public SelectViewModel ViewModel { get { return this.DataContext as SelectViewModel; } }
		public SelectPage()
		{
			this.InitializeComponent();
			this.DataContext = new SelectViewModel();
		}
	}
}
