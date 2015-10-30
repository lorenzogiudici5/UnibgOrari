using System;
using Xamarin.Forms;
using OrariUnibg.Helpers;
using OrariUnibg.Views;

namespace OrariUnibg
{
	public class LoginView : ContentPage
	{
		#region Constructor
		public LoginView ()
		{
			Content = getView ();
		}
		#endregion

		#region Private Fields

		#endregion

		#region Private Methods
		private View getView()
		{
			var _btnNext = new Button () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.EndAndExpand,
				Text = "Fine",
				TextColor = ColorHelper.White,
				BackgroundColor = ColorHelper.Transparent,
			};
			_btnNext.Clicked += _btnNext_Clicked;

			var layout = new StackLayout () {
				BackgroundColor = ColorHelper.Blue700,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness (0, 15, 0, 40),
				Children = {
					new Image () {
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						Source = "splashFlat.png", 
					},
				}
			};

			if (Settings.PrimoAvvio || !Settings.SuccessLogin) //se tutorial da primo avvio aggiungo button per signin
				layout.Children.Add (getLayoutButtons ());
			else //altrimenti è tutorial da impostazioni
				layout.Children.Add (_btnNext);

			Settings.PrimoAvvio = false;

			return layout;
		}

		private StackLayout getLayoutButtons()
		{
			var _btnAccedi = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.EndAndExpand,
				Text = "Accedi",
				FontSize = Device.GetNamedSize(NamedSize.Medium, this),
				TextColor = ColorHelper.Black,
				BackgroundColor = ColorHelper.White,
//				BackgroundColor = ColorHelper.LightBlue500,
			};
			_btnAccedi.Clicked += _btnAccedi_Clicked;

			var _btnRegister = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.EndAndExpand,
				Text = "                 Registrati                 ",
				FontSize = Device.GetNamedSize (NamedSize.Medium, this),
				TextColor = ColorHelper.Black,
				BackgroundColor = ColorHelper.White,
			};
			_btnRegister.Clicked += _btnRegister_Clicked;

			var _btnSalta = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.EndAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Medium, this),
				Text = "Salta",
				TextColor = ColorHelper.Black,
				BackgroundColor = ColorHelper.White,
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
		protected override bool OnBackButtonPressed ()
		{
			DependencyService.Get<IMethods>().Close_App(); //altrmenti nulla
			return true; //ALERT CHIUDERE APP??
		}
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
			Navigation.PopModalAsync ();
		}
		#endregion
	}
}

