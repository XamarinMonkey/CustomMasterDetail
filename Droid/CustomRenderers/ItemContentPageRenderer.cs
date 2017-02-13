using CustomMasterDetail;
using CustomMasterDetail.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Views;

[assembly: ExportRenderer(typeof(ItemContentPage), typeof(ItemContentPageRenderer))]
namespace CustomMasterDetail.Droid
{
	public class ItemContentPageRenderer : PageRenderer
	{
		private ItemContentPage _formsPage;
		private ItemContentLayout _view;

		public ItemContentPageRenderer()
		{
			SetBackgroundColor(Android.Graphics.Color.White);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				dispose();
			}

			if (e.NewElement != null)
			{
				dispose(); //make sure we clear stuff out

				_formsPage = (e.NewElement as ItemContentPage);

				_view = new ItemContentLayout(Context);
				AddView(_view);

				_view.Post(() =>
				{
					if (_formsPage != null && _view != null)
					{
						_formsPage.SetNativeViewAsync(_view);
					}
				});
			}
		}

		private void dispose()
		{
			if (_formsPage != null)
			{
				_formsPage.ClearNativeView();
				_formsPage = null;
			}

			if (_view != null)
			{
				_view.RemoveFromParent();
				_view.Dispose();
				_view = null;
			}
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			base.OnLayout(changed, l, t, r, b);

			if (_view != null)
			{
				var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
				var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

				_view.Measure(msw, msh);
				_view.Layout(0, 0, r - l, b - t);
			}
		}
	}
}
