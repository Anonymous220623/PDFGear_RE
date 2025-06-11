// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.OfficeFindType
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart;

[Flags]
internal enum OfficeFindType
{
  Text = 1,
  Formula = 2,
  FormulaStringValue = 4,
  Error = 8,
  Number = 16, // 0x00000010
  FormulaValue = 32, // 0x00000020
}
