using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomMasterDetail
{
	public static class NavigationManager
	{
		public static MainMasterDetailPage MainPage => (App.Current.MainPage as MainMasterDetailPage);
		public static MainNavigationPage DetailPage => (MainPage.Detail as MainNavigationPage);

		public static void OpenMenu()
		{
			MainPage.IsPresented = true;
		}

		public static async Task ShowItemAsync(string itemName)
		{
			var itemPage = new ItemContentPage(itemName);
			await DetailPage.PushAsync(itemPage, true);
		}
	}
}
