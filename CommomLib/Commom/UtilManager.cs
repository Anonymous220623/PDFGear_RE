// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.UtilManager
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Reflection;
using System.Text.RegularExpressions;

#nullable disable
namespace CommomLib.Commom;

public class UtilManager
{
  public static readonly string WordExtention = "*.docx;*.doc;";
  public static readonly string RtfExtention = "*.rtf;";
  public static readonly string TxtExtention = "*.txt;";
  public static readonly string ExcelExtention = "*.xlsx;*.xls;";
  public static readonly string PPTExtention = "*.pptx;*.pptm;potx;potm;";
  public static readonly string ImageExtention = "*.bmp;*.ico;*.jpeg;*.jpg;*.png;";
  public static readonly string[] WordExtentions = new string[2]
  {
    ".docx",
    ".doc"
  };
  public static readonly string[] RtfExtentions = new string[1]
  {
    ".rtf"
  };
  public static readonly string[] TxtExtentions = new string[1]
  {
    ".txt"
  };
  public static readonly string[] ExcelExtentions = new string[2]
  {
    ".xlsx",
    ".xls"
  };
  public static readonly string[] PPTExtentions = new string[4]
  {
    ".pptx",
    ".pptm",
    ".potx",
    ".potm"
  };
  public static readonly string[] ImageExtentions = new string[5]
  {
    ".bmp",
    ".ico",
    ".jpg",
    ".jpeg",
    ".png"
  };
  public static readonly string[] PDFExtentions = new string[1]
  {
    ".pdf"
  };
  private static string User_CID = "";
  private static string User_CID_Default = "NotSet";

  public static string GetProductName() => "PDFgear";

  public static string GetAppVersion()
  {
    try
    {
      Version version = AppIdHelper.Version;
      return $"{version.Major}.{version.Minor}.{version.Build}";
    }
    catch (Exception ex)
    {
      Version version = Assembly.GetExecutingAssembly().GetName().Version;
      return $"{version.Major}.{version.Minor}.{version.Build}";
    }
  }

  public static string GetUUID()
  {
    string userUuid = ConfigManager.GetUserUUID();
    if (userUuid.Length == 0)
    {
      ConfigManager.GenerateUserUUID();
      userUuid = ConfigManager.GetUserUUID();
    }
    return userUuid;
  }

  public static string GetUserCID()
  {
    if (string.IsNullOrEmpty(UtilManager.User_CID))
      UtilManager.User_CID = "";
    return string.IsNullOrEmpty(UtilManager.User_CID) ? UtilManager.User_CID_Default : UtilManager.User_CID;
  }

  public static bool IsNewUser()
  {
    try
    {
      if (!ConfigManager.GetIsNewUser())
        return false;
      if ((DateTime.Now - new DateTime(2000, 1, 1, 0, 0, 0)).TotalDays - ConfigManager.GetInstallDate() < 1.0)
      {
        if (ConfigManager.GetOriginVersion().CompareTo("2.1.5") > 0)
          return true;
        ConfigManager.SetIsNewUser(false);
        return false;
      }
      ConfigManager.SetIsNewUser(false);
      return false;
    }
    catch
    {
    }
    return false;
  }

  public static bool IsEmailValid(string strEmail)
  {
    return Regex.Match(strEmail, "[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2}").Success;
  }

  public static string GetProductID() => "P3D7F9GE21AR";

  public static string GetAppDataPath() => AppDataHelper.AppDataRoot;

  public static string GetLocalCachePath() => AppDataHelper.LocalCacheFolder;

  public static string GetTemporaryPath() => AppDataHelper.TemporaryFolder;
}
