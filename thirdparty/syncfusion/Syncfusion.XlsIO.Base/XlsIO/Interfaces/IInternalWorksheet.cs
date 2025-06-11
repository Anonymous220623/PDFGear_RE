// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Interfaces.IInternalWorksheet
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Calculate;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Interfaces;

public interface IInternalWorksheet : IWorksheet, ITabSheet, IParentApplication, ICalcData
{
  int DefaultRowHeight { get; }

  int FirstRow { get; set; }

  int FirstColumn { get; set; }

  int LastRow { get; set; }

  int LastColumn { get; set; }

  CellRecordCollection CellRecords { get; }

  WorkbookImpl ParentWorkbook { get; }

  ExcelVersion Version { get; }

  bool IsArrayFormula(long index);

  IInternalWorksheet GetClonedObject(Dictionary<string, string> hashNewNames, WorkbookImpl book);
}
