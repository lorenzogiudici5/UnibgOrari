using System;
using Xamarin.Forms;
using OrariUnibg.Helpers;

namespace OrariUnibg
{
	public class HomeSkipView : ContentPage
	{
		#region Constructor
		public HomeSkipView ()
		{
			Title = "Home";
			Icon = null;
			Content = getView ();
		}
		#endregion

		#region Private Fields
		private Label _lblLogin;
		private Button _btnLogin;
		#endregion

		#region Private Methods
		private View getView()
		{
			_lblLogin = new Label () {
				Text = "Effettuando il login potrai usufruire di interessanti funzionalità, quali: ",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};

			_btnLogin = new Button()
			{
				VerticalOptions = LayoutOptions.EndAndExpand,
				Text = "Login",
				BackgroundColor = ColorHelper.Blue700,
				TextColor = ColorHelper.White,
				BorderColor = ColorHelper.White,
			};
			_btnLogin.Clicked += (object sender, EventArgs e) => Navigation.PushModalAsync(new LoginView()); //torna alla pagina di LOGIN (no tutorial!)

			var layout = new StackLayout () {
				BackgroundColor = ColorHelper.White,
				Padding = new Thickness(10, 10, 10, 10),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {_lblLogin, _btnLogin}
			};

			return layout;
		}

		protected override bool OnBackButtonPressed ()
		{
			DependencyService.Get<IMethods>().Close_App(); //altrmenti nulla
			return true; //ALERT CHIUDERE APP??
		}
		#endregion
	}
}

