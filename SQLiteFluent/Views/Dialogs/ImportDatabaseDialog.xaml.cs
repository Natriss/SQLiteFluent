using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.ViewModels.Dialogs;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SQLiteFluent.Views.Dialogs
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ImportDatabaseDialog : Page
	{
		public ImportDatabaseDialogViewModel ViewModel { get {  return this.DataContext as ImportDatabaseDialogViewModel; } }
		public ImportDatabaseDialog()
		{
			this.InitializeComponent();
			this.DataContext = new ImportDatabaseDialogViewModel();
		}
	}
}