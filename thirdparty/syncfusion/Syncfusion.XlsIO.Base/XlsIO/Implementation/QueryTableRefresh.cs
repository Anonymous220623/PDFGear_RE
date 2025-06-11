// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.QueryTableRefresh
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class QueryTableRefresh
{
  private int m_unboundColumnLeft;
  private int m_unboundColumnRight;
  private int m_nextId;
  private List<QueryTableField> m_queryTableFields;
  private QueryTableImpl m_parentQueryTable;
  private bool m_preserveSortFilterLayout;

  internal int UnboundColumnsLeft
  {
    get => this.m_unboundColumnLeft;
    set => this.m_unboundColumnLeft = value;
  }

  internal int UnboundColumnsRight
  {
    get => this.m_unboundColumnRight;
    set => this.m_unboundColumnRight = value;
  }

  internal List<QueryTableField> QueryFields
  {
    get => this.m_queryTableFields;
    set => this.m_queryTableFields = value;
  }

  internal bool PreserveSortFilterLayout
  {
    get => this.m_preserveSortFilterLayout;
    set => this.m_preserveSortFilterLayout = value;
  }

  internal int NextId
  {
    get => this.m_nextId;
    set => this.m_nextId = value;
  }

  internal QueryTableRefresh(QueryTableImpl parent)
  {
    this.m_queryTableFields = new List<QueryTableField>();
    this.m_parentQueryTable = parent;
  }
}
