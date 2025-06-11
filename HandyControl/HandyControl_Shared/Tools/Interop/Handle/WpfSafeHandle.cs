// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Interop.WpfSafeHandle
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using Microsoft.Win32.SafeHandles;
using System.Security;

#nullable disable
namespace HandyControl.Tools.Interop;

internal abstract class WpfSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
{
  private readonly int _collectorId;

  [SecurityCritical]
  protected WpfSafeHandle(bool ownsHandle, int collectorId)
    : base(ownsHandle)
  {
    HandleCollector.Add(collectorId);
    this._collectorId = collectorId;
  }

  [SecurityCritical]
  [SecuritySafeCritical]
  protected override void Dispose(bool disposing)
  {
    HandleCollector.Remove(this._collectorId);
    base.Dispose(disposing);
  }
}
