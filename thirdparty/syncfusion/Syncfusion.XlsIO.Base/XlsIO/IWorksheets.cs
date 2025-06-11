// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IWorksheets
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IWorksheets : IEnumerable<IWorksheet>, IEnumerable
{
  IApplication Application { get; }

  int Count { get; }

  IWorksheet this[int Index] { get; }

  IWorksheet this[string sheetName] { get; }

  object Parent { get; }

  bool UseRangesCache { get; set; }

  IWorksheet AddCopy(int sheetIndex);

  IWorksheet AddCopy(int sheetIndex, ExcelWorksheetCopyFlags flags);

  IWorksheet AddCopy(IWorksheet sourceSheet);

  IWorksheet AddCopy(IWorksheet sheet, ExcelWorksheetCopyFlags flags);

  void AddCopy(IWorksheets worksheets);

  void AddCopy(IWorksheets worksheets, ExcelWorksheetCopyFlags flags);

  IWorksheet Create(string name);

  IWorksheet Create();

  void Remove(IWorksheet sheet);

  void Remove(string sheetName);

  void Remove(int index);

  IRange FindFirst(string findValue, ExcelFindType flags);

  IRange FindFirst(string findValue, ExcelFindType flags, ExcelFindOptions findOptions);

  IRange FindFirst(double findValue, ExcelFindType flags);

  IRange FindFirst(bool findValue);

  IRange FindFirst(DateTime findValue);

  IRange FindFirst(TimeSpan findValue);

  IRange[] FindAll(string findValue, ExcelFindType flags);

  IRange[] FindAll(string findValue, ExcelFindType flags, ExcelFindOptions findOptions);

  IRange[] FindAll(double findValue, ExcelFindType flags);

  IRange[] FindAll(bool findValue);

  IRange[] FindAll(DateTime findValue);

  IRange[] FindAll(TimeSpan findValue);

  IWorksheet AddCopyBefore(IWorksheet toCopy);

  IWorksheet AddCopyBefore(IWorksheet toCopy, IWorksheet sheetAfter);

  IWorksheet AddCopyAfter(IWorksheet toCopy);

  IWorksheet AddCopyAfter(IWorksheet toCopy, IWorksheet sheetBefore);
}
