using System;
using Xamarin.Forms;
using OrariUnibg.Helpers;
using System.Threading.Tasks;

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
        Image _imgTelegram;
        #endregion

        #region Private Methods
        private View getView()
		{
            var lblCreateBy = new Label() {
                Text = string.Format("Creata da:"),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Medium, this)
			};

            var lblCreator = new Label()
            {
                Text = "Lorenzo Giudici",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                FontAttributes = FontAttributes.Bold,
            };

            var lblCollaborazione = new Label()
            {
                Text = string.Format("Con la collaborazione di:"),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, this)
            };

            var lblCollaboratori1 = new Label()
            {
                Text = "Marco Cossali",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                FontAttributes = FontAttributes.Bold,
            };

            var lblCollaboratori2 = new Label()
            {
                Text = "Andrea Pedrana",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                FontAttributes = FontAttributes.Bold,
            };


            _imgTelegram = new Image()
            {
                Source = "telegram_small.png",
                //Source = "ic_flatOK.png",
                Aspect = Aspect.Fill,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                HeightRequest = 75,
                WidthRequest = 235,
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            //tapGestureRecognizer.Tapped += (s, e) => {

            //};
            _imgTelegram.GestureRecognizers.Add(tapGestureRecognizer);

            var logo = new Image() {HorizontalOptions = LayoutOptions.CenterAndExpand, Aspect = Aspect.AspectFit };
            logo.Source = Device.Idiom == TargetIdiom.Tablet ? "ic_flatOK.png" : "ic_flat_small.png";

            var layout = new StackLayout() {
                Padding = new Thickness(0, 0, 0, 10),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 15,

                Children = {
                    new StackLayout(){
                        Padding = new Thickness(20, 20, 20, 35),
                        Orientation = StackOrientation.Vertical,
                        BackgroundColor = ColorHelper.Blue700,
                        Spacing = 10,
                        Children ={
                            logo,
                            new Label() {Text = "UnibgOrari", TextColor = ColorHelper.White, HorizontalOptions = LayoutOptions.CenterAndExpand, FontSize = Device.GetNamedSize(NamedSize.Large, this)},
                            new Label() {Text = string.Format("Versione {0}", Settings.Versione), TextColor = ColorHelper.White, HorizontalOptions = LayoutOptions.CenterAndExpand, FontSize = Device.GetNamedSize(NamedSize.Medium, this)},
                        }
                    },
                    new StackLayout() { Spacing = 0, Children = { lblCreateBy, lblCreator}},
                    new StackLayout() { Spacing = 0, Children = { lblCollaborazione, lblCollaboratori1, lblCollaboratori2}},
                    _imgTelegram,
                }
			};

			return layout;
		}

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            _imgTelegram.Opacity = .5;
            await Task.Delay(200);
            _imgTelegram.Opacity = 1;

            var uri = new Uri("https://telegram.me/orariunibg_bot");
            Device.OpenUri(uri);
        }
        #endregion
    }
}

