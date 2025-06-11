// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.PdfLightTableStyle
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class PdfLightTableStyle
{
  private PdfCellStyle m_defaultStyle;
  private PdfCellStyle m_alternateStyle;
  private PdfHeaderSource m_headerSource;
  private int m_headerRowCount;
  private PdfCellStyle m_headerStyle;
  private bool m_bRepeateHeader;
  private bool m_bShowHeader;
  private float m_cellSpacing;
  private float m_cellPadding;
  private PdfBorderOverlapStyle m_overlappedBorders;
  private PdfPen m_borderPen;

  public PdfCellStyle DefaultStyle
  {
    get => this.m_defaultStyle;
    set
    {
      this.m_defaultStyle = value != null ? value : throw new ArgumentNullException(nameof (DefaultStyle));
    }
  }

  public PdfCellStyle AlternateStyle
  {
    get => this.m_alternateStyle;
    set => this.m_alternateStyle = value;
  }

  public PdfHeaderSource HeaderSource
  {
    get => this.m_headerSource;
    set => this.m_headerSource = value;
  }

  public int HeaderRowCount
  {
    get => this.m_headerRowCount;
    set
    {
      this.m_headerRowCount = value >= 0 ? value : throw new ArgumentOutOfRangeException("HeaderRowsCount", "This parameter can't be less then zero");
    }
  }

  public PdfCellStyle HeaderStyle
  {
    get => this.m_headerStyle;
    set => this.m_headerStyle = value;
  }

  public bool RepeatHeader
  {
    get => this.m_bRepeateHeader;
    set => this.m_bRepeateHeader = value;
  }

  public bool ShowHeader
  {
    get => this.m_bShowHeader;
    set => this.m_bShowHeader = value;
  }

  public float CellSpacing
  {
    get => this.m_cellSpacing;
    set => this.m_cellSpacing = value;
  }

  public float CellPadding
  {
    get => this.m_cellPadding;
    set => this.m_cellPadding = value;
  }

  public PdfBorderOverlapStyle BorderOverlapStyle
  {
    get => this.m_overlappedBorders;
    set => this.m_overlappedBorders = value;
  }

  public PdfPen BorderPen
  {
    get => this.m_borderPen;
    set => this.m_borderPen = value;
  }

  public PdfLightTableStyle()
  {
    this.m_defaultStyle = new PdfCellStyle();
    this.m_bRepeateHeader = true;
    this.m_overlappedBorders = PdfBorderOverlapStyle.Overlap;
  }
}
