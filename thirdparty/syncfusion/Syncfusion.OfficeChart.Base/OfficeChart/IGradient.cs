// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IGradient
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IGradient
{
  ChartColor BackColorObject { get; }

  Color BackColor { get; set; }

  OfficeKnownColors BackColorIndex { get; set; }

  ChartColor ForeColorObject { get; }

  Color ForeColor { get; set; }

  OfficeKnownColors ForeColorIndex { get; set; }

  OfficeGradientStyle GradientStyle { get; set; }

  OfficeGradientVariants GradientVariant { get; set; }

  int CompareTo(IGradient gradient);

  void TwoColorGradient();

  void TwoColorGradient(OfficeGradientStyle style, OfficeGradientVariants variant);
}
