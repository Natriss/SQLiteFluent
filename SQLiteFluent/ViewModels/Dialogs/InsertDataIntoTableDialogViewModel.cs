using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.Helpers;
using SQLiteFluent.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SQLiteFluent.ViewModels.Dialogs
{
	public class InsertDataIntoTableDialogViewModel
	{
		private ObservableCollection<UIElement> _uIElements = new();
		public StackPanel StackPanel { get; private set; }

		public InsertDataIntoTableDialogViewModel(StackPanel stackPanel)
		{
			StackPanel = stackPanel;
		}

		public void FillStackPanel(ObservableCollection<DatabaseTreeItem> tableColumns)
		{
			foreach (DatabaseTreeItem column in tableColumns)
			{
				GenerateForm(column);
			}
			StackPanel.Children.Clear();
			foreach (UIElement element in _uIElements)
			{
				StackPanel.Children.Add(element);
			}
		}

		private void GenerateForm(DatabaseTreeItem column)
		{
            if (SettingsHelper.AutoIncrement && column.IsPrimaryKey)
            {
				return;
            }

            Grid grid = new();
			RowDefinition rowDefinitionTitle = new()
			{
				Height = GridLength.Auto
			};
			RowDefinition rowDefinitionValue = new()
			{
				Height = GridLength.Auto
			};
			grid.RowDefinitions.Add(rowDefinitionTitle);
			grid.RowDefinitions.Add(rowDefinitionValue);

			TextBlock textBlock = new()
			{
				Text = $"{column.Name} - {column.FieldType}",
			};
			Grid.SetRow(textBlock, 0);

			TextBox textBox = new()
			{
				Name = column.Name,
				PlaceholderText = column.FieldDefaultValue,
			};
			Grid.SetRow(textBox, 1);

			grid.RowSpacing = 8;
			grid.Children.Add(textBlock);
			grid.Children.Add(textBox);
			_uIElements.Add(grid);
		}

		public Dictionary<string, string> GetTableValues()
		{
			Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
			UIElementCollection grids = StackPanel.Children;
			foreach (Grid element in grids.Cast<Grid>())
			{
				UIElement textBox = element.Children.Where(item => item.GetType() == typeof(TextBox)).First();
				keyValuePairs.Add(((TextBox)textBox).Name, string.IsNullOrWhiteSpace(((TextBox)textBox).Text) ? ((TextBox)textBox).PlaceholderText : ((TextBox)textBox).Text);
			}
			return keyValuePairs;
		}
	}
}
