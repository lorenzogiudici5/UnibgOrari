using System;
using System.ComponentModel;
using System.Collections.Generic;
using Xamarin.Forms;

namespace OrariUniBg.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public BaseViewModel ()
		{
		}

		private string title = string.Empty;
		public const string TitlePropertyName = "Title";

		/// <summary>
		/// Gets or sets the "Title" property
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get { return title; }
			set { SetProperty (ref title, value, TitlePropertyName);}
		}

		private string subTitle = string.Empty;
        public const string SubtitlePropertyName = "Subtitle";
		/// <summary>
		/// Gets or sets the "Subtitle" property
		/// </summary>
		public string Subtitle
		{
			get { return subTitle; }
			set { SetProperty (ref subTitle, value, SubtitlePropertyName);}
		}

        private string icon = null;
        /// <summary>
        /// Gets or sets the "Icon" of the viewmodel
        /// </summary>
        public const string IconPropertyName = "Icon";

		private bool isBusy;
		/// <summary>
		/// Gets or sets if the view is busy.
		/// </summary>
		public const string IsBusyPropertyName = "IsBusy";
		public bool IsBusy 
		{
			get { return isBusy; }
			set { SetProperty (ref isBusy, value, IsBusyPropertyName);}
		}

		private bool canLoadMore = true;
		/// <summary>
		/// Gets or sets if we can load more.
		/// </summary>
		public const string CanLoadMorePropertyName = "CanLoadMore";
		public bool CanLoadMore
		{
			get { return canLoadMore; }
			set { SetProperty (ref canLoadMore, value, CanLoadMorePropertyName);}
		}

        protected void SetProperty<T>(
            ref T backingStore, T value,
            string propertyName,
            Action onChanged = null,
            Action<T> onChanging = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            if (onChanging != null)
                onChanging(value);

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }


		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		public void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged == null)
				return;

			PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
		}
	}
}

