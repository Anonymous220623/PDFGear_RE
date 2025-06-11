// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.ColorTransForm
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation;

internal class ColorTransForm
{
  private ColorMode _colorMode;
  private int _hexValue;

  public ColorTransForm(ColorMode colorMode, int value)
  {
    this._colorMode = colorMode;
    this._hexValue = value;
  }

  public ColorMode ColorMode
  {
    get => this._colorMode;
    set => this._colorMode = value;
  }

  public int HexValue
  {
    get => this._hexValue;
    set => this._hexValue = value;
  }

  public ColorTransForm Clone() => (ColorTransForm) this.MemberwiseClone();
}
