// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.ColorValues
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class ColorValues
{
  private HSLColor hSLColorValue;
  private PresetColor presetColorValue;
  private PresetColor schemeColorValue;
  private RGBColors sCrgbColorValue;
  private PresetColor srgbColorValue;
  private PresetColor systemColor;

  internal HSLColor HSLColorValue
  {
    get => this.hSLColorValue;
    set => this.hSLColorValue = value;
  }

  internal PresetColor PresetColorValue
  {
    get => this.presetColorValue;
    set => this.presetColorValue = value;
  }

  internal PresetColor SchemeColorValue
  {
    get => this.schemeColorValue;
    set => this.schemeColorValue = value;
  }

  internal RGBColors SCrgbColorValue
  {
    get => this.sCrgbColorValue;
    set => this.sCrgbColorValue = value;
  }

  internal PresetColor SrgbColorValue
  {
    get => this.srgbColorValue;
    set => this.srgbColorValue = value;
  }

  internal PresetColor SystemColor
  {
    get => this.systemColor;
    set => this.systemColor = value;
  }

  internal ColorValues Clone()
  {
    ColorValues colorValues = (ColorValues) this.MemberwiseClone();
    if (this.hSLColorValue != null)
      colorValues.hSLColorValue = this.hSLColorValue.Clone();
    if (this.presetColorValue != null)
      colorValues.presetColorValue = this.presetColorValue.Clone();
    if (this.schemeColorValue != null)
      colorValues.schemeColorValue = this.schemeColorValue.Clone();
    if (this.SCrgbColorValue != null)
      colorValues.SCrgbColorValue = this.SCrgbColorValue.Clone();
    if (this.srgbColorValue != null)
      colorValues.srgbColorValue = this.srgbColorValue.Clone();
    if (this.systemColor != null)
      colorValues.systemColor = this.systemColor.Clone();
    return colorValues;
  }
}
