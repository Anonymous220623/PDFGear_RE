// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.Matrix3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public struct Matrix3D
{
  private const int MATRIXSIZE = 4;
  private readonly double[][] mData;

  public Matrix3D(
    double m11,
    double m12,
    double m13,
    double m14,
    double m21,
    double m22,
    double m23,
    double m24,
    double m31,
    double m32,
    double m33,
    double m34,
    double m41,
    double m42,
    double m43,
    double m44)
    : this(4)
  {
    this.mData[0][0] = m11;
    this.mData[1][0] = m12;
    this.mData[2][0] = m13;
    this.mData[3][0] = m14;
    this.mData[0][1] = m21;
    this.mData[1][1] = m22;
    this.mData[2][1] = m23;
    this.mData[3][1] = m24;
    this.mData[0][2] = m31;
    this.mData[1][2] = m32;
    this.mData[2][2] = m33;
    this.mData[3][2] = m34;
    this.mData[0][3] = m41;
    this.mData[1][3] = m42;
    this.mData[2][3] = m43;
    this.mData[3][3] = m44;
  }

  private Matrix3D(int size)
  {
    this.mData = new double[size][];
    for (int index = 0; index < size; ++index)
      this.mData[index] = new double[size];
  }

  public static Matrix3D Identity => Matrix3D.GetIdentity();

  public bool IsAffine
  {
    get
    {
      return this.mData[0][3] == 0.0 && this.mData[1][3] == 0.0 && this.mData[2][3] == 0.0 && this.mData[3][3] == 1.0;
    }
  }

  public double this[int i, int j]
  {
    get => this.mData[i][j];
    set => this.mData[i][j] = value;
  }

  public static Matrix3D operator +(Matrix3D m1, Matrix3D m2)
  {
    Matrix3D matrix3D = new Matrix3D(4);
    for (int i = 0; i < 4; ++i)
    {
      for (int j = 0; j < 4; ++j)
        matrix3D[i, j] = m1[i, j] + m2[i, j];
    }
    return matrix3D;
  }

  public static Vector3D operator *(Matrix3D m1, Vector3D point)
  {
    double vx = m1.mData[0][0] * point.X + m1.mData[1][0] * point.Y + m1.mData[2][0] * point.Z + m1.mData[3][0];
    double vy = m1.mData[0][1] * point.X + m1.mData[1][1] * point.Y + m1.mData[2][1] * point.Z + m1.mData[3][1];
    double vz = m1.mData[0][2] * point.X + m1.mData[1][2] * point.Y + m1.mData[2][2] * point.Z + m1.mData[3][2];
    if (!m1.IsAffine)
    {
      double num = 1.0 / (m1.mData[0][3] * point.X + m1.mData[1][3] * point.Y + m1.mData[2][3] * point.Z + m1.mData[3][3]);
      vx *= num;
      vy *= num;
      vz *= num;
    }
    return new Vector3D(vx, vy, vz);
  }

  public static Vector3D operator &(Matrix3D m1, Vector3D v1)
  {
    return new Vector3D(m1.mData[0][0] * v1.X + m1.mData[1][0] * v1.Y + m1.mData[2][0] * v1.Z, m1.mData[0][1] * v1.X + m1.mData[1][1] * v1.Y + m1.mData[2][1] * v1.Z, m1.mData[0][2] * v1.X + m1.mData[1][2] * v1.Y + m1.mData[2][2] * v1.Z);
  }

  public static Matrix3D operator *(double f1, Matrix3D m1)
  {
    int length = m1.mData.Length;
    Matrix3D matrix3D = new Matrix3D(length);
    for (int index1 = 0; index1 < length; ++index1)
    {
      for (int index2 = 0; index2 < length; ++index2)
        matrix3D.mData[index1][index2] = m1.mData[index1][index2] * f1;
    }
    return matrix3D;
  }

  public static Matrix3D operator *(Matrix3D m1, Matrix3D m2)
  {
    Matrix3D identity = Matrix3D.GetIdentity();
    for (int i = 0; i < 4; ++i)
    {
      for (int j = 0; j < 4; ++j)
      {
        double num = 0.0;
        for (int index = 0; index < 4; ++index)
          num += m1[index, j] * m2[i, index];
        identity[i, j] = num;
      }
    }
    return identity;
  }

  public static bool operator ==(Matrix3D m1, Matrix3D m2)
  {
    bool flag = true;
    for (int index1 = 0; index1 < m1.mData.Length; ++index1)
    {
      for (int index2 = 0; index2 < m1.mData.Length; ++index2)
      {
        if (m1.mData[index1][index2] != m2.mData[index1][index2])
          flag = false;
      }
    }
    return flag;
  }

  public static bool operator !=(Matrix3D m1, Matrix3D m2)
  {
    bool flag = true;
    for (int index1 = 0; index1 < m1.mData.Length; ++index1)
    {
      for (int index2 = 0; index2 < m1.mData.Length; ++index2)
      {
        if (m1.mData[index1][index2] != m2.mData[index1][index2])
          flag = false;
      }
    }
    return !flag;
  }

  public static double GetD(Matrix3D matrix3D)
  {
    return Matrix3D.GetDeterminant((IList<double[]>) matrix3D.mData);
  }

  public static Matrix3D GetIdentity()
  {
    Matrix3D identity = new Matrix3D(4);
    for (int index = 0; index < 4; ++index)
      identity[index, index] = 1.0;
    return identity;
  }

  public static Matrix3D Transform(double x, double y, double z)
  {
    Matrix3D identity = Matrix3D.GetIdentity();
    identity.mData[3][0] = x;
    identity.mData[3][1] = y;
    identity.mData[3][2] = z;
    return identity;
  }

  public static Matrix3D Turn(double angle)
  {
    Matrix3D identity = Matrix3D.GetIdentity();
    identity[0, 0] = Math.Cos(angle);
    identity[2, 0] = -Math.Sin(angle);
    identity[0, 2] = Math.Sin(angle);
    identity[2, 2] = Math.Cos(angle);
    return identity;
  }

  public static Matrix3D Tilt(double angle)
  {
    Matrix3D identity = Matrix3D.GetIdentity();
    identity[1, 1] = Math.Cos(angle);
    identity[2, 1] = Math.Sin(angle);
    identity[1, 2] = -Math.Sin(angle);
    identity[2, 2] = Math.Cos(angle);
    return identity;
  }

  public static Matrix3D Transposed(Matrix3D matrix3D)
  {
    Matrix3D identity = Matrix3D.Identity;
    for (int index1 = 0; index1 < 4; ++index1)
    {
      for (int index2 = 0; index2 < 4; ++index2)
        identity[index1, index2] = matrix3D[index2, index1];
    }
    return identity;
  }

  public static Matrix3D Shear(double xy, double xz, double yx, double yz, double zx, double zy)
  {
    Matrix3D identity = Matrix3D.Identity;
    identity[1, 0] = xy;
    identity[2, 0] = xz;
    identity[0, 1] = yx;
    identity[2, 1] = yz;
    identity[0, 2] = zx;
    identity[1, 2] = zy;
    return identity;
  }

  public override bool Equals(object obj)
  {
    Matrix3D matrix3D = (Matrix3D) obj;
    bool flag = true;
    for (int index1 = 0; index1 < matrix3D.mData.Length; ++index1)
    {
      for (int index2 = 0; index2 < matrix3D.mData.Length; ++index2)
      {
        if (matrix3D.mData[index1][index2] != this.mData[index1][index2])
          flag = false;
      }
    }
    return flag;
  }

  public override int GetHashCode() => this.mData.GetHashCode();

  internal static Matrix3D GetInvertal(Matrix3D matrix3D)
  {
    Matrix3D identity = Matrix3D.Identity;
    for (int index1 = 0; index1 < 4; ++index1)
    {
      for (int index2 = 0; index2 < 4; ++index2)
        identity[index1, index2] = Matrix3D.GetMinor(matrix3D, index1, index2);
    }
    Matrix3D matrix3D1 = Matrix3D.Transposed(identity);
    return 1.0 / Matrix3D.GetD(matrix3D) * matrix3D1;
  }

  internal static double GetMinor(Matrix3D dd, int columnIndex, int rowIndex)
  {
    return ((columnIndex + rowIndex) % 2 == 0 ? 1.0 : -1.0) * Matrix3D.GetDeterminant((IList<double[]>) Matrix3D.GetMMtr((IList<double[]>) dd.mData, columnIndex, rowIndex));
  }

  internal static Matrix3D TiltArbitrary(double angle, Vector3D vector3D)
  {
    Matrix3D identity = Matrix3D.GetIdentity();
    double x = vector3D.X;
    double y = vector3D.Y;
    double z = vector3D.Z;
    double num1 = x * x;
    double num2 = y * y;
    double num3 = z * z;
    double d = num1 + num2 + num3;
    double num4 = Math.Sqrt(d);
    double num5 = Math.Cos(angle);
    double num6 = Math.Sin(angle);
    identity[0, 0] = (num1 + (num2 + num3) * num5) / d;
    identity[0, 1] = (x * y * (1.0 - num5) - z * num4 * num6) / d;
    identity[0, 2] = (x * z * (1.0 - num5) + y * num4 * num6) / d;
    identity[0, 3] = 0.0;
    identity[1, 0] = (x * y * (1.0 - num5) + z * num4 * num6) / d;
    identity[1, 1] = (num2 + (num1 + num3) * num5) / d;
    identity[1, 2] = (y * z * (1.0 - num5) - x * num4 * num6) / d;
    identity[1, 3] = 0.0;
    identity[2, 0] = (x * z * (1.0 - num5) - y * num4 * num6) / d;
    identity[2, 1] = (y * z * (1.0 - num5) + x * num4 * num6) / d;
    identity[2, 2] = (num3 + (num1 + num2) * num5) / d;
    identity[2, 3] = 0.0;
    identity[3, 0] = 0.0;
    identity[3, 1] = 0.0;
    identity[3, 2] = 0.0;
    identity[3, 3] = 1.0;
    return identity;
  }

  private static double GetDeterminant(IList<double[]> dd)
  {
    int count = dd.Count;
    double determinant = 0.0;
    if (count < 2)
    {
      determinant = dd[0][0];
    }
    else
    {
      int num = 1;
      for (int index = 0; index < count; ++index)
      {
        double[][] mmtr = Matrix3D.GetMMtr(dd, index, 0);
        determinant += (double) num * dd[index][0] * Matrix3D.GetDeterminant((IList<double[]>) mmtr);
        num = num > 0 ? -1 : 1;
      }
    }
    return determinant;
  }

  private static double[][] GetMMtr(IList<double[]> dd, int columnIndex, int rowIndex)
  {
    int length = dd.Count - 1;
    double[][] mmtr = new double[length][];
    for (int index1 = 0; index1 < length; ++index1)
    {
      int index2 = index1 >= columnIndex ? index1 + 1 : index1;
      mmtr[index1] = new double[length];
      for (int index3 = 0; index3 < length; ++index3)
      {
        int index4 = index3 >= rowIndex ? index3 + 1 : index3;
        mmtr[index1][index3] = dd[index2][index4];
      }
    }
    return mmtr;
  }
}
