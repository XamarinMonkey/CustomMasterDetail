using System;
using System.Threading.Tasks;

namespace CustomMasterDetail
{
	public interface INativeItemView
	{
		Task LoadItemAsync(string name);
	}
}
