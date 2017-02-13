using System;
using Android.Views;

namespace CustomMasterDetail.Droid
{
	public class SingleTapGestureDetector : GestureDetector.SimpleOnGestureListener
	{
		private Action _singleTapAction;

		public SingleTapGestureDetector(Action singleTapAction)
		{
			_singleTapAction = singleTapAction;
		}

		public override bool OnSingleTapUp (MotionEvent e)
		{
			_singleTapAction?.Invoke();
			return base.OnSingleTapUp (e);
		}
	}
}