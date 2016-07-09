using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Helpers;
using OrariUnibg.Models;
using Xamarin.Forms;
using OrariUnibg.Services;
using OrariUnibg.ViewModels;
using Plugin.Connectivity;
using Plugin.Toasts;

namespace OrariUnibg.Views
{
    class SelectCompleto : ContentPage
    {
        #region Private Fields
        private int _facoltàIndex = 0;
        private int _laureaIndex = 0;
        Picker pickerFacoltà;
        Picker pickerLaurea;
        Picker pickerAnno;
        Picker pickerSemestre;
        Picker pickerRaggruppa;
        ActivityIndicator activityIndicator;
        Label lblError;
        private int limit = 3;
        private String[] anni;
        private String[] ragg;
        List<Facolta> listFacolta = new List<Facolta>();
        Dictionary<string, int> dictionaryLauree = new Dictionary<string, int>();
        Dictionary<string, string> sem;
        #endregion

        public SelectCompleto()
        {
            Title = "Completo";
            Icon = null;
            listFacolta = Facolta.facolta;
            pickerFacoltà = new Picker()
            {
                Title = "Facoltà"
            };
            pickerLaurea = new Picker()
            {
                Title = "Corso di Laurea"
            };

            int index = 0;
            foreach (var f in listFacolta)
            {
                pickerFacoltà.Items.Add(f.Nome);
                if (f.IdFacolta == Settings.FacoltaId && f.DB == Settings.FacoltaDB)
                    _facoltàIndex = index;

                index++;
            }

            //pickerFacoltà.SelectedIndex = facoltàIndex;

            pickerFacoltà.SelectedIndexChanged += (sender, args) =>
            {
                var s = pickerFacoltà.Items[pickerFacoltà.SelectedIndex];
                Facolta facolta = listFacolta.Where(x => x.Nome == s).First();
                //dictionaryLauree = LaureeDictionary.getLauree(facolta).Where(x => x.Value != 0).ToDictionary(x => x.Key, x => x.Value);
                dictionaryLauree = LaureeDictionary.getLauree(facolta).Where(x => x.Value != 0).ToDictionary(x => x.Key, x => x.Value);
                pickerLaurea.Items.Clear();

                if (dictionaryLauree.Count == 0)
                    return;

                index = 0;
                foreach (var item in dictionaryLauree)
                {
                    pickerLaurea.Items.Add(item.Key);
                    if (item.Value == Settings.LaureaId)
                        _laureaIndex = index;

                    index++;
                }


                pickerLaurea.SelectedIndex = 0;
            };

            anni = new String[] {"TUTTI gli anni", "1° Anno", "2° Anno", "3° Anno", "4° Anno", "5° Anno" };

            pickerLaurea.SelectedIndexChanged += (sender, args) =>
            {
                int i = 0;
                pickerAnno.IsEnabled = true;

                if (pickerLaurea.Items.Count > 0)
                {
                    if (pickerLaurea.Items[pickerLaurea.SelectedIndex].Contains("Magistrale"))
                        limit = 5;
                    else if (pickerLaurea.Items[pickerLaurea.SelectedIndex].Contains("LM"))
                        limit = 2;
                    else limit = 3;

                    pickerAnno.Items.Clear();
                    foreach (var x in anni)
                    {
                        if (i > limit)
                            break;
                        pickerAnno.Items.Add(x);
                        i++;
                    }
                    pickerAnno.SelectedIndex = 0;
                }

            };

            pickerAnno = new Picker() { Title = "Ordina per..", };

            pickerSemestre = new Picker() { Title = "Semestre" };
            sem = new Dictionary<string, string>()
            {
                {"Primo", "completo"}, {"Secondo", "secondo"}
            };

            pickerRaggruppa = new Picker() { Title = "Raggruppa per.." };
            ragg = new String[] { "Corso", "Giorno" };

            foreach (var x in anni)
                pickerAnno.Items.Add(x);

            foreach (var s in sem)
                pickerSemestre.Items.Add(s.Key);

            foreach (var g in ragg)
                pickerRaggruppa.Items.Add(g);

            //pickerFacoltà.SelectedIndex = Settings.FacoltaIndex;
            pickerFacoltà.SelectedIndex = _facoltàIndex;

            //if (Settings.LaureaIndex == 0)
            //    pickerLaurea.SelectedIndex = 0;
            //else
                pickerLaurea.SelectedIndex = _laureaIndex;
            //pickerLaurea.SelectedIndex = Settings.LaureaIndex;

            //pickerAnno.SelectedIndex = int.Parse(Settings.Anno) + 1;
            pickerAnno.SelectedIndex = (int)Settings.AnnoIndex;

            //Se siamo a Marzo, secondo semsestre
            pickerSemestre.SelectedIndex = DateTime.Today.Month >= new DateTime(2016, 03, 01).Month && DateTime.Today.Month <= new DateTime(2016, 06, 01).Month ? 1 : 0;

            pickerRaggruppa.SelectedIndex = Settings.Raggruppa;

            lblError = new Label()
            {
                Text = "ORARIO NON DISPONIBILE O IN CORSO DI DEFINIZIONE",
                TextColor = ColorHelper.Red500,
                FontSize = Device.GetNamedSize(NamedSize.Small, this),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                IsVisible = false,
            };
            activityIndicator = new ActivityIndicator()
            {
                IsVisible = false,
                IsRunning = true,
                HorizontalOptions = LayoutOptions.Fill
            };

			var btn = new Button()
			{
				VerticalOptions = LayoutOptions.EndAndExpand,
				Text = "Ricerca",
				BackgroundColor = ColorHelper.Blue700,
				TextColor = ColorHelper.White,
				BorderColor = ColorHelper.White,
			};
			btn.Clicked += btn_Clicked;
            
            var layout = new StackLayout()
            {
				BackgroundColor = ColorHelper.White,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(10, 10, 10, 10),
                Orientation = StackOrientation.Vertical,
                Children = { pickerFacoltà, pickerLaurea, pickerAnno, pickerSemestre, pickerRaggruppa, lblError, activityIndicator, btn }
            };

            Content = layout;
        }

