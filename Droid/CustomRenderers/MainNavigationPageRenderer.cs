using System;
using Android.Views;
using CustomMasterDetail;
using CustomMasterDetail.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

using AView = Android.Views.View;

[assembly: ExportRenderer(typeof(MainNavigationPage), typeof(MainNavigationPageRenderer))]
namespace CustomMasterDetail.Droid
{
	public class MainNavigationPageRenderer : NavigationPageRenderer 
	{
		public MainNavigationPageRenderer()
		{
			SetBackgroundColor(Android.Graphics.Color.White);
		}

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<NavigationPage> e)
		{
			base.OnElementChanged(e);

			if(e.OldElement != null)
			{
				e.OldElement.Pushed -= handleOnPushed;
			}

			if(e.NewElement != null)
			{
				e.NewElement.Pushed += handleOnPushed;
			}
		}

		private void handleOnPushed(object sender, EventArgs e)
		{
			var w = MeasureSpec.MakeMeasureSpec(MeasuredWidth, MeasureSpecMode.Exactly);
			var h = MeasureSpec.MakeMeasureSpec(MeasuredHeight, MeasureSpecMode.Exactly);
			Measure(w, h);
			Layout(Left, Top, Right, Bottom);
		}
	}
}
