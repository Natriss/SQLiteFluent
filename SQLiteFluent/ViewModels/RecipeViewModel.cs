using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SQLiteFluent.Data.File;
using SQLiteFluent.Models;
using SQLiteFluent.Views.Recipes;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace SQLiteFluent.ViewModels
{
	public class RecipeViewModel : ObservableObject
	{
		private readonly ObservableCollection<PaneNavLink> _items = new()
		{
			new PaneNavLink() { Name = "Select", CodeGlyph = "\uE734", Page = typeof(SelectPage) }
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
