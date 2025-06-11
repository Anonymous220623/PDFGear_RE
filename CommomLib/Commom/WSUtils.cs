// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.WSUtils
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Globalization;
using System.Windows;

#nullable disable
namespace CommomLib.Commom;

public class WSUtils
{
  public static void LoadWindowInfo(string source = null)
  {
    WindowState windowState = WSUtils.GetWindowState(source);
    Application.Current.MainWindow.WindowState = windowState;
    if (windowState != WindowState.Normal || WSUtils.RestoreWindowInfo(source))
      return;
    Application.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
  }

  public static void SaveWindowInfo(string source = null)
  {
    try
    {
      WSUtils.SetWindowState(Application.Current.MainWindow.WindowState, source);
      ConfigManager.SetWindowSize(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}|{1}|{2}|{3}", (object) Application.Current.MainWindow.Left, (object) Application.Current.MainWindow.Top, (object) Application.Current.MainWindow.Width, (object) Application.Current.MainWindow.Height), source);
    }
    catch
    {
    }
  }

  private static bool RestoreWindowInfo(string source = null)
  {
    try
    {
      string windowSize = ConfigManager.GetWindowSize(source);
      if (string.IsNullOrWhiteSpace(windowSize))
        return false;
      string[] strArray = windowSize.Replace(",", ".").Split('|');
      NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands;
      double result1;
      double result2;
      double result3;
      double result4;
      if (!double.TryParse(strArray[0], style, (IFormatProvider) CultureInfo.InvariantCulture, out result1) || !double.TryParse(strArray[1], style, (IFormatProvider) CultureInfo.InvariantCulture, out result2) || !double.TryParse(strArray[2], style, (IFormatProvider) CultureInfo.InvariantCulture, out result3) || !double.TryParse(strArray[3], style, (IFormatProvider) CultureInfo.InvariantCulture, out result4) || result3 <= 0.0 || result4 <= 0.0)
        return false;
      Rect workArea = SystemParameters.WorkArea;
      if (result1 < 0.0)
        result1 = 0.0;
      if (result2 < workArea.Top)
        result2 = workArea.Top;
      double width = workArea.Width;
      double height = workArea.Height;
      if (result1 + result3 / 2.0 > width)
        result1 = width - result3;
      if (result2 + result4 / 2.0 > height)
        result2 = height - result4;
      if (result1 < 0.0)
        result1 = 0.0;
      if (result2 < workArea.Top)
        result2 = workArea.Top;
      Application.Current.MainWindow.Left = result1;
      Application.Current.MainWindow.Top = result2;
      Application.Current.MainWindow.Width = result3;
      Application.Current.MainWindow.Height = result4;
      return true;
    }
    catch
    {
    }
    return false;
  }

  private static WindowState GetWindowState(string source = null)
  {
    return ConfigManager.GetWindowState(source) == 0 ? WindowState.Maximized : WindowState.Normal;
  }

  private static void SetWindowState(WindowState ws, string source = null)
  {
    if (ws == WindowState.Maximized)
      ConfigManager.SetWindowState(0, source);
    else
      ConfigManager.SetWindowState(1, source);
  }
}
