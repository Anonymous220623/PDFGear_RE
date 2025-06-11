// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Extension.ValueExtension
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Expression.Drawing;
using System.Windows;

#nullable disable
namespace HandyControl.Tools.Extension;

public static class ValueExtension
{
  public static Thickness Add(this Thickness a, Thickness b)
  {
    return new Thickness(a.Left + b.Left, a.Top + b.Top, a.Right + b.Right, a.Bottom + b.Bottom);
  }

  public static bool IsZero(this Thickness thickness)
  {
    return MathHelper.IsZero(thickness.Left) && MathHelper.IsZero(thickness.Top) && MathHelper.IsZero(thickness.Right) && MathHelper.IsZero(thickness.Bottom);
  }

  public static bool IsUniform(this Thickness thickness)
  {
    return MathHelper.AreClose(thickness.Left, thickness.Top) && MathHelper.AreClose(thickness.Left, thickness.Right) && MathHelper.AreClose(thickness.Left, thickness.Bottom);
  }

  public static bool IsNaN(this double value) => double.IsNaN(value);
}
