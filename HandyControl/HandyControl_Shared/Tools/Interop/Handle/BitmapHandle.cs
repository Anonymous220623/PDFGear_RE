// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Interop.BitmapHandle
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

#nullable disable
namespace HandyControl.Tools.Interop;

internal sealed class BitmapHandle : WpfSafeHandle
{
  [SecurityCritical]
  private BitmapHandle()
    : this(true)
  {
  }

  [SecurityCritical]
  private BitmapHandle(bool ownsHandle)
    : base(ownsHandle, CommonHandles.GDI)
  {
  }

  [SecurityCritical]
  [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
  protected override bool ReleaseHandle() => InteropMethods.DeleteObject(this.handle);

  [SecurityCritical]
  internal HandleRef MakeHandleRef(object obj) => new HandleRef(obj, this.handle);

  [SecurityCritical]
  internal static BitmapHandle CreateFromHandle(IntPtr hbitmap, bool ownsHandle = true)
  {
    BitmapHandle fromHandle = new BitmapHandle(ownsHandle);
    fromHandle.handle = hbitmap;
    return fromHandle;
  }
}
