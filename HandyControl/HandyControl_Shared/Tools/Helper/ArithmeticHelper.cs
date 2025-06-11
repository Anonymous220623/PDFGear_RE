// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.ArithmeticHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Media3D;

#nullable disable
namespace HandyControl.Tools;

internal class ArithmeticHelper
{
  public static int[] DivideInt2Arr(int num, int count)
  {
    int[] numArray = new int[count];
    int num1 = num / count;
    int num2 = num % count;
    for (int index = 0; index < count; ++index)
      numArray[index] = num1;
    for (int index = 0; index < num2; ++index)
      ++numArray[index];
    return numArray;
  }

  public static Point CalSafePoint(
    FrameworkElement element,
    FrameworkElement showElement,
    Thickness thickness = default (Thickness))
  {
    if (element == null || showElement == null)
      return new Point();
    Point screen = element.PointToScreen(new Point(0.0, 0.0));
    if (screen.X < 0.0)
      screen.X = 0.0;
    if (screen.Y < 0.0)
      screen.Y = 0.0;
    double num1 = SystemParameters.WorkArea.Width - ((double.IsNaN(showElement.Width) ? showElement.ActualWidth : showElement.Width) + thickness.Left + thickness.Right);
    double num2 = SystemParameters.WorkArea.Height - ((double.IsNaN(showElement.Height) ? showElement.ActualHeight : showElement.Height) + thickness.Top + thickness.Bottom);
    return new Point(num1 > screen.X ? screen.X : num1, num2 > screen.Y ? screen.Y : num2);
  }

  public static Rect GetLayoutRect(FrameworkElement element)
  {
    double num1 = element.ActualWidth;
    double num2 = element.ActualHeight;
    if (element is Image || element is MediaElement)
    {
      if (element.Parent is Canvas)
      {
        num1 = double.IsNaN(element.Width) ? num1 : element.Width;
        num2 = double.IsNaN(element.Height) ? num2 : element.Height;
      }
      else
      {
        num1 = element.RenderSize.Width;
        num2 = element.RenderSize.Height;
      }
    }
    double width = element.Visibility == Visibility.Collapsed ? 0.0 : num1;
    double height = element.Visibility == Visibility.Collapsed ? 0.0 : num2;
    Thickness margin = element.Margin;
    Rect layoutSlot = LayoutInformation.GetLayoutSlot(element);
    double num3 = 0.0;
    double num4 = 0.0;
    double num5;
    switch (element.HorizontalAlignment)
    {
      case HorizontalAlignment.Left:
        num5 = layoutSlot.Left + margin.Left;
        break;
      case HorizontalAlignment.Center:
        num5 = (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - width / 2.0;
        break;
      case HorizontalAlignment.Right:
        num5 = layoutSlot.Right - margin.Right - width;
        break;
      case HorizontalAlignment.Stretch:
        num5 = Math.Max(layoutSlot.Left + margin.Left, (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - width / 2.0);
        break;
      default:
        num5 = num3;
        break;
    }
    double x = num5;
    double num6;
    switch (element.VerticalAlignment)
    {
      case VerticalAlignment.Top:
        num6 = layoutSlot.Top + margin.Top;
        break;
      case VerticalAlignment.Center:
        num6 = (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - height / 2.0;
        break;
      case VerticalAlignment.Bottom:
        num6 = layoutSlot.Bottom - margin.Bottom - height;
        break;
      case VerticalAlignment.Stretch:
        num6 = Math.Max(layoutSlot.Top + margin.Top, (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - height / 2.0);
        break;
      default:
        num6 = num4;
        break;
    }
    double y = num6;
    return new Rect(x, y, width, height);
  }

  public static double CalAngle(Point center, Point p)
  {
    return Math.Atan2(p.Y - center.Y, p.X - center.X) * 180.0 / Math.PI;
  }

  public static Vector3D CalNormal(Point3D p0, Point3D p1, Point3D p2)
  {
    return Vector3D.CrossProduct(new Vector3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z), new Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z));
  }
}
