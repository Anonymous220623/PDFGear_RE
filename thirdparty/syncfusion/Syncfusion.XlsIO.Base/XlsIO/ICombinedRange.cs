// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ICombinedRange
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface ICombinedRange : IRange, IParentApplication, IEnumerable<IRange>, IEnumerable
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
