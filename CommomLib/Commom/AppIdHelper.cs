// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.AppIdHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

#nullable disable
namespace CommomLib.Commom;

public static class AppIdHelper
{
  public const string AppUserModelId = "578ab678-3bcf-4410-8b82-675d5d214865";
  public const string AppName = "PDFgear";
  private static bool? isAdmin;
  public const string ProgId = "PdfGear.App.1";
  [ThreadStatic]
  private static bool hasUserChoiceLatestCache;

  public static Version Version => typeof (AppIdHelper).Assembly.GetName().Version;

  public static void RegisterAppUserModelId()
  {
    AppIdHelper.SetCurrentProcessExplicitAppUserModelID("578ab678-3bcf-4410-8b82-675d5d214865");
  }

  [DllImport("shell32.dll", SetLastError = true)]
  private static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

  public static bool IsAdmin
  {
    get
    {
      if (!AppIdHelper.isAdmin.HasValue)
      {
        try
        {
          AppIdHelper.isAdmin = new bool?(new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator));
        }
        catch
        {
          AppIdHelper.isAdmin = new bool?(false);
        }
      }
      return AppIdHelper.isAdmin.Value;
    }
  }

  public static string GetDefaultAppProgId(string ext)
  {
    if (string.IsNullOrEmpty(ext))
      return (string) null;
    ext = ext.Trim();
    try
    {
      string name1;
      string name2;
      if (ext.Contains("."))
      {
        name1 = $"Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\{ext}\\UserChoiceLatest\\ProgId";
        name2 = $"Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\{ext}\\UserChoice";
      }
      else
      {
        name1 = $"Software\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\{ext}\\UserChoiceLatest\\ProgId";
        name2 = $"Software\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\{ext}\\UserChoice";
      }
      using (RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default).OpenSubKey(name1))
      {
        string defaultAppProgId = registryKey1?.GetValue("ProgId") as string;
        if (string.IsNullOrEmpty(defaultAppProgId))
        {
          using (RegistryKey registryKey2 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default).OpenSubKey(name2))
            defaultAppProgId = registryKey2?.GetValue("ProgId") as string;
        }
        return defaultAppProgId;
      }
    }
    catch
    {
    }
    return (string) null;
  }

  public static void RemoveOpenWithListAppFlag(string ext)
  {
    AppIdHelper.RemoveOpenWithListAppFlag("PdfGear.App.1", ext);
  }

  public static void RemoveOpenWithListAppFlag(string progId, string ext)
  {
    if (string.IsNullOrEmpty(progId))
      return;
    if (string.IsNullOrEmpty(ext))
      return;
    try
    {
      using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ApplicationAssociationToasts", true))
        registryKey?.DeleteValue($"{progId}_{ext}", false);
    }
    catch
    {
    }
  }

  public static bool HasUserChoiceLatest
  {
    get
    {
      if (!AppIdHelper.hasUserChoiceLatestCache)
      {
        try
        {
          using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.pdf\\UserChoiceLatest"))
            AppIdHelper.hasUserChoiceLatestCache = registryKey != null;
        }
        catch
        {
          AppIdHelper.hasUserChoiceLatestCache = true;
        }
      }
      return AppIdHelper.hasUserChoiceLatestCache;
    }
  }
}
