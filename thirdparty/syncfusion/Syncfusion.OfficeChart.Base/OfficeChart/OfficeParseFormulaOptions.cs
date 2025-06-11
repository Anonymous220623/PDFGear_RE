// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.OfficeParseFormulaOptions
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart;

[Flags]
internal enum OfficeParseFormulaOptions
{
  None = 0,
  RootLevel = 1,
  InArray = 2,
  InName = 4,
  ParseOperand = 8,
  ParseComplexOperand = 16, // 0x00000010
  UseR1C1 = 32, // 0x00000020
}
