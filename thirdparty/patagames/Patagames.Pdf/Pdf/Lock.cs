// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Lock
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Threading;

#nullable disable
namespace Patagames.Pdf;

internal class Lock : IDisposable
{
  private static Lock _syncMT = new Lock()
  {
    IsEnabled = true
  };
  private object _sync = new object();
  private long _wasTaken;

  public bool IsEnabled { get; set; }

  public static Lock SyncMT
  {
    get
    {
      if (Lock._syncMT.IsEnabled)
      {
        Monitor.Enter(Lock._syncMT._sync);
        ++Lock._syncMT._wasTaken;
      }
      return Lock._syncMT;
    }
  }

  public void Dispose()
  {
    if (--this._wasTaken < 0L)
      return;
    if (this._wasTaken < 0L)
      this._wasTaken = 0L;
    Monitor.Exit(this._sync);
  }
}
