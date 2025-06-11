// Decompiled with JetBrains decompiler
// Type: Standard.SafeConnectionPointCookie
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace Standard;

internal sealed class SafeConnectionPointCookie : SafeHandleZeroOrMinusOneIsInvalid
{
  private IConnectionPoint _cp;

  public SafeConnectionPointCookie(IConnectionPointContainer target, object sink, Guid eventId)
    : base(true)
  {
    Verify.IsNotNull<IConnectionPointContainer>(target, nameof (target));
    Verify.IsNotNull<object>(sink, nameof (sink));
    Verify.IsNotDefault<Guid>(eventId, nameof (eventId));
    this.handle = IntPtr.Zero;
    IConnectionPoint ppCP = (IConnectionPoint) null;
    try
    {
      target.FindConnectionPoint(ref eventId, out ppCP);
      int pdwCookie;
      ppCP.Advise(sink, out pdwCookie);
      this.handle = pdwCookie != 0 ? new IntPtr(pdwCookie) : throw new InvalidOperationException("IConnectionPoint::Advise returned an invalid cookie.");
      this._cp = ppCP;
      ppCP = (IConnectionPoint) null;
    }
    finally
    {
      Utility.SafeRelease<IConnectionPoint>(ref ppCP);
    }
  }

  public void Disconnect() => this.ReleaseHandle();

  [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
  protected override bool ReleaseHandle()
  {
    try
    {
      if (!this.IsInvalid)
      {
        int dwCookie = IntPtr.Size == 4 ? this.handle.ToInt32() : (int) this.handle.ToInt64();
        this.handle = IntPtr.Zero;
        try
        {
          this._cp.Unadvise(dwCookie);
        }
        finally
        {
          Utility.SafeRelease<IConnectionPoint>(ref this._cp);
        }
      }
      return true;
    }
    catch
    {
      return false;
    }
  }
}
