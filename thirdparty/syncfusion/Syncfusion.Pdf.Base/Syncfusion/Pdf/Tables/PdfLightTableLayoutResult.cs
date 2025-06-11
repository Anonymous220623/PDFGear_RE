// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.PdfLightTableLayoutResult
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class PdfLightTableLayoutResult : PdfLayoutResult
{
  private PdfStringLayoutResult[] m_cellResults;
  private int m_rowIndex;

  internal PdfStringLayoutResult[] CellResults => this.m_cellResults;

  public int LastRowIndex => this.m_rowIndex;

  internal PdfLightTableLayoutResult(
    PdfPage page,
    RectangleF bounds,
    int rowIndex,
    PdfStringLayoutResult[] cellResults)
    : base(page, bounds)
  {
    this.m_rowIndex = rowIndex;
    this.m_cellResults = cellResults;
  }
}
