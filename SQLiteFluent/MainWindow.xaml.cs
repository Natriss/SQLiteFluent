using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using SQLiteFluent.Helpers;
using SQLiteFluent.Views;
using System;
using Rect = Windows.Foundation.Rect;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SQLiteFluent
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : Window
	{
		public IRelayCommand GoBackCommand { get; private set; }
		public MainWindow()
		{
			this.InitializeComponent();
			ExtendsContentIntoTitleBar = true;
			Title = "ApplicationName".GetLocalized();
			SetTitleBar(appTitleBar);
			SystemBackdrop = new MicaBackdrop();

			NavigationHelper.SetRootFrame(rootFrame);
			NavigationHelper.NavigateTo(typeof(MainPage));

			GoBackCommand = new RelayCommand(NavigationHelper.GoBack);

			appTitleBar.Loaded += AppTitleBar_Loaded;
			appTitleBar.SizeChanged += AppTitleBar_SizeChanged;
		}

		private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			SetIneractableRegionsInCustomTitleBar();
		}

		private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
		{
			SetIneractableRegionsInCustomTitleBar();
		}

		private void SetIneractableRegionsInCustomTitleBar()
		{
			double rasterizationScale = appTitleBar.XamlRoot.RasterizationScale;

			GeneralTransform transform = backButton.TransformToVisual(null);
			Rect bounderies = transform.TransformBounds(new Rect(0, 0, backButton.ActualWidth, backButton.ActualHeight));

			Windows.Graphics.RectInt32 BackButton = GetRect(bounderies, rasterizationScale);

			var rectArray = new Windows.Graphics.RectInt32[] { BackButton };

			InputNonClientPointerSource nonClientInputSrc = InputNonClientPointerSource.GetForWindowId(this.AppWindow.Id);
			nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, rectArray);
		}

		private Windows.Graphics.RectInt32 GetRect(Rect bounds, double scale)
		{
			return new Windows.Graphics.RectInt32(
				_X: (int)Math.Round(bounds.X * scale),
				_Y: (int)Math.Round(bounds.Y * scale),
				_Width: (int)Math.Round(bounds.Width * scale),
				_Height: (int)Math.Round(bounds.Height * scale)
			);
		}
	}
}
