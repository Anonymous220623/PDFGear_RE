// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.OfficeCopyRangeOptions
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart;

[Flags]
internal enum OfficeCopyRangeOptions
{
  None = 0,
  UpdateFormulas = 1,
  UpdateMerges = 2,
  CopyStyles = 4,
  CopyShapes = 8,
  CopyErrorIndicators = 16, // 0x00000010
  CopyConditionalFormats = 32, // 0x00000020
  CopyDataValidations = 64, // 0x00000040
  All = CopyDataValidations | CopyConditionalFormats | CopyErrorIndicators | CopyShapes | CopyStyles | UpdateMerges | UpdateFormulas, // 0x0000007F
}
