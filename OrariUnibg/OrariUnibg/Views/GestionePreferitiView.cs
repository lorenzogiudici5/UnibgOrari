using System;
using Xamarin.Forms;
using OrariUnibg.Services.Database;

namespace OrariUnibg
{
	public class GestionePreferitiView : ContentPage
	{
		#region Constructor
		public GestionePreferitiView ()
		{
			_db = new DbSQLite ();
			Title = "Corsi Preferiti";
			Icon = null;
			Content = getView ();
		}
		#endregion

		#region Private Fields
		private DbSQLite _db;
		private ListView _listView;
		#endregion

		#region Private Methods
		private View getView()
		{
			_listView = new ListView()
			{
				ItemTemplate = new DataTemplate(typeof(MieiCorsiCell)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				SeparatorColor = Color.Transparent,
				HasUnevenRows = true,
				ItemsSource = _db.GetAllMieiCorsi ()
			};
//			_listView.SetBinding(ListView.ItemsSourceProperty, "ListaLezioni");
			_listView.ItemSelected += (sender, e) => {
				((ListView)sender).SelectedItem = null;
			};

			var layout = new StackLayout () {
				Padding = new Thickness (15, 10, 15, 10),
				Spacing = 5,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					_listView
				}
			};

			MessagingCenter.Subscribe<MieiCorsiCell> (this, "delete_corso_fav_impostazioni", (sender) => {
				_listView.ItemsSource = _db.GetAllMieiCorsi();
			});
					
			return layout;
		}
		#endregion
	}
}

