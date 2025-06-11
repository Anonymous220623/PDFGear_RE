// Decompiled with JetBrains decompiler
// Type: FileWatcher.SingleInstance
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

using System.Threading;

#nullable disable
namespace FileWatcher;

public static class SingleInstance
{
  private const string MutexName = "CB79BC3B-4851-4371-BA8E-213000C3AE44";
  private static Mutex mutex;
  private static bool? isMainInstance;
  private static object locker = new object();

  public static bool IsMainInstance
  {
    get
    {
      if (!SingleInstance.isMainInstance.HasValue)
      {
        lock (SingleInstance.locker)
        {
          if (!SingleInstance.isMainInstance.HasValue)
          {
            bool createdNew;
            SingleInstance.mutex = new Mutex(true, "CB79BC3B-4851-4371-BA8E-213000C3AE44", out createdNew);
            SingleInstance.isMainInstance = new bool?(createdNew);
            if (!createdNew)
            {
              SingleInstance.mutex.Dispose();
              SingleInstance.mutex = (Mutex) null;
            }
          }
        }
      }
      return SingleInstance.isMainInstance.Value;
    }
  }

  internal static void TryReleaseMutex()
  {
    SingleInstance.mutex?.Dispose();
    SingleInstance.mutex = (Mutex) null;
    SingleInstance.isMainInstance = new bool?(false);
  }
}
