// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.PolylineHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace HandyControl.Expression.Drawing;

internal static class PolylineHelper
{
  public static IEnumerable<PolylineData> GetWrappedPolylines(
    IList<PolylineData> lines,
    ref double startArcLength)
  {
    int num = 0;
    for (int index = 0; index < lines.Count; ++index)
    {
      num = index;
      startArcLength -= lines[index].TotalLength;
      if (MathHelper.LessThanOrClose(startArcLength, 0.0))
        break;
    }
    if (!MathHelper.LessThanOrClose(startArcLength, 0.0))
      throw new ArgumentOutOfRangeException(nameof (startArcLength));
    startArcLength += lines[num].TotalLength;
    return lines.Skip<PolylineData>(num).Concat<PolylineData>(lines.Take<PolylineData>(num + 1));
  }

  public static void PathMarch(
    PolylineData polyline,
    double startArcLength,
    double cornerThreshold,
    Func<MarchLocation, double> stopCallback)
  {
    int num1 = polyline != null ? polyline.Count : throw new ArgumentNullException(nameof (polyline));
    if (num1 <= 1)
      throw new ArgumentOutOfRangeException(nameof (polyline));
    bool flag = false;
    double num2 = startArcLength;
    double before = 0.0;
    int index = 0;
    double num3 = Math.Cos(cornerThreshold * Math.PI / 180.0);
    while (true)
    {
      do
      {
        double length1;
        do
        {
          length1 = polyline.Lengths[index];
          if (!MathHelper.IsFiniteDouble(num2))
            return;
          if (MathHelper.IsVerySmall(num2))
          {
            num2 = stopCallback(MarchLocation.Create(MarchStopReason.CompleteStep, index, before, length1 - before, num2));
            flag = true;
          }
          else if (MathHelper.GreaterThan(num2, 0.0))
          {
            if (MathHelper.LessThanOrClose(num2 + before, length1))
            {
              before += num2;
              num2 = stopCallback(MarchLocation.Create(MarchStopReason.CompleteStep, index, before, length1 - before, 0.0));
              flag = true;
            }
            else if (index < num1 - 2)
            {
              ++index;
              double num4 = length1 - before;
              num2 -= num4;
              before = 0.0;
              if (flag && num3 != 1.0 && polyline.Angles[index] > num3)
              {
                double length2 = polyline.Lengths[index];
                num2 = stopCallback(MarchLocation.Create(MarchStopReason.CornerPoint, index, before, length2 - before, num2));
              }
            }
            else
            {
              double num5 = length1 - before;
              double remain = num2 - num5;
              double length3 = polyline.Lengths[index];
              before = polyline.Lengths[index];
              num2 = stopCallback(MarchLocation.Create(MarchStopReason.CompletePolyline, index, before, length3 - before, remain));
              flag = true;
            }
          }
        }
        while (!MathHelper.LessThan(num2, 0.0));
        if (MathHelper.GreaterThanOrClose(num2 + before, 0.0))
        {
          before += num2;
          num2 = stopCallback(MarchLocation.Create(MarchStopReason.CompleteStep, index, before, length1 - before, 0.0));
          flag = true;
        }
        else if (index > 0)
        {
          --index;
          num2 += before;
          before = polyline.Lengths[index];
        }
        else
          goto label_21;
      }
      while (!flag || num3 == 1.0 || polyline.Angles[index + 1] <= num3);
      double length4 = polyline.Lengths[index];
      num2 = stopCallback(MarchLocation.Create(MarchStopReason.CornerPoint, index, before, length4 - before, num2));
      continue;
label_21:
      double remain1 = num2 + before;
      double length5 = polyline.Lengths[index];
      before = 0.0;
      num2 = stopCallback(MarchLocation.Create(MarchStopReason.CompletePolyline, index, before, length5 - before, remain1));
      flag = true;
    }
  }
}
