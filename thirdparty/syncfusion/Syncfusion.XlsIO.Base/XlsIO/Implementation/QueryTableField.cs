// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.QueryTableField
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class QueryTableField
{
  private int m_fieldId;
  private string m_name;
  private int m_tableColumnId;
  private bool m_dataBound;
  private QueryTableRefresh m_parent;

  internal int FieldId
  {
    get => this.m_fieldId;
    set => this.m_fieldId = value;
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal int TableColumnId
  {
    get => this.m_tableColumnId;
    set => this.m_tableColumnId = value;
  }

  internal bool DataBound
  {
    get => this.m_dataBound;
    set => this.m_dataBound = value;
  }

  internal QueryTableField(int id, int columnId, QueryTableRefresh parent)
  {
    this.m_fieldId = id;
    this.m_tableColumnId = columnId;
    this.m_parent = parent;
    this.m_dataBound = true;
  }
}
