// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.RateUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using pdfeditor.Views;
using System.Threading.Tasks;
using System.Windows;

#nullable disable
namespace pdfeditor.Utils;

internal class RateUtils
{
  public static bool CheckAndShowRate(string file)
  {
    if (!ConfigManager.GetCouldRateFlag())
      return false;
    if (!ConfigManager.GetSubscriptionFlag())
    {
      ConfigManager.SetSubscriptionFlag(true);
      RateWindow rateWindow = new RateWindow();
      rateWindow.Owner = Application.Current.MainWindow;
      rateWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      rateWindow.ShowDialog();
      rateWindow.Activate();
      return false;
    }
    if (!ConfigManager.GetPhoneQRCodeFlag())
    {
      GAManager.SendEvent("Ads", "GearForMobile", "ClosePDF", 1L);
      ConfigManager.SetPhoneQRCodeFlag(true);
      GearForMobilephone gearForMobilephone = new GearForMobilephone();
      gearForMobilephone.Owner = Application.Current.MainWindow;
      gearForMobilephone.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      gearForMobilephone.ShowDialog();
      gearForMobilephone.Activate();
      return false;
    }
    if (!RateUtils.IsNeedToShowSurvey())
      return false;
    ConfigManager.SetSurveyFlag(true);
    Survey survey = new Survey();
    survey.Owner = Application.Current.MainWindow;
    survey.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    survey.ShowDialog();
    survey.Activate();
    return false;
  }

  public static bool IsNeedToShowSurvey()
  {
    return !ConfigManager.GetSurveyFlag() && CultureInfoUtils.ActualAppLanguage != "zh-CN" && ConfigManager.getAppLaunchCount() > 10L;
  }

  public static async Task<bool> ShowRatingReviewDialog() => false;
}
