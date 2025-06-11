// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IAutoFilter
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IAutoFilter
{
  IAutoFilterCondition FirstCondition { get; }

  IAutoFilterCondition SecondCondition { get; }

  bool IsFiltered { get; }

  bool IsAnd { get; set; }

  bool IsPercent { get; }

  bool IsSimple1 { get; }

  bool IsSimple2 { get; }

  bool IsTop { get; set; }

  bool IsTop10 { get; set; }

  int Top10Number { get; set; }
}
