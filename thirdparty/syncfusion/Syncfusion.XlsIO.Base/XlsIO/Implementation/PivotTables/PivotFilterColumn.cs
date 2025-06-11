// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotFilterColumn
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotFilterColumn
{
  private int m_iColumnId;
  private bool m_bHiddenButton;
  private bool m_bShowButton;
  private PivotCustomFilters m_customFilters;
  private FilterColumnFilters m_filterColumnFiltes;
  private PivotTop10Filter m_top10Filter;
  private PivotDynamicFilter m_dynamicFilter;

  public int ColumnId
  {
    get => this.m_iColumnId;
    set => this.m_iColumnId = value;
  }

  public bool HiddenButton
  {
    get => this.m_bHiddenButton;
    set => this.m_bHiddenButton = value;
  }

  public bool ShowButton
  {
    get => this.m_bShowButton;
    set => this.m_bShowButton = value;
  }

  public PivotCustomFilters CustomFilters
  {
    get => this.m_customFilters;
    set => this.m_customFilters = value;
  }

  public FilterColumnFilters FilterColumnFilter
  {
    get => this.m_filterColumnFiltes;
    set => this.m_filterColumnFiltes = value;
  }

  public PivotTop10Filter Top10Filters
  {
    get => this.m_top10Filter;
    set => this.m_top10Filter = value;
  }

  public PivotDynamicFilter DynamicFilter
  {
    get => this.m_dynamicFilter;
    set => this.m_dynamicFilter = value;
  }
}
