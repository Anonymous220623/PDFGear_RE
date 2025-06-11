// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeChartLayout
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeChartLayout : IParentApplication
{
  IOfficeChartManualLayout ManualLayout { get; set; }

  LayoutTargets LayoutTarget { get; set; }

  LayoutModes LeftMode { get; set; }

  LayoutModes TopMode { get; set; }

  double Left { get; set; }

  double Top { get; set; }

  LayoutModes WidthMode { get; set; }

  LayoutModes HeightMode { get; set; }

  double Width { get; set; }

  double Height { get; set; }
}
