// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeChartLegend
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeChartLegend
{
  IOfficeChartFrameFormat FrameFormat { get; }

  IOfficeChartTextArea TextArea { get; }

  int X { get; set; }

  int Y { get; set; }

  OfficeLegendPosition Position { get; set; }

  bool IsVerticalLegend { get; set; }

  IChartLegendEntries LegendEntries { get; }

  bool IncludeInLayout { get; set; }

  IOfficeChartLayout Layout { get; set; }

  void Clear();

  void Delete();
}
