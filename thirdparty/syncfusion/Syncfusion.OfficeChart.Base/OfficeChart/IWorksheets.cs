// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IWorksheets
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IWorksheets : IEnumerable
{
  IApplication Application { get; }

  int Count { get; }

  IWorksheet this[int Index] { get; }

  IWorksheet this[string sheetName] { get; }

  object Parent { get; }

  bool UseRangesCache { get; set; }

  IWorksheet AddCopy(int sheetIndex);

  IWorksheet AddCopy(int sheetIndex, OfficeWorksheetCopyFlags flags);

  IWorksheet AddCopy(IWorksheet sourceSheet);

  IWorksheet AddCopy(IWorksheet sheet, OfficeWorksheetCopyFlags flags);

  void AddCopy(IWorksheets worksheets);

  void AddCopy(IWorksheets worksheets, OfficeWorksheetCopyFlags flags);

  IWorksheet Create(string name);

  IWorksheet Create();

  void Remove(IWorksheet sheet);

  void Remove(string sheetName);

  void Remove(int index);

  IRange FindFirst(string findValue, OfficeFindType flags);

  IRange FindFirst(string findValue, OfficeFindType flags, OfficeFindOptions findOptions);

  IRange FindFirst(double findValue, OfficeFindType flags);

  IRange FindFirst(bool findValue);

  IRange FindFirst(DateTime findValue);

  IRange FindFirst(TimeSpan findValue);

  IRange[] FindAll(string findValue, OfficeFindType flags);

  IRange[] FindAll(string findValue, OfficeFindType flags, OfficeFindOptions findOptions);

  IRange[] FindAll(double findValue, OfficeFindType flags);

  IRange[] FindAll(bool findValue);

  IRange[] FindAll(DateTime findValue);

  IRange[] FindAll(TimeSpan findValue);

  IWorksheet AddCopyBefore(IWorksheet toCopy);

  IWorksheet AddCopyBefore(IWorksheet toCopy, IWorksheet sheetAfter);

  IWorksheet AddCopyAfter(IWorksheet toCopy);

  IWorksheet AddCopyAfter(IWorksheet toCopy, IWorksheet sheetBefore);
}
