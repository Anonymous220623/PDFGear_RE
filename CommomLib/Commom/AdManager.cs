// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.AdManager
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

#nullable disable
namespace CommomLib.Commom;

public class AdManager
{
  public static PromoteAd ad;
  private static HttpClient httpClient;

  public static async Task UpdateAdConfig()
  {
  }

  public static bool GetAdConfig() => true;

  public static async Task<bool> GetFileWatcherDisabledAsync() => false;

  private static string GetConfigFilePath()
  {
    string str = Path.Combine(UtilManager.GetLocalCachePath(), "Config");
    if (!Directory.Exists(str))
      Directory.CreateDirectory(str);
    return Path.Combine(str, "conf.json");
  }

  public static bool NeedToGetConfig()
  {
    bool getConfig = true;
    if ((DateTime.Now - new DateTime(2000, 1, 1, 0, 0, 0)).TotalSeconds - (double) ConfigManager.GetLastConfigTimestamp() < 86400.0)
      getConfig = false;
    return getConfig;
  }

  public static async Task UpdateUserCommunityConfigAsync()
  {
  }

  public static AdManager.UserCommunityConfig GetUserCommunityConfig()
  {
    return (AdManager.UserCommunityConfig) null;
  }

  public static bool NeedToGetUserCommunityConfig() => false;

  public class UserCommunityConfig
  {
    internal UserCommunityConfig(
      string qqGroupName,
      string qqGroupNumber,
      string qqGroupLink,
      bool isEmailUCEnabled,
      bool isQQUCEnabled)
    {
      this.QQGroupName = qqGroupName;
      this.QQGroupNumber = qqGroupNumber;
      this.QQGroupLink = qqGroupLink;
      this.IsEmailUCEnabled = isEmailUCEnabled;
      this.IsQQUCEnabled = isQQUCEnabled;
    }

    public string QQGroupName { get; }

    public string QQGroupNumber { get; }

    public string QQGroupLink { get; }

    public bool IsEmailUCEnabled { get; }

    public bool IsQQUCEnabled { get; }
  }
}
