// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.PdfGridCellStyle
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;

#nullable disable
namespace Syncfusion.Pdf.Grid;

public class PdfGridCellStyle : PdfGridRowStyle
{
  private PdfBorders m_borders = PdfBorders.Default;
  private PdfPaddings m_cellPadding;
  private PdfEdges m_edges;
  private PdfStringFormat m_format;
  private PdfImage m_backgroundImage;

  public PdfStringFormat StringFormat
  {
    get => this.m_format;
    set => this.m_format = value;
  }

  public PdfBorders Borders
  {
    get => this.m_borders;
    set => this.m_borders = value;
  }

  public PdfImage BackgroundImage
  {
    get => this.m_backgroundImage;
    set => this.m_backgroundImage = value;
  }

  internal PdfEdges Edges
  {
    get
    {
      if (this.m_edges == null)
        this.m_edges = new PdfEdges();
      return this.m_edges;
    }
    set => this.m_edges = value;
  }

  public PdfPaddings CellPadding
  {
    get
    {
      if (this.m_cellPadding == null)
        this.m_cellPadding = this.GridCellPadding;
      return this.m_cellPadding;
    }
    set
    {
      if (this.m_cellPadding == null)
        this.m_cellPadding = new PdfPaddings();
      this.m_cellPadding = value;
    }
  }
}
