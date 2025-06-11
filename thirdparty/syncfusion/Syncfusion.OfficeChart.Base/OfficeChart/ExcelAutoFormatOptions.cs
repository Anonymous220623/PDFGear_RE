// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.ExcelAutoFormatOptions
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart;

[Flags]
internal enum ExcelAutoFormatOptions
{
  Number = 1,
  Border = 2,
  Font = 4,
  Patterns = 8,
  Alignment = 16, // 0x00000010
  Width_Height = 32, // 0x00000020
  None = 0,
  All = Width_Height | Alignment | Patterns | Font | Border | Number, // 0x0000003F
}
