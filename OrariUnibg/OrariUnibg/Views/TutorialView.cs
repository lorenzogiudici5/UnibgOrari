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
			Padding = new Thickness (0, 0, 0, 0);
			BackgroundColor = ColorHelper.Blue700;
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
								Aspect = Aspect.AspectFit,
									Source = img, 
//									VerticalOptions = LayoutOptions.FillAndExpand, 
//									HorizontalOptions = LayoutOptions.FillAndExpand
								},
						}
					}
				};
				this.Children.Add (page);
			}

//			var layout = new StackLayout () {
//				VerticalOptions = LayoutOptions.FillAndExpand,
//				HorizontalOptions = LayoutOptions.FillAndExpand,
//				Padding = new Thickness (0, 15, 0, 40),
//				Children = {
//					new Image () {
//						HorizontalOptions = LayoutOptions.CenterAndExpand,
//						Source = "splash.png", 
//						//									VerticalOptions = LayoutOptions.FillAndExpand, 
//						//									HorizontalOptions = LayoutOptions.FillAndExpand
//					},
//				}
//			};
			this.Children.Add (new LoginView ());

//			this.Children.Add (new ContentPage { 
//				Content = layout
//			});

//			if (Settings.PrimoAvvio) //se tutorial da primo avvio aggiungo button per signin
//				layout.Children.Add (getLayoutButtons ());
//			else //altrimenti è tutorial da impostazioni
//				layout.Children.Add (_btnNext);
				
		}

		private StackLayout getLayoutButtons()
		{
			var _btnAccedi = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.EndAndExpand,
				Text = "Accedi",
				FontSize = Device.GetNamedSize(NamedSize.Medium, this),
				TextColor = ColorHelper.White,
				BackgroundColor = ColorHelper.LightBlue500,
			};
			_btnAccedi.Clicked += _btnAccedi_Clicked;

			var _btnRegister = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.EndAndExpand,
				Text = "             Registrati             ",
				FontSize = Device.GetNamedSize (NamedSize.Medium, this),
				TextColor = ColorHelper.White,
				BackgroundColor = ColorHelper.LightBlue500,
			};
			_btnRegister.Clicked += _btnRegister_Clicked;
								
			var _btnSalta = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.EndAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Medium, this),
				Text = "Salta",
				TextColor = ColorHelper.White,
				BackgroundColor = ColorHelper.LightBlue500,
			};
			_btnSalta.Clicked += _btnSalta_Clicked;
				
			var layout = new StackLayout () {
				Orientation = StackOrientation.Vertical, 
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.EndAndExpand,
				Spacing = 20, 
				Children = { _btnAccedi, _btnRegister, _btnSalta  }
			};
					
			return layout;
		}
			
			
		#endregion

		#region Event Handler
		void _btnRegister_Clicked (object sender, EventArgs e)
		{
			var nav = new NavigationPage (
				new InformationView ()) 
			{
				BarBackgroundColor = ColorHelper.Blue700,
				BarTextColor = ColorHelper.White
			};
			Navigation.PushModalAsync (nav);
		}

		void _btnAccedi_Clicked (object sender, EventArgs e)
		{
			var nav = new MasterDetailView ();
//				new NavigationPage (
//				new MasterDetailView ()) 
//				{
//					BarBackgroundColor = ColorHelper.Blue700,
//					BarTextColor = ColorHelper.White
//				};
			Navigation.PushModalAsync (nav);
		}

		void _btnSalta_Clicked (object sender, EventArgs e)
		{
			var nav = new MasterDetailView ();
//			var nav = new NavigationPage (
//				new MasterDetailView ()) 
//				{
//					BarBackgroundColor = ColorHelper.Blue700,
//					BarTextColor = ColorHelper.White
//				};
			Navigation.PushModalAsync (nav);
		}

		void _btnNext_Clicked (object sender, EventArgs e)
		{
			//IF PRIMO AVVIO -> information View
			//ELSE, l'ho kanciato dalle impostazioni, quindi, POP MODAL 

//			if (Settings.PrimoAvvio) {
//				var nav = new NavigationPage (new InformationView ()) {
//					BarBackgroundColor = ColorHelper.Blue700,
//					BarTextColor = ColorHelper.White
//				};
//
//				Navigation.PushModalAsync (nav);
//			} else
				Navigation.PopModalAsync ();

		}
		#endregion
	}
}

