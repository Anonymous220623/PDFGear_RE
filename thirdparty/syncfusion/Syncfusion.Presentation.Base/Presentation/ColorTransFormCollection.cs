// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.ColorTransFormCollection
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Themes;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation;

internal class ColorTransFormCollection : List<ColorTransForm>
{
  public new int Count => base.Count;

  public new ColorTransForm this[int index] => base[index];

  public new void Add(ColorTransForm item) => base.Add(item);

  public new void Insert(int index, ColorTransForm item) => base.Insert(index, item);

  public new void RemoveAt(int index) => base.RemoveAt(index);

  public new bool Remove(ColorTransForm item) => base.Remove(item);

  public new int IndexOf(ColorTransForm item) => base.IndexOf(item);

  public new void Clear() => base.Clear();

  internal void AddColorTransForm(ColorMode colorMode, int value)
  {
    this.Add(new ColorTransForm(colorMode, value));
  }

  internal void AddTransform(ColorMode colorMode, int hexValue)
  {
    this.Add(new ColorTransForm(colorMode, hexValue));
  }

  internal int ApplyTransformation(int baseColor, bool isShapeColor)
  {
    if (!isShapeColor)
    {
      int colorModeValue = this.GetColorModeValue(ColorMode.Tint);
      return ThemeUtilities.GetColFromValue(baseColor, (double) colorModeValue / 100000.0);
    }
    for (int index = 0; index < this.Count; ++index)
    {
      ColorTransForm colorTransForm = this[index];
      switch (colorTransForm.ColorMode)
      {
        case ColorMode.Tint:
          baseColor = ThemeUtilities.ApplyTint(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.Shade:
          baseColor = ThemeUtilities.ApplyShade(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.Alpha:
          baseColor = ThemeUtilities.ApplyAlpha(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.AlphaMod:
          baseColor = ThemeUtilities.ApplyAlphaMod(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.AlphaOff:
          baseColor = ThemeUtilities.ApplyAlphaOff(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.Red:
          baseColor = ThemeUtilities.ApplyRed(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.RedMod:
          baseColor = ThemeUtilities.ApplyRedMod(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.RedOff:
          baseColor = ThemeUtilities.ApplyRedOff(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.Green:
          baseColor = ThemeUtilities.ApplyGreen(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.GreenMod:
          baseColor = ThemeUtilities.ApplyGreenMod(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.GreenOff:
          baseColor = ThemeUtilities.ApplyGreenOff(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.Blue:
          baseColor = ThemeUtilities.ApplyBlue(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.BlueMod:
          baseColor = ThemeUtilities.ApplyBlueMod(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.BlueOff:
          baseColor = ThemeUtilities.ApplyBlueOff(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.Hue:
          baseColor = ThemeUtilities.ApplyHue(baseColor, (double) colorTransForm.HexValue / 60000.0);
          break;
        case ColorMode.HueMod:
          baseColor = ThemeUtilities.ApplyHueMod(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.HueOff:
          baseColor = ThemeUtilities.ApplyHueOff(baseColor, (double) colorTransForm.HexValue / 60000.0);
          break;
        case ColorMode.Sat:
          baseColor = ThemeUtilities.ApplySat(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.SatMod:
          baseColor = ThemeUtilities.ApplySatMod(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.SatOff:
          baseColor = ThemeUtilities.ApplySatOff(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.Lum:
          baseColor = ThemeUtilities.ApplyLum(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.LumMod:
          baseColor = ThemeUtilities.ApplyLumMod(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.LumOff:
          baseColor = ThemeUtilities.ApplyLumOff(baseColor, (double) colorTransForm.HexValue / 100000.0);
          break;
        case ColorMode.Gamma:
          baseColor = ThemeUtilities.ApplyGamma(baseColor);
          break;
        case ColorMode.InvGamma:
          baseColor = ThemeUtilities.ApplyInvGamma(baseColor);
          break;
        case ColorMode.Comp:
          baseColor = ThemeUtilities.ApplyComp(baseColor);
          break;
        case ColorMode.Gray:
          baseColor = ThemeUtilities.ApplyGray(baseColor);
          break;
        case ColorMode.Inv:
          baseColor = ThemeUtilities.ApplyInv(baseColor);
          break;
      }
    }
    return baseColor;
  }

  internal void Copy(ColorTransFormCollection colorTransFormCollection)
  {
    foreach (ColorTransForm colorTransForm in (List<ColorTransForm>) colorTransFormCollection)
      this.AddTransform(colorTransForm.ColorMode, colorTransForm.HexValue);
  }

  internal bool Equals(ColorTransFormCollection transFormCollection)
  {
    if (this.Count != transFormCollection.Count)
      return false;
    for (int index = 0; index < this.Count; ++index)
    {
      ColorTransForm colorTransForm = this[index];
      if (colorTransForm.HexValue != transFormCollection.GetColorModeValue(colorTransForm.ColorMode))
        return false;
    }
    return true;
  }

  internal int GetColorModeValue(ColorMode colorMode)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index].ColorMode == colorMode)
        return this[index].HexValue;
    }
    return 0;
  }

  internal ColorTransForm GetColorTransForm(ColorMode colorMode)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index].ColorMode == colorMode)
        return this[index];
    }
    return (ColorTransForm) null;
  }

  internal void Close() => this.Clear();

  public ColorTransFormCollection Clone()
  {
    ColorTransFormCollection transFormCollection = new ColorTransFormCollection();
    foreach (ColorTransForm colorTransForm in (List<ColorTransForm>) this)
      transFormCollection.Add(colorTransForm.Clone());
    return transFormCollection;
  }
}
