// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.Vector3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public struct Vector3D
{
  public static readonly Vector3D Empty = new Vector3D(0.0, 0.0, 0.0);
  private double x;
  private double y;
  private double z;

  public Vector3D(double vx, double vy, double vz)
  {
    this.x = vx;
    this.y = vy;
    this.z = vz;
  }

  public Vector3D(Point points, double vz)
  {
    this.x = points.X;
    this.y = points.Y;
    this.z = vz;
  }

  public double X => this.x;

  public double Y => this.y;

  public double Z => this.z;

  public bool IsValid => !double.IsNaN(this.x) && !double.IsNaN(this.y) && !double.IsNaN(this.z);

  public static Vector3D operator -(Vector3D v1, Vector3D v2)
  {
    return new Vector3D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
  }

  public static Vector3D operator +(Vector3D v1, Vector3D v2)
  {
    return new Vector3D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
  }

  public static Vector3D operator *(Vector3D v1, Vector3D v2)
  {
    return new Vector3D(v1.y * v2.z - v2.y * v1.z, v1.z * v2.x - v2.z * v1.x, v1.x * v2.y - v2.x * v1.y);
  }

  public static double operator &(Vector3D v1, Vector3D v2)
  {
    return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
  }

  public static Vector3D operator *(Vector3D v1, double val)
  {
    return new Vector3D(v1.x * val, v1.y * val, v1.z * val);
  }

  public double GetLength() => Math.Sqrt(this & this);

  public void Normalize()
  {
    double length = this.GetLength();
    this.x /= length;
    this.y /= length;
    this.z /= length;
  }

  public override string ToString() => $"X = {this.x}, Y = {this.y}, Z = {this.z}";

  public override bool Equals(object obj)
  {
    return obj is Vector3D vector3D && Math.Abs(vector3D.x - this.x) < 0.0 && Math.Abs(vector3D.y - this.y) < 0.0 && Math.Abs(vector3D.z - this.z) < 0.0;
  }

  public override int GetHashCode()
  {
    return this.x.GetHashCode() ^ this.y.GetHashCode() ^ this.z.GetHashCode();
  }
}
