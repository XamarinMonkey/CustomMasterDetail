using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace CustomMasterDetail.Droid
{
	public class ViewerPageButton : RelativeLayout
	{
		public delegate void ViewerPageButtonHandler(ViewerPageButton button);
		public event ViewerPageButtonHandler OnClick;

		private View _backgroundView => FindViewById<View>(Resource.Id.BackgroundView);
		private TextView _numberButton => FindViewById<TextView>(Resource.Id.NumberButton);

		private GestureDetector _gestureDetector;
		private int _pageNumber;

		public int PageNumber
		{
			get { return _pageNumber; }
			set 
			{ 
				Tag = value;
				_pageNumber = value; 
				_numberButton.Text = _pageNumber.ToString();
				ContentDescription = String.Format("ViewerPagination.PageBtn[{0}]", value);
			}
		}

		public string Text
		{
			get { return _numberButton.Text; }
			set { _numberButton.Text = value; }
		}

		public ViewerPageButton(Context context) : base(context)
		{
			View.Inflate(context, Resource.Layout.ViewerPageButtonLayout, this);
			_gestureDetector = new GestureDetector(context, new SingleTapGestureDetector(handleSingleTap));
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			_gestureDetector.OnTouchEvent(e);
			return true;
		}

		private void handleSingleTap()
		{
			OnClick?.Invoke(this);
		}

		public void SetActive(bool active)
		{
			if(active)
			{
				_backgroundView.Visibility = ViewStates.Visible;
				_numberButton.SetTextColor(Color.Green);
			}
			else
			{
				_backgroundView.Visibility = ViewStates.Invisible;
				_numberButton.SetTextColor(Color.White);
			}
		}
	}
}