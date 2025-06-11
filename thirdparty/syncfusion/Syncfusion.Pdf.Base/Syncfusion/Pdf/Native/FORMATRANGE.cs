// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.FORMATRANGE
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Native;

[StructLayout(LayoutKind.Sequential)]
internal class FORMATRANGE
{
  public IntPtr hdc = IntPtr.Zero;
  public IntPtr hdcTarget = IntPtr.Zero;
  public RECT rc = new RECT();
  public RECT rcPage = new RECT();
  public CHARRANGE chrg = new CHARRANGE();
}
