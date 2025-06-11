// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ThreadLocalStorageHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Threading;

#nullable disable
namespace NLog.Internal;

internal static class ThreadLocalStorageHelper
{
  public static object AllocateDataSlot() => (object) Thread.AllocateDataSlot();

  public static T GetDataForSlot<T>(object slot, bool create = true) where T : class, new()
  {
    LocalDataStoreSlot slot1 = (LocalDataStoreSlot) slot;
    object data = Thread.GetData(slot1);
    if (data == null)
    {
      if (!create)
        return default (T);
      data = (object) new T();
      Thread.SetData(slot1, data);
    }
    return (T) data;
  }
}
