// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.OrientedValue
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

public class OrientedValue
{
  public static double GetOrientedXValue(Point point, System.Windows.Controls.Orientation orientation)
  {
    return orientation == System.Windows.Controls.Orientation.Horizontal ? point.X : point.Y;
  }

  public static double GetOrientedYValue(Point point, System.Windows.Controls.Orientation orientation)
  {
    return orientation == System.Windows.Controls.Orientation.Horizontal ? point.Y : point.X;
  }

  public static double GetOrientedTopValue(Rect rect, System.Windows.Controls.Orientation orientation)
  {
    return orientation == System.Windows.Controls.Orientation.Horizontal ? rect.Top : rect.Left;
  }

  public static double GetOrientedBottomValue(Rect rect, System.Windows.Controls.Orientation orientation)
  {
    return orientation == System.Windows.Controls.Orientation.Horizontal ? rect.Bottom : rect.Right;
  }

  public static double GetOrientedLeftValue(Rect rect, System.Windows.Controls.Orientation orientation)
  {
    return orientation == System.Windows.Controls.Orientation.Horizontal ? rect.Left : rect.Top;
  }

  public static double GetOrientedRightValue(Rect rect, System.Windows.Controls.Orientation orientation)
  {
    return orientation == System.Windows.Controls.Orientation.Horizontal ? rect.Right : rect.Bottom;
  }

  public static double GetOrientedWidthValue(Size size, System.Windows.Controls.Orientation orientation)
  {
    return orientation == System.Windows.Controls.Orientation.Horizontal ? size.Width : size.Height;
  }

  public static double GetOrientedHeightValue(Size size, System.Windows.Controls.Orientation orientation)
  {
    return orientation == System.Windows.Controls.Orientation.Horizontal ? size.Height : size.Width;
  }

  public static double GetOrientedWidthValue(Rect rect, System.Windows.Controls.Orientation orientation)
  {
    return orientation == System.Windows.Controls.Orientation.Horizontal ? rect.Width : rect.Height;
  }

  public static double GetOrientedHeightValue(Rect rect, System.Windows.Controls.Orientation orientation)
  {
    return orientation == System.Windows.Controls.Orientation.Horizontal ? rect.Height : rect.Width;
  }

  public static Size GetOrientedSize(double width, double height, System.Windows.Controls.Orientation orientation)
  {
    return orientation == System.Windows.Controls.Orientation.Horizontal ? new Size(width, height) : new Size(height, width);
  }
}
