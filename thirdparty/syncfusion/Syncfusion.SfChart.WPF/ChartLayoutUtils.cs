// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartLayoutUtils
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class ChartLayoutUtils
{
  private const double C_half = 0.5;

  public static Rect GetRectByCenter(Point center, Size size)
  {
    return new Rect(center.X - size.Width / 2.0, center.Y - size.Height / 2.0, size.Width, size.Height);
  }

  public static Rect GetRectByCenter(double cx, double cy, double width, double height)
  {
    return new Rect(cx - width / 2.0, cy - height / 2.0, width, height);
  }

  public static Point GetCenter(Size size) => new Point(0.5 * size.Width, 0.5 * size.Height);

  public static Point GetCenter(Rect rect)
  {
    Point center = ChartLayoutUtils.GetCenter(new Size(rect.Width, rect.Height));
    return new Point(center.X + rect.Left, center.Y + rect.Top);
  }

  public static Rect Subtractthickness(Rect rect, Thickness thickness)
  {
    rect.X += thickness.Left;
    rect.Y += thickness.Top;
    if (rect.Width > thickness.Left + thickness.Right)
      rect.Width -= thickness.Left + thickness.Right;
    else
      rect.Width = 0.0;
    if (rect.Height > thickness.Top + thickness.Bottom)
      rect.Height -= thickness.Top + thickness.Bottom;
    else
      rect.Height = 0.0;
    return rect;
  }

  public static Size Subtractthickness(Size size, Thickness thickness)
  {
    size.Width = Math.Max(size.Width - thickness.Left - thickness.Right, 0.0);
    size.Height = Math.Max(size.Height - thickness.Top - thickness.Bottom, 0.0);
    return size;
  }

  public static Rect Addthickness(Rect rect, Thickness thickness)
  {
    rect.X -= thickness.Left;
    rect.Y -= thickness.Top;
    rect.Width += thickness.Left + thickness.Right;
    rect.Height += thickness.Top + thickness.Bottom;
    return rect;
  }

  public static Size Addthickness(Size size, Thickness thickness)
  {
    if (thickness.Left >= 0.0 && thickness.Right >= 0.0)
      size.Width += thickness.Left + thickness.Right;
    if (thickness.Top >= 0.0 && thickness.Bottom >= 0.0)
      size.Height += thickness.Top + thickness.Bottom;
    return size;
  }

  public static Size CheckSize(Size size)
  {
    size.Width = double.IsInfinity(size.Width) ? 0.0 : size.Width;
    size.Height = double.IsInfinity(size.Height) ? 0.0 : size.Height;
    return size;
  }

  internal static T GetVisualChild<T>(DependencyObject parent) where T : DependencyObject
  {
    visualChild = default (T);
    int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
    for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
    {
      DependencyObject child = VisualTreeHelper.GetChild(parent, childIndex);
      if (!(child is T visualChild))
        visualChild = ChartLayoutUtils.GetVisualChild<T>(child);
      if ((object) visualChild != null)
        break;
    }
    return visualChild;
  }
}
