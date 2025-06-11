// Decompiled with JetBrains decompiler
// Type: Standard.DoubleUtilities
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

#nullable disable
namespace Standard;

internal static class DoubleUtilities
{
  private const double Epsilon = 1.53E-06;

  public static bool AreClose(double value1, double value2)
  {
    if (value1 == value2)
      return true;
    double num = value1 - value2;
    return num < 1.53E-06 && num > -1.53E-06;
  }

  public static bool LessThan(double value1, double value2)
  {
    return value1 < value2 && !DoubleUtilities.AreClose(value1, value2);
  }

  public static bool GreaterThan(double value1, double value2)
  {
    return value1 > value2 && !DoubleUtilities.AreClose(value1, value2);
  }

  public static bool LessThanOrClose(double value1, double value2)
  {
    return value1 < value2 || DoubleUtilities.AreClose(value1, value2);
  }

  public static bool GreaterThanOrClose(double value1, double value2)
  {
    return value1 > value2 || DoubleUtilities.AreClose(value1, value2);
  }

  public static bool IsFinite(double value) => !double.IsNaN(value) && !double.IsInfinity(value);

  public static bool IsValidSize(double value)
  {
    return DoubleUtilities.IsFinite(value) && DoubleUtilities.GreaterThanOrClose(value, 0.0);
  }
}
