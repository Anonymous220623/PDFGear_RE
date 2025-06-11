// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.RGBColors
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class RGBColors
{
  private string red;
  private string green;
  private string blue;

  internal string Red
  {
    get => this.red;
    set => this.red = value;
  }

  internal string Green
  {
    get => this.green;
    set => this.green = value;
  }

  internal string Blue
  {
    get => this.blue;
    set => this.blue = value;
  }

  internal RGBColors Clone()
  {
    RGBColors rgbColors = (RGBColors) this.MemberwiseClone();
    if (rgbColors.red != null)
      rgbColors.red = this.red;
    if (rgbColors.green != null)
      rgbColors.green = this.green;
    if (rgbColors.blue != null)
      rgbColors.blue = this.blue;
    return rgbColors;
  }
}
