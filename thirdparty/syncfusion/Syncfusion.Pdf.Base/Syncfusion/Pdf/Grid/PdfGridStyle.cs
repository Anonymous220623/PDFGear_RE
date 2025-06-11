// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.PdfGridStyle
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Grid;

public class PdfGridStyle : PdfGridStyleBase
{
  private float m_cellSpacing;
  private PdfPaddings m_cellPadding;
  private PdfBorderOverlapStyle m_borderOverlapStyle;
  private bool m_bAllowHorizontalOverflow;
  private PdfHorizontalOverflowType m_HorizontalOverflowType;

  public float CellSpacing
  {
    get => this.m_cellSpacing;
    set => this.m_cellSpacing = value;
  }

  public PdfPaddings CellPadding
  {
    get
    {
      if (this.m_cellPadding == null)
        this.m_cellPadding = new PdfPaddings();
      this.GridCellPadding = this.m_cellPadding;
      return this.m_cellPadding;
    }
    set
    {
      this.m_cellPadding = value;
      this.GridCellPadding = this.m_cellPadding;
    }
  }

  public PdfBorderOverlapStyle BorderOverlapStyle
  {
    get => this.m_borderOverlapStyle;
    set => this.m_borderOverlapStyle = value;
  }

  public bool AllowHorizontalOverflow
  {
    get => this.m_bAllowHorizontalOverflow;
    set => this.m_bAllowHorizontalOverflow = value;
  }

  public PdfHorizontalOverflowType HorizontalOverflowType
  {
    get => this.m_HorizontalOverflowType;
    set => this.m_HorizontalOverflowType = value;
  }

  public PdfGridStyle()
  {
    this.m_borderOverlapStyle = PdfBorderOverlapStyle.Overlap;
    this.m_bAllowHorizontalOverflow = false;
    this.m_HorizontalOverflowType = PdfHorizontalOverflowType.LastPage;
  }
}
