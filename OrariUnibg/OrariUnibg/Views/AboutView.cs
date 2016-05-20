using System;
using Xamarin.Forms;
using OrariUnibg.Helpers;

namespace OrariUnibg
{
	public class AboutView : ContentPage
	{
		public AboutView ()
		{
			BackgroundColor = ColorHelper.White;
			Title = "About us";
			Content = getView ();
		}

		#region Private Fields
		private Label _lblCreate;
		#endregion

		#region Private Methods
		private View getView()
		{
			_lblCreate = new Label () {
				Text = "Autore: Lorenzo Giudici",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Medium, this)
			};
				
			var layout = new StackLayout () {
				
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Vertical,
				Spacing = 15,

				Children = {
					new StackLayout(){
						Padding = new Thickness(20, 20, 20, 40),
						Orientation = StackOrientation.Vertical,
						BackgroundColor = ColorHelper.Blue700,
						Spacing = 10,
						Children ={
							new Image(){Source = "ic_flatOK.png", HorizontalOptions = LayoutOptions.CenterAndExpand, Aspect = Aspect.AspectFit},
							new Label() {Text = "UnibgOrari", TextColor = ColorHelper.White, HorizontalOptions = LayoutOptions.CenterAndExpand, FontSize = Device.GetNamedSize(NamedSize.Large, this)},
							new Label() {Text = string.Format("Versione {0}", Settings.Versione), TextColor = ColorHelper.White, HorizontalOptions = LayoutOptions.CenterAndExpand, FontSize = Device.GetNamedSize(NamedSize.Medium, this)},

						}
					},
					_lblCreate,

				}
			};

			return layout;
		}
		#endregion
	}
}

