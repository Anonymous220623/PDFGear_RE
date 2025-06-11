// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Interfaces.IInternalWorksheet
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Interfaces;

internal interface IInternalWorksheet : IWorksheet, ITabSheet, IParentApplication
{
  int DefaultRowHeight { get; }

  int FirstRow { get; set; }

  int FirstColumn { get; set; }

  int LastRow { get; set; }

  int LastColumn { get; set; }

  CellRecordCollection CellRecords { get; }

  WorkbookImpl ParentWorkbook { get; }

  OfficeVersion Version { get; }

  bool IsArrayFormula(long index);

  IInternalWorksheet GetClonedObject(Dictionary<string, string> hashNewNames, WorkbookImpl book);
}
