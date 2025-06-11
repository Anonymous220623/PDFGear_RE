// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IAutoFilter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IAutoFilter
{
  IAutoFilterCondition FirstCondition { get; }

  IAutoFilterCondition SecondCondition { get; }

  bool IsFiltered { get; }

  bool IsAnd { get; set; }

  bool IsPercent { get; }

  bool IsSimple1 { get; }

  bool IsSimple2 { get; }

  bool IsTop { get; set; }

  bool IsTop10 { get; set; }

  int Top10Number { get; set; }

  IFilter FilteredItems { get; }

  ExcelFilterType FilterType { get; set; }

  void AddColorFilter(Color color, ExcelColorFilterType colorFilterType);

  void AddIconFilter(ExcelIconSetType iconSetType, int iconId);

  void AddTextFilter(IEnumerable<string> filterCollection);

  void AddTextFilter(string filter);

  bool RemoveText(IEnumerable<string> filterCollection);

  bool RemoveText(string filter);

  void AddDateFilter(
    int year,
    int month,
    int day,
    int hour,
    int mintue,
    int second,
    DateTimeGroupingType groupingType);

  void AddDateFilter(DateTime dateTime, DateTimeGroupingType groupingType);

  bool RemoveDate(
    int year,
    int month,
    int day,
    int hour,
    int mintue,
    int second,
    DateTimeGroupingType groupingType);

  bool RemoveDate(DateTime dateTime, DateTimeGroupingType groupingType);

  void AddDynamicFilter(DynamicFilterType dynamicFilterType);

  bool RemoveDynamicFilter();

  void RemoveColorFilter();

  void RemoveIconFilter();
}
