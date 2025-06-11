// Decompiled with JetBrains decompiler
// Type: PDFKit.Common
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.IO;
using System.Windows.Threading;

#nullable disable
namespace PDFKit;

public static class Common
{
  private static string actualLanguage;
  private static Func<string> productNameGetter;
  private static Func<string> versionGetter;
  internal static Action<string, string, string, long> sendEventAction;
  private static Action<string> logAction;
  private static Dispatcher uiDispatcher;
  private static string appDataFolder;

  public static void Initialize(
    string actualLanguage,
    Func<string> productNameGetter,
    Func<string> versionGetter,
    Action<string, string, string, long> sendEventAction,
    Action<string> logAction)
  {
    Common.actualLanguage = !string.IsNullOrEmpty(actualLanguage) ? actualLanguage : throw new ArgumentException(nameof (actualLanguage));
    Common.productNameGetter = productNameGetter ?? throw new ArgumentException(nameof (productNameGetter));
    Common.versionGetter = versionGetter ?? throw new ArgumentException(nameof (versionGetter));
    Common.sendEventAction = sendEventAction ?? throw new ArgumentException(nameof (sendEventAction));
    Common.logAction = logAction ?? throw new ArgumentException(nameof (logAction));
    Common.uiDispatcher = Dispatcher.CurrentDispatcher;
  }

  internal static string GetProductName() => Common.productNameGetter();

  internal static string GetAppVersion() => Common.versionGetter();

  internal static void WriteLog(string log)
  {
    Action<string> logAction = Common.logAction;
    if (logAction == null)
      return;
    logAction(log);
  }

  public static void SetAppDataFolder(string appDataFolder) => Common.appDataFolder = appDataFolder;

  internal static string AppDataFolder => Common.appDataFolder;

  internal static string LocalCacheFolder => Path.Combine(Common.AppDataFolder, "LocalCache");

  internal static string LocalFolder => Path.Combine(Common.AppDataFolder, "AppData");

  internal static string TemporaryFolder => Path.Combine(Common.AppDataFolder, "TempState");

  internal static Dispatcher UIDispatcher => Common.uiDispatcher;

  internal static string ActualLanguage => Common.actualLanguage;
}
