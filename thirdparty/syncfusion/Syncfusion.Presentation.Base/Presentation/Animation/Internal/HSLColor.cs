// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.HSLColor
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class HSLColor
{
  private float hue;
  private float saturation;
  private float luminance;

  internal float Hue
  {
    get => this.hue;
    set => this.hue = value;
  }

  internal float Saturation
  {
    get => this.saturation;
    set => this.saturation = value;
  }

  internal float Luminance
  {
    get => this.luminance;
    set => this.luminance = value;
  }

  internal HSLColor Clone() => (HSLColor) this.MemberwiseClone();
}
