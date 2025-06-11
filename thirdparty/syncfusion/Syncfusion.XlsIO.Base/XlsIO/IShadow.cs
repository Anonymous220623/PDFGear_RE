// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IShadow
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IShadow
{
  Excel2007ChartPresetsOuter ShadowOuterPresets { get; set; }

  Excel2007ChartPresetsInner ShadowInnerPresets { get; set; }

  Excel2007ChartPresetsPrespective ShadowPrespectivePresets { get; set; }

  bool HasCustomShadowStyle { get; set; }

  int Transparency { get; set; }

  int Size { get; set; }

  int Blur { get; set; }

  int Angle { get; set; }

  int Distance { get; set; }

  Color ShadowColor { get; set; }

  void CustomShadowStyles(
    Excel2007ChartPresetsOuter iOuter,
    int iTransparency,
    int iSize,
    int iBlur,
    int iAngle,
    int iDistance,
    bool iCustomShadowStyle);

  void CustomShadowStyles(
    Excel2007ChartPresetsInner iInner,
    int iTransparency,
    int iBlur,
    int iAngle,
    int iDistance,
    bool iCustomShadowStyle);

  void CustomShadowStyles(
    Excel2007ChartPresetsPrespective iPerspective,
    int iTransparency,
    int iSize,
    int iBlur,
    int iAngle,
    int iDistance,
    bool iCustomShadowStyle);
}
