using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.ViewModels.Dialogs;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SQLiteFluent.Views.Dialogs
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class AddDatabaseDialog : Page
	{
		public AddDatabaseDialogViewModel ViewModel {  get { return this.DataContext as AddDatabaseDialogViewModel; } }
		public AddDatabaseDialog()
		{
			this.InitializeComponent();
			this.DataContext = new AddDatabaseDialogViewModel();
		}
	}
}
