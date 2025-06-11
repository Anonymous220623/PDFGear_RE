// Decompiled with JetBrains decompiler
// Type: Standard.SafeFindHandle
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Microsoft.Win32.SafeHandles;
using System.Security.Permissions;

#nullable disable
namespace Standard;

internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
{
  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
  private SafeFindHandle()
    : base(true)
  {
  }

  protected override bool ReleaseHandle() => NativeMethods.FindClose(this.handle);
}
