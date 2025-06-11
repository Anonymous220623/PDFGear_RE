// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.PdfColumn
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class PdfColumn
{
  private const float DefaultWidth = 10f;
  private float m_width;
  private PdfStringFormat m_stringFormat;
  private string m_columnName;
  internal bool isCustomWidth;
  internal PdfLightTableDataSourceType m_dataSourceType;

  public PdfStringFormat StringFormat
  {
    get => this.m_stringFormat;
    set => this.m_stringFormat = value;
  }

  public float Width
  {
    get => this.m_width;
    set
    {
      if ((double) value < 0.0)
        throw new ArgumentException("The width should be a positive number.", nameof (Width));
      if (this.m_dataSourceType != PdfLightTableDataSourceType.TableDirect)
        this.isCustomWidth = true;
      this.m_width = value;
    }
  }

  public string ColumnName
  {
    get => this.m_columnName;
    set => this.m_columnName = value;
  }

  public PdfColumn()
  {
  }

  internal PdfColumn(float width)
    : this()
  {
    this.m_width = width;
  }

  public PdfColumn(string columnName)
  {
    this.m_columnName = columnName;
    this.m_width = 10f;
  }
}
