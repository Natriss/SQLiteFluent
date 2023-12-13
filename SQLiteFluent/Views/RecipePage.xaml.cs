using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.Helpers;
using SQLiteFluent.Models;
using SQLiteFluent.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SQLiteFluent.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class RecipePage : Page
	{
		public RecipeViewModel ViewModel { get { return this.DataContext as RecipeViewModel; } }

		public RecipePage()
		{
			this.InitializeComponent();
			this.DataContext = new RecipeViewModel();
			ViewModel.ContentPaneFrame = contentPaneFrame;
		}

		private void ListView_ItemClick(object sender, ItemClickEventArgs e)
		{
			PaneNavLink item = e.ClickedItem as PaneNavLink;
			if (ViewModel.ContentPaneFrame.Content != null && item.Page == ViewModel.ContentPaneFrame.Content.GetType()) { return; }
			ViewModel.OpenPaneNavLink(item.Page);
		}
	}
}
