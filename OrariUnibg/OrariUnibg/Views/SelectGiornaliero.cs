using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Helpers;
using OrariUnibg.Models;
using OrariUnibg.Views.ViewCells;
using Xamarin.Forms;
using OrariUnibg.Services;
using OrariUnibg.ViewModels;
using OrariUnibg.Services.Database;
using Connectivity.Plugin;
using Toasts.Forms.Plugin.Abstractions;

namespace OrariUnibg.Views
{
    public class SelectGiornaliero : ContentPage
    {
		#region Private Fields
        Picker pickerFacoltà;
        Picker pickerLaurea;
        Picker pickerOrder;
        DatePicker pickData;
        ActivityIndicator activityIndicator;
        Label lblError;
        List<Facolta> listFacolta = new List<Facolta>();
        Dictionary<string, int> dictionaryLauree = new Dictionary<string, int>();
        DbSQLite _db;
		#endregion

		#region Constructor
        public SelectGiornaliero()
		{
			_db = new DbSQLite ();
			Title = "Giornaliero";

			Content = getView ();
		}

		#endregion

		#region Private Methods
		private View getView()
		{
            listFacolta = Facolta.facolta;
            pickerFacoltà = new Picker()
            {
                Title = "Facoltà"
            };
            pickerLaurea = new Picker()
            {
                Title = "Corso di Laurea"
            };

            foreach (var f in listFacolta)
            {
                pickerFacoltà.Items.Add(f.Nome);
            }

            pickerFacoltà.SelectedIndexChanged += (sender, args) =>
            {
                var s = pickerFacoltà.Items[pickerFacoltà.SelectedIndex];
                Facolta facolta = listFacolta.Where(x => x.Nome == s).First();
                dictionaryLauree = LaureeDictionary.getLauree(facolta);
                pickerLaurea.Items.Clear();

                foreach (var item in dictionaryLauree)
                    pickerLaurea.Items.Add(item.Key);

                pickerLaurea.SelectedIndex = 0;
            };

            pickData = new DatePicker()
            {
                Format = "D"
            };

            pickerOrder = new Picker()
            {
                Title = "Ordina per..",
            };
            String[] ordina = new String[] { "Alfabetico", "Orario"};

            foreach (var x in ordina)
                pickerOrder.Items.Add(x);

            pickerFacoltà.SelectedIndex = Settings.FacoltaIndex;
            pickerLaurea.SelectedIndex = Settings.LaureaIndex + 1;
            pickerOrder.SelectedIndex = Settings.Order;

            var btn = new Button()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Text = "Ricerca",
                BackgroundColor = ColorHelper.Blue700,
                TextColor = ColorHelper.White,
                BorderColor = ColorHelper.White,
            };

            activityIndicator = new ActivityIndicator()
            {
                IsVisible = false,
                IsRunning = true,
                HorizontalOptions = LayoutOptions.Fill
            };

            lblError = new Label()
            {
                Text = "ORARIO NON DISPONIBILE O IN CORSO DI DEFINIZIONE",
                TextColor = ColorHelper.Red,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Small, this),
                IsVisible = false,
            };
            
            btn.Clicked += btn_Clicked;
            var layout = new StackLayout()
            {
                BackgroundColor = ColorHelper.White,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(10, 10, 10, 10),
                Orientation = StackOrientation.Vertical,
                Children = { pickerFacoltà, pickerLaurea, pickData, pickerOrder, lblError, activityIndicator, btn, }
            };

			return layout;
		}
         
			#endregion

		#region EventHandlers
		protected override bool OnBackButtonPressed ()
		{
			return true; //ALERT CHIUDERE APP??
		}

        private async void btn_Clicked(object sender, EventArgs e)
        {
            lblError.IsVisible = false;
            activityIndicator.IsVisible = true;

            Facolta fac = listFacolta.Where(x => x.Nome == pickerFacoltà.Items[pickerFacoltà.SelectedIndex]).First();
            int facolta = fac.IdFacolta;
            string db = fac.DB;
            int laureaId = dictionaryLauree.Where(x => x.Key == pickerLaurea.Items[pickerLaurea.SelectedIndex]).First().Value;
            string laurea = dictionaryLauree.Where(x => x.Key == pickerLaurea.Items[pickerLaurea.SelectedIndex]).First().Key;
            DateTime data = pickData.Date;
            int order = pickerOrder.SelectedIndex;

			if (!CrossConnectivity.Current.IsConnected) { //non connesso a internet
				activityIndicator.IsVisible = false;
				lblError.IsVisible = true;
				var toast = DependencyService.Get<IToastNotificator>();
				await toast.Notify (ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds (3));
				return;
			}
            string s = await Web.GetOrarioGiornaliero(db, facolta, laureaId, data.ToString("dd'/'MM'/'yyyy"));

            //Settings.FacoltaIndex = pickerFacoltà.SelectedIndex;
            //Settings.LaureaIndex = pickerLaurea.SelectedIndex == 0 ? pickerLaurea.SelectedIndex : pickerLaurea.SelectedIndex - 1;
            Settings.Order = order;

            if (s == string.Empty)
            {
                activityIndicator.IsVisible = false;
                lblError.IsVisible = true;
                return;
            }

            List<CorsoGiornaliero> lista = Web.GetSingleOrarioGiornaliero(s, order, data);
            activityIndicator.IsVisible = false;
            if (lista.Count > 0)
            {
                OrariGiornoViewModel orariViewModel = new OrariGiornoViewModel() { Facolta = fac, LaureaString = laurea.ToUpper(), Data = data, ListOrari = lista };
                var nav = new OrarioGiornaliero();
                nav.BindingContext = orariViewModel;
                await this.Navigation.PushAsync(nav);
            }
               
            else
            {
                lblError.IsVisible = true;
                return;
            }
                
        }
		#endregion
    }
}
