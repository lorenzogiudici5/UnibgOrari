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
		ContentPage page1;
		StackLayout layout1;
		ContentPage page2;
		StackLayout layout2;
		ContentPage page3;
		StackLayout layout3;
		#endregion

		#region Private Methods
		private void getPages()
		{
			List<Page> pages = new List<Page> ();
			String[] images = { "Screen.png" };

//			var _btnNext = new Button () {
//				HorizontalOptions = LayoutOptions.CenterAndExpand,
//				VerticalOptions = LayoutOptions.EndAndExpand,
//				Text = "Fine",
//				TextColor = ColorHelper.White,
//				BackgroundColor = ColorHelper.Transparent,
//			};
//			_btnNext.Clicked += _btnNext_Clicked;

//			var _btnSkip = new Button () {
//				HorizontalOptions = LayoutOptions.StartAndExpand,
//				Text = "Skip",
//				TextColor = ColorHelper.White,
//				BackgroundColor = ColorHelper.Transparent,
//			};
//
//			_btnSkip.Clicked += _btnSkip_Clicked;

//			var endBar = new StackLayout () {
//				Orientation = StackOrientation.Horizontal,
//				VerticalOptions = LayoutOptions.EndAndExpand,
//				HorizontalOptions = LayoutOptions.FillAndExpand,
//				Children = {_btnNext}
//			};

			layout1 = new StackLayout {
				Padding = new Thickness(20, 10, 10, 5),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Vertical,
				Children = {
					new StackLayout(){Orientation = StackOrientation.Vertical, Children = 
						{
							new Label () {
								Text = "Corsi Preferiti",
								TextColor = ColorHelper.White,
//								HorizontalOptions = LayoutOptions.CenterAndExpand,
								FontSize = Device.GetNamedSize (NamedSize.Large, this)
							},

							new Label () {
								Text = "Seleziona i tuoi corsi preferiti per visualizzare ogni lezioni comodamente nella homepage e ricevere notifiche su variazioni o cambi di aula.",
								TextColor = ColorHelper.White,
//								HorizontalOptions = LayoutOptions.CenterAndExpand,
								FontSize = Device.GetNamedSize (NamedSize.Medium, this)
							},
						}
					},
					new Image () {
						Aspect = Aspect.AspectFit,
						Source = "Screen2.png", 
					},
				}
			};

			page1 = new ContentPage { Content = layout1};

			layout2 = new StackLayout {
				Padding = new Thickness(20, 10, 10, 5),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Vertical,
				Children = {
					new StackLayout(){Orientation = StackOrientation.Vertical, Children = 
						{
							new Label () {
								Text = "Orario Giornaliero",
								TextColor = ColorHelper.White,
//								HorizontalOptions = LayoutOptions.CenterAndExpand,
								FontSize = Device.GetNamedSize (NamedSize.Large, this)
							},

							new Label () {
								Text = "Consulta gli orari del giorno di ogni facoltà e corso di laurea. Ordina, cerca ed esporta!",
								TextColor = ColorHelper.White,
//								HorizontalOptions = LayoutOptions.CenterAndExpand,
								FontSize = Device.GetNamedSize (NamedSize.Medium, this)
							},
						}
					},

					new Image () {
						Aspect = Aspect.AspectFit,
						Source = "Screen1.png", 
					},
				}
			};
			page2 = new ContentPage {Content = layout2};

			layout3 = new StackLayout {
				Padding = new Thickness(20, 10, 10, 5),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Vertical,
				Children = {
					new StackLayout(){Orientation = StackOrientation.Vertical, Children = 
						{
							new Label () {
								Text = "Orario Completo",
								TextColor = ColorHelper.White,
//								HorizontalOptions = LayoutOptions.CenterAndExpand,
								FontSize = Device.GetNamedSize (NamedSize.Large, this)
							},

							new Label () {
								Text = "Visualizza l'orario completo raggruppato per giorno o per corso, il tuo orario settimanale non è mai stato così chiaro!",
								TextColor = ColorHelper.White,
//								HorizontalOptions = LayoutOptions.CenterAndExpand,
								FontSize = Device.GetNamedSize (NamedSize.Medium, this)
							},
						}
					},

					new Image () {
						Aspect = Aspect.AspectFit,
						Source = "Screen3.png", 
					},
				}
			};
			page3 = new ContentPage {Content = layout3};
					
			this.Children.Add (page1);
			this.Children.Add (page2);
			this.Children.Add (page3);
			this.Children.Add (new LoginView ());
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

		#region Override
		protected override bool OnBackButtonPressed ()
		{
			Navigation.PopModalAsync ();
			return true;
		}
		protected override void OnSizeAllocated (double width, double height)
		{
			base.OnSizeAllocated (width, height);

			SizeChanged += (sender, e) => {
				if(height > width) //portrait
				{
					layout1.Orientation = StackOrientation.Vertical;
					layout2.Orientation = StackOrientation.Vertical;
					layout3.Orientation = StackOrientation.Vertical;
				}
				else //landscape
				{
					layout1.Orientation = StackOrientation.Horizontal;
					layout2.Orientation = StackOrientation.Horizontal;
					layout3.Orientation = StackOrientation.Horizontal;
				}

			};
		}
		#endregion
	}
}

