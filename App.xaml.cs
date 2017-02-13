using Xamarin.Forms;

namespace CustomMasterDetail
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			//bootstrap

			var mainPage = new MainMasterDetailPage();
			mainPage.Master = new MasterContentPage();

			var detailPage = new MainNavigationPage();
			detailPage.PushAsync(new ItemListContentPage());
			mainPage.Detail = detailPage;

			MainPage = mainPage;
		}
	}
}
