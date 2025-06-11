// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.BeginRowLayoutEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class BeginRowLayoutEventArgs : EventArgs
{
  private int m_rowIndex;
  private PdfCellStyle m_cellStyle;
  private int[] m_spanMap;
  private bool m_bCancel;
  private bool m_bSkip;
  private bool m_ignoreColumnFormat;
  private float m_minHeight;

  public int RowIndex => this.m_rowIndex;

  public PdfCellStyle CellStyle
  {
    get => this.m_cellStyle;
    set
    {
      this.m_cellStyle = value != null ? value : throw new ArgumentNullException(nameof (CellStyle));
    }
  }

  public int[] ColumnSpanMap
  {
    get => this.m_spanMap;
    set => this.m_spanMap = value;
  }

  public bool Cancel
  {
    get => this.m_bCancel;
    set => this.m_bCancel = value;
  }

  public bool Skip
  {
    get => this.m_bSkip;
    set => this.m_bSkip = value;
  }

  public bool IgnoreColumnFormat
  {
    get => this.m_ignoreColumnFormat;
    set => this.m_ignoreColumnFormat = value;
  }

  public float MinimalHeight
  {
    get => this.m_minHeight;
    set
    {
      this.m_minHeight = (double) value >= 0.0 ? value : throw new ArgumentOutOfRangeException(nameof (MinimalHeight), "The value can't be less then zero.");
    }
  }

  internal BeginRowLayoutEventArgs(int rowIndex, PdfCellStyle cellStyle)
  {
    this.m_rowIndex = rowIndex;
    this.m_cellStyle = cellStyle;
  }
}
