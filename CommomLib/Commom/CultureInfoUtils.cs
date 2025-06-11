// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.CultureInfoUtils
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace CommomLib.Commom;

public static class CultureInfoUtils
{
  private static string appSettingsLangName;
  private static string actualAppLanguage;
  private static string suggestAppLanguage;
  private static IReadOnlyList<string> allSupportLanguage;

  public static CultureInfo SystemCultureInfo { get; private set; }

  public static CultureInfo SystemUICultureInfo { get; private set; }

  public static void Initialize()
  {
    CultureInfoUtils.SystemCultureInfo = CultureInfo.CurrentCulture;
    CultureInfoUtils.SystemUICultureInfo = CultureInfo.CurrentUICulture;
    CultureInfoUtils.InitializeDefaultLanguageFromSetup();
    string settingsLangName = CultureInfoUtils.GetAppSettingsLangName();
    if (string.IsNullOrEmpty(settingsLangName))
      return;
    CultureInfo cultureInfo;
    CultureInfo.CurrentCulture = cultureInfo = CultureInfo.GetCultureInfo(settingsLangName);
    CultureInfo.CurrentUICulture = cultureInfo;
    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
  }

  public static IReadOnlyList<string> AllSupportLanguage
  {
    get
    {
      return CultureInfoUtils.allSupportLanguage ?? (CultureInfoUtils.allSupportLanguage = CultureInfoUtils.GetAllLanguages());
    }
  }

  public static string ActualAppLanguage
  {
    get
    {
      if (CultureInfoUtils.actualAppLanguage == null)
      {
        string settingsLangName = CultureInfoUtils.GetAppSettingsLangName();
        CultureInfoUtils.actualAppLanguage = string.IsNullOrEmpty(settingsLangName) ? CultureInfoUtils.SuggestAppLanguage : settingsLangName;
      }
      return CultureInfoUtils.actualAppLanguage;
    }
  }

  public static string SuggestAppLanguage
  {
    get
    {
      if (CultureInfoUtils.suggestAppLanguage == null)
      {
        string[] strArray = new string[5]
        {
          "WinFeedBackEmailContentPlaceHoderContent",
          "WinFeedBackErrorInvalidEmailCapMsg",
          "WinFeedBackInvalidEmailMsg",
          "WinFeedBackSendProblemfileContent",
          "WinFeedBackSendSuccessMsg"
        };
        foreach (string name1 in strArray)
        {
          string a = Resources.ResourceManager.GetString(name1, CultureInfoUtils.SystemUICultureInfo ?? CultureInfo.CurrentUICulture);
          if (!string.IsNullOrEmpty(a))
          {
            foreach (string name2 in (IEnumerable<string>) CultureInfoUtils.AllSupportLanguage)
            {
              string b = Resources.ResourceManager.GetString(name1, CultureInfo.GetCultureInfo(name2));
              if (string.Equals(a, b))
              {
                CultureInfoUtils.suggestAppLanguage = name2;
                break;
              }
            }
          }
        }
        if (CultureInfoUtils.suggestAppLanguage == null)
          CultureInfoUtils.suggestAppLanguage = "en";
      }
      return CultureInfoUtils.suggestAppLanguage;
    }
  }

  private static string GetAppSettingsLangName()
  {
    if (CultureInfoUtils.appSettingsLangName == null)
      CultureInfoUtils.appSettingsLangName = ConfigManager.GetApplicationLanugageName() ?? "";
    return CultureInfoUtils.appSettingsLangName;
  }

  private static void InitializeDefaultLanguageFromSetup()
  {
    if (!string.IsNullOrEmpty(ConfigManager.GetApplicationLanugageName() ?? "") || ConfigManager.getAppLaunchCount() != 0L)
      return;
    string setupLanguage = CultureInfoUtils.GetSetupLanguage();
    if (string.IsNullOrEmpty(setupLanguage) || string.Equals(CultureInfoUtils.SuggestAppLanguage, setupLanguage, StringComparison.OrdinalIgnoreCase))
      return;
    ConfigManager.SetApplicationLanugageName(setupLanguage);
  }

  public static RegionInfo GetHomeLocation()
  {
    try
    {
      string userDefaultGeoName = CultureInfoUtils.GetUserDefaultGeoName();
      return string.IsNullOrEmpty(userDefaultGeoName) ? (RegionInfo) null : new RegionInfo(userDefaultGeoName);
    }
    catch
    {
      return (RegionInfo) null;
    }
  }

  private static string GetUserDefaultGeoName()
  {
    StringBuilder geoName = new StringBuilder(16 /*0x10*/);
    CultureInfoUtils.GetUserDefaultGeoName(geoName, 16 /*0x10*/);
    return geoName.ToString();
  }

  [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
  private static extern int GetUserDefaultGeoName([MarshalAs(UnmanagedType.LPWStr)] StringBuilder geoName, int geoNameCount);

  private static IReadOnlyList<string> GetAllLanguages()
  {
    return (IReadOnlyList<string>) ((IEnumerable<string>) new string[1]
    {
      "en"
    }).Concat<string>(((IEnumerable<DirectoryInfo>) new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetDirectories()).Where<DirectoryInfo>((Func<DirectoryInfo, bool>) (c =>
    {
      try
      {
        return ((IEnumerable<FileInfo>) c.GetFiles()).Any<FileInfo>((Func<FileInfo, bool>) (x => x.Name == "CommomLib.resources.dll"));
      }
      catch
      {
      }
      return false;
    })).Select<DirectoryInfo, string>((Func<DirectoryInfo, string>) (c => c.Name))).OrderBy<string, string>((Func<string, string>) (c => CultureInfo.GetCultureInfo(c)?.EnglishName), (IComparer<string>) StringComparer.OrdinalIgnoreCase).ToList<string>();
  }

  private static string GetSetupLanguage()
  {
    try
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{7DACF63A-4EE4-4837-9AF9-C65D4509FFB4}_is1"))
      {
        if (registryKey?.GetValue("Inno Setup: Language") is string str)
        {
          if (!string.IsNullOrEmpty(str))
          {
            string lower = str.Trim().ToLower();
            if (lower != null)
            {
              switch (lower.Length)
              {
                case 2:
                  switch (lower[0])
                  {
                    case 'd':
                      if (lower == "de")
                        return "de";
                      break;
                    case 'e':
                      switch (lower)
                      {
                        case "en":
                          return "en";
                        case "es":
                          return "es";
                      }
                      break;
                    case 'f':
                      if (lower == "fr")
                        return "fr";
                      break;
                    case 'i':
                      if (lower == "it")
                        return "it";
                      break;
                    case 'j':
                      if (lower == "ja")
                        return "ja";
                      break;
                    case 'k':
                      if (lower == "ko")
                        return "ko";
                      break;
                    case 'n':
                      if (lower == "nl")
                        return "nl";
                      break;
                    case 'p':
                      if (lower == "pt")
                        return "pt";
                      break;
                    case 'r':
                      if (lower == "ru")
                        return "ru";
                      break;
                  }
                  break;
                case 3:
                  if (lower == "chs")
                    return "zh-CN";
                  break;
              }
            }
            return (string) null;
          }
        }
      }
    }
    catch
    {
    }
    return (string) null;
  }
}
