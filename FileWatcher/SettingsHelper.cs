// Decompiled with JetBrains decompiler
// Type: FileWatcher.SettingsHelper
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

using CommomLib.Commom;

#nullable disable
namespace FileWatcher;

internal static class SettingsHelper
{
  private const string StartupTaskId = "FileWatcherStartupTask";
  private const string LastCheckUpdateTimeKey = "LastCheckUpdateTimeKey";
  private const string HasUpdateKey = "HasUpdateKey";
  private const string DisabledByServerKey = "FileWatcherDisabledByServer";
  private const string LastCheckDisabledKey = "FileWatcherLastCheckDisabled";
  private static StartupTaskHelper startupTaskHelper = new StartupTaskHelper();
  private static readonly string[] defaultListenFolders = new string[3]
  {
    "Desktop",
    "Downloads",
    "Documents"
  };

  public static bool IsEnabled => SettingsHelper.startupTaskHelper.IsStartupTaskEnabled;

  public static string[] ListenFolders => SettingsHelper.defaultListenFolders;

  public static void SetIsEnabled(bool isEnabled)
  {
    if (isEnabled)
      SettingsHelper.startupTaskHelper.Enable();
    else
      SettingsHelper.startupTaskHelper.Disable();
  }
}
