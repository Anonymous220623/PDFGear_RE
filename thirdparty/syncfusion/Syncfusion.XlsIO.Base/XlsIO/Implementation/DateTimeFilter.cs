// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.DateTimeFilter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class DateTimeFilter : IMultipleFilter
{
  private DateTime m_dateTime;
  private DateTimeGroupingType m_groupingType;

  public DateTime DateTimeValue
  {
    get => this.m_dateTime;
    internal set => this.m_dateTime = value;
  }

  public DateTimeGroupingType GroupingType
  {
    get => this.m_groupingType;
    internal set => this.m_groupingType = value;
  }

  public ExcelCombinationFilterType CombinationFilterType
  {
    get => ExcelCombinationFilterType.DateTimeFilter;
  }
}
