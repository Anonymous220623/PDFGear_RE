// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.PicFormatOption
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class PicFormatOption
{
  private int _bottom;
  private int _left;
  private int _right;
  private double _scale = 1.0;
  private int _top;

  internal double Bottom => (double) this._bottom / 1000.0;

  internal double Left => (double) this._left / 1000.0;

  internal double Right => (double) this._right / 1000.0;

  internal double Top => (double) this._top / 1000.0;

  internal double Scale
  {
    get => this._scale;
    set => this._scale = value;
  }

  internal void Copy(PicFormatOption picFormatOption)
  {
    this._scale = picFormatOption._scale;
    this._left = picFormatOption._left;
    this._top = picFormatOption._top;
    this._bottom = picFormatOption._bottom;
    this._right = picFormatOption._right;
  }

  internal int ObtainLeft() => this._left;

  internal int ObtainTop() => this._top;

  internal int ObtainRight() => this._right;

  internal int ObtainBottom() => this._bottom;

  internal void SetLeft(int left) => this._left = left;

  internal void SetTop(int top) => this._top = top;

  internal void SetRight(int right) => this._right = right;

  internal void SetBottom(int bottom) => this._bottom = bottom;

  public PicFormatOption Clone() => (PicFormatOption) this.MemberwiseClone();
}
