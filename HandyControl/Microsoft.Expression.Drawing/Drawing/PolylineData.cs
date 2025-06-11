// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.PolylineData
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#nullable disable
namespace HandyControl.Expression.Drawing;

internal class PolylineData
{
  private IList<double> _accumulates;
  private IList<double> _angles;
  private IList<double> _lengths;
  private IList<Vector> _normals;
  private double? _totalLength;

  public PolylineData(IList<Point> points)
  {
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    this.Points = points.Count > 1 ? points : throw new ArgumentOutOfRangeException(nameof (points));
  }

  public IList<double> AccumulatedLength => this._accumulates ?? this.ComputeAccumulatedLength();

  public IList<double> Angles => this._angles ?? this.ComputeAngles();

  public int Count => this.Points.Count;

  public bool IsClosed => this.Points[0] == this.Points.Last<Point>();

  public IList<double> Lengths => this._lengths ?? this.ComputeLengths();

  public IList<Vector> Normals => this._normals ?? this.ComputeNormals();

  public IList<Point> Points { get; }

  public double TotalLength => this._totalLength ?? this.ComputeTotalLength();

  private IList<double> ComputeAccumulatedLength()
  {
    this._accumulates = (IList<double>) new double[this.Count];
    this._accumulates[0] = 0.0;
    for (int index = 1; index < this.Count; ++index)
      this._accumulates[index] = this._accumulates[index - 1] + this.Lengths[index - 1];
    this._totalLength = new double?(this._accumulates.Last<double>());
    return this._accumulates;
  }

  private IList<double> ComputeAngles()
  {
    this._angles = (IList<double>) new double[this.Count];
    for (int index = 1; index < this.Count - 1; ++index)
      this._angles[index] = -GeometryHelper.Dot(this.Normals[index - 1], this.Normals[index]);
    this._angles[0] = !this.IsClosed ? (this._angles[this.Count - 1] = 1.0) : (this._angles[this.Count - 1] = -GeometryHelper.Dot(this.Normals[0], this.Normals[this.Count - 2]));
    return this._angles;
  }

  private IList<double> ComputeLengths()
  {
    this._lengths = (IList<double>) new double[this.Count];
    for (int index = 0; index < this.Count; ++index)
      this._lengths[index] = this.Difference(index).Length;
    return this._lengths;
  }

  private IList<Vector> ComputeNormals()
  {
    this._normals = (IList<Vector>) new Vector[this.Points.Count];
    for (int index = 0; index < this.Count - 1; ++index)
      this._normals[index] = GeometryHelper.Normal(this.Points[index], this.Points[index + 1]);
    this._normals[this.Count - 1] = this._normals[this.Count - 2];
    return this._normals;
  }

  private double ComputeTotalLength()
  {
    this.ComputeAccumulatedLength();
    return this._totalLength.Value;
  }

  public Vector Difference(int index)
  {
    return this.Points[(index + 1) % this.Count].Subtract(this.Points[index]);
  }

  public Vector SmoothNormal(int index, double fraction, double cornerRadius)
  {
    if (cornerRadius > 0.0)
    {
      double length = this.Lengths[index];
      if (MathHelper.IsVerySmall(length))
      {
        int index1 = index - 1;
        if (index1 < 0 && this.IsClosed)
          index1 = this.Count - 1;
        int index2 = index + 1;
        if (this.IsClosed && index2 >= this.Count - 1)
          index2 = 0;
        return index1 >= 0 && index2 < this.Count ? GeometryHelper.Lerp(this.Normals[index2], this.Normals[index1], 0.5).Normalized() : this.Normals[index];
      }
      double num = Math.Min(cornerRadius / length, 0.5);
      if (fraction <= num)
      {
        int index3 = index - 1;
        if (this.IsClosed && index3 == -1)
          index3 = this.Count - 1;
        if (index3 >= 0)
        {
          double alpha = (num - fraction) / (2.0 * num);
          return GeometryHelper.Lerp(this.Normals[index], this.Normals[index3], alpha).Normalized();
        }
      }
      else if (fraction >= 1.0 - num)
      {
        int index4 = index + 1;
        if (this.IsClosed && index4 >= this.Count - 1)
          index4 = 0;
        if (index4 < this.Count)
        {
          double alpha = (fraction + num - 1.0) / (2.0 * num);
          return GeometryHelper.Lerp(this.Normals[index], this.Normals[index4], alpha).Normalized();
        }
      }
    }
    return this.Normals[index];
  }
}
