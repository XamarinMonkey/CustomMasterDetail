using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace CustomMasterDetail.Droid
{
	public class ItemContentLayout : RelativeLayout, INativeItemView
	{
		private TextView _nameTextView => FindViewById<TextView>(Resource.Id.NameTextView);
		private ViewerPagination _pagination => FindViewById<ViewerPagination>(Resource.Id.Pagination);

		public ItemContentLayout(Context context) : base(context)
		{
			View.Inflate(context, Resource.Layout.ItemContentLayout, this);
		}

		public async Task LoadItemAsync(string name)
		{
			await Task.Delay(100);

			_nameTextView.Text = name;
			_pagination.LoadPageButtons(6, 1);
		}
	}
}
