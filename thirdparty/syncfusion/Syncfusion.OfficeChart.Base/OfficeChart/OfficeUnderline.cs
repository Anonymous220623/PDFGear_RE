// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.OfficeUnderline
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart;

public enum OfficeUnderline
{
  None = 0,
  Single = 1,
  Double = 2,
  [Obsolete("OfficeUnderline - SingleAccounting is not in use anymore. Please use Single instead.")] SingleAccounting = 33, // 0x00000021
  [Obsolete("OfficeUnderline - DoubleAccounting is not in use anymore. Please use Double instead.")] DoubleAccounting = 34, // 0x00000022
  Dash = 35, // 0x00000023
  DotDotDashHeavy = 36, // 0x00000024
  DotDashHeavy = 37, // 0x00000025
  DashHeavy = 38, // 0x00000026
  DashLong = 39, // 0x00000027
  DashLongHeavy = 40, // 0x00000028
  DotDash = 41, // 0x00000029
  DotDotDash = 42, // 0x0000002A
  Dotted = 43, // 0x0000002B
  DottedHeavy = 44, // 0x0000002C
  Heavy = 45, // 0x0000002D
  Wavy = 46, // 0x0000002E
  WavyDouble = 47, // 0x0000002F
  WavyHeavy = 48, // 0x00000030
  Words = 49, // 0x00000031
}
