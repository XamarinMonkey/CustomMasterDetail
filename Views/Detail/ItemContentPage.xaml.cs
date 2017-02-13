using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomMasterDetail
{
	public partial class ItemContentPage : ContentPage
	{
		public string ItemName { get; private set; }
		private INativeItemView _nativeView;

		public ItemContentPage(string itemName)
		{
			ItemName = itemName;

			NavigationPage.SetHasNavigationBar(this, false);
			InitializeComponent();
		}

		public void ClearNativeView()
		{
			_nativeView = null;
		}

		public async Task SetNativeViewAsync(INativeItemView nativeView)
		{
			_nativeView = nativeView;

			if (_nativeView != null)
			{
				await _nativeView.LoadItemAsync(ItemName);
			}
		}
	}
}
