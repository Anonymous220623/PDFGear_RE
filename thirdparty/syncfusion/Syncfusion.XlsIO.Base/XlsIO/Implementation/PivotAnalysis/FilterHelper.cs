// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.FilterHelper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class FilterHelper
{
  private bool caseSensitive = true;

  public FilterHelper() => this.filterExpressions = new List<FilterExpression>();

  public bool CaseSensitive
  {
    get => this.caseSensitive;
    set => this.caseSensitive = value;
  }

  public int Count => this.filterExpressions.Count;

  internal List<FilterExpression> filterExpressions { get; set; }

  public FilterExpression this[int i]
  {
    get => i < this.Count && i > -1 ? this.filterExpressions[i] : (FilterExpression) null;
  }

  public void AddFilterExpression(string name, string expression)
  {
    FilterExpression filterExpression = new FilterExpression(name, expression);
    if (this.filterExpressions != null)
      this.filterExpressions.Add(filterExpression);
    filterExpression.CaseSensitive = this.CaseSensitive;
  }

  public bool RemoveFilterExpression(FilterExpression exp) => this.filterExpressions.Remove(exp);

  public void Clear()
  {
    if (this.filterExpressions == null)
      return;
    this.filterExpressions.Clear();
  }
}
