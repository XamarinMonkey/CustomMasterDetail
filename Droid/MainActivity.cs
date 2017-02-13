using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CustomMasterDetail.Droid
{
	[Activity
		(
			Label = "CustomMasterDetail",
			Theme = "@style/custom",
			NoHistory = true,
		   	MainLauncher=true ,
			ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
		)]
	public class MainActivity : FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			LoadApplication(new App());

			Xamarin.Forms.Forms.SetTitleBarVisibility(AndroidTitleBarVisibility.Never);
		}
	}
}

