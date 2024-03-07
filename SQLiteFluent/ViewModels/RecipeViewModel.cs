using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.Models;
using SQLiteFluent.Views.Recipes;
using SQLiteFluent.Views.Recipes.Advanced;
using System;
using System.Collections.ObjectModel;

namespace SQLiteFluent.ViewModels
{
	public class RecipeViewModel : ObservableObject
	{
		private readonly ObservableCollection<PaneNavLink> _items = new()
		{
			new PaneNavLink() { Name = "Select", CodeGlyph = "\uE734", Page = typeof(SelectPage) },
			new PaneNavLink() { Name = "Update", CodeGlyph = "\uE734", Page = typeof(UpdatePage) },
			new PaneNavLink() { Name = "Delete", CodeGlyph = "\uE734", Page = typeof(DeletePage) },
			new PaneNavLink() { Name = "Drop", CodeGlyph = "\uE734", Page = typeof(DropPage) },
			new PaneNavLink() { Name = "Create", CodeGlyph = "\uE734", Page = typeof(CreatePage) },
		};

		public ObservableCollection<PaneNavLink> ItemSource { get { return _items; } }

		private readonly ObservableCollection<PaneNavLink> _itemsAdvanced = new()
		{
			new PaneNavLink() { Name = "Pragma", CodeGlyph = "\uE734", Page = typeof(PragmaPage) },
		};

		public ObservableCollection<PaneNavLink> ItemSourceAdvanced { get { return _itemsAdvanced; } }

		public IRelayCommand OpenPaneNavLinkCommand { get; private set; }

		public Frame ContentPaneFrame { get; set; }

		public RecipeViewModel()
		{
			OpenPaneNavLinkCommand = new RelayCommand<Type>(OpenPaneNavLink);
		}

		public void OpenPaneNavLink(Type type)
		{
			this.ContentPaneFrame.Navigate(type);
		}
	}
}
