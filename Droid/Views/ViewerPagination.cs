using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Animation;

namespace CustomMasterDetail.Droid
{
	public class ViewerPagination : RelativeLayout
	{
		//events

		public event EventHandler OnPageButtonHit;

		//UI

		private RelativeLayout _contentLayout => FindViewById<RelativeLayout>(Resource.Id.ContentLayout);
		private HorizontalScrollView _pageScroll;
		private LinearLayout _scrollContentLayout;
		private ViewerPageButton _activeBtn;
		private ViewerPageButton _nextActiveBtn; //helps to ignore scroll detection after page button pressed

		public ViewerPagination(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			View.Inflate(context, Resource.Layout.ViewerPaginationLayout, this);
			initLargeDisplay(context);
		}

		public void LoadPageButtons(int pageCount, int activePage)
		{
			//add page button or individual page buttons

			addPageButtons(pageCount, activePage);
		}

		public void SetActivePage(int activePage)
		{
			setActivePageLarge(activePage);
		}

		#region large display

		private void initLargeDisplay(Context context)
		{
			var pageScrollLp = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
			pageScrollLp.AddRule(LayoutRules.CenterHorizontal);

			_pageScroll = new HorizontalScrollView(context);
			_pageScroll.LayoutParameters = pageScrollLp;
			_contentLayout.AddView(_pageScroll);

			_scrollContentLayout = new LinearLayout(context);
			_scrollContentLayout.Orientation = Orientation.Horizontal;
			_scrollContentLayout.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
			_pageScroll.AddView(_scrollContentLayout);
		}

		private void addPageButtons(int pageCount, int activePage)
		{
			_scrollContentLayout.RemoveAllViewsInLayout();

			for (int i = 1; i <= pageCount; i++)
			{
				ViewerPageButton b = new ViewerPageButton(Context);
				b.PageNumber = i;
				b.OnClick += handlePageBtnClick;

				if(i == activePage)
				{
					_activeBtn = b;
				}

				b.SetActive(_activeBtn == b);

				_scrollContentLayout.AddView(b);
			}
		}

		private void setActivePageLarge(int activePage)
		{
			if(_nextActiveBtn == null)
			{
				Post(() =>
				{
					ViewerPageButton b = (ViewerPageButton)_scrollContentLayout.GetChildAt(activePage - 1);
					setBtnDisplay(b, true);
					scrollToPageBtn(b);
				});
			}
			else if(_nextActiveBtn.PageNumber == activePage)
			{
				_nextActiveBtn = null;
			}
		}

		private void handlePageBtnClick(ViewerPageButton b)
		{
			if (b != _activeBtn && _nextActiveBtn == null)
			{
				_nextActiveBtn = b;

				setBtnDisplay(b, true);
				scrollToPageBtn(b);

				_pageScroll.Post(() =>
				{
					OnPageButtonHit?.Invoke(b, EventArgs.Empty);
				});
			}
		}

		private bool scrollToPageBtn(ViewerPageButton b)
		{
			if (_scrollContentLayout.Width > _pageScroll.Width)
			{
				int scrollX = 0;

				if (b.Left >= (_pageScroll.Width / 2.0f))
					scrollX = (int)((b.Left + (b.Width / 2.0f)) - (_pageScroll.Width / 2.0f));

				_pageScroll.ScrollTo(scrollX, 0);

				return (scrollX != _pageScroll.ScrollX);
			}

			return false;
		}

		private void setBtnDisplay(ViewerPageButton b, bool active)
		{
			if (active)
			{
				if (_activeBtn != b)
				{
					_activeBtn?.SetActive(false);
				}
				_activeBtn = b;
			}

			b?.SetActive(active);
		}

		#endregion

	}
}