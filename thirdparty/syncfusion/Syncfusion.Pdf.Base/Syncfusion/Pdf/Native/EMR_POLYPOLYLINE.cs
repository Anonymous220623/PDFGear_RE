// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.EMR_POLYPOLYLINE
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Native;

internal struct EMR_POLYPOLYLINE
{
  public RECT rclBounds;
  public uint nPolys;
  public uint cpts;
  [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)]
  public uint[] aPolyCounts;
  [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.Struct)]
  public POINT[] apts;
}
