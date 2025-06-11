// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.ListIndexInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class ListIndexInfo : IComparable<ListIndexInfo>
{
  private int lastIndex = -1;

  public ListIndexInfo() => this.Type = RowType.None;

  public RowType Type { get; set; }

  public ListIndexInfo ParentInfo { get; set; }

  internal int StartIndex { get; set; }

  internal int LastIndex
  {
    get => this.lastIndex;
    set => this.lastIndex = value;
  }

  internal IComparable Display { get; set; }

  internal List<ListIndexInfo> Children { get; set; }

  internal List<SummaryBase> Summaries { get; set; }

  public override string ToString()
  {
    return $"{this.Display} st={this.Children.Count} st={this.StartIndex} last={this.LastIndex}";
  }

  public int CompareTo(ListIndexInfo other)
  {
    if (this.Display != null || other == null)
      return this.Display.CompareTo((object) other?.Display);
    return other.Display == null ? 0 : -1;
  }
}
