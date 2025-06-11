// Decompiled with JetBrains decompiler
// Type: SoundTouch.PeakFinder
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using System;

#nullable disable
namespace SoundTouch;

internal sealed class PeakFinder
{
  private int _minPosition;
  private int _maxPosition;

  public PeakFinder() => this._minPosition = this._maxPosition = 0;

  public double DetectPeak(in ReadOnlySpan<float> data, int minPos, int maxPos)
  {
    this._minPosition = minPos;
    this._maxPosition = maxPos;
    int peakPosition1 = minPos;
    double num1 = (double) data[minPos];
    for (int index = minPos + 1; index < maxPos; ++index)
    {
      if ((double) data[index] > num1)
      {
        num1 = (double) data[index];
        peakPosition1 = index;
      }
    }
    double peakCenter1 = this.GetPeakCenter(in data, peakPosition1);
    double num2 = peakCenter1;
    for (int y = 1; y < 3; ++y)
    {
      double num3 = Math.Pow(2.0, (double) y);
      int peakPosition2 = (int) (peakCenter1 / num3 + 0.5);
      if (peakPosition2 >= minPos)
      {
        int top = this.FindTop(in data, peakPosition2);
        if (top != 0)
        {
          double peakCenter2 = this.GetPeakCenter(in data, top);
          double num4 = num3 * peakCenter2 / peakCenter1;
          if (num4 >= 0.96 && num4 <= 1.04)
          {
            int num5 = (int) (peakCenter1 + 0.5);
            int num6 = (int) (peakCenter2 + 0.5);
            if ((double) data[num6] >= 0.4 * (double) data[num5])
              num2 = peakCenter2;
          }
        }
      }
      else
        break;
    }
    return num2;
  }

  private static double CalcMassCenter(in ReadOnlySpan<float> data, int firstPos, int lastPos)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    for (int index = firstPos; index <= lastPos; ++index)
    {
      num1 += (float) index * data[index];
      num2 += data[index];
    }
    return (double) num2 < 1E-06 ? 0.0 : (double) num1 / (double) num2;
  }

  private int FindCrossingLevel(
    in ReadOnlySpan<float> data,
    float level,
    int peakPosition,
    int direction)
  {
    ref float local = ref data[peakPosition];
    for (int crossingLevel = peakPosition; crossingLevel >= this._minPosition && crossingLevel + direction < this._maxPosition; crossingLevel += direction)
    {
      if ((double) data[crossingLevel + direction] < (double) level)
        return crossingLevel;
    }
    return -1;
  }

  private int FindTop(in ReadOnlySpan<float> data, int peakPosition)
  {
    float num1 = data[peakPosition];
    int num2 = peakPosition - 10;
    if (num2 < this._minPosition)
      num2 = this._minPosition;
    int num3 = peakPosition + 10;
    if (num3 > this._maxPosition)
      num3 = this._maxPosition;
    for (int index = num2; index <= num3; ++index)
    {
      if ((double) data[index] > (double) num1)
      {
        peakPosition = index;
        num1 = data[index];
      }
    }
    return peakPosition == num2 || peakPosition == num3 ? 0 : peakPosition;
  }

  private int FindGround(in ReadOnlySpan<float> data, int peakPosition, int direction)
  {
    int num1 = 0;
    float num2 = data[peakPosition];
    int ground = peakPosition;
    int num3 = peakPosition;
    while (num3 > this._minPosition + 1 && num3 < this._maxPosition - 1)
    {
      int num4 = num3;
      num3 += direction;
      if ((double) data[num3] - (double) data[num4] <= 0.0)
      {
        if (num1 != 0)
          --num1;
        if ((double) data[num3] < (double) num2)
        {
          ground = num3;
          num2 = data[num3];
        }
      }
      else
      {
        ++num1;
        if (num1 > 5)
          break;
      }
    }
    return ground;
  }

  private double GetPeakCenter(in ReadOnlySpan<float> data, int peakPosition)
  {
    int ground1 = this.FindGround(in data, peakPosition, -1);
    int ground2 = this.FindGround(in data, peakPosition, 1);
    float num1 = data[peakPosition];
    float level;
    if (ground1 == ground2)
    {
      level = num1;
    }
    else
    {
      float num2 = (float) (0.5 * ((double) data[ground1] + (double) data[ground2]));
      level = (float) (0.699999988079071 * (double) num1 + 0.30000001192092896 * (double) num2);
    }
    int crossingLevel1 = this.FindCrossingLevel(in data, level, peakPosition, -1);
    int crossingLevel2 = this.FindCrossingLevel(in data, level, peakPosition, 1);
    return crossingLevel1 < 0 || crossingLevel2 < 0 ? 0.0 : PeakFinder.CalcMassCenter(in data, crossingLevel1, crossingLevel2);
  }
}
