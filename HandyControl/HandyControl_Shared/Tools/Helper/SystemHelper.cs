// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Helper.SystemHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Interop;
using Microsoft.Win32;
using System.Runtime.InteropServices;

#nullable disable
namespace HandyControl.Tools.Helper;

internal class SystemHelper
{
  private const string SkinTypeRegistryKeyName = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize";
  private const string SkinTypeRegistryValueName = "AppsUseLightTheme";

  public static SystemVersionInfo GetSystemVersionInfo()
  {
    InteropValues.RTL_OSVERSIONINFOEX lpVersionInformation = new InteropValues.RTL_OSVERSIONINFOEX();
    lpVersionInformation.dwOSVersionInfoSize = (uint) Marshal.SizeOf<InteropValues.RTL_OSVERSIONINFOEX>(lpVersionInformation);
    InteropMethods.Gdip.RtlGetVersion(out lpVersionInformation);
    return new SystemVersionInfo((int) lpVersionInformation.dwMajorVersion, (int) lpVersionInformation.dwMinorVersion, (int) lpVersionInformation.dwBuildNumber);
  }

  public static bool DetermineIfInLightThemeMode()
  {
    object obj = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", (object) "0");
    return obj != null && obj.ToString() != "0";
  }
}
