// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.PdfLightTableBuiltinStyleSettings
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class PdfLightTableBuiltinStyleSettings
{
  private bool m_bandedColumns;
  private bool m_bandedRows;
  private bool m_firstColumn;
  private bool m_lastColumn;
  private bool m_headerRow;
  private bool m_lastRow;

  public bool ApplyStyleForBandedColumns
  {
    get => this.m_bandedColumns;
    set => this.m_bandedColumns = value;
  }

  public bool ApplyStyleForBandedRows
  {
    get => this.m_bandedRows;
    set => this.m_bandedRows = value;
  }

  public bool ApplyStyleForFirstColumn
  {
    get => this.m_firstColumn;
    set => this.m_firstColumn = value;
  }

  public bool ApplyStyleForHeaderRow
  {
    get => this.m_headerRow;
    set => this.m_headerRow = value;
  }

  public bool ApplyStyleForLastColumn
  {
    get => this.m_lastColumn;
    set => this.m_lastColumn = value;
  }

  public bool ApplyStyleForLastRow
  {
    get => this.m_lastRow;
    set => this.m_lastRow = value;
  }
}
