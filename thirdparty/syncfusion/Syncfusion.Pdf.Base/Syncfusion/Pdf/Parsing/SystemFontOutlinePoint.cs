// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontOutlinePoint
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.PdfViewer.Base;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontOutlinePoint
{
  public Point Point { get; set; }

  public byte Flags { get; set; }

  public byte Instruction { get; set; }

  public bool IsOnCurve => ((int) this.Flags & 1) != 0;

  public SystemFontOutlinePoint(byte flags) => this.Flags = flags;

  public SystemFontOutlinePoint(double x, double y, byte flags)
    : this(flags)
  {
    this.Point = new Point(x, y);
  }
}