		#region EventHandlers
		protected override bool OnBackButtonPressed ()
		{
			return true; //ALERT CHIUDERE APP??
		}
        private async void btn_Clicked(object sender, EventArgs e)
        {
            Facolta fac = listFacolta.Where(x => x.Nome == pickerFacoltà.Items[pickerFacoltà.SelectedIndex]).First();
            int facolta = fac.IdFacolta;
            string db = fac.DB;
            int laureaId = dictionaryLauree.Where(x => x.Key == pickerLaurea.Items[pickerLaurea.SelectedIndex]).First().Value;
            string laurea = dictionaryLauree.Where(x => x.Key == pickerLaurea.Items[pickerLaurea.SelectedIndex]).First().Key;
            int anno = pickerAnno.SelectedIndex;
            string annoString = anni[anno];
            string semestre = sem.Where(x => x.Key == pickerSemestre.Items[pickerSemestre.SelectedIndex]).First().Value;
            string semestreString = sem.Where(x => x.Key == pickerSemestre.Items[pickerSemestre.SelectedIndex]).First().Key;
            bool group = pickerRaggruppa.SelectedIndex == 0 ? false : true;

            activityIndicator.IsVisible = true;

            //Settings.FacoltaIndex = pickerFacoltà.SelectedIndex;
            //Settings.LaureaIndex = pickerLaurea.SelectedIndex;
            //Settings.AnnoIndex = anno;
            Settings.Raggruppa = pickerRaggruppa.SelectedIndex;

			if (!CrossConnectivity.Current.IsConnected) { //non connesso a internet
				activityIndicator.IsVisible = false;
				lblError.IsVisible = true;
				var toast = DependencyService.Get<IToastNotificator>();
				await toast.Notify (ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds (3));
				return;
			}

            string s = await Web.GetOrarioCompleto(semestre, db, facolta, laureaId, anno);

            if(s == string.Empty)
            {
                activityIndicator.IsVisible = false;
                lblError.IsVisible = true;
                return;           
            }

            List<CorsoCompleto> lista = Web.GetSingleOrarioCompleto(s);
            activityIndicator.IsVisible = false;

            if (lista.Count > 0)
            {
                var completoViewModel = new OrariCompletoViewModel() { Facolta = fac, LaureaString = laurea, Anno = annoString, Semestre = semestreString, ListOrari = lista, Group = group };
                var nav = new OrarioCompleto();
                nav.BindingContext = completoViewModel;
                await this.Navigation.PushAsync(nav);

                //                await this.Navigation.PushAsync(new OrarioCompletoGroup(lista, fac.Nome, laurea, annoString, semestreString, group));
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
