using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.Models;
using SQLiteFluent.ViewModels.Dialogs;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SQLiteFluent.Views.Dialogs
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class AddColumnIntoTableDialog : Page
	{
		public AddColumnIntoTableDialogViewModel ViewModel { get { return this.DataContext as AddColumnIntoTableDialogViewModel; } }
		public AddColumnIntoTableDialog()
		{
			this.InitializeComponent();
			this.DataContext = new AddColumnIntoTableDialogViewModel();
		}
	}
}
