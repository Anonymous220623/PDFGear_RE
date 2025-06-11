// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.OfficeParseOptions
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart;

[Flags]
internal enum OfficeParseOptions
{
  Default = 0,
  [Obsolete("This value is obsolete and won't affect on the XlsIO. It will be removed in next release. Sorry for inconvenience.")] SkipStyles = 1,
  DoNotParseCharts = 2,
  [Obsolete("This value is obsolete and won't affect on the XlsIO performance. It will be removed in next release. Sorry for inconvenience.")] StringsReadOnly = 4,
  DoNotParsePivotTable = 8,
  ParseWorksheetsOnDemand = 16, // 0x00000010
}
