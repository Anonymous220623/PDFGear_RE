// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.ISheetReference
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[CLSCompliant(false)]
internal interface ISheetReference : IReference
{
  string BaseToString(FormulaUtil formulaUtil, int iRow, int iColumn, bool bR1C1);
}
