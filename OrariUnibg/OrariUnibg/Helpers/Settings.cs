// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace OrariUnibg.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
      private static ISettings AppSettings
      {
          get
          {
              return CrossSettings.Current;
          }
      }

        

        #region User Authentication Constants
        const string UserIdKey = "userid";
        static readonly string UserIdDefault = string.Empty;

        const string AuthTokenKey = "authtoken";
        static readonly string AuthTokenDefault = string.Empty;

        const string SocialIdKey = "socialid";
        static readonly string SocialIdDefault = string.Empty;

        const string UsernameKey = "username";
        static readonly string UsernameDefault = string.Empty;

        const string GivenNameKey = "givenname";
        static readonly string GivenNameDefault = string.Empty;

        const string SurnameKey = "surname";
        static readonly string SurnameDefault = string.Empty;

        const string NameKey = "name";
        static readonly string NameDefault = string.Empty;

        const string EmailKey = "email";
        static readonly string EmailDefault = string.Empty;

        const string PictureKey = "picture";
        static readonly string PictureDefault = string.Empty;
        #endregion

        #region Setting Constants
        private const string AppNameKey = "appame_key";
		private const string VersionKey = "version_key";
		private const string FirmaKey = "firma_key";
		private const string DisableStatisticDataKey = "disablestatisticdata_key";
