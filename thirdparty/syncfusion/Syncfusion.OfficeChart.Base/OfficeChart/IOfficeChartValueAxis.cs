// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeChartValueAxis
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeChartValueAxis : IOfficeChartAxis
{
  double MinimumValue { get; set; }

  double MaximumValue { get; set; }

  double MajorUnit { get; set; }

  double MinorUnit { get; set; }

  double CrossesAt { get; set; }

  bool IsAutoMin { get; set; }

  bool IsAutoMax { get; set; }

  bool IsAutoMajor { get; set; }

  bool IsAutoMinor { get; set; }

  bool IsAutoCross { get; set; }

  bool IsLogScale { get; set; }

  bool IsMaxCross { get; set; }

  double DisplayUnitCustom { get; set; }

  OfficeChartDisplayUnit DisplayUnit { get; set; }

  bool HasDisplayUnitLabel { get; set; }

  IOfficeChartTextArea DisplayUnitLabel { get; }
}
