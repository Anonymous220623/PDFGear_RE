// Decompiled with JetBrains decompiler
// Type: Standard.Assert
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

#nullable disable
namespace Standard;

internal static class Assert
{
  private static void _Break() => Debugger.Break();

  [Conditional("DEBUG")]
  public static void Evaluate(Assert.EvaluateFunction argument) => argument();

  [Conditional("DEBUG")]
  [Obsolete("Use Assert.AreEqual instead of Assert.Equals", false)]
  public static void Equals<T>(T expected, T actual)
  {
  }

  [Conditional("DEBUG")]
  public static void AreEqual<T>(T expected, T actual)
  {
    if ((object) expected == null)
    {
      if ((object) actual == null || actual.Equals((object) expected))
        return;
      Assert._Break();
    }
    else
    {
      if (expected.Equals((object) actual))
        return;
      Assert._Break();
    }
  }

  [Conditional("DEBUG")]
  public static void AreNotEqual<T>(T notExpected, T actual)
  {
    if ((object) notExpected == null)
    {
      if ((object) actual != null && !actual.Equals((object) notExpected))
        return;
      Assert._Break();
    }
    else
    {
      if (!notExpected.Equals((object) actual))
        return;
      Assert._Break();
    }
  }

  [Conditional("DEBUG")]
  public static void Implies(bool condition, bool result)
  {
    if (!condition || result)
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void Implies(bool condition, Assert.ImplicationFunction result)
  {
    if (!condition || result())
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void IsNeitherNullNorEmpty(string value)
  {
  }

  [Conditional("DEBUG")]
  public static void IsNeitherNullNorWhitespace(string value)
  {
    if (string.IsNullOrEmpty(value))
      Assert._Break();
    if (value.Trim().Length != 0)
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void IsNotNull<T>(T value) where T : class
  {
    if ((object) value != null)
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void IsDefault<T>(T value) where T : struct => value.Equals((object) default (T));

  [Conditional("DEBUG")]
  public static void IsNotDefault<T>(T value) where T : struct
  {
    value.Equals((object) default (T));
  }

  [Conditional("DEBUG")]
  public static void IsFalse(bool condition)
  {
    if (!condition)
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void IsFalse(bool condition, string message)
  {
    if (!condition)
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void IsTrue(bool condition)
  {
    if (condition)
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void IsTrue(bool condition, string message)
  {
    if (condition)
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void Fail() => Assert._Break();

  [Conditional("DEBUG")]
  public static void Fail(string message) => Assert._Break();

  [Conditional("DEBUG")]
  public static void IsNull<T>(T item) where T : class
  {
    if ((object) item == null)
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void BoundedDoubleInc(
    double lowerBoundInclusive,
    double value,
    double upperBoundInclusive)
  {
    if (value >= lowerBoundInclusive && value <= upperBoundInclusive)
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void BoundedInteger(int lowerBoundInclusive, int value, int upperBoundExclusive)
  {
    if (value >= lowerBoundInclusive && value < upperBoundExclusive)
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void IsApartmentState(ApartmentState expectedState)
  {
    if (Thread.CurrentThread.GetApartmentState() == expectedState)
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void NullableIsNotNull<T>(T? value) where T : struct
  {
    if (value.HasValue)
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void NullableIsNull<T>(T? value) where T : struct
  {
    if (!value.HasValue)
      return;
    Assert._Break();
  }

  [Conditional("DEBUG")]
  public static void IsNotOnMainThread()
  {
    if (!Application.Current.Dispatcher.CheckAccess())
      return;
    Assert._Break();
  }

  public delegate void EvaluateFunction();

  public delegate bool ImplicationFunction();
}
