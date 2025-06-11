// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.EMR_EXTCREATEPEN
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Native;

internal struct EMR_EXTCREATEPEN
{
  public int ihPen;
  public int offBmi;
  public int cbBmi;
  public int offBits;
  public int cbBits;
  public int elpPenStyle;
  public int elpWidth;
  public uint elpBrushStyle;
  public int elpColor;
  public IntPtr elpHatch;
  public int elpNumEntries;
  public int[] elpStyleEntry;
}
