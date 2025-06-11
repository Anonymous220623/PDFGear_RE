// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.ICombinedRange
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface ICombinedRange : IRange, IParentApplication, IEnumerable
{
  string GetNewAddress(Dictionary<string, string> names, out string strSheetName);

  IRange Clone(object parent, Dictionary<string, string> hashNewNames, WorkbookImpl book);

  void ClearConditionalFormats();

  Rectangle[] GetRectangles();

  int GetRectanglesCount();

  int CellsCount { get; }

  string AddressGlobal2007 { get; }

  string WorksheetName { get; }
}
