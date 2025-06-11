// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Interop.IconHandle
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Runtime.ConstrainedExecution;
using System.Security;

#nullable disable
namespace HandyControl.Tools.Interop;

internal sealed class IconHandle : WpfSafeHandle
{
  [SecurityCritical]
  private IconHandle()
    : base(true, CommonHandles.Icon)
  {
  }

  [SecurityCritical]
  [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
  protected override bool ReleaseHandle() => InteropMethods.DestroyIcon(this.handle);

  [SecurityCritical]
  [SecuritySafeCritical]
  internal static IconHandle GetInvalidIcon() => new IconHandle();

  [SecurityCritical]
  internal IntPtr CriticalGetHandle() => this.handle;
}
