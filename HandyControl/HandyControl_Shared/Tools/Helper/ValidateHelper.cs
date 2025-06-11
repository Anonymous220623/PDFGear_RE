// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.ValidateHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

#nullable disable
namespace HandyControl.Tools;

public class ValidateHelper
{
  public static bool IsInRangeOfDouble(object value)
  {
    double d = (double) value;
    return !double.IsNaN(d) && !double.IsInfinity(d);
  }

  public static bool IsInRangeOfPosDouble(object value)
  {
    double d = (double) value;
    return !double.IsNaN(d) && !double.IsInfinity(d) && d > 0.0;
  }

  public static bool IsInRangeOfPosDoubleIncludeZero(object value)
  {
    double d = (double) value;
    return !double.IsNaN(d) && !double.IsInfinity(d) && d >= 0.0;
  }

  public static bool IsInRangeOfNegDouble(object value)
  {
    double d = (double) value;
    return !double.IsNaN(d) && !double.IsInfinity(d) && d < 0.0;
  }

  public static bool IsInRangeOfNegDoubleIncludeZero(object value)
  {
    double d = (double) value;
    return !double.IsNaN(d) && !double.IsInfinity(d) && d <= 0.0;
  }

  public static bool IsInRangeOfPosInt(object value) => (int) value > 0;

  public static bool IsInRangeOfPosIntIncludeZero(object value) => (int) value >= 0;

  public static bool IsInRangeOfNegInt(object value) => (int) value < 0;

  public static bool IsInRangeOfNegIntIncludeZero(object value) => (int) value <= 0;
}
