// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.MarchLocation
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Collections.Generic;
using System.Windows;

#nullable disable
namespace HandyControl.Expression.Drawing;

internal class MarchLocation
{
  public double After { get; private set; }

  public double Before { get; private set; }

  public int Index { get; private set; }

  public double Ratio { get; private set; }

  public MarchStopReason Reason { get; private set; }

  public double Remain { get; private set; }

  public static MarchLocation Create(
    MarchStopReason reason,
    int index,
    double before,
    double after,
    double remain)
  {
    double rhs = before + after;
    return new MarchLocation()
    {
      Reason = reason,
      Index = index,
      Remain = remain,
      Before = MathHelper.EnsureRange(before, new double?(0.0), new double?(rhs)),
      After = MathHelper.EnsureRange(after, new double?(0.0), new double?(rhs)),
      Ratio = MathHelper.EnsureRange(MathHelper.SafeDivide(before, rhs, 0.0), new double?(0.0), new double?(1.0))
    };
  }

  public double GetArcLength(IList<double> accumulatedLengths)
  {
    return MathHelper.Lerp(accumulatedLengths[this.Index], accumulatedLengths[this.Index + 1], this.Ratio);
  }

  public Vector GetNormal(PolylineData polyline, double cornerRadius = 0.0)
  {
    return polyline.SmoothNormal(this.Index, this.Ratio, cornerRadius);
  }

  public Point GetPoint(IList<Point> points)
  {
    return GeometryHelper.Lerp(points[this.Index], points[this.Index + 1], this.Ratio);
  }
}
