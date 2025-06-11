// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Utils.IAPUtils
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using CommomLib.Commom;
using CommomLib.IAP;
using System;

#nullable disable
namespace PDFLauncher.Utils;

public static class IAPUtils
{
  public static async void GetIAPProductsAsync()
  {
    try
    {
      if (await IAPHelper.ShouldRenewAsync())
      {
        int num1 = await IAPHelper.RenewAccessTokenAsync() ? 1 : 0;
      }
      else
      {
        int num2 = await IAPHelper.UpdateUserInfoAsync() ? 1 : 0;
      }
    }
    catch
    {
    }
  }

  public static void ShowPurchaseWindows(string source, string ext)
  {
    IAPHelper.ShowActivationWindow(source, ext);
  }

  public static void ShowPaidWindows()
  {
  }

  public static bool IsPaidUserWrapper() => IAPHelper.IsPaidUser;

  public static bool NeedToShowIAP()
  {
    if (IAPUtils.IsPaidUserWrapper())
      return false;
    if ((DateTime.Now - new DateTime(2000, 1, 1, 0, 0, 0)).TotalSeconds - (double) ConfigManager.GetLastShowIAPTimestamp() < 86400.0)
    {
      if (ConfigManager.GetStartupShowIAPOneDayCount() >= 3L)
        return false;
    }
    else
      ConfigManager.SetStartupShowIAPOneDayCount(0L);
    long timestamp = ConfigManager.GetStartupShowIAPOneDayCount() + 1L;
    ConfigManager.SetStartupShowIAPOneDayCount(timestamp);
    if (timestamp == 1L)
      ConfigManager.SetLastShowIAPTimestamp((long) (DateTime.Now - new DateTime(2000, 1, 1, 0, 0, 0)).TotalSeconds);
    return true;
  }
}
