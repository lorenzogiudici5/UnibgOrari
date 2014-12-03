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
    private const string FacoltaKey = "facolta_key";
    private const string LaureaKey = "laurea_key";
    private const string AnnoKey = "anno_key";
    private const string OrderKey = "order_key";
    private const string RaggruppaKey = "raggruppa_key";

    private static readonly string SettingsDefault = string.Empty;
    private static readonly int DefaultValue = 0;

    #endregion


    public static string GeneralSettings
    {
      get
      {
        return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
      }
      set
      {
        AppSettings.AddOrUpdateValue(SettingsKey, value);
      }
    }

    public static int Facolta
    {
        get
        {
            return AppSettings.GetValueOrDefault(FacoltaKey, DefaultValue);
        }
        set
        {
            AppSettings.AddOrUpdateValue(FacoltaKey, value);
        }
    }
    public static int Laurea
    {
        get
        {
            return AppSettings.GetValueOrDefault(LaureaKey, DefaultValue);
        }
        set
        {
            AppSettings.AddOrUpdateValue(LaureaKey, value);
        }
    }
    public static int Anno
    {
        get
        {
            return AppSettings.GetValueOrDefault(AnnoKey, DefaultValue);
        }
        set
        {
            AppSettings.AddOrUpdateValue(AnnoKey, value);
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

  }
}