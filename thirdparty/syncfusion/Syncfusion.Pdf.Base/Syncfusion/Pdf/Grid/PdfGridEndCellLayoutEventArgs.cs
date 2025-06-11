// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.PdfGridEndCellLayoutEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Grid;

public class PdfGridEndCellLayoutEventArgs : GridCellLayoutEventArgs
{
  private PdfGridCellStyle m_style;

  public PdfGridCellStyle Style => this.m_style;

  internal PdfGridEndCellLayoutEventArgs(
    PdfGraphics graphics,
    int rowIndex,
    int cellInder,
    RectangleF bounds,
    string value,
    PdfGridCellStyle style,
    bool isHeaderRow)
    : base(graphics, rowIndex, cellInder, bounds, value, isHeaderRow)
  {
    this.m_style = style;
  }
}
