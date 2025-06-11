// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IListObject
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IListObject
{
  string TableStyleName { get; set; }

  string Name { get; set; }

  IRange Location { get; set; }

  IList<IListObjectColumn> Columns { get; }

  int Index { get; }

  TableBuiltInStyles BuiltInTableStyle { get; set; }

  IWorksheet Worksheet { get; }

  string DisplayName { get; set; }

  int TotalsRowCount { get; }

  bool ShowTotals { get; set; }

  bool ShowTableStyleRowStripes { get; set; }

  bool ShowTableStyleColumnStripes { get; set; }

  bool ShowLastColumn { get; set; }

  bool ShowFirstColumn { get; set; }

  bool ShowHeaderRow { get; set; }

  string AlternativeText { get; set; }

  string Summary { get; set; }

  QueryTableImpl QueryTable { get; }

  ExcelTableType TableType { get; }

  IAutoFilters AutoFilters { get; }

  bool ShowAutoFilter { get; set; }

  void Refresh();
}
