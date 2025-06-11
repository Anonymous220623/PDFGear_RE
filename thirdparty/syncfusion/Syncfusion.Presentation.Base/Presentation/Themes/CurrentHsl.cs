// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Themes.CurrentHsl
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Themes;

internal class CurrentHsl
{
  private double _hue;
  private double _luminance;
  private double _saturation;

  internal CurrentHsl()
  {
    this._hue = 0.0;
    this._saturation = 0.0;
    this._luminance = 0.0;
  }

  internal CurrentHsl(double doubleValue1, double doubleValue2, double doubleValue3)
  {
    this._hue = doubleValue1 > 360.0 ? 360.0 : (doubleValue1 < 0.0 ? 0.0 : doubleValue1);
    this._saturation = doubleValue2 > 1.0 ? 1.0 : (doubleValue2 < 0.0 ? 0.0 : doubleValue2);
    this._luminance = doubleValue3 > 1.0 ? 1.0 : (doubleValue3 < 0.0 ? 0.0 : doubleValue3);
  }

  public override int GetHashCode()
  {
    return this.CurrentHue().GetHashCode() ^ this.CurrentSaturation().GetHashCode() ^ this.CurrentLuminance().GetHashCode();
  }

  internal double CurrentHue() => this._hue;

  internal double CurrentLuminance() => this._luminance;

  internal double CurrentSaturation() => this._saturation;

  internal void RoundOffHue(double doubleValue)
  {
    this._hue = doubleValue > 360.0 ? 360.0 : (doubleValue < 0.0 ? 0.0 : doubleValue);
  }

  internal void RoundOffLuminance(double doubleValue)
  {
    this._luminance = doubleValue > 1.0 ? 1.0 : (doubleValue < 0.0 ? 0.0 : doubleValue);
  }

  internal void RoundOffSaturation(double doubleValue)
  {
    this._saturation = doubleValue > 1.0 ? 1.0 : (doubleValue < 0.0 ? 0.0 : doubleValue);
  }
}
