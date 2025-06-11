// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IShadow
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IShadow
{
  Office2007ChartPresetsOuter ShadowOuterPresets { get; set; }

  Office2007ChartPresetsInner ShadowInnerPresets { get; set; }

  Office2007ChartPresetsPerspective ShadowPerspectivePresets { get; set; }

  bool HasCustomShadowStyle { get; set; }

  int Transparency { get; set; }

  int Size { get; set; }

  int Blur { get; set; }

  int Angle { get; set; }

  int Distance { get; set; }

  Color ShadowColor { get; set; }

  void CustomShadowStyles(
    Office2007ChartPresetsOuter iOuter,
    int iTransparency,
    int iSize,
    int iBlur,
    int iAngle,
    int iDistance,
    bool iCustomShadowStyle);

  void CustomShadowStyles(
    Office2007ChartPresetsInner iInner,
    int iTransparency,
    int iBlur,
    int iAngle,
    int iDistance,
    bool iCustomShadowStyle);

  void CustomShadowStyles(
    Office2007ChartPresetsPerspective iPerspective,
    int iTransparency,
    int iSize,
    int iBlur,
    int iAngle,
    int iDistance,
    bool iCustomShadowStyle);
}
