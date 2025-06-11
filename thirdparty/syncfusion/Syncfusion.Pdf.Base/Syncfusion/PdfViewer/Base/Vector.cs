// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Vector
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal struct Vector(double x, double y) : IFormattable
{
  internal double _x = x;
  internal double _y = y;

  public double Length => Math.Sqrt(this._x * this._x + this._y * this._y);

  public double LengthSquared => this._x * this._x + this._y * this._y;

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

  public static Vector Add(Vector vector1, Vector vector2)
  {
    return new Vector(vector1._x + vector2._x, vector1._y + vector2._y);
  }

  public static Point Add(Vector vector, Point point)
  {
    return new Point(point._x + vector._x, point._y + vector._y);
  }

  public static double AngleBetween(Vector vector1, Vector vector2)
  {
    return Math.Atan2(vector1._x * vector2._y - vector2._x * vector1._y, vector1._x * vector2._x + vector1._y * vector2._y) * (180.0 / Math.PI);
  }

  public static double CrossProduct(Vector vector1, Vector vector2)
  {
    return vector1._x * vector2._y - vector1._y * vector2._x;
  }

  public static double Determinant(Vector vector1, Vector vector2)
  {
    return vector1._x * vector2._y - vector1._y * vector2._x;
  }

  public static Vector Divide(Vector vector, double scalar) => vector * 1.0 / scalar;

  public static bool Equals(Vector vector1, Vector vector2)
  {
    return vector1.X.Equals(vector2.X) && vector1.Y.Equals(vector2.Y);
  }

  public override bool Equals(object o)
  {
    return o != null && o is Vector vector2 && Vector.Equals(this, vector2);
  }

  public bool Equals(Vector value) => Vector.Equals(this, value);

  public override int GetHashCode()
  {
    double x = this.X;
    double y = this.Y;
    return x.GetHashCode() ^ y.GetHashCode();
  }

  public static Vector Multiply(Vector vector, double scalar)
  {
    return new Vector(vector._x * scalar, vector._y * scalar);
  }

  public static Vector Multiply(double scalar, Vector vector)
  {
    return new Vector(vector._x * scalar, vector._y * scalar);
  }

  public static double Multiply(Vector vector1, Vector vector2)
  {
    return vector1._x * vector2._x + vector1._y * vector2._y;
  }

  public void Negate()
  {
    this._x = -this._x;
    this._y = -this._y;
  }

  public static Vector operator +(Vector vector1, Vector vector2)
  {
    return new Vector(vector1._x + vector2._x, vector1._y + vector2._y);
  }

  public static Point operator +(Vector vector, Point point)
  {
    return new Point(point._x + vector._x, point._y + vector._y);
  }

  public static Vector operator /(Vector vector, double scalar) => vector * 1.0 / scalar;

  public static bool operator ==(Vector vector1, Vector vector2)
  {
    return vector1.X == vector2.X && vector1.Y == vector2.Y;
  }

  public static explicit operator Size(Vector vector)
  {
    return new Size(Math.Abs(vector._x), Math.Abs(vector._y));
  }

  public static explicit operator Point(Vector vector) => new Point(vector._x, vector._y);

  public static bool operator !=(Vector vector1, Vector vector2) => !(vector1 == vector2);

  public static Vector operator *(Vector vector, double scalar)
  {
    return new Vector(vector._x * scalar, vector._y * scalar);
  }

  public static Vector operator *(double scalar, Vector vector)
  {
    return new Vector(vector._x * scalar, vector._y * scalar);
  }

  public static double operator *(Vector vector1, Vector vector2)
  {
    return vector1._x * vector2._x + vector1._y * vector2._y;
  }

  public static Vector operator -(Vector vector1, Vector vector2)
  {
    return new Vector(vector1._x - vector2._x, vector1._y - vector2._y);
  }

  public static Vector operator -(Vector vector) => new Vector(-vector._x, -vector._y);

  public static Vector Subtract(Vector vector1, Vector vector2)
  {
    return new Vector(vector1._x - vector2._x, vector1._y - vector2._y);
  }

  string IFormattable.ToString(string format, IFormatProvider formatProvider) => this.ToString();
}
