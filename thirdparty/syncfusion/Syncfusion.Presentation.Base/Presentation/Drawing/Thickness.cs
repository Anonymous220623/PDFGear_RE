// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.Thickness
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class Thickness
{
  private double _top;
  private double _left;
  private double _right;
  private double _bottom;

  internal Thickness(double uniformLength)
  {
    this._top = uniformLength;
    this._left = uniformLength;
    this._right = uniformLength;
    this._bottom = uniformLength;
  }

  internal Thickness(double left, double top, double right, double bottom)
  {
    this._top = top;
    this._left = left;
    this._right = right;
    this._bottom = bottom;
  }

  internal double Left => this._left;

  internal double Top => this._top;

  internal double Right => this._right;

  internal double Bottom => this._bottom;
}
