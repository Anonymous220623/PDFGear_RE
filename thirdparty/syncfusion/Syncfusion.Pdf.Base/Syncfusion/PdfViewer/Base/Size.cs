// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Size
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal struct Size(double width, double height) : IFormattable
{
  internal double _width = width >= 0.0 && height >= 0.0 ? width : throw new ArgumentException("Size_WidthAndHeightCannotBeNegative");
  internal double _height = height;
  private static readonly Size s_empty = Size.CreateEmptySize();

  public static Size Empty => Size.s_empty;

  public double Height
  {
    get => this._height;
    set
    {
      if (this.IsEmpty)
        throw new InvalidOperationException("Size_CannotModifyEmptySize");
      this._height = value >= 0.0 ? value : throw new ArgumentException("Size_HeightCannotBeNegative");
    }
  }

  public bool IsEmpty => this._width < 0.0;

  public double Width
  {
    get => this._width;
    set
    {
      if (this.IsEmpty)
        throw new InvalidOperationException("Size_CannotModifyEmptySize");
      this._width = value >= 0.0 ? value : throw new ArgumentException("Size_WidthCannotBeNegative");
    }
  }

  private static Size CreateEmptySize()
  {
    return new Size()
    {
      _width = double.NegativeInfinity,
      _height = double.NegativeInfinity
    };
  }

  public static bool Equals(Size size1, Size size2)
  {
    if (size1.IsEmpty)
      return size2.IsEmpty;
    return size1.Width.Equals(size2.Width) && size1.Height.Equals(size2.Height);
  }

  public override bool Equals(object o) => o != null && o is Size size2 && Size.Equals(this, size2);

  public bool Equals(Size value) => Size.Equals(this, value);

  public override int GetHashCode()
  {
    if (this.IsEmpty)
      return 0;
    double width = this.Width;
    double height = this.Height;
    return width.GetHashCode() ^ height.GetHashCode();
  }

  public static bool operator ==(Size size1, Size size2)
  {
    return size1.Width == size2.Width && size1.Height == size2.Height;
  }

  public static explicit operator Vector(Size size) => new Vector(size._width, size._height);

  public static explicit operator Point(Size size) => new Point(size._width, size._height);

  public static bool operator !=(Size size1, Size size2) => !(size1 == size2);

  string IFormattable.ToString(string format, IFormatProvider provider) => this.ToString();
}
