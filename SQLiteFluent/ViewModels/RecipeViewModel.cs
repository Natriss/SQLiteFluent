using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.Models;
using SQLiteFluent.Views.Recipes;
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
		};

		public ObservableCollection<PaneNavLink> ItemSource { get { return _items; } }

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
