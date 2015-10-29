using System;
using Xamarin.Forms;
using System.Collections.Generic;
using OrariUnibg.Helpers;

namespace OrariUnibg
{
	public class ManageFileView : ContentPage
	{
		#region Constructor
		public ManageFileView ()
		{
			Title = "Gestione file";
			Content = getView ();
		}
		#endregion

		#region Private Fields
		ListView _listView;
		List<FileViewModel> _filesList;
		#endregion

		#region Private Methods
		private View getView()
		{
			_listView = new ListView () {
				ItemTemplate = new DataTemplate( () =>
					{
						Label titleFile = new Label(){TextColor = ColorHelper.Black, FontSize = Device.GetNamedSize(NamedSize.Medium, this)};
						titleFile.SetBinding(Label.TextProperty, "Name");

						return new ViewCell
						{
							View = new StackLayout
							{
								Padding = new Thickness(10, 10, 10, 10),
								Children = {titleFile }
							}
						};
					})
			};
			_listView.SetBinding (Label.TextProperty, new Binding("Name"));
			_listView.ItemSelected += _listView_ItemSelected;

			getFiles ();

			var layout = new StackLayout () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = { _listView }
			};
					
			return layout;
		}

		private async void getFiles()
		{
			var path = await DependencyService.Get<IFile> ().GetInternalFolder();
			String[] files = DependencyService.Get<IFile> ().GetFiles (path);

			_filesList = new List<FileViewModel> ();
			foreach (var f in files)
				_filesList.Add (new FileViewModel (){ Name = f });

			_listView.ItemsSource = _filesList;
		}

		#endregion

		#region Event Handlers
		async void _listView_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null) {
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

			var file = (FileViewModel)e.SelectedItem;
			var filename = file.Name;
			var folder = await DependencyService.Get<IFile> ().GetInternalFolder ();
			var path = DependencyService.Get<IFile> ().Combine (folder, filename);

			var a = await DisplayActionSheet (filename, "Annulla", null, "Apri", "Condividi", "Cancella");
			switch (a) {
			case "Apri":
				DependencyService.Get<IFile> ().Show (path);
				break;

			case "Condividi":
				DependencyService.Get<IFile> ().Share (path);
				break;

			case "Cancella":
				DependencyService.Get<IFile> ().Delete (path);
				getFiles ();
				break;

			default:
				break;
			}

			((ListView)sender).SelectedItem = null;
		}
		#endregion
	}
}

