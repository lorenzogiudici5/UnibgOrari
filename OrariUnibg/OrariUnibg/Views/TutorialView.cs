using System;
using Xamarin.Forms;
using System.Collections.Generic;
using OrariUnibg.Helpers;
using OrariUnibg.Views;

namespace OrariUnibg
{
	public class TutorialView : CarouselPage
	{
		#region Constructor
		public TutorialView(){
			BackgroundColor = ColorHelper.Blue;
			Title = "Tutorial";
			getPages ();

		}

		#endregion

		#region Private Fields

		#endregion

		#region Private Methods
		private void getPages()
		{
			List<Page> pages = new List<Page> ();
			String[] images = { "Screen1.png", "Screen2.png" };
			var _btnNext = new Button () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.EndAndExpand,
				Text = "Fine",
				TextColor = ColorHelper.White,
				BackgroundColor = ColorHelper.Transparent,
			};
			_btnNext.Clicked += _btnNext_Clicked;

//			var _btnSkip = new Button () {
//				HorizontalOptions = LayoutOptions.StartAndExpand,
//				Text = "Skip",
//				TextColor = ColorHelper.White,
//				BackgroundColor = ColorHelper.Transparent,
//			};
//
//			_btnSkip.Clicked += _btnSkip_Clicked;

			var endBar = new StackLayout () {
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.EndAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {_btnNext}
			};

			foreach (var img in images) {
				var page = new ContentPage { 
					Content = 
						new StackLayout {
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.FillAndExpand,
							Children = {
								new Image()
								{
								Aspect = Aspect.Fill,
									Source = img, 
//									VerticalOptions = LayoutOptions.FillAndExpand, 
//									HorizontalOptions = LayoutOptions.FillAndExpand
								},
						}
					}
				};
				this.Children.Add (page);
			}

			this.Children.Add (new ContentPage { 
				Content = new StackLayout ()
					{
					VerticalOptions = LayoutOptions.FillAndExpand,
					Padding = new Thickness(0, 15, 0, 10),
						Children = {
						new Image()
							{
							HorizontalOptions = LayoutOptions.CenterAndExpand,
								Source = "splash.png", 
								//									VerticalOptions = LayoutOptions.FillAndExpand, 
								//									HorizontalOptions = LayoutOptions.FillAndExpand
							},
							_btnNext}
				}
			});
				

		}
			
			
		#endregion

		#region Event Handler
		void _btnNext_Clicked (object sender, EventArgs e)
		{
			//IF PRIMO AVVIO -> information View
			//ELSE, l'ho kanciato dalle impostazioni, quindi, POP MODAL 

			if (Settings.PrimoAvvio) {
				var nav = new NavigationPage (new InformationView ()) {
					BarBackgroundColor = ColorHelper.Blue,
					BarTextColor = ColorHelper.White
				};

				Navigation.PushModalAsync (nav);
			} else
				Navigation.PopModalAsync ();

		}
		#endregion
	}
}

