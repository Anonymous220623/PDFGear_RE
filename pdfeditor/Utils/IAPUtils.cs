// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.IAPUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.IAP;
using pdfeditor.Controls.Users;
using System;
using System.Windows;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Utils;

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
    if (!IAPHelper.ShowActivationWindow(source, ext))
      return;
    DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (Delegate) (() =>
    {
      Window mainWindow = App.Current.MainWindow;
      if (mainWindow == null || !(mainWindow.FindName("UserInfoControl") is UserInfoControl name2))
        return;
      name2.Open();
    }));
  }

  public static void ShowPaidWindows()
  {
  }

  public static bool IsPaidUserWrapper() => IAPHelper.IsPaidUser;
}
