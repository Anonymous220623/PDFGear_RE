// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.OTableCell
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class OTableCell
{
  private object m_value;
  private string m_value2;
  private bool m_booleanValue;
  private string m_currency;
  private DateTime m_dateValue;
  private TimeSpan m_timeValue;
  private string m_formula;
  private int m_columnsSpanned;
  private int m_rowsSpanned;
  private int m_matrixColunsSpanned;
  private int m_matrixRowsSpanned;
  private CellValueType m_type;
  private string m_styleName;
  private int m_columnsRepeated;
  private string m_tableFormula;
  private OParagraph m_paragraph;
  private bool m_isBlank;
  private List<OTextBodyItem> m_textBodyIetm;
  private float m_cellWidth;

  internal float CellWidth
  {
    get => this.m_cellWidth;
    set => this.m_cellWidth = value;
  }

  internal List<OTextBodyItem> TextBodyIetm
  {
    get
    {
      if (this.m_textBodyIetm == null)
        this.m_textBodyIetm = new List<OTextBodyItem>();
      return this.m_textBodyIetm;
    }
    set => this.m_textBodyIetm = value;
  }

  internal object Value
  {
    get => this.m_value;
    set => this.m_value = value;
  }

  internal string Value2
  {
    get => this.m_value2;
    set => this.m_value2 = value;
  }

  internal CellValueType Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  internal string StyleName
  {
    get => this.m_styleName;
    set => this.m_styleName = value;
  }

  internal int ColumnsRepeated
  {
    get => this.m_columnsRepeated;
    set => this.m_columnsRepeated = value;
  }

  internal string TableFormula
  {
    get => this.m_tableFormula;
    set => this.m_tableFormula = value;
  }

  internal OParagraph Paragraph
  {
    get => this.m_paragraph;
    set => this.m_paragraph = value;
  }

  internal bool BooleanValue
  {
    get => this.m_booleanValue;
    set => this.m_booleanValue = value;
  }

  internal string Currency
  {
    get => this.m_currency;
    set => this.m_currency = value;
  }

  internal DateTime DateValue
  {
    get => this.m_dateValue;
    set => this.m_dateValue = value;
  }

  internal TimeSpan TimeValue
  {
    get => this.m_timeValue;
    set => this.m_timeValue = value;
  }

  internal int ColumnsSpanned
  {
    get => this.m_columnsSpanned;
    set => this.m_columnsSpanned = value;
  }

  internal int RowsSpanned
  {
    get => this.m_rowsSpanned;
    set => this.m_rowsSpanned = value;
  }

  internal int MatrixColunsSpanned
  {
    get => this.m_matrixColunsSpanned;
    set => this.m_matrixColunsSpanned = value;
  }

  internal int MatrixRowsSpanned
  {
    get => this.m_matrixRowsSpanned;
    set => this.m_matrixRowsSpanned = value;
  }

  internal bool IsBlank
  {
    get => this.m_isBlank;
    set => this.m_isBlank = value;
  }

  public override bool Equals(object obj)
  {
    return obj is OTableCell otableCell && this.IsBlank && otableCell.IsBlank;
  }

  internal void Dispose()
  {
    if (this.m_paragraph != null)
    {
      this.m_paragraph.Dispose();
      this.m_paragraph = (OParagraph) null;
    }
    if (this.m_textBodyIetm == null)
      return;
    for (int index = 0; index < this.m_textBodyIetm.Count; ++index)
      this.m_textBodyIetm[index] = (OTextBodyItem) null;
    this.m_textBodyIetm.Clear();
    this.m_textBodyIetm = (List<OTextBodyItem>) null;
  }
}
