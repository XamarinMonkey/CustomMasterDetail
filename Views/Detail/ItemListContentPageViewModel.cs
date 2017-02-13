using System;
using System.Collections.Generic;

namespace CustomMasterDetail
{
	public class ItemListContentPageViewModel : BaseViewModel
	{
		public List<string> Items
		{
			get
			{
				var result = new List<string>();
				for (int i = 0; i < 100; i++)
				{
					result.Add(string.Format("Item {0}", (i+1)));
				}
				return result;
			}
		}
	}
}
