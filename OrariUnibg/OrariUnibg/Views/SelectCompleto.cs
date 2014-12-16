using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Helpers;
using OrariUnibg.Models;
using Xamarin.Forms;
using OrariUnibg.Services;

namespace OrariUnibg.Views
{
    class SelectCompleto : ContentPage
    {
        Picker pickerFacoltà;
        Picker pickerLaurea;
        Picker pickerAnno;
        Picker pickerSemestre;
        Picker pickerRaggruppa;
        ActivityIndicator activityIndicator;
        Label lblError;
        String[] anni;
        String[] ragg;
        List<Facolta> listFacolta = new List<Facolta>();
        Dictionary<string, int> dictionaryLauree = new Dictionary<string, int>();
        Dictionary<string, string> sem;
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

            foreach (var f in listFacolta)
            {
                pickerFacoltà.Items.Add(f.Nome);
            }

            pickerFacoltà.SelectedIndexChanged += (sender, args) =>
            {
                var s = pickerFacoltà.Items[pickerFacoltà.SelectedIndex];
                Facolta facolta = listFacolta.Where(x => x.Nome == s).First();
                //dictionaryLauree = LaureeDictionary.getLauree(facolta).Where(x => x.Value != 0).ToDictionary(x => x.Key, x => x.Value);
                dictionaryLauree = LaureeDictionary.getLauree(facolta).Where(x => x.Value != 0).ToDictionary(x => x.Key, x => x.Value);
                pickerLaurea.Items.Clear();

                if (dictionaryLauree.Count == 0)
                    return;
                foreach (var item in dictionaryLauree)
                    pickerLaurea.Items.Add(item.Key);

                pickerLaurea.SelectedIndex = 0;
            };

            pickerAnno = new Picker() { Title = "Ordina per..", };
            anni = new String[] { "TUTTI gli anni", "1° Anno", "2° Anno", "3° Anno"};

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

            pickerFacoltà.SelectedIndex = Settings.FacoltaIndex;

            if (Settings.LaureaIndex == 0)
                pickerLaurea.SelectedIndex = 0;
            else 
                pickerLaurea.SelectedIndex = Settings.LaureaIndex - 1;

            pickerAnno.SelectedIndex = Settings.AnnoIndex;
            pickerSemestre.SelectedIndex = 0;
            pickerRaggruppa.SelectedIndex = Settings.Raggruppa;

            var btn = new Button()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Text = "Ricerca",
                BackgroundColor = Color.FromHex("10528c"),
                TextColor = Color.White,
                BorderColor = Color.White,
            };
            btn.Clicked += btn_Clicked;

            lblError = new Label()
            {
                Text = "ORARIO NON DISPONIBILE O IN CORSO DI DEFINIZIONE",
                TextColor = Color.Red,
                Font = Font.SystemFontOfSize(NamedSize.Small),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                IsVisible = false,
            };
            activityIndicator = new ActivityIndicator()
            {
                IsVisible = false,
                IsRunning = true,
                HorizontalOptions = LayoutOptions.Fill
            };
            
            var layout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(10, 10, 10, 10),
                Orientation = StackOrientation.Vertical,
                Children = { pickerFacoltà, pickerLaurea, pickerAnno, pickerSemestre, pickerRaggruppa, lblError, activityIndicator, btn }
            };

            Content = layout;
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

            Settings.FacoltaIndex = pickerFacoltà.SelectedIndex;
            Settings.LaureaIndex = pickerLaurea.SelectedIndex;
            Settings.AnnoIndex = anno;
            Settings.Raggruppa = pickerRaggruppa.SelectedIndex;

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
                await this.Navigation.PushAsync(new OrarioCompletoGroup(lista, fac.Nome, laurea, annoString, semestreString, group));
            else
            {
                lblError.IsVisible = true;
                return;
            }
        }
    }
}
