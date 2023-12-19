using Microsoft.UI.Xaml;
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
	public sealed partial class InsertDataIntoTableDialog : Page
    {
		public InsertDataIntoTableDialogViewModel ViewModel {  get { return this.DataContext as InsertDataIntoTableDialogViewModel; } }

        public InsertDataIntoTableDialog(ObservableCollection<DatabaseTreeItem> tableColumns)
        {
            this.InitializeComponent();
			this.DataContext = new InsertDataIntoTableDialogViewModel(content);
			ViewModel.FillStackPanel(tableColumns);
		}
    }
}