//       	private const string SettingsKey = "settings_key";
       	private const string PrimoAvvioKey = "primoAvvio_key";
		private const string SuccessLoginKey = "successLogin_key";

       	private const string FacoltaKey = "facolta_key";
       	private const string FacoltaDBKey = "facoltaDB_key";
       	private const string LaureaKey = "laurea_key";
       	private const string AnnoKey = "anno_key";
       	private const string NomeKey = "nome_key";
       	private const string CognomeKey = "cognome_key";
       	private const string MailKey = "mail_key";
      	private const string MatricolaKey = "matricola_key";
        private const string CreatedAtStringKey = "createdAtstring_key";
        private const string UpdatedAtStringKey = "datecreatedstring_key";
        private const string ToUpdateKey = "toupdate_key";

        private const string LastUpdateKey = "lastUpdate_key";
      	private const string SyncKey = "sync_key";
      	private const string NotifyKey = "notify_key";
		private const string UpdateHourKey = "updateHour_key";
		private const string UpdateMinuteKey = "updateMinute_key";
		private const string UpdateIntervalKey = "updateInterval_key";

      	private const string FacoltaIdKey = "facId_key";
      	private const string LaureaIdKey = "laureaId_key";
      	private const string FacoltaIndexKey = "facIndex_key";
      	private const string LaureaIndexKey = "laureaIndex_key";
      	private const string AnnoIndexKey = "annoIndex_key";
      	private const string OrderKey = "order_key";
      	private const string RaggruppaKey = "raggruppa_key";

      	private const string MieiCorsiCountKey = "mieiCorsiCount_key";

        private const string HelpSuggerisciHideKey = "help_key";

        private static readonly string VersionString = "1.0";
		private static readonly string AppNameString = "UnibgOrari";
		private static readonly string FirmaString = string.Format ("\n\n\nCondiviso da {0}", AppNameString);
      	private static readonly string DefaultString = string.Empty;
		private static readonly int DefaultValue = 0;
      	private static readonly bool DefaultBool = true;
		private static readonly bool DefaultBoolFalse = false;
		private static readonly int DefaultUpdateHours = 18;
		private static readonly int DefaultUpdateMinute = 13;
		private static readonly long DefaultUpdateInterval = 3;
        #endregion

        #region User Authentication Settings
        public static string AuthToken
        {
            get { return AppSettings.GetValueOrDefault<string>(AuthTokenKey, AuthTokenDefault); }
            set { AppSettings.AddOrUpdateValue<string>(AuthTokenKey, value); }
        }

        public static string UserId
        {
            get { return AppSettings.GetValueOrDefault<string>(UserIdKey, UserIdDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UserIdKey, value); }
        }

        public static string SocialId
        {
            get { return AppSettings.GetValueOrDefault<string>(SocialIdKey, SocialIdDefault); }
            set { AppSettings.AddOrUpdateValue<string>(SocialIdKey, value); }
        }

        public static string Username
        {
            get { return AppSettings.GetValueOrDefault<string>(UsernameKey, UsernameDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UsernameKey, value); }
        }

        public static string Email
        {
            get { return AppSettings.GetValueOrDefault<string>(EmailKey, EmailDefault); }
            set { AppSettings.AddOrUpdateValue<string>(EmailKey, value); }
        }

        public static string GivenName
        {
            get { return AppSettings.GetValueOrDefault<string>(GivenNameKey, GivenNameDefault); }
            set { AppSettings.AddOrUpdateValue<string>(GivenNameKey, value); }
        }

        public static string Surname
        {
            get { return AppSettings.GetValueOrDefault<string>(SurnameKey, SurnameDefault); }
            set { AppSettings.AddOrUpdateValue<string>(SurnameKey, value); }
        }

        public static string Name
        {
            get { return AppSettings.GetValueOrDefault<string>(NameKey, NameDefault); }
            set { AppSettings.AddOrUpdateValue<string>(NameKey, value); }
        }

        public static string Picture
        {
            get { return AppSettings.GetValueOrDefault<string>(PictureKey, PictureDefault); }
            set { AppSettings.AddOrUpdateValue<string>(PictureKey, value); }
        }

        public static bool IsLoggedIn => !string.IsNullOrWhiteSpace(UserId);
        #endregion


        #region Settings
        public static string AppName
		{
			get
			{
				return AppSettings.GetValueOrDefault(AppNameKey, AppNameString);
			}
			set
			{
				AppSettings.AddOrUpdateValue(AppNameKey, value);
			}
		}
		public static string Versione
		{
			get
			{
				return AppSettings.GetValueOrDefault(VersionKey, VersionString);
			}
			set
			{
				AppSettings.AddOrUpdateValue(VersionKey, value);
			}
		}

        public static bool ToUpdate
        {
            get
            {
                return AppSettings.GetValueOrDefault(ToUpdateKey, DefaultBool);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ToUpdateKey, value);
            }
        }


        public static string Firma
		{
			get
			{
				return AppSettings.GetValueOrDefault(FirmaKey, FirmaString);
			}
			set
			{
				AppSettings.AddOrUpdateValue(FirmaString, value);
			}
		}

		public static bool DisableStatisticData
		{
			get
			{
				return AppSettings.GetValueOrDefault(DisableStatisticDataKey, DefaultBoolFalse);
			}
			set
			{
				AppSettings.AddOrUpdateValue(DisableStatisticDataKey, value);
			}
		}

	    public static bool PrimoAvvio
	    {
	        get
	        {
	            return AppSettings.GetValueOrDefault(PrimoAvvioKey, DefaultBool);
	        }
	        set
	        {
	            AppSettings.AddOrUpdateValue(PrimoAvvioKey, value);
	        }
	    }

		public static bool SuccessLogin
		{
			get
			{
				return AppSettings.GetValueOrDefault(SuccessLoginKey, false);
			}
			set
			{
				AppSettings.AddOrUpdateValue(SuccessLoginKey, value);
			}
		}

    public static bool BackgroundSync
    {
      get
      {
          return AppSettings.GetValueOrDefault(SyncKey, DefaultBool);
      }
      set
      {
          AppSettings.AddOrUpdateValue(SyncKey, value);
      }
    }
    public static bool Notify
    {
        get
        {
            return AppSettings.GetValueOrDefault(NotifyKey, DefaultBool);
        }
        set
        {
            AppSettings.AddOrUpdateValue(NotifyKey, value);
        }
    }
			
		public static int UpdateHour
		{
			get
			{
				return AppSettings.GetValueOrDefault(UpdateHourKey, DefaultUpdateHours);
			}
			set
			{
				AppSettings.AddOrUpdateValue(UpdateHourKey, value);
			}
		}
		public static int UpdateMinute
		{
			get
			{
				return AppSettings.GetValueOrDefault(UpdateMinuteKey, DefaultUpdateMinute);
			}
			set
			{
				AppSettings.AddOrUpdateValue(UpdateMinuteKey, value);
			}
		}

		public static long UpdateInterval
		{
			get
			{
				return AppSettings.GetValueOrDefault(UpdateIntervalKey, DefaultUpdateInterval);
			}
			set
			{
				AppSettings.AddOrUpdateValue(UpdateIntervalKey, value);
			}
		}

		public static string LastUpdate
		{
			get
			{
				return AppSettings.GetValueOrDefault(LastUpdateKey, DefaultString);
			}
			set
			{
				AppSettings.AddOrUpdateValue(LastUpdateKey, value);
                ToUpdate = false;
            }
		}

        public static bool HelpSuggerisciCorsiHide
        {
            get
            {
                return AppSettings.GetValueOrDefault(HelpSuggerisciHideKey, DefaultBoolFalse);
            }
            set
            {
                AppSettings.AddOrUpdateValue(HelpSuggerisciHideKey, value);
                ToUpdate = false;
            }
        }

        #endregion

        #region PersonalInformation
    public static string Matricola
    {
        get
        {
            return AppSettings.GetValueOrDefault(MatricolaKey, DefaultString);
        }
        set
        {
            AppSettings.AddOrUpdateValue(MatricolaKey, value);
        }
    }
    public static string Facolta
    {
        get
        {
            return AppSettings.GetValueOrDefault(FacoltaKey, DefaultString);
        }
        set
        {
            AppSettings.AddOrUpdateValue(FacoltaKey, value);
        }
    }

    public static int? FacoltaId
    {
        get
        {
            return AppSettings.GetValueOrDefault(FacoltaIdKey, DefaultValue);
        }
        set
        {
            AppSettings.AddOrUpdateValue(FacoltaIdKey, value);
        }
    }
    public static int? LaureaId
    {
        get
        {
            return AppSettings.GetValueOrDefault(LaureaIdKey, DefaultValue);
        }
        set
        {
            AppSettings.AddOrUpdateValue(LaureaIdKey, value);
        }
    }
    public static string Laurea
    {
        get
        {
            return AppSettings.GetValueOrDefault(LaureaKey, DefaultString);
        }
        set
        {
            AppSettings.AddOrUpdateValue(LaureaKey, value);
        }
    }
    public static string Anno
    {
        get
        {
            return AppSettings.GetValueOrDefault(AnnoKey, DefaultString);
        }
        set
        {
            AppSettings.AddOrUpdateValue(AnnoKey, value);
        }
    }
    public static string FacoltaDB
    {
        get
        {
            return AppSettings.GetValueOrDefault(FacoltaDBKey, DefaultString);
        }
        set
        {
            AppSettings.AddOrUpdateValue(FacoltaDBKey, value);
        }
    }

    public static string CreatedAtString
	{
		get
		{
			return AppSettings.GetValueOrDefault(CreatedAtStringKey, DefaultString);
		}
		set
		{
			AppSettings.AddOrUpdateValue(CreatedAtStringKey, value);
		}
	}

        public static string UpdatedAtString
        {
            get
            {
                return AppSettings.GetValueOrDefault(CreatedAtStringKey, DefaultString);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CreatedAtStringKey, value);
            }
        }

        #endregion
        
        #region PickerIndex
        public static int? FacoltaIndex
        {
            get
            {
                return AppSettings.GetValueOrDefault(FacoltaIndexKey, DefaultValue);
            }
            set
            {
                AppSettings.AddOrUpdateValue(FacoltaIndexKey, value);
            }
        }
        public static int? LaureaIndex
        {
            get
            {
                return AppSettings.GetValueOrDefault(LaureaIndexKey, DefaultValue);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LaureaIndexKey, value);
            }
        }
        public static int? AnnoIndex
        {
            get
            {
                return AppSettings.GetValueOrDefault(AnnoIndexKey, DefaultValue);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AnnoIndexKey, value);
            }
        }
        public static int Order
        {
            get
            {
                return AppSettings.GetValueOrDefault(OrderKey, DefaultValue);
            }
            set
            {
                AppSettings.AddOrUpdateValue(OrderKey, value);
            }
        }
        public static int Raggruppa
        {
            get
            {
                return AppSettings.GetValueOrDefault(RaggruppaKey, DefaultValue);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RaggruppaKey, value);
            }
        }
      
      #endregion

        #region DB
      public static int MieiCorsiCount
      {
          get
          {
              return AppSettings.GetValueOrDefault(MieiCorsiCountKey, DefaultValue);
          }
          set
          {
              AppSettings.AddOrUpdateValue(MieiCorsiCountKey, value);
          }
      }
      #endregion



	#region Strings

	#endregion



  }
}