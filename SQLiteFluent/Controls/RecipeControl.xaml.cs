using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.ViewModels.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SQLiteFluent.Controls
{
	public sealed partial class RecipeControl : UserControl
    {
        public RecipeControlViewModel ViewModel { get { return this.DataContext as RecipeControlViewModel; } }

        public string Query
        {
            get { return (string)GetValue(QueryProperty); }
            set { SetValue(QueryProperty, value); }
        }

		// Using a DependencyProperty as the backing store for Query.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty QueryProperty =
            DependencyProperty.Register("Query", typeof(string), typeof(RecipeControl), new PropertyMetadata(string.Empty));

        public string Information
        {
            get { return (string)GetValue(InformationProperty); }
            set { SetValue(InformationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Information.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InformationProperty =
            DependencyProperty.Register("Information", typeof(string), typeof(RecipeControl), new PropertyMetadata(string.Empty));

        public RecipeControl()
        {
            this.InitializeComponent();
            this.DataContext = new RecipeControlViewModel();
        }
    }
}
