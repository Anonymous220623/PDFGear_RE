// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.GridCellLayoutEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Grid;

public abstract class GridCellLayoutEventArgs : EventArgs
{
  private int m_rowIndex;
  private int m_cellIndex;
  private string m_value;
  private RectangleF m_bounds;
  private PdfGraphics m_graphics;
  private bool m_isHeaderRow;

  public int RowIndex => this.m_rowIndex;

  public int CellIndex => this.m_cellIndex;

  public string Value => this.m_value;

  public RectangleF Bounds => this.m_bounds;

  public PdfGraphics Graphics => this.m_graphics;

  public bool IsHeaderRow => this.m_isHeaderRow;

  internal GridCellLayoutEventArgs(
    PdfGraphics graphics,
    int rowIndex,
    int cellIndex,
    RectangleF bounds,
    string value,
    bool isHeaderRow)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    this.m_rowIndex = rowIndex;
    this.m_cellIndex = cellIndex;
    this.m_value = value;
    this.m_bounds = bounds;
    this.m_graphics = graphics;
    this.m_isHeaderRow = isHeaderRow;
  }
}
