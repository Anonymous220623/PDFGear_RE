// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.DoubleToPointConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Tools;

[ValueConversion(typeof (Point), typeof (double))]
public class DoubleToPointConverter : IValueConverter
{
  private Point startPoint;
  private Point endPoint;
  private Point centre;
  private Point grad;

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    Point point = (Point) value;
    switch (parameter.ToString())
    {
      case "startx":
        this.startPoint = point;
        return (object) this.startPoint.X;
      case "starty":
        this.startPoint = point;
        return (object) this.startPoint.Y;
      case "endx":
        this.endPoint = point;
        return (object) this.endPoint.X;
      case "endy":
        this.endPoint = point;
        return (object) this.endPoint.Y;
      case "gradx":
        this.grad = point;
        return (object) this.grad.X;
      case "grady":
        this.grad = point;
        return (object) this.grad.Y;
      case "centrex":
        this.centre = point;
        return (object) this.centre.X;
      case "centrey":
        this.centre = point;
        return (object) this.centre.Y;
      default:
        return (object) 0;
    }
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    switch (parameter.ToString())
    {
      case "startx":
        return (object) new Point((double) value, this.startPoint.Y);
      case "starty":
        return (object) new Point(this.startPoint.X, (double) value);
      case "endx":
        return (object) new Point((double) value, this.endPoint.Y);
      case "endy":
        return (object) new Point(this.endPoint.X, (double) value);
      case "gradx":
        return (object) new Point((double) value, this.grad.Y);
      case "grady":
        return (object) new Point(this.grad.X, (double) value);
      case "centrex":
        return (object) new Point((double) value, this.centre.Y);
      case "centrey":
        return (object) new Point(this.centre.X, (double) value);
      default:
        return (object) new Point();
    }
  }
}
