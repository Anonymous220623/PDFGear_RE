// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IPivotField
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IPivotField
{
  string Name { get; set; }

  PivotAxisTypes Axis { get; set; }

  [Obsolete("Use IPivotValueLableFilter's Value1 property instead of this property")]
  string FilterValue { get; set; }

  string NumberFormat { get; set; }

  PivotSubtotalTypes Subtotals { get; set; }

  bool CanDragToRow { get; set; }

  bool CanDragToColumn { get; set; }

  bool CanDragToPage { get; set; }

  bool CanDragOff { get; set; }

  bool ShowBlankRow { get; set; }

  bool CanDragToData { get; set; }

  bool IsFormulaField { get; }

  string Formula { get; set; }

  IPivotFilters PivotFilters { get; }

  IPivotFieldItems Items { get; }

  int Position { get; set; }

  bool IncludeNewItemsInFilter { get; set; }

  bool RepeatLabels { get; set; }

  void Sort(string[] orderByArray);

  void AutoSort(PivotFieldSortType sortType, int lineNumber);
}
