// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.CombinationFilter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class CombinationFilter : IFilter
{
  internal List<IMultipleFilter> m_filterCollection;
  private bool m_isBlank;
  private IAutoFilter m_parent;

  internal List<string> TextFiltersCollection => this.TextCollection();

  public bool IsBlank
  {
    get => this.m_isBlank;
    internal set => this.m_isBlank = value;
  }

  public ExcelFilterType FilterType => ExcelFilterType.CombinationFilter;

  public IMultipleFilter this[int Index] => this.m_filterCollection[Index];

  public int Count => this.m_filterCollection.Count;

  public CombinationFilter(IAutoFilter filter)
  {
    this.m_parent = filter;
    this.m_filterCollection = new List<IMultipleFilter>();
  }

  internal void Dispose()
  {
    this.m_parent = (IAutoFilter) null;
    if (this.m_filterCollection != null)
      this.m_filterCollection.Clear();
    this.m_filterCollection = (List<IMultipleFilter>) null;
  }

  private List<string> TextCollection()
  {
    List<string> stringList = new List<string>();
    foreach (IMultipleFilter filter in this.m_filterCollection)
    {
      if (filter.CombinationFilterType == ExcelCombinationFilterType.TextFilter)
        stringList.Add((filter as TextFilter).Text);
    }
    return stringList;
  }
}
