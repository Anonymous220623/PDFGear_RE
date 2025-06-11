// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.AnimationHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Extension;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Tools;

public class AnimationHelper
{
  public static ThicknessAnimation CreateAnimation(Thickness thickness = default (Thickness), double milliseconds = 200.0)
  {
    ThicknessAnimation animation = new ThicknessAnimation(thickness, new Duration(TimeSpan.FromMilliseconds(milliseconds)));
    PowerEase powerEase = new PowerEase();
    powerEase.EasingMode = EasingMode.EaseInOut;
    animation.EasingFunction = (IEasingFunction) powerEase;
    return animation;
  }

  public static DoubleAnimation CreateAnimation(double toValue, double milliseconds = 200.0)
  {
    DoubleAnimation animation = new DoubleAnimation(toValue, new Duration(TimeSpan.FromMilliseconds(milliseconds)));
    PowerEase powerEase = new PowerEase();
    powerEase.EasingMode = EasingMode.EaseInOut;
    animation.EasingFunction = (IEasingFunction) powerEase;
    return animation;
  }

  internal static void DecomposeGeometryStr(string geometryStr, out double[] arr)
  {
    MatchCollection matchCollection = Regex.Matches(geometryStr, "[+-]?\\d*\\.?\\d+(?:\\.\\d+)?(?:[eE][+-]?\\d+)?");
    arr = new double[matchCollection.Count];
    for (int i = 0; i < matchCollection.Count; ++i)
      arr[i] = matchCollection[i].Value.Value<double>();
  }

  internal static Geometry ComposeGeometry(string[] strings, double[] arr)
  {
    StringBuilder stringBuilder = new StringBuilder(strings[0]);
    for (int index = 0; index < arr.Length; ++index)
    {
      string str = strings[index + 1];
      double d = arr[index];
      if (!double.IsNaN(d))
        stringBuilder.Append(d).Append(str);
    }
    return Geometry.Parse(stringBuilder.ToString());
  }

  internal static Geometry InterpolateGeometry(
    double[] from,
    double[] to,
    double progress,
    string[] strings)
  {
    double[] arr = new double[to.Length];
    for (int index = 0; index < to.Length; ++index)
    {
      double num = from[index];
      arr[index] = num + (to[index] - num) * progress;
    }
    return AnimationHelper.ComposeGeometry(strings, arr);
  }

  internal static double[] InterpolateGeometryValue(double[] from, double[] to, double progress)
  {
    double[] numArray = new double[to.Length];
    for (int index = 0; index < to.Length; ++index)
    {
      double num = from[index];
      numArray[index] = num + (to[index] - num) * progress;
    }
    return numArray;
  }
}
