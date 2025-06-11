// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.ValueBoxes
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Data;

internal static class ValueBoxes
{
  internal static object TrueBox = (object) true;
  internal static object FalseBox = (object) false;
  internal static object VerticalBox = (object) Orientation.Vertical;
  internal static object HorizontalBox = (object) Orientation.Horizontal;
  internal static object VisibleBox = (object) Visibility.Visible;
  internal static object CollapsedBox = (object) Visibility.Collapsed;
  internal static object HiddenBox = (object) Visibility.Hidden;
  internal static object Double01Box = (object) 0.1;
  internal static object Double0Box = (object) 0.0;
  internal static object Double1Box = (object) 1.0;
  internal static object Double10Box = (object) 10.0;
  internal static object Double20Box = (object) 20.0;
  internal static object Double100Box = (object) 100.0;
  internal static object Double200Box = (object) 200.0;
  internal static object Double300Box = (object) 300.0;
  internal static object DoubleNeg1Box = (object) -1.0;
  internal static object Int0Box = (object) 0;
  internal static object Int1Box = (object) 1;
  internal static object Int2Box = (object) 2;
  internal static object Int5Box = (object) 5;
  internal static object Int99Box = (object) 99;

  internal static object BooleanBox(bool value)
  {
    return !value ? ValueBoxes.FalseBox : ValueBoxes.TrueBox;
  }

  internal static object OrientationBox(Orientation value)
  {
    return value != Orientation.Horizontal ? ValueBoxes.VerticalBox : ValueBoxes.HorizontalBox;
  }

  internal static object VisibilityBox(Visibility value)
  {
    switch (value)
    {
      case Visibility.Visible:
        return ValueBoxes.VisibleBox;
      case Visibility.Hidden:
        return ValueBoxes.HiddenBox;
      case Visibility.Collapsed:
        return ValueBoxes.CollapsedBox;
      default:
        throw new ArgumentOutOfRangeException(nameof (value), (object) value, (string) null);
    }
  }
}
