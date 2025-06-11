// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.AppDataHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.IO;

#nullable disable
namespace CommomLib.Commom;

public static class AppDataHelper
{
  private static ApplicationSettings settings = new ApplicationSettings();

  private static string appDataRoot
  {
    get
    {
      return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PDFgear");
    }
  }

  private static string localCacheFolder => Path.Combine(AppDataHelper.appDataRoot, "LocalCache");

  private static string localFolder => Path.Combine(AppDataHelper.appDataRoot, "AppData");

  private static string temporaryFolder => Path.Combine(AppDataHelper.appDataRoot, "TempState");

  public static string AppDataRoot
  {
    get
    {
      AppDataHelper.InitFolders();
      return AppDataHelper.appDataRoot;
    }
  }

  public static string LocalCacheFolder
  {
    get
    {
      AppDataHelper.InitFolders();
      return AppDataHelper.localCacheFolder;
    }
  }

  public static string LocalFolder
  {
    get
    {
      AppDataHelper.InitFolders();
      return AppDataHelper.localFolder;
    }
  }

  public static string TemporaryFolder
  {
    get
    {
      AppDataHelper.InitFolders();
      return AppDataHelper.temporaryFolder;
    }
  }

  public static ApplicationSettings LocalSettings => AppDataHelper.settings;

  private static void InitFolders()
  {
    if (!Directory.Exists(AppDataHelper.appDataRoot))
    {
      try
      {
        Directory.CreateDirectory(AppDataHelper.appDataRoot);
      }
      catch
      {
      }
    }
    if (!Directory.Exists(AppDataHelper.localCacheFolder))
    {
      try
      {
        Directory.CreateDirectory(AppDataHelper.localCacheFolder);
      }
      catch
      {
      }
    }
    if (!Directory.Exists(AppDataHelper.localFolder))
    {
      try
      {
        Directory.CreateDirectory(AppDataHelper.localFolder);
      }
      catch
      {
      }
    }
    if (Directory.Exists(AppDataHelper.temporaryFolder))
      return;
    try
    {
      Directory.CreateDirectory(AppDataHelper.temporaryFolder);
    }
    catch
    {
    }
  }
}
