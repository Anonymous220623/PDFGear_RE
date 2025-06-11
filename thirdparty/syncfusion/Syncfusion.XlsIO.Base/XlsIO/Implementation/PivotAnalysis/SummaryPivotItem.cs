// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.SummaryPivotItem
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class SummaryPivotItem : IComparable
{
  public int RowIndex { get; set; }

  public int ColIndex { get; set; }

  public List<SummaryBase> Values { get; set; }

  public List<IComparable> Keys { get; set; }

  public int CompareTo(object obj)
  {
    int num = 0;
    if (this.Keys != null)
    {
      int index = 0;
      if (obj is SummaryPivotItem summaryPivotItem)
      {
        num = this.Keys.Count.CompareTo(summaryPivotItem.Keys.Count);
        while (num == 0 && index < this.Keys.Count)
          num = this.Keys[index].CompareTo((object) summaryPivotItem.Keys[index++]);
      }
    }
    return num;
  }
}
