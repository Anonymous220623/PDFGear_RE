// Decompiled with JetBrains decompiler
// Type: Standard.SafeHBITMAP
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;

#nullable disable
namespace Standard;

internal sealed class SafeHBITMAP : SafeHandleZeroOrMinusOneIsInvalid
{
  private SafeHBITMAP()
    : base(true)
  {
  }

  [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
  protected override bool ReleaseHandle() => NativeMethods.DeleteObject(this.handle);
}
