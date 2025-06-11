// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Interop.HandleCollector
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

#nullable disable
namespace HandyControl.Tools.Interop;

internal static class HandleCollector
{
  private static HandleCollector.HandleType[] HandleTypes;
  private static int HandleTypeCount;
  private static readonly object HandleMutex = new object();

  internal static IntPtr Add(IntPtr handle, int type)
  {
    HandleCollector.HandleTypes[type - 1].Add();
    return handle;
  }

  [SecuritySafeCritical]
  internal static SafeHandle Add(SafeHandle handle, int type)
  {
    HandleCollector.HandleTypes[type - 1].Add();
    return handle;
  }

  internal static void Add(int type) => HandleCollector.HandleTypes[type - 1].Add();

  internal static int RegisterType(string typeName, int expense, int initialThreshold)
  {
    lock (HandleCollector.HandleMutex)
    {
      if (HandleCollector.HandleTypeCount == 0 || HandleCollector.HandleTypeCount == HandleCollector.HandleTypes.Length)
      {
        HandleCollector.HandleType[] destinationArray = new HandleCollector.HandleType[HandleCollector.HandleTypeCount + 10];
        if (HandleCollector.HandleTypes != null)
          Array.Copy((Array) HandleCollector.HandleTypes, 0, (Array) destinationArray, 0, HandleCollector.HandleTypeCount);
        HandleCollector.HandleTypes = destinationArray;
      }
      HandleCollector.HandleTypes[HandleCollector.HandleTypeCount++] = new HandleCollector.HandleType(expense, initialThreshold);
      return HandleCollector.HandleTypeCount;
    }
  }

  internal static IntPtr Remove(IntPtr handle, int type)
  {
    HandleCollector.HandleTypes[type - 1].Remove();
    return handle;
  }

  [SecuritySafeCritical]
  internal static SafeHandle Remove(SafeHandle handle, int type)
  {
    HandleCollector.HandleTypes[type - 1].Remove();
    return handle;
  }

  internal static void Remove(int type) => HandleCollector.HandleTypes[type - 1].Remove();

  private class HandleType
  {
    private readonly int _initialThreshHold;
    private int _threshHold;
    private int _handleCount;
    private readonly int _deltaPercent;

    internal HandleType(int expense, int initialThreshHold)
    {
      this._initialThreshHold = initialThreshHold;
      this._threshHold = initialThreshHold;
      this._deltaPercent = 100 - expense;
    }

    internal void Add()
    {
      lock (this)
      {
        ++this._handleCount;
        if (!this.NeedCollection())
          return;
      }
      GC.Collect();
      Thread.Sleep((100 - this._deltaPercent) / 4);
    }

    private bool NeedCollection()
    {
      if (this._handleCount > this._threshHold)
      {
        this._threshHold = this._handleCount + this._handleCount * this._deltaPercent / 100;
        return true;
      }
      int num = 100 * this._threshHold / (100 + this._deltaPercent);
      if (num >= this._initialThreshHold && this._handleCount < (int) ((double) num * 0.89999997615814209))
        this._threshHold = num;
      return false;
    }

    internal void Remove()
    {
      lock (this)
      {
        --this._handleCount;
        this._handleCount = Math.Max(0, this._handleCount);
      }
    }
  }
}
