// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Matrix
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal struct Matrix
{
  private MatrixTypes type;

  public static Matrix Identity => new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);

  public double Determinant
  {
    get
    {
      switch (this.type)
      {
        case MatrixTypes.Identity:
        case MatrixTypes.Translation:
          return 1.0;
        case MatrixTypes.Scaling:
        case MatrixTypes.Scaling | MatrixTypes.Translation:
          return this.M11 * this.M22;
        default:
          return this.M11 * this.M22 - this.M12 * this.M21;
      }
    }
  }

  public double M11 { get; set; }

  public double M12 { get; set; }

  public double M21 { get; set; }

  public double M22 { get; set; }

  public double OffsetX { get; set; }

  public double OffsetY { get; set; }

  public Matrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY)
    : this()
  {
    this.M11 = m11;
    this.M12 = m12;
    this.M21 = m21;
    this.M22 = m22;
    this.OffsetX = offsetX;
    this.OffsetY = offsetY;
    this.type = MatrixTypes.Unknown;
    this.CheckMatrixType();
  }

  public static Matrix operator *(Matrix matrix1, Matrix matrix2)
  {
    return new Matrix(matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21, matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22, matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21, matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22, matrix1.OffsetX * matrix2.M11 + matrix1.OffsetY * matrix2.M21 + matrix2.OffsetX, matrix1.OffsetX * matrix2.M12 + matrix1.OffsetY * matrix2.M22 + matrix2.OffsetY);
  }

  public static bool operator ==(Matrix a, Matrix b)
  {
    return a.M11 == b.M11 && a.M21 == b.M21 && a.M12 == b.M12 && a.M22 == b.M22 && a.OffsetX == b.OffsetX && a.OffsetY == b.OffsetY;
  }

  public static bool operator !=(Matrix a, Matrix b) => !(a == b);

  public bool IsIdentity() => this == Matrix.Identity;

  public Matrix Translate(double offsetX, double offsetY)
  {
    if (this.type == MatrixTypes.Identity)
      this.SetMatrix(1.0, 0.0, 0.0, 1.0, offsetX, offsetY, MatrixTypes.Translation);
    else if (this.type == MatrixTypes.Unknown)
    {
      this.OffsetX += offsetX;
      this.OffsetY += offsetY;
    }
    else
    {
      this.OffsetX += offsetX;
      this.OffsetY += offsetY;
      this.type |= MatrixTypes.Translation;
    }
    return this;
  }

  public Matrix Scale(double scaleX, double scaleY, double centerX, double centerY)
  {
    this = new Matrix(scaleX, 0.0, 0.0, scaleY, centerX, centerY) * this;
    return this;
  }

  public Matrix ScaleAppend(double scaleX, double scaleY, double centerX, double centerY)
  {
    this *= new Matrix(scaleX, 0.0, 0.0, scaleY, centerX, centerY);
    return this;
  }

  public Matrix Rotate(double angle, double centerX, double centerY)
  {
    Matrix matrix = new Matrix();
    angle = Math.PI * angle / 180.0;
    double m12 = Math.Sin(angle);
    double num = Math.Cos(angle);
    double offsetX = centerX * (1.0 - num) + centerY * m12;
    double offsetY = centerY * (1.0 - num) - centerX * m12;
    matrix.SetMatrix(num, m12, -m12, num, offsetX, offsetY, MatrixTypes.Unknown);
    this = matrix * this;
    return this;
  }

  public bool Equals(Matrix value)
  {
    return this.M11 == value.M11 && this.M12 == value.M12 && this.M21 == value.M21 && this.M22 == value.M22 && this.OffsetX == value.OffsetX && this.OffsetY == value.OffsetY && this.type.Equals((object) value.type);
  }

  public double Transform(double d)
  {
    double val1 = Math.Sqrt(Math.Pow(this.M11 + this.M21, 2.0) + Math.Pow(this.M12 + this.M22, 2.0));
    double val2 = Math.Sqrt(Math.Pow(this.M11 - this.M21, 2.0) + Math.Pow(this.M12 - this.M22, 2.0));
    return d * Math.Max(val1, val2);
  }

  public Point Transform(Point point)
  {
    double x = point.X;
    double y = point.Y;
    return new Point(x * this.M11 + y * this.M21 + this.OffsetX, x * this.M12 + y * this.M22 + this.OffsetY);
  }

  public override int GetHashCode()
  {
    return ((((((17 * 23 + this.M11.GetHashCode()) * 23 + this.M12.GetHashCode()) * 23 + this.M21.GetHashCode()) * 23 + this.M22.GetHashCode()) * 23 + this.OffsetX.GetHashCode()) * 23 + this.OffsetY.GetHashCode()) * 23 + this.type.GetHashCode();
  }

  public override bool Equals(object obj)
  {
    return obj != null && obj is Matrix matrix && this.Equals(matrix);
  }

  public override string ToString()
  {
    return $"{this.M11} {this.M12} 0 | {this.M21} {this.M22} 0 | {this.OffsetX} {this.OffsetY} 1";
  }

  private void CheckMatrixType()
  {
    this.type = MatrixTypes.Identity;
    if (this.M21 != 0.0 || this.M12 != 0.0)
    {
      this.type = MatrixTypes.Unknown;
    }
    else
    {
      if (this.M11 != 1.0 || this.M22 != 1.0)
        this.type = MatrixTypes.Scaling;
      if (this.OffsetX != 0.0 || this.OffsetY != 0.0)
        this.type |= MatrixTypes.Translation;
      if ((this.type & (MatrixTypes.Scaling | MatrixTypes.Translation)) != MatrixTypes.Identity)
        return;
      this.type = MatrixTypes.Identity;
    }
  }

  private void SetMatrix(
    double m11,
    double m12,
    double m21,
    double m22,
    double offsetX,
    double offsetY,
    MatrixTypes type)
  {
    this.M11 = m11;
    this.M12 = m12;
    this.M21 = m21;
    this.M22 = m22;
    this.OffsetX = offsetX;
    this.OffsetY = offsetY;
    this.type = type;
  }
}
