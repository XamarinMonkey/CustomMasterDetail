using System;
using System.ComponentModel;
using Android.App;
using Android.Support.V4.Widget;
using Android.Views;
using AView = Android.Views.View;

namespace Xamarin.Forms.Platform.Android
{
	public class MainMasterDetailRenderer : DrawerLayout, IVisualElementRenderer, DrawerLayout.IDrawerListener
	{
		#region config

		private const uint _defaultScrimColor = 0x99000000;

		#endregion

		#region events

		//IVisualElementRenderer events

		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		#endregion

		#region properties

		public bool Presented
		{
			get { return _presented; }
			set
			{
				if(_page.IsEnabled)
				{
					if(value != _presented)
					{
						_presented = value;

						if (_presented)
						{
							OpenDrawer(_masterLayout);
						}
						else
						{
							CloseDrawer(_masterLayout);
						}
					}
				}
				else if(_page.IsPresented || _presented)
				{
					_presented = false;
					CloseDrawer(_masterLayout);
				}
			}
		}

		private IMasterDetailPageController _masterDetailPageController => (_page as IMasterDetailPageController);
		private IPageController _masterPageController => (_page.Master as IPageController);
		private IPageController _detailPageController => _page.Detail as IPageController;
		private IPageController _pageController => Element as IPageController;

		//IVisualElementRenderer propreties

		public VisualElement Element => _page;
		public VisualElementTracker Tracker { get; private set; }
		public ViewGroup ViewGroup => this;

		#endregion

		#region vars

		//states

		private int _currentLockMode = -1;
		private bool _isPresentingFromCore;
		private bool _presented;

		//ui

		private MasterDetailPage _page;
		private MasterDetailContainer _detailLayout;
		private MasterDetailContainer _masterLayout;

		#endregion

		#region create and destroy

		public MainMasterDetailRenderer() : base(Forms.Context)
		{
			unchecked
			{
				SetScrimColor((int)_defaultScrimColor);
			}

			//main content view

			_detailLayout = new MasterDetailContainer(false, Context)
			{
				LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
			};
			AddView(_detailLayout);

			//navigation drawer

			_masterLayout = new MasterDetailContainer(true, Context)
			{
				LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
				{
					Gravity = (int)GravityFlags.Start
				}
			};
			AddView(_masterLayout);

			//additional setup

			AddDrawerListener(this);
			Tracker = new VisualElementTracker(this);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				RemoveDrawerListener(this);

				if (Tracker != null)
				{
					Tracker.Dispose();
					Tracker = null;
				}

				if (_detailLayout != null)
				{
					_detailLayout.Dispose();
					_detailLayout = null;
				}

				if (_masterLayout != null)
				{
					_masterLayout.Dispose();
					_masterLayout = null;
				}

				cleanOldElement();
			}

			base.Dispose(disposing);
		}

		private void cleanOldElement()
		{
			if (_page != null)
			{
				_masterDetailPageController.BackButtonPressed -= handleOnBackButtonPressed;
				_page.PropertyChanged -= handlePropertyChanged;
				_page.Appearing -= handleMasterDetailPageAppearing;
				_page.Disappearing -= handleMasterDetailPageDisappearing;
			}

			_page = null;
		}

		#endregion

		#region IVisualElementRenderer functions

		public SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			Measure(widthConstraint, heightConstraint);
			return new SizeRequest(new Size(MeasuredWidth, MeasuredHeight));
		}

		public void SetElement(VisualElement element)
		{
			//clear old element

			var oldElement = _page;
			cleanOldElement();

			//setup new element

			_page = (element as MasterDetailPage);

			//setup new element: subscribe to events

			if (_page != null)
			{
				_masterDetailPageController.BackButtonPressed += handleOnBackButtonPressed;
				_page.PropertyChanged += handlePropertyChanged;
				_page.Appearing += handleMasterDetailPageAppearing;
				_page.Disappearing += handleMasterDetailPageDisappearing;
			}

			//setup new element: update UI

			updateMaster();
			updateDetail();
			updatePanGesture();

			if (_page != null)
			{
				Presented = _page.IsPresented;
			}

			//notify change

			OnElementChanged(oldElement, element);
		}

		public void UpdateLayout()
		{
			Tracker?.UpdateLayout();
		}

		#endregion

		#region DrawerLayout.IDrawerListener functions

		public void OnDrawerClosed(AView drawerView)
		{
			OnMenuClosed();
		}

		public void OnDrawerOpened(AView drawerView)
		{
			OnMenuOpened();
		}

		public void OnDrawerSlide(AView drawerView, float slideOffset)
		{
		}

		public void OnDrawerStateChanged(int newState)
		{
			_presented = IsDrawerVisible(_masterLayout);
			updateIsPresented();
		}

		#endregion

		#region lifecycle

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();
			_pageController.SendAppearing();
		}

		private void handleMasterDetailPageAppearing(object sender, EventArgs e)
		{
			_masterPageController?.SendAppearing();
			_detailPageController?.SendAppearing();
		}

		private void handleOnBackButtonPressed(object sender, BackButtonPressedEventArgs backButtonPressedEventArgs)
		{
			if (IsDrawerOpen((int)GravityFlags.Start))
			{
				if (_currentLockMode != LockModeLockedOpen)
				{
					CloseDrawer((int)GravityFlags.Start);
					backButtonPressedEventArgs.Handled = true;
				}
			}
		}

		private void handleMasterDetailPageDisappearing(object sender, EventArgs e)
		{
			_masterPageController?.SendDisappearing();
			_detailPageController?.SendDisappearing();
		}

		protected override void OnDetachedFromWindow()
		{
			base.OnDetachedFromWindow();
			_pageController.SendDisappearing();
		}

		#endregion

		#region renderer

		protected virtual void OnElementChanged(VisualElement oldElement, VisualElement newElement)
		{
			ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(oldElement, newElement));
		}

		protected virtual void OnMenuOpened()
		{
		}

		protected virtual void OnMenuClosed()
		{
		}

		#endregion

		#region post property change updates

		private void handlePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Master")
			{
				updateMaster();
			}
			else if (e.PropertyName == "Detail")
			{
				updateDetail();
			}
			else if (e.PropertyName == MasterDetailPage.IsPresentedProperty.PropertyName)
			{
				_isPresentingFromCore = true;
				Presented = _page.IsPresented;
				_isPresentingFromCore = false;
			}
			else if (e.PropertyName == MasterDetailPage.IsGestureEnabledProperty.PropertyName)
			{
				updatePanGesture();
			}
		}

		private void updatePanGesture()
		{
			if(_page != null)
			{
				SetDrawerLockMode(_page.IsGestureEnabled ? LockModeUnlocked : LockModeLockedClosed);
			}
		}

		private void updateDetail()
		{
			Context.HideKeyboard(this);
			_detailLayout.SetContent(_page);
		}

		private void updateIsPresented()
		{
			if (_page != null && !_isPresentingFromCore && Presented != _page.IsPresented)
			{
				((IElementController)_page).SetValueFromRenderer(MasterDetailPage.IsPresentedProperty, Presented);
			}
		}

		private void updateMaster()
		{
			_masterLayout.SetContent(_page);
		}

		#endregion
	}
}