// Helpers/Settings.cs
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;

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
      
      #region Setting Constants
      
       	private const string SettingsKey = "settings_key";
       	private const string PrimoAvvioKey = "primoAvvio_key";
       	private const string FacoltaKey = "facolta_key";
       	private const string DBFacoltaKey = "DBfacolta_key";
       	private const string LaureaKey = "laurea_key";
       	private const string AnnoKey = "anno_key";
       	private const string NomeKey = "nome_key";
       	private const string CognomeKey = "cognome_key";
       	private const string MailKey = "mail_key";
      	private const string MatricolaKey = "matricola_key";

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

      	private static readonly string DefaultString = string.Empty;
		private static readonly int DefaultValue = 0;
      	private static readonly bool DefaultBool = true;
		private static readonly int DefaultUpdateHours = 18;
		private static readonly int DefaultUpdateMinute = 13;
		private static readonly int DefaultUpdateInterval = 3;

      #endregion
      
      #region Settings
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

		public static int UpdateInterval
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
			}
		}
    
    #endregion
      
      #region PersonalInformation
    public static string Nome
    {
        get
        {
            return AppSettings.GetValueOrDefault(NomeKey, DefaultString);
        }
        set
        {
            AppSettings.AddOrUpdateValue(NomeKey, value);
        }
    }
    public static string Cognome
    {
        get
        {
            return AppSettings.GetValueOrDefault(CognomeKey, DefaultString);
        }
        set
        {
            AppSettings.AddOrUpdateValue(CognomeKey, value);
        }
    }
    public static string Email
    {
        get
        {
            return AppSettings.GetValueOrDefault(MailKey, DefaultString);
        }
        set
        {
            AppSettings.AddOrUpdateValue(MailKey, value);
        }
    }
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

    public static int FacoltaId
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
    public static int LaureaId
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
    public static string DBfacolta
    {
        get
        {
            return AppSettings.GetValueOrDefault(DBFacoltaKey, DefaultString);
        }
        set
        {
            AppSettings.AddOrUpdateValue(DBFacoltaKey, value);
        }
    }


    #endregion

      #region PickerIndex
        public static int FacoltaIndex
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
        public static int LaureaIndex
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
        public static int AnnoIndex
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



  }
}