// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.OTable
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class OTable : OTextBodyItem
{
  internal const int MaxColumnCount = 16384 /*0x4000*/;
  internal const int MaxRowCount = 1048576 /*0x100000*/;
  private string m_name;
  private string m_styleName;
  private bool m_softPageBreak;
  private List<OTableColumn> m_columns;
  private List<OTableRow> m_rows;
  private bool m_hasDefaultColumnStyle;
  private NamedExpressions m_expressions;

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal string StyleName
  {
    get => this.m_styleName;
    set => this.m_styleName = value;
  }

  internal bool SoftPageBreak
  {
    get => this.m_softPageBreak;
    set => this.m_softPageBreak = value;
  }

  internal List<OTableColumn> Columns
  {
    get
    {
      if (this.m_columns == null)
        this.m_columns = new List<OTableColumn>();
      return this.m_columns;
    }
    set => this.m_columns = value;
  }

  internal List<OTableRow> Rows
  {
    get
    {
      if (this.m_rows == null)
        this.m_rows = new List<OTableRow>();
      return this.m_rows;
    }
    set => this.m_rows = value;
  }

  internal NamedExpressions Expressions
  {
    get => this.m_expressions;
    set => this.m_expressions = value;
  }

  internal bool HasDefaultColumnStyle
  {
    get => this.m_hasDefaultColumnStyle;
    set => this.m_hasDefaultColumnStyle = value;
  }

  internal void Dispose()
  {
    if (this.m_columns != null)
    {
      this.m_columns.Clear();
      this.m_columns = (List<OTableColumn>) null;
    }
    if (this.m_rows == null)
      return;
    foreach (OTableRow row in this.m_rows)
      row.Dispose();
    this.m_rows.Clear();
    this.m_rows = (List<OTableRow>) null;
  }
}
