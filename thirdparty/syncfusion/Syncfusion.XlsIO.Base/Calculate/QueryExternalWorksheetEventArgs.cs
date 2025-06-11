// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.QueryExternalWorksheetEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.Calculate;

internal class QueryExternalWorksheetEventArgs : EventArgs
{
  internal string formula;
  internal ICalcData worksheet;
  internal string worksheetName = string.Empty;
  internal bool IsWorksheetUpdated;

  internal QueryExternalWorksheetEventArgs(string formula) => this.formula = formula;
}
