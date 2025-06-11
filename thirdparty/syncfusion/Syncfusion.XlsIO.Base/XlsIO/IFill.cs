// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IFill
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IFill
{
  ExcelFillType FillType { get; set; }

  ExcelGradientStyle GradientStyle { get; set; }

  ExcelGradientVariants GradientVariant { get; set; }

  double TransparencyTo { get; set; }

  double TransparencyFrom { get; set; }

  ExcelGradientColor GradientColorType { get; set; }

  ExcelGradientPattern Pattern { get; set; }

  ExcelTexture Texture { get; set; }

  ExcelKnownColors BackColorIndex { get; set; }

  ExcelKnownColors ForeColorIndex { get; set; }

  Color BackColor { get; set; }

  Color ForeColor { get; set; }

  ExcelGradientPreset PresetGradientType { get; set; }

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

  void Patterned(ExcelGradientPattern pattern);

  void PresetGradient(ExcelGradientPreset grad);

  void PresetGradient(ExcelGradientPreset grad, ExcelGradientStyle shadStyle);

  void PresetGradient(
    ExcelGradientPreset grad,
    ExcelGradientStyle shadStyle,
    ExcelGradientVariants shadVar);

  void PresetTextured(ExcelTexture texture);

  void TwoColorGradient();

  void TwoColorGradient(ExcelGradientStyle style);

  void TwoColorGradient(ExcelGradientStyle style, ExcelGradientVariants variant);

  void OneColorGradient();

  void OneColorGradient(ExcelGradientStyle style);

  void OneColorGradient(ExcelGradientStyle style, ExcelGradientVariants variant);

  void Solid();
}
