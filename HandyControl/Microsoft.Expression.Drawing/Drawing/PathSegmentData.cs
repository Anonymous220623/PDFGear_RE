// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.PathSegmentData
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Expression.Drawing;

internal sealed class PathSegmentData
{
  public PathSegmentData(Point startPoint, PathSegment pathSegment)
  {
    this.PathSegment = pathSegment;
    this.StartPoint = startPoint;
  }

  public PathSegment PathSegment { get; }

  public Point StartPoint { get; }
}
