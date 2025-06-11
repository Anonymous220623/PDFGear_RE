// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Point
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal struct Point(double x, double y) : IFormattable
{
  internal double _x = x;
  internal double _y = y;

  public double X
  {
    get => this._x;
    set => this._x = value;
  }

  public double Y
  {
    get => this._y;
    set => this._y = value;
  }

  public static implicit operator System.Drawing.Point(Point p) => new System.Drawing.Point((int) p.X, (int) p.Y);

  public static implicit operator PointF(Point p) => new PointF((float) p.X, (float) p.Y);

  public static implicit operator Point(System.Drawing.Point p)
  {
    return new Point((double) p.X, (double) p.Y);
  }

  public static implicit operator Point(PointF p) => new Point((double) p.X, (double) p.Y);

  public static bool Equals(Point point1, Point point2)
  {
    return point1.X.Equals(point2.X) && point1.Y.Equals(point2.Y);
  }

  public override bool Equals(object o)
  {
    return o != null && o is Point point2 && Point.Equals(this, point2);
  }

  public bool Equals(Point value) => Point.Equals(this, value);

  public override int GetHashCode()
  {
    double x = this.X;
    double y = this.Y;
    return x.GetHashCode() ^ y.GetHashCode();
  }

  public void Offset(double offsetX, double offsetY)
  {
    this._x += offsetX;
    this._y += offsetY;
  }

  string IFormattable.ToString(string format, IFormatProvider provider) => this.ToString();
}
