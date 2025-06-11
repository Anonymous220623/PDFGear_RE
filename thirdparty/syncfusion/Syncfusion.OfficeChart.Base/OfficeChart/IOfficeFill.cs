// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeFill
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeFill
{
  OfficeFillType FillType { get; set; }

  OfficeGradientStyle GradientStyle { get; set; }

  OfficeGradientVariants GradientVariant { get; set; }

  double TransparencyTo { get; set; }

  double TransparencyFrom { get; set; }

  OfficeGradientColor GradientColorType { get; set; }

  OfficeGradientPattern Pattern { get; set; }

  OfficeTexture Texture { get; set; }

  OfficeKnownColors BackColorIndex { get; set; }

  OfficeKnownColors ForeColorIndex { get; set; }

  Color BackColor { get; set; }

  Color ForeColor { get; set; }

  OfficeGradientPreset PresetGradientType { get; set; }

  float TransparencyColor { get; set; }

  Image Picture { get; }

  string PictureName { get; }

  bool Visible { get; set; }

  double GradientDegree { get; set; }

  double Transparency { get; set; }

  float TextureVerticalScale { get; set; }

  float TextureHorizontalScale { get; set; }

  float TextureOffsetX { get; set; }

  float TextureOffsetY { get; set; }

  void UserPicture(string path);

  void UserPicture(Image im, string name);

  void UserTexture(Image im, string name);

  void UserTexture(string path);

  void Patterned(OfficeGradientPattern pattern);

  void PresetGradient(OfficeGradientPreset grad);

  void PresetGradient(OfficeGradientPreset grad, OfficeGradientStyle shadStyle);

  void PresetGradient(
    OfficeGradientPreset grad,
    OfficeGradientStyle shadStyle,
    OfficeGradientVariants shadVar);

  void PresetTextured(OfficeTexture texture);

  void TwoColorGradient();

  void TwoColorGradient(OfficeGradientStyle style);

  void TwoColorGradient(OfficeGradientStyle style, OfficeGradientVariants variant);

  void OneColorGradient();

  void OneColorGradient(OfficeGradientStyle style);

  void OneColorGradient(OfficeGradientStyle style, OfficeGradientVariants variant);

  void Solid();
}
