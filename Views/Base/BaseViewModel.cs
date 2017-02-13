using System;
using System.ComponentModel;

namespace CustomMasterDetail
{
	public class BaseViewModel : INotifyPropertyChanged
	{

		#region events

		public event PropertyChangedEventHandler PropertyChanged;

		protected void notifyPropertyChanged(object sender, EventArgs e)
		{
			notifyPropertyChanged();
		}

		protected void notifyPropertyChanged()
		{
			notifyPropertyChanged(string.Empty);
		}

		protected void notifyPropertyChanged(string[] propertyNames)
		{
			foreach (var propertyName in propertyNames)
			{
				notifyPropertyChanged(propertyName);
			}
		}


		protected void notifyPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

	}
}
