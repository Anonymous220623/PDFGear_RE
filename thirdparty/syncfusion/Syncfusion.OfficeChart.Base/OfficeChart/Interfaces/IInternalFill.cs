// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Interfaces.IInternalFill
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Shapes;

#nullable disable
namespace Syncfusion.OfficeChart.Interfaces;

internal interface IInternalFill : IOfficeFill
{
  ChartColor BackColorObject { get; }

  ChartColor ForeColorObject { get; }

  bool Tile { get; set; }

  GradientStops PreservedGradient { get; set; }

  bool IsGradientSupported { get; set; }

  new float TransparencyColor { get; set; }

  new float TextureVerticalScale { get; set; }

  new float TextureHorizontalScale { get; set; }

  new float TextureOffsetX { get; set; }

  new float TextureOffsetY { get; set; }

  string Alignment { get; set; }

  string TileFlipping { get; set; }
}
