using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Views;

namespace Xamarin.Forms.Platform.Android
{
	public class MasterDetailContainer : ViewGroup
	{
		#region config

		private const int _defaultMasterSize = 320;

		#endregion

		#region propreties

		public VisualElement ChildView => _childView;

		#endregion

		#region vars

		private readonly bool _isMaster;
		private MasterDetailPage _parent;
		private VisualElement _childView;
		private double? _statusBarPxHeight;

		#endregion

		#region create and destroy

		public MasterDetailContainer(bool isMaster, Context context) : base(context)
		{
			_isMaster = isMaster;
		}

		public MasterDetailContainer(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_parent = null;
				RemoveAllViews();

				if (_childView != null)
				{
					Platform.GetRenderer(_childView)?.Dispose();
					_childView = null;
				}
			}

			base.Dispose(disposing);
		}

		#endregion

		#region manage subview

		public void SetContent(MasterDetailPage parent)
		{
			_parent = parent;

			VisualElement view = (_isMaster) ? parent.Master : parent.Detail;
			if (_childView != view)
			{
				RemoveAllViews();

				if (_childView != null)
				{
					Platform.GetRenderer(_childView)?.Dispose();
					_childView = null;
				}

				if (view != null)
				{
					_childView = view;
					addChildView(_childView);
				}
			}
		}

		private void addChildView(VisualElement childView)
		{
			IVisualElementRenderer renderer = Platform.GetRenderer(childView);

			if (renderer == null)
			{
				Platform.SetRenderer(childView, renderer = Platform.CreateRenderer(childView));
			}

			if (renderer.ViewGroup.Parent != this)
			{
				if (renderer.ViewGroup.Parent != null)
				{
					renderer.ViewGroup.RemoveFromParent();
				}

				AddView(renderer.ViewGroup);
				renderer.UpdateLayout();
			}
		}

		#endregion

		#region layout

		public void MeasureAndLayoutNative()
		{
			if (_childView != null)
			{
				IVisualElementRenderer renderer = Platform.GetRenderer(_childView);
				if (renderer.ViewGroup != null)
				{
					var nativeView = renderer.ViewGroup;

					var w = MeasureSpec.MakeMeasureSpec(nativeView.MeasuredWidth, MeasureSpecMode.Exactly);
					var h = MeasureSpec.MakeMeasureSpec(nativeView.MeasuredHeight, MeasureSpecMode.Exactly);
					nativeView.Measure(w, h);
					nativeView.Layout(nativeView.Left, nativeView.Top, nativeView.Right, nativeView.Bottom);
				}
			}
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			if (changed && _childView != null)
			{
				IMasterDetailPageController masterDetailPageController = (_parent as IMasterDetailPageController);
				if (masterDetailPageController != null)
				{
					if (_isMaster)
					{
						masterDetailPageController.MasterBounds = getMasterBounds(l, t, r, b);
					}
					else
					{
						masterDetailPageController.DetailBounds = getDetailBounds(l, t, r, b);
					}
				}

				Platform.GetRenderer(_childView)?.UpdateLayout();
			}
		}

		private double getOffsetY()
		{
			if (_statusBarPxHeight == null)
			{
				int resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
				if (resourceId > 0)
				{
					_statusBarPxHeight = Resources.GetDimensionPixelSize(resourceId);
				}
			}

			return Context.FromPixels((double)_statusBarPxHeight);
		}

		private Rectangle getMasterBounds(int left, int top, int right, int bottom)
		{
			double screenWidth = Context.FromPixels(right - left);
			double maxMenuWidth = screenWidth * 0.9f;

			double x = 0;
			double y = getOffsetY();
			double width = Math.Min(_defaultMasterSize, maxMenuWidth);
			double height = Context.FromPixels(bottom - top) - y;

			return new Rectangle(x, y, width, height);
		}

		private Rectangle getDetailBounds(int left, int top, int right, int bottom)
		{
			double x = 0;
			double y = getOffsetY();
			double width = Context.FromPixels(right - left);
			double height = Context.FromPixels(bottom - top) - y;

			return new Rectangle(x, y, width, height);
		}

		#endregion

		#region touch events

		public override bool OnInterceptTouchEvent(MotionEvent ev)
		{
			if (!_isMaster && (_parent != null && _parent.IsPresented))
			{
				return true;
			}

			return base.OnInterceptTouchEvent(ev);
		}

		#endregion
	}
}