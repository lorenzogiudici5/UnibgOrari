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
            _lblLogin = new Label() {
                Text = "Effettua il login per sfruttare al meglio UnibgOrari!",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                TextColor = ColorHelper.Green500,
            };

            var str1 = "Aggiungi i tuoi corsi preferiti";
            var str2 = "Sincronizza i dati tra diversi dispositivi";
            var str3 = "Ricevi notifiche se una lezione viene sospesa";
            var str4 = "Aggiornamento automatico delle lezioni";
            var str5 = "Memorizza i PDF generati";
            var str6 = ". . e molto altro ancora!";

            var lblInfo = new Label()
            {
                Text = string.Format("- {1}{0}- {2}{0}- {3}{0}- {4}{0}- {5}{0}- {6}{0}", "\n", str1, str2, str3, str4, str5, str6 ),
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
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
				Padding = new Thickness(15, 10, 15, 10),
                Spacing = 5,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {_lblLogin, lblInfo, _btnLogin}
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

