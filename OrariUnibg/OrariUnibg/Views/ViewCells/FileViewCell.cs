using System;
using Xamarin.Forms;

namespace OrariUnibg
{
	public class FileViewCell : ViewCell
	{
		#region Constructor
		public FileViewCell (){
			View = getView ();
		}
		#endregion

		#region Private Fields
		Label _lblFile;
		#endregion

		#region Private Methods
		private View getView()
		{
			var layout = new StackLayout () {
			};

			return layout;
		}
		#endregion

	}
}

