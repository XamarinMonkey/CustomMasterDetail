using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CustomMasterDetail
{
	public partial class ItemListContentPage : ContentPage
	{
		private ItemListContentPageViewModel _viewModel => (BindingContext as ItemListContentPageViewModel);

		public ItemListContentPage()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			BindingContext = new ItemListContentPageViewModel();
			InitializeComponent();
		}

		private void handleMenuButtonTapped(object sender, EventArgs e)
		{
			NavigationManager.OpenMenu();
		}

		private async void handleItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (ItemListView.SelectedItem != null)
			{
				await NavigationManager.ShowItemAsync(ItemListView.SelectedItem as string);
				ItemListView.SelectedItem = null;
			}

		}
	}
}
